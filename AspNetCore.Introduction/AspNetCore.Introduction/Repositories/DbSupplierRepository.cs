using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;

namespace AspNetCore.Introduction.Repositories
{
    public class DbSupplierRepository : BaseRepository<Suppliers>, IDbSupplierRepository
    {
        public DbSupplierRepository(AspNetCoreIntroductionContext context) :
            base(context)
        {
        }
    }
}