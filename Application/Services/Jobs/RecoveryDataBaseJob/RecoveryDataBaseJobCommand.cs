using Application.Common.Interfaces;
using Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;
using Domain.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Application.Services.Jobs.RecoveryDataBaseJob;

public class RecoveryDataBaseJobCommand : IRequest<string>
{
}

public class RecoveryDataBaseJobCommandHandler : IRequestHandler<RecoveryDataBaseJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public RecoveryDataBaseJobCommandHandler(ISender mediator, IApplicationDbContext context, IConfiguration configuration)
    {
        _mediator = mediator;
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Handle(RecoveryDataBaseJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var systemConfiguration = await _mediator.Send(new GetSystemConfigurationQuery());
            var backUpPath = systemConfiguration.DefaultFilesPath;
            int lat = Environment.CurrentDirectory.LastIndexOf("\\") + 1;
            string Name = Environment.CurrentDirectory[lat..];
            Name = Name.Replace("-", "").ToUpper();
            var ConnectionString = _configuration.GetConnectionString(Name);
            if (ConnectionString == null)
                Name = "DefaultConnection";
            DateTime DateTime = DateTime.Now;
            ServerConnection serverConnection = new(new SqlConnection(_configuration.GetConnectionString(Name)));
            Server server = new(serverConnection);
            Backup backup = new();
            backup.Action = BackupActionType.Database;
            backup.BackupSetDescription = "AdventureWorks - full backup";
            backup.BackupSetName = "AdventureWorks backup";
            backup.Database = serverConnection.DatabaseName;
            if (!Directory.Exists(backUpPath))
            {
                Directory.CreateDirectory(backUpPath);
            }
            string name = backUpPath + serverConnection.DatabaseName + "-" + DateTime.ToString("dd-MM-yyyy HH-mm-ss") + ".bak";
            BackupDeviceItem deviceItem = new(name, DeviceType.File);
            backup.Devices.Add(deviceItem);
            backup.Incremental = false;
            backup.LogTruncation = BackupTruncateLogType.Truncate;
            backup.SqlBackup(server);

            BackUp backUp = new() { Name = name, BackUpPath = backUpPath, DateTime = DateTime, DataBaseName = serverConnection.DatabaseName };

            _context.BackUp.Add(backUp);

            await _context.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {

        }


        return "";
    }
}