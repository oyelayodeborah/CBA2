namespace MyCBA.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MyCBA.Core.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MyCBA.Core.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyCBA.Core.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var getBranch = context.Branches.ToList().Count();
            var getRole = context.Roles.ToList().Count();
            if (getBranch == 0)
            {
                context.Branches.AddOrUpdate(x => x.id, new Branch() { name = "Yaba" });

            }
            if (getRole == 0)
            {
                context.Roles.AddOrUpdate(x => x.id, new Role() { name = "Admin" });
            }
        }
    }
}
