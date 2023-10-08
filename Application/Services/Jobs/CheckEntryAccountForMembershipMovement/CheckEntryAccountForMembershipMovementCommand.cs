using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;

namespace Application.Services.Jobs.CheckEntryAccountForMembershipMovement;

public class CheckEntryAccountForMembershipMovementCommand : IRequest<string>
{
}

public class CheckEntryAccountForMembershipMovementCommandHandler : IRequestHandler<CheckEntryAccountForMembershipMovementCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CheckEntryAccountForMembershipMovementCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CheckEntryAccountForMembershipMovementCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var MembershipMovements = _context.MembershipMovement.Where(x => _context.EntryMovement.Where(e => e.Fktable == x.Id && e.TableName == "MembershipMovement").SingleOrDefault() == null).ToList();
            foreach (var membershipMovement in MembershipMovements)
            {
                var entry = _context.EntryMovement.Where(x => x.TableName == "MembershipMovement" && x.Fktable == membershipMovement.Id).ToList();

                if (entry.Count() > 0) continue;

                List<string> keys = new List<string> { "MembershipMovement" };

                var suggestEntrys = _context.EntryMovement.Where(x => keys.Contains(x.Entry.Type) && x.AccountId == membershipMovement.Member.AccountId && x.TableName == null && x.Fktable == 0).ToList();
                if (suggestEntrys.Count() <= 0)
                {
                    await AddEntryAccounting(membershipMovement);
                    continue;
                }
                var found = false;
                foreach (var suggestEntry in suggestEntrys)
                {
                    var membershipMovementIds = NumbersFromString(suggestEntry.Description);
                    if (membershipMovementIds.Contains(membershipMovement.Id))
                    {
                        var bothEntry = _context.EntryMovement.Where(x => x.EntryId == suggestEntry.EntryId).ToList();
                        bothEntry.ForEach(x =>
                        {
                            x.TableName = "MembershipMovement";
                            x.Fktable = membershipMovement.Id;
                        });

                        await _context.SaveChangesAsync(new CancellationToken(), null);
                        found = true;
                        continue;
                    }
                }
                if (!found)
                    await AddEntryAccounting(membershipMovement);

            }

            return "";

        }

        catch (Exception ex)
        {
            return "";
        }

    }
    public async Task AddEntryAccounting(MembershipMovement membershipMovement)
    {
        try
        {
            _context.EntryAccounting.Add(new EntryAccounting
            {
                Description = "",
                FakeDate = membershipMovement.StartDate,
                Status = 0,
                Type = "MembershipMovement",
                EntryMovements = new List<EntryMovement>() { new EntryMovement {
                        TableName = "MembershipMovement",
                        Fktable =membershipMovement.Id,
                        AccountId =_context.Member.Where(x=>x.Id == membershipMovement.MemberId).SingleOrDefault().AccountId,// membershipMovement.Member.AccountId,
                        Description ="اشتراك رقم " + membershipMovement.Id ,
                        Credit =membershipMovement.TotalAmmount,
                        Debit =0,
                      },
                      new EntryMovement {
                        TableName = "MembershipMovement",
                        Fktable =membershipMovement.Id,
                        AccountId = 3,
                        Description ="اشتراك رقم " + membershipMovement.Id ,
                        Credit =0,
                        Debit =membershipMovement.TotalAmmount,
                      },
                    }
            });

            await _context.SaveChangesAsync(new CancellationToken(), null);

        }
        catch
        {

        }
    }
    public List<long> NumbersFromString(string text)
    {

        // Define a regular expression pattern to match numbers (including decimals)
        string pattern = @"[-+]?\d+";

        // Create a regular expression object
        Regex regex = new Regex(pattern);

        // Use the regular expression to find all matches in the text
        MatchCollection matches = regex.Matches(text);
        var matchLong = new List<long>();

        // Loop through the matches and extract the numbers
        foreach (Match match in matches)
        {
            // Parse the matched text to a numeric type (e.g., double)
            if (long.TryParse(match.Value, out long number))
            {
                matchLong.Add(number);
            }
        }
        return matchLong;
    }
}