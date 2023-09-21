using zkemkeeper;
using ZktClientService.Models;

namespace ZktClientService.Services;

public class ZkemClient
{
    private readonly Action<object, EventFlagEnum> _raiseDeviceEvent;
    private readonly CZKEM _objCZKEM = new CZKEM();
    public bool isConnected = false; //the boolean value identifies whether the device is connected

    public ZkemClient(Action<object, EventFlagEnum> raiseDeviceEvent)
    {
        _raiseDeviceEvent = raiseDeviceEvent;
    }

    #region 'What we will be using'

    public bool BatchUpdate(int dwMachineNumber)
    {
        return _objCZKEM.BatchUpdate(dwMachineNumber);
    }


    public bool Connect_Net(string IPAdd, int Port)
    {

        if (_objCZKEM.Connect_Net(IPAdd, Port))
        {
            //65535, 32767
            if (_objCZKEM.RegEvent(1, 65535))
            {
                // [ Register your events here ]
                // [ Go through the _IZKEMEvents_Event class for a complete list of events
                _objCZKEM.OnConnected += ObjCZKEM_OnConnected;
                _objCZKEM.OnDisConnected += objCZKEM_OnDisConnected;
                _objCZKEM.OnEnrollFinger += ObjCZKEM_OnEnrollFinger;
                _objCZKEM.OnFinger += ObjCZKEM_OnFinger;


                _objCZKEM.OnFingerFeature += new _IZKEMEvents_OnFingerFeatureEventHandler(zkemClient_OnFingerFeature);
                _objCZKEM.OnEnrollFingerEx += new _IZKEMEvents_OnEnrollFingerExEventHandler(zkemClient_OnEnrollFingerEx);

                _objCZKEM.OnDeleteTemplate += new _IZKEMEvents_OnDeleteTemplateEventHandler(zkemClient_OnDeleteTemplate);
                _objCZKEM.OnNewUser += new _IZKEMEvents_OnNewUserEventHandler(zkemClient_OnNewUser);
                _objCZKEM.OnHIDNum += new _IZKEMEvents_OnHIDNumEventHandler(zkemClient_OnHIDNum);
                _objCZKEM.OnAlarm += new _IZKEMEvents_OnAlarmEventHandler(zkemClient_OnAlarm);
                _objCZKEM.OnDoor += new _IZKEMEvents_OnDoorEventHandler(zkemClient_OnDoor);
                _objCZKEM.OnWriteCard += new _IZKEMEvents_OnWriteCardEventHandler(zkemClient_OnWriteCard);
                _objCZKEM.OnEmptyCard += new _IZKEMEvents_OnEmptyCardEventHandler(zkemClient_OnEmptyCard);


                _objCZKEM.OnVerify += new _IZKEMEvents_OnVerifyEventHandler(zkemClient_OnVerifyEventHandler);
                _objCZKEM.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(zkemClient_OnAttTransactionEx);
                _objCZKEM.OnAttTransaction += new _IZKEMEvents_OnAttTransactionEventHandler(zkemClient_OnAttTransaction);
                ///_objCZKEM.OnAttTransactionEx_New += new _IZKEMEvents_OnAttTransactionEx_NewEventHandler(zkemClient_OnAttTransactionEx_New);
                ///_objCZKEM.OnGeneralEvent += new _IZKEMEvents_OnGeneralEventEventHandler(zkemClient_OnGeneralEvent);
                _objCZKEM.OnKeyPress += new _IZKEMEvents_OnKeyPressEventHandler(zkemClient_OnKeyPress);
                _objCZKEM.OnEMData += new _IZKEMEvents_OnEMDataEventHandler(zkemClient_OnEMData);

            }
            isConnected = true;
            return true;
        }
        return false;
    }

