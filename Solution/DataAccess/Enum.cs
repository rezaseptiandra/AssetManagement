using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum Dialect
    {
        SQLServer = 0,
        PostgreSQL = 1,
        SQLite = 2,
        MySQL = 3
    }
    public enum OrderType
    {
        ASC = 0,
        DESC = 1
    }
}
