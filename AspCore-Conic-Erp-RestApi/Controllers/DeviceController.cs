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

        private ConicErpContext DB;
        public DeviceController(ConicErpContext dbcontext)
        {
            DB = dbcontext;
        }
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
            try
            {
                var Devices = DB.Devices.Where(x => x.Feel == true).ToList();
                Devices.ForEach(e => CheckDeviceHere((int)e.Id));
                return Ok(true);
            }
            catch {
                return Ok(false);
            }
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
        public IActionResult DirectlyPrint(string PrinterName ,string Ip ,string PortName )
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
            if (CheckDeviceHere(Id))return Ok(true);
            else return Ok(false);

        }
        [Route("Device/StartEnrollUser")]
        [HttpGet]
        public IActionResult StartEnrollUser(long DeviceId, string UserId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {

                int iFingerIndex = 111;
                int idwErrorCode = 0;
                bool startenroll_retult = false;
               objZkeeper.CancelOperation();
             //   objZkeeper.SSR_DeleteEnrollData((int)DeviceId,sUserID, 0);//If the specified index of user's templates has existed ,delete it first.(SSR_DelUserTmp is also available sometimes)
         //     objZkeeper.RefreshData(1);//the data in the device should be refreshed

                if (objZkeeper.StartEnrollEx(UserId, iFingerIndex ,0))
                {
                    iCanSaveTmp = 1;

                    objZkeeper.StartIdentify();//After enrolling templates,you should let the device into the 1:N verification condition
                    objZkeeper.RefreshData(1);//the data in the device should be refreshed
                    startenroll_retult = true;

                }
                else
                {
                    objZkeeper.GetLastError(ref idwErrorCode);
                    startenroll_retult = false;
                }
                return Ok(startenroll_retult);
            }
            else
                return Ok("Device Is Not Connected");
        }
        [Route("Device/SetUser")]
        [HttpGet]
        public IActionResult SetUser(long DeviceId ,  string UserId , string Name , string TableName)
        {
            if (CheckDeviceHere((int)DeviceId)) {
           
               objZkeeper.EnableDevice(1, false);
   
                //bool GetUser = objZkeeper.GetUserInfo(1,(int)member.Id,ref  Name, ref password, ref Privilege, ref Enable);

                bool SetUser = objZkeeper.SSR_SetUserInfo(1, UserId, Name, "", 0, true);
                if (SetUser)
                {
                    string strface = "";
                    int length = 0;
                    bool GetUserFace = objZkeeper.GetUserFaceStr(1, UserId, 50, ref strface, ref length);
                    var FingerPrint = DB.FingerPrints.Where(f => f.Fk == UserId && f.TableName == TableName).SingleOrDefault();

                    if (GetUserFace)
                    {

                        if (FingerPrint != null)
                        {
                            FingerPrint.Length = length;
                            FingerPrint.Str = strface;
                            FingerPrint.Fk =UserId;
                            FingerPrint.TableName =TableName;
                            FingerPrint.Type ="Face";

                            bool SetUserFace = objZkeeper.SetUserFaceStr(1, UserId, 50, strface, length);
                            // SetUserFace = objZkeeper.SSR_SetUserTmpStr(1, member.Id.ToString(), 50, strface);
                         //    SetUserFace = objZkeeper.SetUserFace(1, member.Id.ToString(), 0, ref x, length);
                        }
                        else
                        {
                            DB.FingerPrints.Add(new FingerPrint
                            {
                               Length = length,
                                Str = strface,
                                Fk = UserId,
                            TableName = TableName,
                            Type = "Face"
                        });
                            bool SetUserFace = objZkeeper.SetUserFaceStr(1, UserId, 50, strface, length);
                        //    objZkeeper.SetUserFace(1, member.Id.ToString(), 50, strface, length);
                        }
                    }
                    else
                    {
                        if (FingerPrint != null) {
                            bool SetUserFace = objZkeeper.SetUserFaceStr(1,  UserId, 50, FingerPrint.Str, FingerPrint.Length);
                        }
                    }
                }
                objZkeeper.RefreshData(1);
                DB.SaveChanges();
                objZkeeper.EnableDevice(1, true);
                objZkeeper.Disconnect();

                return Ok(SetUser);
            }
            else return Ok("Device Is Not Connected");
        }

        [Route("Device/GetUserLog")]
        [HttpGet]
        public IActionResult GetUserLog(long DeviceId, string UserId ,string TableName)
        {
            try
            {
                if (CheckDeviceHere((int)DeviceId))
                {
                    ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, 1);
                    if (MachineLog != null && MachineLog.Count > 0)
                    {
                        foreach (var ML in MachineLog.Where(mlo=> mlo.IndRegID == Convert.ToInt64(UserId) ).ToList())
                        {
                            DateTime datetime = DateTime.Parse(ML.DateTimeRecord);
                            var isLogSaveIt = DB.DeviceLogs.Where(l => l.Fk == UserId && l.TableName==TableName && l.DateTime == datetime).Count();
                            if (isLogSaveIt <= 0)
                            {
                                DeviceLog Log = new DeviceLog
                                {
                                    Type = "In",
                                    DateTime = DateTime.Parse(ML.DateTimeRecord),
                                    DeviceId = DeviceId,
                                    Status = 0,
                                    TableName = TableName,
                                    Fk = UserId,
                                    Description = ""
                                };
                                if (Log.DateTime < DateTime.Today)
                                    Log.Status = -1;
                                DB.DeviceLogs.Add(Log);
                                DB.SaveChanges();
                            }
                        }
                        objZkeeper.Disconnect();
                        return Ok(true);
                    }
                    else
                    {
                        objZkeeper.Disconnect();
                        return Ok("There Don't Have Log ");
                    }
                }
                else return Ok("Device Is Not Connected");
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
                objZkeeper.SetDeviceTime2(1, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
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
                Device.Description = "Device Is Not Connected";
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

                bool SetUser = objZkeeper.SSR_SetUserInfo(1, member.Id.ToString(), member.Name, "", 0, Enable);
                if (!SetUser)
               return false;

                return true;
            }
            else
                return false;

        }
        [Route("Device/SetAll")]
        [HttpGet]
        public IActionResult SetAll(long DeviceId ,string TableName)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                dynamic List = new List<dynamic>();
                if (TableName == "Member") {
                    DateTime last = DateTime.Today.AddMonths(-3);
                     List = DB.Members.Where(x => x.MembershipMovements.Count() != 0 && (x.MembershipMovements != null ? x.MembershipMovements.OrderByDescending(x => x.Id).LastOrDefault().EndDate >= last : false))
                        .Select(s=> new { s.Id , s.Name}).ToList();
                }
                if (TableName == "Employee")
                {
                    List = DB.Employees.Where(x => x.Status ==0)
                       .Select(s => new { s.Id, s.Name }).ToList();
                }
                foreach (var O in List)
                {
                    SetUser((int)DeviceId, O.Id , O.Name , TableName);
                }
                objZkeeper.Disconnect();

                return Ok(true);
            }
            else
                return Ok("Device Is Not Connected");
        }
        [Route("Device/GetAllFingerPrints")]
        [HttpGet]
        public IActionResult GetAllFingerPrints(long DeviceId ,string TableName)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                var List = new List<UserDevice>();
                if(TableName == "Member")
                List = DB.Members?.Select(s => new UserDevice { Id = s.Id.ToString(),Name= s.Name }).ToList();
                if (TableName == "Employee")
                    List = DB.Employees?.Select(s=> new UserDevice  { Id = s.Id.ToString() , Name = s.Name}).ToList();

                foreach (var O in List)
                {
                   
                    var FingerPrint = DB.FingerPrints.Where(f => f.Fk == O.Id && f.TableName == TableName).SingleOrDefault();
                    if (FingerPrint == null)
                    {
                        string strface = "";
                        int length = 0;
                        bool GetUserFace = objZkeeper.GetUserFaceStr(1, O.Id, 50, ref strface, ref length);

                        if (GetUserFace)
                        {
                            DB.FingerPrints.Add(new FingerPrint
                            {
                                Length = length,
                                Str = strface,
                                Fk = O.Id,
                                TableName = TableName,
                                Type = "Face"
                            });
                        }
                    }
                    else {
                        continue;
                    }
                    DB.SaveChanges();
                }
                objZkeeper.Disconnect();

                return Ok(true);
            }
            else
                return Ok("Device Is Not Connected");
        }

        [Route("Device/GetAllLog")]
        [HttpGet]
        public IActionResult GetAllLog(long DeviceId ,string TableName)
        {
            if (CheckDeviceHere((int)DeviceId))
            {
                ICollection<MachineInfo> MachineLog = manipulator?.GetLogData(objZkeeper, 1);
                if (MachineLog != null && MachineLog.Count > 0)
                {
                        foreach (var ML in MachineLog.ToList())
                        {
                            DateTime datetime = DateTime.Parse(ML.DateTimeRecord);
                            datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, 0);
                            var isLogSaveIt = DB.DeviceLogs.Where(l => l.Fk == ML.IndRegID.ToString() && l.TableName == TableName && l.DateTime == datetime).Count();
                            if (isLogSaveIt <= 0)
                            {
                                DeviceLog Log = new DeviceLog
                                {
                                   Type = "In",
                                    DateTime = DateTime.Parse(ML.DateTimeRecord),
                                    DeviceId = DeviceId,
                                    Status = 0,
                                    TableName = TableName,
                                    Fk = ML.IndRegID.ToString(),
                                    Description = ""
                                };
                                if (Log.DateTime < DateTime.Today)
                                    Log.Status = -1;
                                DB.DeviceLogs.Add(Log);
                                DB.SaveChanges();
                            }
                            else
                            {
                                continue;
                            }
                    }
                    DB.SaveChanges();
                    objZkeeper.ClearGLog(0);
                    objZkeeper.Disconnect();
                    return Ok(true);
                }
                else
                {
                    objZkeeper.Disconnect();
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
               bool ClearKeeperData = objZkeeper.ClearKeeperData(1);
               bool ClearGLog = objZkeeper.ClearGLog(1);
                bool ClearSLog = objZkeeper.ClearSLog(1);
                objZkeeper.Disconnect();

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
               bool ClearAdministrators = objZkeeper.ClearAdministrators(1);
                objZkeeper.Disconnect();

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
        [Route("Device/TurnOff")]
        [HttpGet]
        public IActionResult TurnOff(long DeviceId)
        {
            if (CheckDeviceHere((int)DeviceId))
            {

                if (objZkeeper.PowerOffDevice(0))
                    return Ok("The device is being PowerOffDevice, Please wait...  true");

                else
                    return Ok("Operation failed,please try again false");
            }
            else
                return Ok("Device Is Not Connected");
        }
         public class UserDevice
        {
            public string Id { get; set; }
            public string Name { get; set; }

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
