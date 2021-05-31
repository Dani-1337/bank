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
    public class CreditContext : DbContext
    {
        [LocalDbSetAttr]
        public DbSet<Credit> _credits { get; set; }

        public CreditContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
        {
        }
    }
}
