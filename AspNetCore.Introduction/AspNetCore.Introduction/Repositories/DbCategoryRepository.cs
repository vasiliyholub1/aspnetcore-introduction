using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;

namespace AspNetCore.Introduction.Repositories
{
    public class DbCategoryRepository : BaseRepository<Categories>, IDbCategoryRepository
    {
        public DbCategoryRepository(AspNetCoreIntroductionContext context) :
            base(context)
        {
        }
    }
}