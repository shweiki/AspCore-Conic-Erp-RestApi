using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Entities;
using System.IO.Ports;
using System.Text;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class DeviceController : Controller
    {
        DeviceManipulator manipulator = new DeviceManipulator();

        private ZkemClient objZkeeper;

        private ConicErpContext DB = new ConicErpContext();
        [Route("Device/OpenCashDrawer")]
        [HttpGet]
        public IActionResult OpenCashDrawer(string Com)
        {
            Encoding enc = Encoding.Unicode;
            SerialPort sp = new SerialPort();
            sp.PortName = Com;
            sp.Encoding = enc;
            sp.BaudRate = 38400;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.DtrEnable = true;
            sp.Open();
            if (sp.IsOpen)
            {
                sp.Write(char.ConvertFromUtf32(28699) + char.ConvertFromUtf32(9472) + char.ConvertFromUtf32(3365));
                sp.Close();
                return Ok(Com);

            }else

            return Ok("this Com Is Not Find");
        }


        [Route("Device/GetDevice")]
        [HttpGet]
        public IActionResult GetDevice()
        {
            var Device = DB.Devices.Select(d => new { d.Id, d.Name , d.IsPrime , d.Status, d.Port ,d.Ip , d.Description }).ToList();
                        
            return Ok(Device);
        }


        [Route("Device/CheckDevice")]
        [HttpGet]
        public IActionResult CheckDevice(int ID)
        {
            var Device = DB.Devices.Where(x => x.Id == ID).SingleOrDefault();
            if (Device != null)
            {
                int DeviceId = (int)Device.Id;
                bool IsDeviceConnected = false;
                bool isValidIpA = UniversalStatic.ValidateIP(Device.Ip);

                if (!isValidIpA)
                    Device.Description = "The Device IP is invalid !!";
                isValidIpA = UniversalStatic.PingTheDevice(Device.Ip);
                if (!isValidIpA)
                    Device.Description = "The device at " + Device.Ip + ":" + Device.Port + " did not respond!!";

                if (isValidIpA && DisconnectDeviceHere(ID))
                {
                    objZkeeper = new ZkemClient(RaiseDeviceEvent);
                    Device.Description = "Is Device Connected : ";
                    IsDeviceConnected = objZkeeper.Connect_Net(Device.Ip, Device.Port);
                }
                DB.SaveChanges();
                return Ok(IsDeviceConnected);
            }
            else return  Ok("false");

        }

        [Route("Device/SetUser")]
        [HttpGet]
        public IActionResult SetUser(long DeviceId ,  long UserID  )
        {
            if (CheckDeviceHere((int)DeviceId)) {

                var member = DB.Members.Where(m => m.Id == UserID).FirstOrDefault();
     
                bool SetUser = objZkeeper.SSR_SetUserInfo((int)DeviceId, member.Id.ToString(), member.Name, "", 0, true);
                if (SetUser)
                {

                    var memeberface = DB.MemberFaces.Where(f => f.MemberId == member.Id).SingleOrDefault();
                    if (memeberface != null)
                    {
                        bool SetUserFace = objZkeeper.SetUserFaceStr((int)DeviceId, member.Id.ToString(), 50, memeberface.FaceStr, memeberface.FaceLength);
                    }
                    else { 
                    string strface = "";
                    int length = 0;
                    bool GetUserFace = objZkeeper.GetUserFaceStr((int)DeviceId, member.Id.ToString(), 50, ref strface, ref length);

                    if (GetUserFace)
                    {
                        DB.MemberFaces.Add(new MemberFace
                        {
                            FaceLength = length,
                            FaceStr = strface,
                            MemberId = member.Id,

                        });
                        DB.SaveChanges();

                    }
                    }


                }

                return Ok(SetUser);
            }
            else
                return Ok("Device Is Not Connected");
        }

        [Route("Device/GetUserLog")]
        [HttpGet]
        public IActionResult GetUserLog(long DeviceId, long UserID)
        {
            try
            {
                if (CheckDeviceHere((int)DeviceId))
                {
                    ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, (int)DeviceId);

                    if (MachineLog != null && MachineLog.Count > 0)
                    {
                        var member = DB.Members.Where(m => m.Id == UserID).FirstOrDefault();

                        foreach (var ML in MachineLog.Where(mlo=> mlo.IndRegID == (int)member.Id).ToList())
                        {
                            DateTime datetime = DateTime.Parse(ML.DateTimeRecord);
                            var isLogSaveIt = DB.MemberLogs.Where(l => l.MemberId == member.Id && l.DateTime == datetime).Count();
                            if (isLogSaveIt <= 0)
                            {
                                MemberLog Log = new MemberLog
                                {
                                    Type = "In",
                                    MemberId = member.Id,
                                    DateTime = DateTime.Parse(ML.DateTimeRecord),
                                    DeviceId = DeviceId,
                                    Status = 0,
                                    Description = ""
                                };
                                if (Log.DateTime < DateTime.Today)
                                    Log.Status = -1;
                                DB.MemberLogs.Add(Log);
                                DB.SaveChanges();
                            }

                        }
                        return Ok(true);

                    }
                    else
                    {
                        return Ok("There Don't Have Log ");

                    }
                }
                else {
                    return Ok("Device Is Not Connected");
                }
            }
            catch
            {
                return NotFound();
            }
            
        }
        public bool CheckDeviceHere(int ID)
        {
            

            var Device = DB.Devices.Where(x => x.Id == ID).SingleOrDefault();
    
            
            bool IsDeviceConnected = false;
            bool isValidIpA = UniversalStatic.ValidateIP(Device.Ip);

            if (!isValidIpA)
                Device.Description = "The Device IP is invalid !!";
            isValidIpA = UniversalStatic.PingTheDevice(Device.Ip);
            if (!isValidIpA)
                Device.Description = "The device at " + Device.Ip + ":" + Device.Port + " did not respond!!";
            if (isValidIpA && DisconnectDeviceHere(ID))
            {
                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                Device.Description = "Is Device Connected : ";
                IsDeviceConnected = objZkeeper.Connect_Net(Device.Ip, Device.Port);
            }
            DB.SaveChanges();
            return IsDeviceConnected;
        }
        public bool DisconnectDeviceHere(int ID)
        {
            var Device = DB.Devices.Where(x => x.Id == ID).SingleOrDefault();
            int DeviceId = (int)Device.Id;
            bool isValidIpA = UniversalStatic.ValidateIP(Device.Ip);

            if (!isValidIpA)
                Device.Description = "The Device IP is invalid !!";
            isValidIpA = UniversalStatic.PingTheDevice(Device.Ip);
            if (!isValidIpA)
                Device.Description = "The device at " + Device.Ip + ":" + Device.Port + " did not respond!!";
            if (isValidIpA)
            {
                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                Device.Description = "Is Disconnect Connected  ";
                 objZkeeper.Disconnect();
            }
            DB.SaveChanges();
            return true;
        }
        public bool EnableMemberToDevice(long DeviceId , long UserID , bool Enable)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                var member = DB.Members.Where(m => m.Id == UserID).FirstOrDefault();

                bool SetUser = objZkeeper.SSR_SetUserInfo((int)DeviceId, member.Id.ToString(), member.Name, "", 0, Enable);
                if (!SetUser)
               return false;                
                
                return true;

            }
            else
                return false;

        }
        [Route("Device/SetAllMembers")]
        [HttpGet]
        public IActionResult SetAllMembers(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                IList<Member> Members = DB.Members?.ToList();
                DateTime last = DateTime.Today.AddMonths(-3);

                foreach (Member M in Members)
                {
                    if (M.MembershipMovements.Count() <= 0) continue;
                    if (M.MembershipMovements.LastOrDefault().EndDate < last) continue;
                    bool SetUser = objZkeeper.SSR_SetUserInfo((int)DeviceId, M.Id.ToString(), M.Name, "", 0, false);
                    if (SetUser)
                    {
                        var memeberface = DB.MemberFaces.Where(f => f.MemberId == M.Id).SingleOrDefault();
                        if (memeberface != null)
                        {
                            bool SetUserFace = objZkeeper.SetUserFaceStr((int)DeviceId, M.Id.ToString(), 50, memeberface.FaceStr, memeberface.FaceLength);
                            if (SetUserFace)
                            {
                                continue;
                            }

                        }
                        else  continue; 

                    }
                    else continue;

                }
                return Ok(true);

            }
            else
                return Ok("Device Is Not Connected");
        }
        [Route("Device/GetAllFaceMembers")]
        [HttpGet]
        public IActionResult GetAllFaceMembers(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                IList<Member> Members = DB.Members?.ToList();
                foreach (Member M in Members)
                {

                    var memeberface = DB.MemberFaces.Where(f => f.MemberId == M.Id).SingleOrDefault();
                    if (memeberface == null)
                    {
                        string strface = "";
                        int length = 0;
                        bool GetUserFace = objZkeeper.GetUserFaceStr((int)DeviceId, M.Id.ToString(), 50, ref strface, ref length);

                        if (GetUserFace)
                        {
                            DB.MemberFaces.Add(new MemberFace
                            {
                                FaceLength = length,
                                FaceStr = strface,
                                MemberId = M.Id,

                            });

                        }
                    }
                    else {
                        continue;
                    }

                    DB.SaveChanges();

                }

                return Ok(true);
            }
            else
                return Ok("Device Is Not Connected");
        }

        [Route("Device/GetAllLogMembers")]
        [HttpGet]
        public IActionResult GetAllLogMembers(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, objZkeeper.MachineNumber);
                if (MachineLog != null && MachineLog.Count > 0)
                {
                
                        foreach (var ML in MachineLog.ToList())
                        {
                        var member = DB.Members.Where(x => x.Id == ML.IndRegID).SingleOrDefault();

                             DateTime datetime = DateTime.Parse(ML.DateTimeRecord);
                            datetime =  new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour , datetime.Minute, 0);
                            var isLogSaveIt = DB.MemberLogs.Where(l => l.MemberId == member.Id && l.DateTime == datetime).Count();
                            if (isLogSaveIt <= 0)
                            {
                                MemberLog Log = new MemberLog
                                {
                                    Type = "In",
                                    MemberId = member.Id,
                                    DateTime = datetime,
                                    DeviceId = DeviceId,
                                    Status = 0,
                                    Description = "From Device"
                                };
                                if (Log.DateTime < DateTime.Today)
                                    Log.Status = -1;
                                DB.MemberLogs.Add(Log);
                                DB.SaveChanges();
                            }
                            else {
                                continue;
                                    }

                        }

                        DB.SaveChanges();

                    objZkeeper.ClearGLog(objZkeeper.MachineNumber);
                return Ok(true);
                }
                else
                {
                    return Ok("There Don't Have Log ");

                }
            }
            else
                return Ok("Device Is Not Connected");
        }

        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {

                        break;
                    }

                default:
                    break;
            }

        }
    }
}
