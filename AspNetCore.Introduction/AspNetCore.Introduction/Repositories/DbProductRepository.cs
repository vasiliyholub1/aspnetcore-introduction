using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;

namespace AspNetCore.Introduction.Repositories
{
    public class DbProductRepository : BaseRepository<Products>, IDbProductRepository
    {
        public DbProductRepository(AspNetCoreIntroductionContext context) :
            base(context)
        {
        }
    }
}