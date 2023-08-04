using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Jobs.GetMemberLogFromZktDataBaseJob;

public class GetMemberLogFromZktDataBaseJobCommand : IRequest<string>
{
}

public class GetMemberLogFromZktDataBaseJobCommandHandler : IRequestHandler<GetMemberLogFromZktDataBaseJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public GetMemberLogFromZktDataBaseJobCommandHandler(ISender mediator, IApplicationDbContext context, IConfiguration configuration)
    {
        _mediator = mediator;
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Handle(GetMemberLogFromZktDataBaseJobCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("ZkbiotimeConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) { return ""; }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String sql = "SELECT  L.punch_time, L.emp_code , T.ip_address  ,L.id FROM zkbiotime.dbo.iclock_transaction as L" +
                    " INNER JOIN  [zkbiotime].[dbo].[iclock_terminal] as T ON L.terminal_id= T.id" +
                    " where L.reserved Is null";

                using (SqlCommand command = new(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader[1].ToString() == "0") continue;

                                DateTime action_time = DateTime.Parse(reader[0].ToString());
                                action_time = new DateTime(action_time.Year, action_time.Month, action_time.Day, action_time.Hour, action_time.Minute, 0);

                                string objectId = reader[1].ToString();
                                string Ip = reader[2].ToString();
                                await RegisterLog(objectId, action_time, Ip);

                                UpdateFromZkBioReserved(reader[3].ToString());

                            }
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return "";

        }
        finally
        {

        }

        return "";
    }

    public bool UpdateFromZkBioReserved(string id)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("ZkbiotimeConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) { return false; }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String sql = "update [zkbiotime].[dbo].[iclock_transaction] set reserved = 'true' where id = " + id + "";

                using (SqlCommand command = new(sql, connection))
                {
                    command.ExecuteNonQuery();

                }
                connection.Close();
                return true;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public async Task<bool> RegisterLog(string Id, DateTime datetime, string Ip)
    {
        long ID = Convert.ToInt32(Id);
        string TableName = "";
        var member = _context.Member.Where(m => m.Id == ID).SingleOrDefault();
        var Employee = _context.Employee.Where(m => m.Id == ID).SingleOrDefault();
        if (member != null) TableName = "Member";
        if (Employee != null) TableName = "Employee";

        var isLogSaveIt = _context.DeviceLog.Where(l => l.Fk == Id && l.TableName == TableName).ToList();
        isLogSaveIt = _context.DeviceLog.Where(Ld => Ld.DateTime == datetime).ToList();
        var Device = _context.Device.Where(x => x.Ip == Ip).SingleOrDefault();
        if (isLogSaveIt.Count <= 0 && Device != null)
        {
            var Log = new DeviceLog
            {
                Type = "In",
                DateTime = datetime,
                DeviceId = Device.Id,
                Status = 0,
                Description = "Event Log",
                TableName = TableName,
                Fk = Id.ToString(),
            };
            _context.DeviceLog.Add(Log);
            await _context.SaveChangesAsync();

            return true;
        }
        else
        {
            return false;
        }
    }

}
