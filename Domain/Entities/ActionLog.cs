#nullable disable

namespace Domain.Entities;

public partial class ActionLog
{
    public long Id { get; set; }
    public int OprationId { get; set; }
    public DateTime PostingDateTime { get; set; }
    public string Description { get; set; }
    public string UserId { get; set; }
    public string TableName { get; set; }
    public string Fktable { get; set; }
    public virtual Oprationsy Opration { get; set; }
}
