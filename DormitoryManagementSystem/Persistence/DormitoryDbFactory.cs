using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DormitoryDbFactory : DesignTimeDbContextFactoryBase<DormitoryDbContext>
    {
        protected override DormitoryDbContext CreateNewInstance(DbContextOptions<DormitoryDbContext> options)
        {
            return new DormitoryDbContext(options);
        }
    }
}