    public void Disconnect()
    {
        // [ Unregister events
        _objCZKEM.OnConnected -= ObjCZKEM_OnConnected;
        _objCZKEM.OnDisConnected -= objCZKEM_OnDisConnected;
        _objCZKEM.OnEnrollFinger -= ObjCZKEM_OnEnrollFinger;
        _objCZKEM.OnFinger -= ObjCZKEM_OnFinger;


        _objCZKEM.OnFingerFeature -= new _IZKEMEvents_OnFingerFeatureEventHandler(zkemClient_OnFingerFeature);
        _objCZKEM.OnEnrollFingerEx -= new _IZKEMEvents_OnEnrollFingerExEventHandler(zkemClient_OnEnrollFingerEx);

        _objCZKEM.OnDeleteTemplate -= new _IZKEMEvents_OnDeleteTemplateEventHandler(zkemClient_OnDeleteTemplate);
        _objCZKEM.OnNewUser -= new _IZKEMEvents_OnNewUserEventHandler(zkemClient_OnNewUser);
        _objCZKEM.OnHIDNum -= new _IZKEMEvents_OnHIDNumEventHandler(zkemClient_OnHIDNum);
        _objCZKEM.OnAlarm -= new _IZKEMEvents_OnAlarmEventHandler(zkemClient_OnAlarm);
        _objCZKEM.OnDoor -= new _IZKEMEvents_OnDoorEventHandler(zkemClient_OnDoor);
        _objCZKEM.OnWriteCard -= new _IZKEMEvents_OnWriteCardEventHandler(zkemClient_OnWriteCard);
        _objCZKEM.OnEmptyCard -= new _IZKEMEvents_OnEmptyCardEventHandler(zkemClient_OnEmptyCard);


        _objCZKEM.OnVerify -= new _IZKEMEvents_OnVerifyEventHandler(zkemClient_OnVerifyEventHandler);
        _objCZKEM.OnAttTransactionEx -= new _IZKEMEvents_OnAttTransactionExEventHandler(zkemClient_OnAttTransactionEx);
        // _objCZKEM.OnAttTransaction -= new _IZKEMEvents_OnAttTransactionEventHandler(zkemClient_OnAttTransaction);
        //  _objCZKEM.OnAttTransactionEx_New -= new _IZKEMEvents_OnAttTransactionEx_NewEventHandler(zkemClient_OnAttTransactionEx_New);
        // _objCZKEM.OnGeneralEvent -= new _IZKEMEvents_OnGeneralEventEventHandler(zkemClient_OnGeneralEvent);
        _objCZKEM.OnKeyPress -= new _IZKEMEvents_OnKeyPressEventHandler(zkemClient_OnKeyPress);
        _objCZKEM.OnEMData -= new _IZKEMEvents_OnEMDataEventHandler(zkemClient_OnEMData);

        isConnected = false;
        _objCZKEM.Disconnect();
    }
    public bool Beep(int DelayMS)
    {
        return _objCZKEM.Beep(DelayMS);
    }
    public bool BeginBatchUpdate(int dwMachineNumber, int UpdateFlag)
    {
        return _objCZKEM.BeginBatchUpdate(dwMachineNumber, UpdateFlag);
    }

    public bool ClearData(int dwMachineNumber, int DataFlag)
    {
        return _objCZKEM.ClearData(dwMachineNumber, DataFlag);
    }
    public bool ClearGLog(int dwMachineNumber)
    {
        return _objCZKEM.ClearGLog(dwMachineNumber);
    }
    private void zkemClient_OnAttTransactionEx(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
    {
        string Ip = "";
        _objCZKEM.GetDeviceIP(1, ref Ip);
        _raiseDeviceEvent(new
        {
            Id = EnrollNumber,
            Ip,
            Datetime = new DateTime(Year, Month, Day, Hour, Minute, 0),
            //   IsInValid,
            //  AttState,
            //   VerifyMethod,
        }, EventFlagEnum.OnAttTransactionExEvent);

    }

    private void zkemClient_OnEMData(int DataType, int DataLen, ref sbyte DataBuffer)
    {
    }
    private void zkemClient_OnKeyPress(int Key)
    {
    }
    private void zkemClient_OnGeneralEvent(string DataStr)
    {
    }
    private void zkemClient_OnFingerFeature(int Score)
    {
    }
    private void zkemClient_OnEnrollFingerEx(string EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
    {
    }
    private void zkemClient_OnDeleteTemplate(int EnrollNumber, int FingerIndex)
    {
    }
    private void zkemClient_OnNewUser(int EnrollNumber)
    {
    }
    private void zkemClient_OnHIDNum(int CardNumber)
    {
    }
    private void zkemClient_OnAlarm(int AlarmType, int EnrollNumber, int Verified)
    {
    }
    private void zkemClient_OnDoor(int EventType)
    {
    }
    private void zkemClient_OnWriteCard(int EnrollNumber, int ActionResult, int Length)
    {
    }
    private void zkemClient_OnEmptyCard(int ActionResult)
    {
    }
    private void ObjCZKEM_OnFinger()
    {
    }

    private void ObjCZKEM_OnEnrollFinger(int EnrollNumber, int FingerIndex, int ActionResult, int TemplateLength)
    {
    }

    private void ObjCZKEM_OnConnected()
    {
        _raiseDeviceEvent(this, EventFlagEnum.Connect);
    }


    void objCZKEM_OnDisConnected()
    {
        // Implementing the Event
        _raiseDeviceEvent(this, EventFlagEnum.Disconnect);
    }


    public bool DelUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex)
    {
        return DelUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex);
    }

