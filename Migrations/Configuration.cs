using System.Reflection;
using Appa_MVC.Models;

namespace Appa_MVC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Inspinia_MVC5_SeedProject.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var baseDir = Path.GetDirectoryName(path) + "\\Migrations\\VesselsView.sql";

            var baseDir2 = Path.GetDirectoryName(path) + "\\Migrations\\SuppliersView.sql";

            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir));
            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir2));
        }
    }
}
