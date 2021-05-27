﻿using Bank_Data.Model;
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
		public DbSet<User> _users { get; set; }

		public UserContext() : base("Data Source=.;Initial Catalog=Bank;Integrated Security=True")
		{
		}
	}
}
