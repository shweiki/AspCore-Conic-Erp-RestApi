using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.MembershipMovement.Queries.GetMembershipMovementList;

public class GetMembershipMovementListQuery : IRequest<MembershipMovementListDto>
{
    public string? CreatedBy { get; set; }
    public int Start { get; set; }
    public int Limit { get; set; }
    public string SortBy { get; set; } = "";
    public bool IsDesc { get; set; }
}

public class GetMembershipMovementListQueryHandler : IRequestHandler<GetMembershipMovementListQuery, MembershipMovementListDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;


    public GetMembershipMovementListQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    public async Task<MembershipMovementListDto> Handle(GetMembershipMovementListQuery request, CancellationToken cancellationToken)
    {
        //ArgumentNullException.ThrowIfNull(_currentUserService.UserUniqueId, nameof(_currentUserService.UserUniqueId));
        var userId = await _identityService.GetUserIdAsync(_currentUserService.Username);

        var itemsQuery = _context.MembershipMovement.Include(x => x.Membership).AsQueryable();


        int totalCountBeforeFilter = await itemsQuery.CountAsync();

        if (!string.IsNullOrWhiteSpace(request.CreatedBy))
        {
            request.CreatedBy = request.CreatedBy.Trim().ToLower();
            itemsQuery = itemsQuery.Where(x => x.CreatedBy.ToLower().Contains(request.CreatedBy));
        }
        request.SortBy = request.SortBy?.ToLower();
        var itemsOrdered = request.IsDesc ? request.SortBy switch
        {
            "status" => itemsQuery.OrderByDescending(x => x.Status),
            "creationDate" => itemsQuery.OrderByDescending(x => x.Created),
            "createdBy" => itemsQuery.OrderByDescending(x => x.CreatedBy),
            _ => itemsQuery.OrderByDescending(x => x.Created),
        } : request.SortBy switch
        {
            "status" => itemsQuery.OrderBy(x => x.Status),
            "creationDate" => itemsQuery.OrderBy(x => x.Created),
            "createdBy" => itemsQuery.OrderBy(x => x.CreatedBy),
            _ => itemsQuery.OrderBy(x => x.Created),
        };

        var itemsTaken = itemsOrdered.Skip(request.Start).Take(request.Limit);
        var items = await itemsTaken
            .ProjectTo<MembershipMovementDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        int totalCount = await itemsQuery
            .CountAsync(cancellationToken);


        return new MembershipMovementListDto
        {
            Items = items,
            TotalCount = totalCount,
            TotalCountBeforeFilter = totalCountBeforeFilter
        };


    }
}
