using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PostService.Entities.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Entities
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
        }

        public DbSet<Post> Post{ get; set; }
        public DbSet<TypeOfPost> TypeOfPost { get; set; }

    }
}
