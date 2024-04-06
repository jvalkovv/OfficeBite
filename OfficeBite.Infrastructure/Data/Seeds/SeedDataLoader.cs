using Microsoft.EntityFrameworkCore;
using OfficeBite.Infrastructure.Data.Seeds.Interfaces;

namespace OfficeBite.Infrastructure.Data.Seeds
{
    public class SeedDataLoader : ISeedDataLoader
    {
        private readonly OfficeBiteDbContext dbContext;

        public SeedDataLoader(OfficeBiteDbContext dbContext)
        {
            dbContext = dbContext;
        }


        public void InitializeSeedData()
        {
            SeedDataConfiguration.Initialize((IServiceProvider)dbContext);
        }
    }
}
