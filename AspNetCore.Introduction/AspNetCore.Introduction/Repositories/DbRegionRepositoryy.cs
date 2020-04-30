using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;

namespace AspNetCore.Introduction.Repositories
{
    public class DbRegionRepository : BaseRepository<Regions>, IDbRegionRepository
    {
        public DbRegionRepository(AspNetCoreIntroductionContext context) :
            base(context)
        {
        }
    }
}