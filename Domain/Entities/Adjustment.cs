namespace Domain.Entities;

public partial class Adjustment
{
    public long Id { get; set; }
    public string Name { get; set; }
    public double AdjustmentAmount { get; set; }
    public string Type { get; set; }
    public bool IsStaticAdjustment { get; set; }
    public virtual ICollection<SalaryAdjustmentLog> SalaryAdjustmentLogs { get; set; }
}
