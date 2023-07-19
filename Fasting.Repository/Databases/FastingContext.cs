using Fasting.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Fasting.Repository.Databases
{
    public class FastingContext : BaseDbContext
    {
        public FastingContext(DbContextOptions<FastingContext> options) : base(options) { }


    }
}
