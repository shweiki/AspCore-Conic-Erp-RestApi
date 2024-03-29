﻿using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Text.RegularExpressions;

namespace RestApi.Controllers;

[Authorize]
public class BackupRestoreController : Controller
{
    private readonly IApplicationDbContext DB;
    private readonly IConfiguration Configuration;

    public BackupRestoreController(IConfiguration configuration, IApplicationDbContext dbcontext)
    {
        Configuration = configuration;
        DB = dbcontext;

    }

    [Route("BackupRestore/GetBackup")]
    [HttpGet]
    public IActionResult GetBackup()
    {
        var BackUps = DB.BackUp.ToList();
        return Ok(BackUps);
    }

    [Route("BackupRestore/Restore")]
    [HttpGet]
    public IActionResult Restore(string DirectoryBak)
    {
        int lat = Environment.CurrentDirectory.LastIndexOf("\\") + 1;
        string Name = Environment.CurrentDirectory[lat..];
        Name = Name.Replace("-", "").ToUpper();
        var ConnectionString = Configuration.GetConnectionString(Name);
        if (ConnectionString == null)
            Name = "Default";
        ServerConnection serverConnection = new(new SqlConnection(Configuration.GetConnectionString(Name)));
        Server dbServer = new(serverConnection);

        Restore _Restore = new()
        {
            Database = serverConnection.DatabaseName,
            Action = RestoreActionType.Database,
            ReplaceDatabase = true,
            NoRecovery = false
        };
        if (!System.IO.File.Exists(DirectoryBak))
        {
            return Ok("File is not Exsit");

        }
        else
        {
            BackupDeviceItem source = new(DirectoryBak, DeviceType.File);
            _Restore.Devices.Add(source);
            CloseAllConnection("USE master alter database " + serverConnection.DatabaseName + " set offline with rollback immediate");

            //   _Restore.PercentComplete += DB_Restore_PersentComplete;
            //      _Restore.Complete += DB_Restore_Complete;
            _Restore.SqlRestore(dbServer);
            CloseAllConnection("USE master alter database " + serverConnection.DatabaseName + " set online");

            return Ok(true);
        }




    }

    // function to close any opend database connections
    private void CloseAllConnection(string commandSql)
    {
        int lat = Environment.CurrentDirectory.LastIndexOf("\\") + 1;
        string Name = Environment.CurrentDirectory[lat..];
        Name = Name.Replace("-", "").ToUpper();

        // split script on GO command
        IEnumerable<string> commandStrings = Regex.Split(commandSql, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        SqlConnection _connection = new(Configuration.GetConnectionString(Name));
        _connection.Open();
        foreach (string commandString in commandStrings)
        {
            if (commandString.Trim() != "")
            {
                using var command = new SqlCommand(commandString, _connection);
                command.ExecuteNonQuery();
            }
        }
        _connection.Close();
        _connection.Dispose();
    }


}
