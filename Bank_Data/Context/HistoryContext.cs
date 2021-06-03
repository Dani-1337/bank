using Bank_Data.Attributes;
using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Data.Context
{
    public class HistoryContext : DbContext
    {
        [LocalDbSetAttr]
        public DbSet<List<History>> _histories { get; set; }

        public HistoryContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
        {
        }
    }
}
