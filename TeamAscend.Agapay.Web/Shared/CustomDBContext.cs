using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TeamAscend.Agapay.Web.Shared
{
    public partial class AgapayTestDBContext
    {
        public AgapayTestDBContext()
        {
            //To do here
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();

            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("AgapayTestDBCon"));
            //optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=AgapayTestDB;User ID=sa;Password=pass@123;Trust Server Certificate=True");

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("AgapayTestDBCon");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
