using Application.Common.Helper;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SignTEC.Application.Features.SystemConfiguration.Queries.GetSystemConfiguration;
using System.Text.RegularExpressions;

namespace Application.Services.Systems.FixBase64ToPathWithLoadedJob;

public class FixBase64ToPathWithLoadedJobCommand : IRequest<string>
{
}

public class FixBase64ToPathWithLoadedJobCommandHandler : IRequestHandler<FixBase64ToPathWithLoadedJobCommand, string>
{
    private readonly ISender _mediator;
    private readonly IApplicationDbContext _context;

    public FixBase64ToPathWithLoadedJobCommandHandler(ISender mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<string> Handle(FixBase64ToPathWithLoadedJobCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var systemConfiguration = await _mediator.Send(new GetSystemConfigurationQuery());


            var files = await _context.FileData.Where(x => x.FileType == "image" && x.FilePath == null)
            .Select(x => new
            {
                x.Id,
                x.Type,
                x.FileType,
                x.TableName,
                x.Fktable,
            })
            .ToListAsync();
            foreach (var file in files)
            {

                var fileData = await _context.FileData.SingleOrDefaultAsync(i => i.Id == file.Id);

                if (string.IsNullOrWhiteSpace(fileData.Type))
                {
                    fileData.Type = "Picture";
                }
                string base64String = Regex.Replace(fileData.File, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

                string path = ImageHelper.LoadJpeg(base64String, systemConfiguration.DefaultFilesPath, fileData.Type, fileData.TableName, fileData.Id);

                if (!string.IsNullOrWhiteSpace(path))
                {
                    fileData.FilePath = path;
                    fileData.File = "";
                    await _context.SaveChangesAsync();
                }

            }
            return "";

        }
        catch (Exception ex)
        {
            return "";
        }

    }
}