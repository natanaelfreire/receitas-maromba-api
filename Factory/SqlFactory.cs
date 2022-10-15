using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ReceitasMarombaApi.Factory
{
    public class SqlFactory
    {
        public IDbConnection SqlConnection()
        {
            return new SqlConnection("Server=localhost; Database=BANCO_RECEITAS_MAROMBA; User=sa; Password=Sample123$; Trusted_Connection=False; TrustServerCertificate=True;");
        }
    }
}