using Bank_Data.Context;
using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Business.Business
{
	public class UserBusiness : BaseBusiness<User, UserContext>
	{
		public override List<User> GetAll()
		{
			using (UserContext context = new UserContext())
			{
				return context._users.Include("Address").ToList();
			}
		}

		public override User GetId(int id)
		{
			using (UserContext context = new UserContext())
			{
				return context._users.Include("Address").Single(e => e.Id == id);
			}
		}

		public override List<User> GetFind(Func<User, bool> filter)
		{
			using (UserContext context = new UserContext())
			{
				try
				{
					return context._users.Include("Address").Where(filter).ToList();
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		public override void Add(User obj)
		{
			using (UserContext context = new UserContext())
			{
				context._addresses.Add(obj.Address);
				context._users.Add(obj);
				context.SaveChanges();
			}
		}

		public override void Update(User obj)
		{
			using (UserContext context = new UserContext())
			{
				User target = context._users.Include("Address").Single(e => e.Id == obj.Id);
				if (target != null)
				{
					context.Entry(target).CurrentValues.SetValues(obj);
				}

				Address target2 = context._addresses.Single(e => e.Id == obj.Address.Id);
				if (target2 != null)
				{
					context.Entry(target2).CurrentValues.SetValues(obj);
				}

				context.SaveChanges();
			}
		}

		public override void DeleteId(int id)
		{
			using (UserContext context = new UserContext())
			{
				User target = context._users.Include("Address").Single(e => e.Id == id);
				if (target != null)
				{
					Address target2 = context._addresses.Single(e => e.Id == target.Address.Id);
					if (target2 != null)
					{
						context._addresses.Remove(target2);
					}

					context._users.Remove(target);
					context.SaveChanges();
				}
			}
		}

		public override void DeleteBulk(Func<User, bool> filter)
		{
			using (UserContext context = new UserContext())
			{
				List<User> targets = GetFind(filter);
				if (targets != null)
				{
					foreach (User target in targets)
					{
						DeleteId(target.Id);
					}

					context.SaveChanges();
				}
			}
		}
	}
}
