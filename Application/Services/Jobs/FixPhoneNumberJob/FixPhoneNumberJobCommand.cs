using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Jobs.FixPhoneNumberJob;

public class FixPhoneNumberJobCommand : IRequest<string>
{
}

public class FixPhoneNumberJobCommandHandler : IRequestHandler<FixPhoneNumberJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;

    public FixPhoneNumberJobCommandHandler(ISender mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<string> Handle(FixPhoneNumberJobCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var items = await _context.Member.Where(i => i.PhoneNumber1 != null).ToListAsync();
            foreach (var item in items)
            {
                var validNumber = item.PhoneNumber1.Trim().Replace(" ", "");

                switch (validNumber.Length)
                {
                    case 9:
                        item.PhoneNumber1 = "+962" + validNumber;
                        break;
                    case 10:
                        item.PhoneNumber1 = "+962" + validNumber.Substring(1);
                        break;
                    case 12:
                        item.PhoneNumber1 = "+" + validNumber;
                        break;
                    default:
                        break;
                }

                if (item.PhoneNumber2 != null)
                {
                    validNumber = item.PhoneNumber2.Trim().Replace(" ", "");

                    switch (validNumber.Length)
                    {
                        case 9:
                            item.PhoneNumber1 = "+962" + validNumber;
                            break;
                        case 10:
                            item.PhoneNumber1 = "+962" + validNumber.Substring(1);
                            break;
                        case 12:
                            item.PhoneNumber1 = "+" + validNumber;
                            break;
                        default:
                            break;
                    }
                }


            }


            await _context.SaveChangesAsync(new CancellationToken(), null);


            return "";

        }
        catch (Exception ex)
        {
            return "";
        }

    }
}