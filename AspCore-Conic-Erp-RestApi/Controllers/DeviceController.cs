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
        private int iCanSaveTmp = 0;

        private ConicErpContext DB = new ConicErpContext();
        [Route("Device/GetById")]
        [HttpGet]
        public IActionResult GetById(long? Id)
        {
            var Device = DB.Devices.Where(x => x.Id == Id).Select(x=> new { x.Id, x.Name, x.Feel, x.Status, x.Port,x.MAC, x.Ip, x.Description }).SingleOrDefault();

            return Ok(Device);
        }
        [Route("Device/Create")]
        [HttpPost]
        public IActionResult Create(Device collection)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DB.Devices.Add(collection);
                    DB.SaveChanges();
                    return Ok(collection.Id);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }

        [Route("Device/Edit")]
        [HttpPost]
        public IActionResult Edit(Device collection)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    Device Device = DB.Devices.Where(x => x.Id == collection.Id).SingleOrDefault();
                Device.Name = collection.Name;
                Device.Ip = collection.Ip;
                Device.MAC = collection.MAC;
                Device.Port = collection.Port;
                Device.Feel = collection.Feel;
                Device.Status = collection.Status;
                Device.LastSetDateTime = collection.LastSetDateTime;
                Device.Description = collection.Description;
             
                    DB.SaveChanges();
                    return Ok(true);
                }
                catch
                {
                    //Console.WriteLine(collection);
                    return Ok(false);
                }
            }
            return Ok(false);
        }
        
        [Route("Device/FeelDevice")]
        [HttpGet]
        public IActionResult FeelDevice()
        {
            var Devices = DB.Devices.Where(x=>x.Feel ==true).ToList();
            Devices.ForEach(e => CheckDeviceHere((int)e.Id));
            return Ok(true);
        }
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
            var Device = DB.Devices.Select(d => new { d.Id, d.Name , d.Feel, d.Status, d.Port ,d.Ip , d.Description }).ToList();
                        
            return Ok(Device);
        }

        [HttpGet]

        [Route("Device/CheckDevice")]
        public IActionResult CheckDevice(int Id)
        {
            if (CheckDeviceHere(Id))
                return Ok(true);
            else return Ok(false);

        }
        [Route("Device/StartEnrollUser")]
        [HttpGet]
        public IActionResult StartEnrollUser(long DeviceId, long UserId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {

                string sUserID = UserId.ToString();
                int iFingerIndex = 111;
                int idwErrorCode = 0;

               objZkeeper.CancelOperation();
             //   objZkeeper.SSR_DeleteEnrollData((int)DeviceId,sUserID, 0);//If the specified index of user's templates has existed ,delete it first.(SSR_DelUserTmp is also available sometimes)
              objZkeeper.RefreshData(objZkeeper.MachineNumber);//the data in the device should be refreshed

                if (objZkeeper.StartEnrollEx(sUserID, iFingerIndex ,0))
                {
                    iCanSaveTmp = 1;

                    objZkeeper.StartIdentify();//After enrolling templates,you should let the device into the 1:N verification condition
                    objZkeeper.RefreshData(objZkeeper.MachineNumber);//the data in the device should be refreshed

                }
                else
                {
                    objZkeeper.GetLastError(ref idwErrorCode);
                }
                return Ok(iCanSaveTmp ==1? true: false);
            }
            else
                return Ok("Device Is Not Connected");
        }
        [Route("Device/SetUser")]
        [HttpGet]
        public IActionResult SetUser(long DeviceId ,  long UserId  )
        {
            if (CheckDeviceHere((int)DeviceId)) {
                objZkeeper.EnableDevice(objZkeeper.MachineNumber, false);

                var member = DB.Members.Where(m => m.Id == UserId).FirstOrDefault();
                string Name ="";
                string password ="";
                int Privilege =0;
                bool Enable = false;
                //bool GetUser = objZkeeper.GetUserInfo(objZkeeper.MachineNumber,(int)member.Id,ref  Name, ref password, ref Privilege, ref Enable);
                byte x = 0;

                bool SetUser = objZkeeper.SSR_SetUserInfo(objZkeeper.MachineNumber, member.Id.ToString(), member.Name, "", 1, true);
                if (SetUser)
                {
                    string strface = "";
                    int length = 0;
                    bool GetUserFace = objZkeeper.GetUserFaceStr(objZkeeper.MachineNumber, member.Id.ToString(), 50, ref strface, ref length);
                    var memeberface = DB.MemberFaces.Where(f => f.MemberId == member.Id).SingleOrDefault();

                    if (GetUserFace)
                    {

                        if (memeberface != null)
                        {
                            memeberface.FaceLength = length;
                            memeberface.FaceStr = strface;
                            memeberface.MemberId = member.Id;

                            bool SetUserFace = objZkeeper.SetUserFaceStr(objZkeeper.MachineNumber, member.Id.ToString(), 50, strface, length);
                             SetUserFace = objZkeeper.SSR_SetUserTmpStr(objZkeeper.MachineNumber, member.Id.ToString(), 50, strface);
                             SetUserFace = objZkeeper.SetUserFace(objZkeeper.MachineNumber, member.Id.ToString(), 0, ref x, length);

                        }
                        else
                        {
                            DB.MemberFaces.Add(new MemberFace
                            {
                                FaceLength = length,
                                FaceStr = strface,
                                MemberId = member.Id,
                            });

                            bool SetUserFace = objZkeeper.SetUserFace(objZkeeper.MachineNumber, member.Id.ToString(), 0, ref x, length);
                        //    objZkeeper.SetUserFace(objZkeeper.MachineNumber, member.Id.ToString(), 50, strface, length);
                        }
                    }
                    else
                    {
                        if (memeberface != null) {
                            bool SetUserFace = objZkeeper.SetUserFace(objZkeeper.MachineNumber,  member.Id.ToString(), 50, ref x, memeberface.FaceLength);

                        }

                    }

                }
                objZkeeper.RefreshData(objZkeeper.MachineNumber);
                DB.SaveChanges();
                objZkeeper.EnableDevice(objZkeeper.MachineNumber, true);

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
                    ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, objZkeeper.MachineNumber);

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
            if (isValidIpA)
            {
                objZkeeper = new ZkemClient(RaiseDeviceEvent);
                Device.Description = "Is Device Connected : ";
            
                IsDeviceConnected = objZkeeper.Connect_Net(Device.Ip, Device.Port);
                objZkeeper.SetDeviceTime2(objZkeeper.MachineNumber, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

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
                Device.Description = "Is Device Connected  ";
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

                bool SetUser = objZkeeper.SSR_SetUserInfo(objZkeeper.MachineNumber, member.Id.ToString(), member.Name, "", 0, Enable);
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
                    SetUser((int)DeviceId, M.Id);
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
                        bool GetUserFace = objZkeeper.GetUserFaceStr(objZkeeper.MachineNumber, M.Id.ToString(), 50, ref strface, ref length);

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
               bool ClearKeeperData = objZkeeper.ClearKeeperData(objZkeeper.MachineNumber);
               bool ClearGLog = objZkeeper.ClearGLog(objZkeeper.MachineNumber);
                bool ClearSLog = objZkeeper.ClearSLog(objZkeeper.MachineNumber);

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
               bool ClearAdministrators = objZkeeper.ClearAdministrators(objZkeeper.MachineNumber);
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
