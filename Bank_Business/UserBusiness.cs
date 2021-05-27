using Bank_Data.Context;
using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Business
{
    public class UserBusiness
    {
		UserContext context;

		/// <summary>
		/// Get all addresses in the database
		/// </summary>
		/// <returns>All the addresses in the database</returns>
		public List<User> GetAll()
		{
			using (context = new UserContext())
			{
				return context._users.ToList();
			}
		}

		/// <summary>
		/// Get an user with the specified <b>id</b>
		/// </summary>
		/// <param name="id">The id to find</param>
		/// <returns>User with <b>id</b> as its id</returns>
		public User GetId(int id)
		{
			using (context = new UserContext())
			{
				return context._users.Single(e => e.Id == id);
			}
		}

		/// <summary>
		/// Find all addresses with the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use <b><i>(lambda/delegate can be used)</i></b></param>
		/// <returns>User matching the <b>filter</b><br/> Null if not found or the <b>filter</b> throws ArgumentNullException</returns>
		public List<User> GetFind(Func<User, bool> filter)
		{
			using (context = new UserContext())
			{
				try
				{
					return context._users.Where(filter).ToList();
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Adds <b>user</b> to the database
		/// </summary>
		/// <param name="user">The user to add</param>
		public void Add(User user)
		{
			using (context = new UserContext())
			{
				context._users.Add(user);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Update <b>user</b> and push changes to database
		/// </summary>
		/// <param name="user">User to update, <b><i>id</i></b> is how you specify which user to update</param>
		public void Update(User user)
		{
			using (context = new UserContext())
			{
				User target = context._users.Single(e => e.Id == user.Id);
				if (target != null)
				{
					context.Entry(target).CurrentValues.SetValues(user);
					context.SaveChanges(); // better to cache it and update database after a while, but whatever
				}
			}
		}

		/// <summary>
		/// Deletes user with the specified <b>id</b>
		/// </summary>
		/// <param name="id">Id of the user to delete</param>
		public void DeleteId(int id)
		{
			using (context = new UserContext())
			{
				User target = context._users.Single(e => e.Id == id);
				if (target != null)
				{
					context._users.Remove(target);
					context.SaveChanges();
				}
			}
		}

		/// <summary>
		/// Deletes addresses in bulk that apply to the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use</param>
		public void DeleteBulk(Func<User, bool> filter)
		{
			using (context = new UserContext())
			{
				List<User> targets = GetFind(filter);
				if (targets != null)
				{
					foreach (User target in targets)
					{
						context._users.Remove(target);
					}

					context.SaveChanges();
				}
			}
		}
	}
}
