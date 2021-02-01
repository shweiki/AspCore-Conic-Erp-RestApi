using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Configuration;
using System.Linq;
using Entities;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Security.Claims;

namespace AspCore_Conic_Erp_RestApi.Controllers
{
    [Authorize]
    public class BackupRestoreController : Controller
    {


        private ConicErpContext DB = new ConicErpContext();
        private string DatabaseName = "Conic_Erp";
        private string BackUpPath = "C:\\BackUp\\";

        [Route("BackupRestore/GetBackup")]
        [HttpGet]
        public IActionResult GetBackup()
        {
            var BackUps = DB.BackUps.ToList();
            return Ok(BackUps);
        }
        [Route("BackupRestore/Backup")]
        [HttpGet]
        public IActionResult Backup()
        {

            DateTime DateTime = DateTime.Now;
            ServerConnection serverConnection = new ServerConnection(Environment.MachineName + "\\SQLEXPRESS");
            Server server = new Server(serverConnection);
            Backup backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.BackupSetDescription = "AdventureWorks - full backup";
            backup.BackupSetName = "AdventureWorks backup";
            backup.Database = DatabaseName;
            if (!Directory.Exists(BackUpPath))
            {
                Directory.CreateDirectory(BackUpPath);
            }
            string name = BackUpPath + DB.CompanyInfos.Where(x => x.Id == 1).SingleOrDefault().Name + "-" + DateTime + ".bak";
            BackupDeviceItem deviceItem = new BackupDeviceItem(name, DeviceType.File);
            backup.Devices.Add(deviceItem);
            backup.Incremental = false;
            backup.LogTruncation = BackupTruncateLogType.Truncate;
            backup.SqlBackup(server);
            BackUp backup1 = new BackUp { Name = name, BackUpPath = BackUpPath, DateTime = DateTime, UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value , DataBaseName = DatabaseName };
            DB.BackUps.Add(backup1);
            DB.SaveChanges();
            return Ok(true);
        }


        [Route("BackupRestore/Restore")]
        [HttpGet]
        public IActionResult Restore(int backUpId)
        {

            var Backup = DB.BackUps.Where(x => x.Id == backUpId).SingleOrDefault();
                ServerConnection serverConnection = new ServerConnection(Environment.MachineName + "\\SQLEXPRESS");
                Server dbServer = new Server(serverConnection);

                Restore _Restore = new Restore()
                {
                    Database = DatabaseName,
                    Action = RestoreActionType.Database,
                    ReplaceDatabase = true,
                    NoRecovery = false
                };
            if (!System.IO.File.Exists(Backup.Name))
            {
             return Ok("File is not Exsit");

            }
            else
            {
                BackupDeviceItem source = new BackupDeviceItem(Backup.Name, DeviceType.File);
                _Restore.Devices.Add(source);
                CloseAllConnection("USE master alter database " + DatabaseName + " set offline with rollback immediate");

                //   _Restore.PercentComplete += DB_Restore_PersentComplete;
                //      _Restore.Complete += DB_Restore_Complete;
                _Restore.SqlRestore(dbServer);
                CloseAllConnection("USE master alter database " + DatabaseName + " set online");

                return Ok(true);
            }




        }


        // function to close any opend database connections
        private void CloseAllConnection(string commandSql)
        {


         
            // split script on GO command
            IEnumerable<string> commandStrings = Regex.Split(commandSql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);


            SqlConnection _connection = new SqlConnection("Server="+ Environment.MachineName + "\\SQLEXPRESS; Database="+ DatabaseName + ";Trusted_Connection=True;MultipleActiveResultSets=true");

            _connection.Open();
            foreach (string commandString in commandStrings)
            {
                if (commandString.Trim() != "")
                {
                    using (var command = new SqlCommand(commandString, _connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            _connection.Close();
            _connection.Dispose();
        }






    }
}
