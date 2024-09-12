using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace clsKarateDataAccesse
{
    public static class clsAccesseSetting
    {
        public static string? ConnectionString;
        public static void Initialize(IConfiguration configuration)
        {
            ConnectionString = configuration.GetSection("ConnectionString").Value;
        }

    }
}
