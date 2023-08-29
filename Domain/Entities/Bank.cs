#nullable disable


namespace Domain.Entities;

public partial class Bank
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long? AccountNumber { get; set; }
    public string AccountType { get; set; }
    public string Currency { get; set; }
    public string BranchName { get; set; }
    public long? Iban { get; set; }
    public long? AccountId { get; set; }
    public int Status { get; set; }
    public bool IsPrime { get; set; }
    public string Description { get; set; }

    public virtual TreeAccount Account { get; set; }
}
