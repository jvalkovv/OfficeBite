using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OfficeBite.Infrastructure.Data
{
    public class OfficeBiteDbContext : IdentityDbContext
    {
        public OfficeBiteDbContext(DbContextOptions<OfficeBiteDbContext> options)
            : base(options)
        {
        }
    }
}
