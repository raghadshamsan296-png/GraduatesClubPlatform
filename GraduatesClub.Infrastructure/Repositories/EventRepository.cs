using GraduatesClub.Domain.Entities;
using GraduatesClub.Domain.Interfaces;
using GraduatesClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GraduatesClub.Infrastructure.Repositories;

public sealed class EventRepository : GenericRepository<ClubEvent>, IEventRepository
{
    public EventRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IReadOnlyList<ClubEvent>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet
            .AsNoTracking()
            .Include(x => x.Alumni)
            .OrderBy(x => x.EventDate)
            .ToListAsync(cancellationToken);

    public Task<ClubEvent?> GetWithAlumniByIdAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet
            .AsNoTracking()
            .Include(x => x.Alumni)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
