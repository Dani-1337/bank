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
    public class UserContext : DbContext
    {
		[LocalDbSetAttr]
		public DbSet<User> _users { get; set; }
		public DbSet<Address> _addresses { get; set; }

		public UserContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
		{
		}
	}
}
