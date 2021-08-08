using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Entities;
using System.IO.Ports;
using System.Text;
using ESC_POS_USB_NET.Printer;

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
            if (Com != null)
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
                try
                {
                    sp.Open();
                    sp.Write(char.ConvertFromUtf32(28699) + char.ConvertFromUtf32(9472) + char.ConvertFromUtf32(3365));
                    sp.Close();
                    return Ok(Com);
                }
                catch
                {
                    return Ok("this Com Is Not Find");
                }
            }
            else return Ok(false);
        }


        [HttpGet]
        [Route("Device/DirectlyPrint")]
        public IActionResult DirectlyPrint(string? PrinterName ,string? Ip ,string? PortName )
        {
            Printer printer = new Printer(PrinterName);
            printer.Append("يسشيس)");

            printer.FullPaperCut();
            printer.PrintDocument();
            return Ok(true);
        }


        [Route("Device/GetDevice")]
        [HttpGet]
        public IActionResult GetDevice()
        {
            var Device = DB.Devices.Select(d => new { d.Id, d.Name , d.IsPrime , d.Status, d.Port ,d.Ip , d.Description }).ToList();
                        
            return Ok(Device);
        }

        [HttpGet]

        [Route("Device/CheckDevice")]
        public IActionResult CheckDevice(int Id)
        {
            var Device = DB.Devices.Where(x => x.Id == Id).SingleOrDefault();
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

                if (isValidIpA && DisconnectDeviceHere(Id))
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
        public IActionResult SetUser(long DeviceId ,  long UserId  )
        {
            if (CheckDeviceHere((int)DeviceId)) {

                var member = DB.Members.Where(m => m.Id == UserId).FirstOrDefault();
     
                bool SetUser = objZkeeper.SSR_SetUserInfo((int)DeviceId, member.Id.ToString(), member.Name, "", 0, true);
                if (SetUser)
                {
                    string strface = "";
                    int length = 0;
                    bool GetUserFace = objZkeeper.GetUserFaceStr((int)DeviceId, member.Id.ToString(), 50, ref strface, ref length);
                    if (GetUserFace)
                    {
                        var memeberface = DB.MemberFaces.Where(f => f.MemberId == member.Id).SingleOrDefault();
                        if (memeberface != null)
                        {
                            memeberface.FaceLength = length;
                            memeberface.FaceStr = strface;
                            memeberface.MemberId = member.Id;

                            bool SetUserFace = objZkeeper.SetUserFaceStr((int)DeviceId, member.Id.ToString(), 50, memeberface.FaceStr, memeberface.FaceLength);
                        }
                        else
                        {
                            DB.MemberFaces.Add(new MemberFace
                            {
                                FaceLength = length,
                                FaceStr = strface,
                                MemberId = member.Id,
                            });
                        }
                    }
                }
                DB.SaveChanges();
                return Ok(SetUser);
            }
            else
                return Ok("Device Is Not Connected");
        }

        [Route("Device/GetUserLog")]
        [HttpGet]
        public IActionResult GetUserLog(long DeviceId, long UserId)
        {
            try
            {
                if (CheckDeviceHere((int)DeviceId))
                {
                    ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, (int)DeviceId);

                    if (MachineLog != null && MachineLog.Count > 0)
                    {
                        var member = DB.Members.Where(m => m.Id == UserId).FirstOrDefault();

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
        public bool CheckDeviceHere(int Id)
        {

            var Device = DB.Devices.Where(x => x.Id == Id).SingleOrDefault();
            bool IsDeviceConnected = false;
            bool isValidIpA = UniversalStatic.ValidateIP(Device.Ip);

            if (!isValidIpA)
                Device.Description = "The Device IP is invalid !!";
            isValidIpA = UniversalStatic.PingTheDevice(Device.Ip);
            if (!isValidIpA)
                Device.Description = "The device at " + Device.Ip + ":" + Device.Port + " did not respond!!";
            if (isValidIpA && DisconnectDeviceHere(Id))
            {
                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                Device.Description = "Is Device Connected : ";
                IsDeviceConnected = objZkeeper.Connect_Net(Device.Ip, Device.Port);
                objZkeeper.SetDeviceTime2((int)Device.Id, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            }
            DB.SaveChanges();
            return IsDeviceConnected;
        }
        public bool DisconnectDeviceHere(int Id)
        {
            var Device = DB.Devices.Where(x => x.Id == Id).SingleOrDefault();
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
        public bool EnableMemberToDevice(long DeviceId , long UserId , bool Enable)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                var member = DB.Members.Where(m => m.Id == UserId).FirstOrDefault();

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
                DateTime last = DateTime.Today.AddMonths(-3);
                IList<Member> Members = DB.Members.Where(x=>x.MembershipMovements.Count() != 0 && (x.MembershipMovements != null ? x.MembershipMovements.OrderByDescending(x => x.Id).LastOrDefault().EndDate >= last : false )).ToList();

                foreach (Member M in Members)
                {
                   // var MemberShipLast = DB.MembershipMovements.Where(mm => mm.MemberId == M.Id && mm.EndDate >= last).ToList();
                   // if (MemberShipLast.Count() <=0 ) continue;
                    bool SetUser = objZkeeper.SSR_SetUserInfo(0, M.Id.ToString(), M.Name, "", 0, false);
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
                ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, 0);
                if (MachineLog != null && MachineLog.Count > 0)
                {
                
                        foreach (var ML in MachineLog.ToList())
                        {
                        var member = DB.Members.Where(x => x.Id == ML.IndRegID).SingleOrDefault();
                        if (member != null)
                        {
                            DateTime datetime = DateTime.Parse(ML.DateTimeRecord);
                            datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
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
                            else
                            {
                                continue;
                            }
                        }
                        else continue;
                    }

                        DB.SaveChanges();

                    objZkeeper.ClearGLog(0);
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
        [Route("Device/ClearUserLog")]
        [HttpGet]
        public IActionResult ClearUserLog(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
               bool ClearKeeperData = objZkeeper.ClearKeeperData(0);
               bool ClearGLog = objZkeeper.ClearGLog(0);
               bool ClearSLog = objZkeeper.ClearSLog(0);
                return Ok("ClearKeeperData : " + ClearKeeperData + "-ClearGLog : "+ ClearGLog+ "-ClearSLog : "+ ClearSLog);

            }
            else
                return Ok("Device Is Not Connected");
        }   
        [Route("Device/ClearAdministrators")]
        [HttpGet]
        public IActionResult ClearAdministrators(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
               bool ClearAdministrators = objZkeeper.ClearAdministrators(0);
                return Ok("ClearAdministrators : " + ClearAdministrators + "");

            }
            else
                return Ok("Device Is Not Connected");
        }
        [Route("Device/RestartDevice")]
        [HttpGet]
        public IActionResult RestartDevice(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
             
                    if (objZkeeper.RestartDevice(0))
                        return  Ok("The device is being restarted, Please wait...  true");
                    
                else
                        return Ok("Operation failed,please try again false" );
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
