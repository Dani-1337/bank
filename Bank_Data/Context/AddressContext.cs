using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_Data.Attributes;

namespace Bank_Data.Context
{
    public class AddressContext : DbContext
    {
        [LocalDbSetAttr]
        public DbSet<Address> _addresses { get; set; }

        public AddressContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
        {
        }
    }
}
