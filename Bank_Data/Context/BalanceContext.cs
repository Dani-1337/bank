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
    public class BalanceContext : DbContext
    {
        public DbSet<Balance> _balances { get; set; }
        public DbSet<List<Credit>> _credits { get; set; }
        public DbSet<List<History>> _histories { get; set; }


        public BalanceContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
        {
        }
    }
}
