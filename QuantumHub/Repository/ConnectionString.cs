using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumHub.Repository
{
    public static class ConnectionString
    {
        public static string getConnectionString()
        {
#if DEBUG
            return @"server=localhost;userid=root;password=Siberia$111;database=quantumencrypt";
#else
            string connectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");

            string[] options = connectionString.Split(";");
            string database = options[0].Split("=")[1]; ;
            string serverport = options[1].Split("=")[1];
            string server = serverport.Split(":")[0];
            string port = serverport.Split(":")[1];
            string user = options[2].Split("=")[1];
            string password = options[3].Split("=")[1]; ;

            connectionString = $"server={server};port={port};database={database};user={user};password={password};";

            return connectionString;
#endif
        }
    }
}
