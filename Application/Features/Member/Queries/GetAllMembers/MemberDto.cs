namespace Application.Features.Member.Queries.GetAllMembers;

public class MemberDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateTime? DateofBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public long AccountId { get; set; }
    public string Ssn { get; set; }
    public string Tag { get; set; }
    public string Vaccine { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}
