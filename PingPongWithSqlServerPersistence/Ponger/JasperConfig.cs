using Jasper;
using Jasper.Persistence.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Ponger
{
    internal class JasperConfig : JasperOptions
    {
        public JasperConfig()
        {
            // Listen for incoming messages using the 
            // lightweight TCP transport at port 
            // 2222
            Endpoints
                .ListenAtPort(2222)
                
                // Use message persistence here
                .Durably();
        }
        
        public override void Configure(IHostEnvironment hosting, IConfiguration config)
        {
            if (hosting.IsDevelopment())
            {
                // In development mode, we're just going to have the message persistence
                // schema objects dropped and rebuilt on app startup so you're
                // always starting from a clean slate
                Advanced.StorageProvisioning = StorageProvisioning.Rebuild;
            }

            // Just the normal work to get the connection string out of 
            // application configuration
            var connectionString = config.GetConnectionString("sqlserver");

            // Setting up Sql Server-backed message persistence
            Extensions.PersistMessagesWithSqlServer(connectionString, "pongs");

        }
    }

}