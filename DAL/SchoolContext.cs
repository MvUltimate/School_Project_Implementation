using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    //Identity pour pouvoir faire de l'authentification
    public class SchoolContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Class> Classes { get; set; }
        
        public DbSet<Departement> Departements { get; set; }

        public DbSet<SAP> SAPs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        /*protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SchoolProject");
        }*/
        public void DropDatabase()
        {
            Database.EnsureDeleted();
        }

    }
}