    public bool DisableDeviceWithTimeOut(int dwMachineNumber, int TimeOutSec)
    {
        return _objCZKEM.DisableDeviceWithTimeOut(dwMachineNumber, TimeOutSec);
    }

    private void zkemClient_OnVerifyEventHandler(int UserId)
    {


    }
    private void zkemClient_OnAttTransactionEx_New(string EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, string WorkCode)
    {


    }


    private void zkemClient_OnAttTransaction(int EnrollNumber, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second)
    {
    }



    public bool EnableDevice(int dwMachineNumber, bool bFlag)
    {
        return _objCZKEM.EnableDevice(dwMachineNumber, bFlag);
    }


    public bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
    {
        return _objCZKEM.GetDeviceTime(dwMachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
    }


    public bool GetUserInfo(int dwMachineNumber, int dwEnrollNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled)
    {
        return GetUserInfo(dwMachineNumber, dwEnrollNumber, ref Name, ref Password, ref Privilege, ref Enabled);
    }
    public bool GetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, out int VerifyStyle, out byte Reserved)
    {
        return _objCZKEM.GetUserInfoEx(dwMachineNumber, dwEnrollNumber, out VerifyStyle, out Reserved);
    }

    public bool GetUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref byte TmpData, ref int TmpLength)
    {
        return _objCZKEM.GetUserTmp(dwMachineNumber, dwEnrollNumber, dwFingerIndex, ref TmpData, ref TmpLength);
    }

    public bool GetUserTmpEx(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int Flag, out byte TmpData, out int TmpLength)
    {
        return _objCZKEM.GetUserTmpEx(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out Flag, out TmpData, out TmpLength);
    }

    public bool GetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out int Flag, out string TmpData, out int TmpLength)
    {
        return _objCZKEM.GetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out Flag, out TmpData, out TmpLength);
    }



    public bool PowerOffDevice(int dwMachineNumber)
    {
        return _objCZKEM.PowerOffDevice(dwMachineNumber);
    }


    public bool QueryState(ref int State)
    {
        return _objCZKEM.QueryState(ref State);
    }

    public bool ReadAllGLogData(int dwMachineNumber)
    {
        return _objCZKEM.ReadAllGLogData(dwMachineNumber);
    }


    public bool ReadAllTemplate(int dwMachineNumber)
    {
        return _objCZKEM.ReadAllTemplate(dwMachineNumber);
    }

    public bool ReadAllUserID(int dwMachineNumber)
    {
        return _objCZKEM.ReadAllUserID(dwMachineNumber);
    }

    public bool RefreshData(int dwMachineNumber)
    {
        return _objCZKEM.RefreshData(dwMachineNumber);
    }

    public bool RegEvent(int dwMachineNumber, int EventMask)
    {
        return _objCZKEM.RegEvent(dwMachineNumber, EventMask);
    }

    public bool RestartDevice(int dwMachineNumber)
    {
        return _objCZKEM.RestartDevice(dwMachineNumber);
    }


    public bool SaveTheDataToFile(int dwMachineNumber, string TheFilePath, int FileFlag)
    {
        return _objCZKEM.SaveTheDataToFile(dwMachineNumber, TheFilePath, FileFlag);
    }


    public bool SetUserTmpExStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, int Flag, string TmpData)
    {
        return _objCZKEM.SetUserTmpExStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, Flag, TmpData);
    }

    public bool SSR_EnableUser(int dwMachineNumber, string dwEnrollNumber, bool bFlag)
    {
        return _objCZKEM.SSR_EnableUser(dwMachineNumber, dwEnrollNumber, bFlag);
    }

    public bool SSR_GetAllUserInfo(int dwMachineNumber, out string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled)
    {
        return _objCZKEM.SSR_GetAllUserInfo(dwMachineNumber, out dwEnrollNumber, out Name, out Password, out Privilege, out Enabled);
    }
    public bool GetAllGLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwInOutMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
    {
        throw new NotImplementedException();
    }

    public bool GetAllSLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int Params4, ref int Params1, ref int Params2, ref int dwManipulation, ref int Params3, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute)
    {
        throw new NotImplementedException();
    }

    public bool GetAllUserInfo(int dwMachineNumber, ref int dwEnrollNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled)
    {
        throw new NotImplementedException();
    }


    public bool SSR_GetGeneralLogData(int dwMachineNumber, out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, ref int dwWorkCode)
    {
        return _objCZKEM.SSR_GetGeneralLogData(dwMachineNumber, out dwEnrollNumber, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode);
    }

    public bool SSR_GetUserInfo(int dwMachineNumber, string dwEnrollNumber, out string Name, out string Password, out int Privilege, out bool Enabled)
    {
        return _objCZKEM.SSR_GetUserInfo(dwMachineNumber, dwEnrollNumber, out Name, out Password, out Privilege, out Enabled);
    }

    public bool SSR_GetUserTmpStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out string TmpData, out int TmpLength)
    {
        return _objCZKEM.SSR_GetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, out TmpData, out TmpLength);
    }

    public bool SSR_SetUserInfo(int dwMachineNumber, string dwEnrollNumber, string Name, string Password, int Privilege, bool Enabled)
    {
        return _objCZKEM.SSR_SetUserInfo(dwMachineNumber, dwEnrollNumber, Name, Password, Privilege, Enabled);
    }

    public bool StartEnroll(int UserID, int FingerID)
    {
        return _objCZKEM.StartEnroll(UserID, FingerID);
    }

    public bool StartEnrollEx(string UserID, int FingerID, int Flag)
    {
        return _objCZKEM.StartEnrollEx(UserID, FingerID, Flag);
    }

    public bool StartIdentify()
    {
        return _objCZKEM.StartIdentify();
    }


    public bool GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnable)
    { return _objCZKEM.GetAllUserID(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege, dwEnable); }

    public bool GetFirmwareVersion(int dwMachineNumber, ref string strVersion)
    { return _objCZKEM.GetFirmwareVersion(dwMachineNumber, strVersion); }

    public bool GetVendor(ref string strVendor)
    { return _objCZKEM.GetVendor(strVendor); }

    public bool GetWiegandFmt(int dwMachineNumber, ref string sWiegandFmt)
    { return _objCZKEM.GetWiegandFmt(dwMachineNumber, sWiegandFmt); }
    public bool GetSDKVersion(ref string strVersion)
    { return _objCZKEM.GetSDKVersion(strVersion); }

    public bool GetSerialNumber(int dwMachineNumber, out string dwSerialNumber)
    { return _objCZKEM.GetSerialNumber(dwMachineNumber, out dwSerialNumber); }

    public bool GetDeviceMAC(int dwMachineNumber, ref string sMAC)
    { return _objCZKEM.GetDeviceMAC(dwMachineNumber, sMAC); }

    public void GetLastError(ref int dwErrorCode)
    {
        _objCZKEM.GetLastError(dwErrorCode);
    }
    #endregion


    #region 'Not Implemented'

    public bool ClearDataEx(int dwMachineNumber, string TableName)
    {
        throw new NotImplementedException();
    }
    public bool GetUserInfoByCard(int dwMachineNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled)
    {
        throw new NotImplementedException();
    }

    public bool GetUserInfoByPIN2(int dwMachineNumber, ref string Name, ref string Password, ref int Privilege, ref bool Enabled)
    {
        throw new NotImplementedException();
    }


    public bool CancelBatchUpdate(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool CancelOperation()
    {
        return _objCZKEM.CancelOperation();
    }

    public bool CaptureImage(bool FullImage, ref int Width, ref int Height, ref byte Image, string ImageFile)
    {
        throw new NotImplementedException();
    }

    public bool ClearAdministrators(int dwMachineNumber)
    {
        return _objCZKEM.ClearAdministrators(dwMachineNumber);
    }


    public bool ClearKeeperData(int dwMachineNumber)
    {
        return _objCZKEM.ClearKeeperData(dwMachineNumber);
    }

    public bool ClearLCD()
    {
        throw new NotImplementedException();
    }

    public bool ClearPhotoByTime(int dwMachineNumber, int iFlag, string sTime, string eTime)
    {
        throw new NotImplementedException();
    }

    public bool ClearSLog(int dwMachineNumber)
    {
        return _objCZKEM.ClearSLog(dwMachineNumber);
    }

    public bool ClearSMS(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ClearUserSMS(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool Connect_USB(int MachineNumber)
    {
        throw new NotImplementedException();
    }

    public void ConvertPassword(int dwSrcPSW, ref int dwDestPSW, int dwLength)
    {
        throw new NotImplementedException();
    }

    public bool DelCustomizeAttState(int dwMachineNumber, int StateID)
    {
        throw new NotImplementedException();
    }

    public bool DelCustomizeVoice(int dwMachineNumber, int VoiceID)
    {
        throw new NotImplementedException();
    }

    public bool DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber)
    {
        throw new NotImplementedException();
    }

    public bool DeleteSMS(int dwMachineNumber, int ID)
    {
        throw new NotImplementedException();
    }

    public bool DeleteUserInfoEx(int dwMachineNumber, int dwEnrollNumber)
    {
        throw new NotImplementedException();
    }

    public bool DeleteUserSMS(int dwMachineNumber, int dwEnrollNumber, int SMSID)
    {
        throw new NotImplementedException();
    }

    public bool DeleteWorkCode(int WorkCodeID)
    {
        throw new NotImplementedException();
    }

    public bool DelUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex)
    {
        throw new NotImplementedException();
    }


    public bool EmptyCard(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool EnableClock(int Enabled)
    {
        throw new NotImplementedException();
    }

    public bool EnableCustomizeAttState(int dwMachineNumber, int StateID, int Enable)
    {
        throw new NotImplementedException();
    }

    public bool EnableCustomizeVoice(int dwMachineNumber, int VoiceID, int Enable)
    {
        throw new NotImplementedException();
    }


    public bool EnableUser(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, bool bFlag)
    {
        throw new NotImplementedException();
    }

    public bool FPTempConvert(ref byte TmpData1, ref byte TmpData2, ref int Size)
    {
        throw new NotImplementedException();
    }
    public bool GetDoorState(int MachineNumber, ref int State)
    {
        throw new NotImplementedException();
    }

    public bool GetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnrollData, ref int dwPassWord)
    {
        throw new NotImplementedException();
    }

    public bool GetEnrollDataStr(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref string dwEnrollData, ref int dwPassWord)
    {
        throw new NotImplementedException();
    }

    public bool ReadAllSLogData(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool GetUserTmpStr(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref string TmpData, ref int TmpLength)
    {
        throw new NotImplementedException();
    }

    public bool GetUserTZs(int dwMachineNumber, int dwEnrollNumber, ref int TZs)
    {
        throw new NotImplementedException();
    }

    public bool GetUserTZStr(int dwMachineNumber, int dwEnrollNumber, ref string TZs)
    {
        throw new NotImplementedException();
    }



    public bool PlayVoice(int Position, int Length)
    {
        return _objCZKEM.PlayVoice(Position, Length);
    }



    public bool GetUserFaceStr(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref string TmpData, ref int TmpLength)
    {
        return _objCZKEM.GetUserFaceStr(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref TmpData, ref TmpLength);
    }

    public bool GetUserGroup(int dwMachineNumber, int dwEnrollNumber, ref int UserGrp)
    {
        throw new NotImplementedException();
    }

    public bool GetUserIDByPIN2(int PIN2, ref int UserID)
    {
        throw new NotImplementedException();
    }

    public bool FPTempConvertNew(ref byte TmpData1, ref byte TmpData2, ref int Size)
    {
        throw new NotImplementedException();
    }

    public bool FPTempConvertNewStr(string TmpData1, ref string TmpData2, ref int Size)
    {
        throw new NotImplementedException();
    }

    public bool FPTempConvertStr(string TmpData1, ref string TmpData2, ref int Size)
    {
        throw new NotImplementedException();
    }

    public bool GetACFun(ref int ACFun)
    {
        throw new NotImplementedException();
    }


    public int GetBackupNumber(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool GetCardFun(int dwMachineNumber, ref int CardFun)
    {
        throw new NotImplementedException();
    }

    public bool GetDataFile(int dwMachineNumber, int DataFlag, string FileName)
    {
        throw new NotImplementedException();
    }

    public bool GetDataFileEx(int dwMachineNumber, string SourceFileName, string DestFileName)
    {
        throw new NotImplementedException();
    }

    public bool GetDaylight(int dwMachineNumber, ref int Support, ref string BeginTime, ref string EndTime)
    {
        throw new NotImplementedException();
    }

    public bool GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue)
    {
        throw new NotImplementedException();
    }

    public bool GetDeviceIP(int dwMachineNumber, ref string IPAddr)
    {
        return _objCZKEM.GetDeviceIP(dwMachineNumber, ref IPAddr);
    }


    public bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue)
    {
        throw new NotImplementedException();
    }

    public bool GetDeviceStrInfo(int dwMachineNumber, int dwInfo, out string Value)
    {
        throw new NotImplementedException();
    }



    public bool ClearWorkCode()
    {
        throw new NotImplementedException();
    }

    public bool Connect_Com(int ComPort, int MachineNumber, int BaudRate)
    {
        throw new NotImplementedException();
    }

    public bool Connect_Modem(int ComPort, int MachineNumber, int BaudRate, string Telephone)
    {
        throw new NotImplementedException();
    }


    public int AccGroup
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int BASE64
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int CommPort
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int ConvertBIG5
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int MachineNumber
    {
        get
        {
            return 1;
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public uint PIN2
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int PINWidth
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public int PullMode
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public bool ReadMark
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public int SSRPin
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool ACUnlock(int dwMachineNumber, int Delay)
    {
        throw new NotImplementedException();
    }

    public bool BackupData(string DataFile)
    {
        throw new NotImplementedException();
    }

    public bool ReadAOptions(string AOption, out string AValue)
    {
        throw new NotImplementedException();
    }

    public bool ReadAttRule(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadCustData(int dwMachineNumber, ref string CustData)
    {
        throw new NotImplementedException();
    }

    public bool ReadDPTInfo(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadFile(int dwMachineNumber, string FileName, string FilePath)
    {
        throw new NotImplementedException();
    }

    public bool ReadGeneralLogData(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadLastestLogData(int dwMachineNumber, int NewLog, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
    {
        throw new NotImplementedException();
    }

    public bool ReadRTLog(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadSuperLogData(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadSuperLogDataEx(int dwMachineNumber, int dwYear_S, int dwMonth_S, int dwDay_S, int dwHour_S, int dwMinute_S, int dwSecond_S, int dwYear_E, int dwMonth_E, int dwDay_E, int dwHour_E, int dwMinute_E, int dwSecond_E, int dwLogIndex)
    {
        throw new NotImplementedException();
    }

    public bool ReadTurnInfo(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool ReadUserAllTemplate(int dwMachineNumber, string dwEnrollNumber)
    {
        throw new NotImplementedException();
    }


    public bool RestoreData(string DataFile)
    {
        throw new NotImplementedException();
    }


    public bool SendCMDMsg(int dwMachineNumber, int Param1, int Param2)
    {
        throw new NotImplementedException();
    }

    public bool SendFile(int dwMachineNumber, string FileName)
    {
        throw new NotImplementedException();
    }

    public bool SetCommPassword(int CommKey)
    {
        throw new NotImplementedException();
    }

    public bool SetCustomizeAttState(int dwMachineNumber, int StateID, int NewState)
    {
        throw new NotImplementedException();
    }

    public bool SetCustomizeVoice(int dwMachineNumber, int VoiceID, string FileName)
    {
        throw new NotImplementedException();
    }

    public bool SetDaylight(int dwMachineNumber, int Support, string BeginTime, string EndTime)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceCommPwd(int dwMachineNumber, int CommKey)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceIP(int dwMachineNumber, string IPAddr)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceMAC(int dwMachineNumber, string sMAC)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceTime(int dwMachineNumber)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceTime2(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
    {
        return _objCZKEM.SetDeviceTime2(dwMachineNumber, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
    }

    public bool SetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, ref int dwEnrollData, int dwPassWord)
    {
        throw new NotImplementedException();
    }

    public bool SetEnrollDataStr(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, string dwEnrollData, int dwPassWord)
    {
        throw new NotImplementedException();
    }

    public bool SetGroupTZs(int dwMachineNumber, int GroupIndex, ref int TZs)
    {
        throw new NotImplementedException();
    }

    public bool SetGroupTZStr(int dwMachineNumber, int GroupIndex, string TZs)
    {
        throw new NotImplementedException();
    }

    public bool SetHoliday(int dwMachineNumber, string Holiday)
    {
        throw new NotImplementedException();
    }

    public bool SetLanguageByID(int dwMachineNumber, int LanguageID, string Language)
    {
        throw new NotImplementedException();
    }

    public bool SetLastCount(int count)
    {
        throw new NotImplementedException();
    }

    public bool SetOptionCommPwd(int dwMachineNumber, string CommKey)
    {
        throw new NotImplementedException();
    }

    public bool SetSMS(int dwMachineNumber, int ID, int Tag, int ValidMinutes, string StartTime, string Content)
    {
        throw new NotImplementedException();
    }

    public bool SetStrCardNumber(string ACardNumber)
    {
        throw new NotImplementedException();
    }

    public bool SetSysOption(int dwMachineNumber, string Option, string Value)
    {
        throw new NotImplementedException();
    }

    public bool SetTZInfo(int dwMachineNumber, int TZIndex, string TZ)
    {
        throw new NotImplementedException();
    }

    public bool SetUnlockGroups(int dwMachineNumber, string Grps)
    {
        throw new NotImplementedException();
    }

    public bool SetUserFace(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, ref byte TmpData, int TmpLength)
    {
        return _objCZKEM.SetUserFace(dwMachineNumber, dwEnrollNumber, dwFaceIndex, ref TmpData, TmpLength);

    }

    public bool SetUserFaceStr(int dwMachineNumber, string dwEnrollNumber, int dwFaceIndex, string TmpData, int TmpLength)
    {

        return _objCZKEM.SetUserFaceStr(dwMachineNumber, dwEnrollNumber, dwFaceIndex, TmpData, TmpLength);
    }

    public bool SetUserGroup(int dwMachineNumber, int dwEnrollNumber, int UserGrp)
    {
        throw new NotImplementedException();
    }

    public bool SetUserInfo(int dwMachineNumber, int dwEnrollNumber, string Name, string Password, int Privilege, bool Enabled)
    {
        return _objCZKEM.SetUserInfo(dwMachineNumber, dwEnrollNumber, Name, Password, Privilege, Enabled);
    }

    public bool SetUserInfoEx(int dwMachineNumber, int dwEnrollNumber, int VerifyStyle, ref byte Reserved)
    {
        throw new NotImplementedException();
    }

    public bool SetUserSMS(int dwMachineNumber, int dwEnrollNumber, int SMSID)
    {
        throw new NotImplementedException();
    }

    public bool SetUserTmp(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, ref byte TmpData)
    {
        throw new NotImplementedException();
    }

    public bool SetUserTmpEx(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, int Flag, ref byte TmpData)
    {
        throw new NotImplementedException();
    }


    public bool SetUserTmpStr(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex, string TmpData)
    {
        throw new NotImplementedException();
    }

    public bool SetUserTZs(int dwMachineNumber, int dwEnrollNumber, ref int TZs)
    {
        throw new NotImplementedException();
    }

    public bool SetUserTZStr(int dwMachineNumber, int dwEnrollNumber, string TZs)
    {
        throw new NotImplementedException();
    }

    public bool SetWiegandFmt(int dwMachineNumber, string sWiegandFmt)
    {
        throw new NotImplementedException();
    }

    public bool SetWorkCode(int WorkCodeID, int AWorkCode)
    {
        throw new NotImplementedException();
    }

    public void set_AccTimeZones(int Index, int pVal)
    {
        throw new NotImplementedException();
    }

    public void set_CardNumber(int Index, int pVal)
    {
        throw new NotImplementedException();
    }

    public void set_STR_CardNumber(int Index, string pVal)
    {
        throw new NotImplementedException();
    }

    public bool SplitTemplate(ref byte Template, IntPtr Templates, ref int FingerCount, ref int FingerSize)
    {
        throw new NotImplementedException();
    }

    public bool SSR_ClearWorkCode()
    {
        throw new NotImplementedException();
    }

    public bool SSR_DeleteEnrollData(int dwMachineNumber, string dwEnrollNumber, int dwBackupNumber)
    {
        return _objCZKEM.SSR_DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwBackupNumber);
    }

    public bool SSR_DeleteEnrollDataExt(int dwMachineNumber, string dwEnrollNumber, int dwBackupNumber)
    {
        throw new NotImplementedException();
    }

    public bool SSR_DeleteUserSMS(int dwMachineNumber, string dwEnrollNumber, int SMSID)
    {
        throw new NotImplementedException();
    }

    public bool SSR_DeleteWorkCode(int PIN)
    {
        throw new NotImplementedException();
    }

    public bool SSR_DelUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex)
    {
        throw new NotImplementedException();
    }

    public bool SSR_DelUserTmpExt(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex)
    {
        throw new NotImplementedException();
    }


    public bool SSR_GetDeviceData(int dwMachineNumber, out string Buffer, int BufferSize, string TableName, string FiledNames, string Filter, string Options)
    {
        throw new NotImplementedException();
    }


    public bool SSR_GetGroupTZ(int dwMachineNumber, int GroupNo, ref int Tz1, ref int Tz2, ref int Tz3, ref int VaildHoliday, ref int VerifyStyle)
    {
        throw new NotImplementedException();
    }

    public bool SSR_GetHoliday(int dwMachineNumber, int HolidayID, ref int BeginMonth, ref int BeginDay, ref int EndMonth, ref int EndDay, ref int TimeZoneID)
    {
        throw new NotImplementedException();
    }

    public bool SSR_GetShortkey(int ShortKeyID, ref int ShortKeyFun, ref int StateCode, ref string StateName, ref int AutoChange, ref string AutoChangeTime)
    {
        throw new NotImplementedException();
    }

    public bool SSR_GetSuperLogData(int MachineNumber, out int Number, out string Admin, out string User, out int Manipulation, out string Time, out int Params1, out int Params2, out int Params3)
    {
        throw new NotImplementedException();
    }

    public bool SSR_GetUnLockGroup(int dwMachineNumber, int CombNo, ref int Group1, ref int Group2, ref int Group3, ref int Group4, ref int Group5)
    {
        throw new NotImplementedException();
    }


    public bool SSR_GetUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, out byte TmpData, out int TmpLength)
    {
        throw new NotImplementedException();
    }



    public bool SSR_GetWorkCode(int AWorkCode, out string Name)
    {
        throw new NotImplementedException();
    }

    public bool SSR_OutPutHTMLRep(int dwMachineNumber, string dwEnrollNumber, string AttFile, string UserFile, string DeptFile, string TimeClassFile, string AttruleFile, int BYear, int BMonth, int BDay, int BHour, int BMinute, int BSecond, int EYear, int EMonth, int EDay, int EHour, int EMinute, int ESecond, string TempPath, string OutFileName, int HTMLFlag, int resv1, string resv2)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetDeviceData(int dwMachineNumber, string TableName, string Datas, string Options)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetGroupTZ(int dwMachineNumber, int GroupNo, int Tz1, int Tz2, int Tz3, int VaildHoliday, int VerifyStyle)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetHoliday(int dwMachineNumber, int HolidayID, int BeginMonth, int BeginDay, int EndMonth, int EndDay, int TimeZoneID)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetShortkey(int ShortKeyID, int ShortKeyFun, int StateCode, string StateName, int StateAutoChange, string StateAutoChangeTime)
    {
        throw new NotImplementedException();
    }


    public bool SSR_SetUserSMS(int dwMachineNumber, string dwEnrollNumber, int SMSID)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetUserTmp(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, ref byte TmpData)
    {
        throw new NotImplementedException();
    }


    public bool SSR_SetUnLockGroup(int dwMachineNumber, int CombNo, int Group1, int Group2, int Group3, int Group4, int Group5)
    {
        throw new NotImplementedException();
    }


    public bool SSR_SetUserTmpExt(int dwMachineNumber, int IsDeleted, string dwEnrollNumber, int dwFingerIndex, ref byte TmpData)
    {
        throw new NotImplementedException();
    }

    public bool SSR_SetUserTmpStr(int dwMachineNumber, string dwEnrollNumber, int dwFingerIndex, string TmpData)
    {
        return _objCZKEM.SSR_SetUserTmpStr(dwMachineNumber, dwEnrollNumber, dwFingerIndex, TmpData);
    }

    public bool SSR_SetWorkCode(int AWorkCode, string Name)
    {
        throw new NotImplementedException();
    }


    public bool StartVerify(int UserID, int FingerID)
    {
        throw new NotImplementedException();
    }

    public bool UpdateFile(string FileName)
    {
        throw new NotImplementedException();
    }

    public bool UpdateFirmware(string FirmwareFile)
    {
        throw new NotImplementedException();
    }

    public bool UpdateLogo(int dwMachineNumber, string FileName)
    {
        throw new NotImplementedException();
    }

    public bool UseGroupTimeZone()
    {
        throw new NotImplementedException();
    }

    public bool WriteCard(int dwMachineNumber, int dwEnrollNumber, int dwFingerIndex1, ref byte TmpData1, int dwFingerIndex2, ref byte TmpData2, int dwFingerIndex3, ref byte TmpData3, int dwFingerIndex4, ref byte TmpData4)
    {
        throw new NotImplementedException();
    }

    public bool WriteCustData(int dwMachineNumber, string CustData)
    {
        throw new NotImplementedException();
    }

    public bool WriteLCD(int Row, int Col, string Text)
    {
        throw new NotImplementedException();
    }


    #endregion

}
