using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_Data.Context;
using Bank_Data.Model;

namespace Bank_Business
{
	class AddressBusiness
	{
		AddressContext context;

		/// <summary>
		/// Get all addresses in the database
		/// </summary>
		/// <returns>All the addresses in the database</returns>
		public List<Address> GetAll()
		{
			using (context = new AddressContext())
			{
				return context._addresses.ToList();
			}
		}

		/// <summary>
		/// Get an address with the specified <b>id</b>
		/// </summary>
		/// <param name="id">The id to find</param>
		/// <returns>Address with <b>id</b> as its id</returns>
		public Address GetId(int id)
		{
			using (context = new AddressContext())
			{
				return context._addresses.Single(e => e.Id == id);
			}
		}

		/// <summary>
		/// Find all addresses with the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use <b><i>(lambda/delegate can be used)</i></b></param>
		/// <returns>Address matching the <b>filter</b><br/> Null if not found or the <b>filter</b> throws ArgumentNullException</returns>
		public List<Address> GetFind(Func<Address, bool> filter)
		{
			using (context = new AddressContext())
			{
				try
				{
					return context._addresses.Where(filter).ToList();
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Adds <b>address</b> to the database
		/// </summary>
		/// <param name="address">The address to add</param>
		public void Add(Address address)
		{
			using (context = new AddressContext())
			{
				context._addresses.Add(address);
				context.SaveChanges();
			}
		}

		/// <summary>
		/// Update <b>address</b> and push changes to database
		/// </summary>
		/// <param name="address">Address to update, <b><i>id</i></b> is how you specify which address to update</param>
		public void Update(Address address)
		{
			using (context = new AddressContext())
			{
				Address target = context._addresses.Single(e => e.Id == address.Id);
				if (target != null)
				{
					context.Entry(target).CurrentValues.SetValues(address);
					context.SaveChanges(); // better to cache it and update database after a while, but whatever
				}
			}
		}

		/// <summary>
		/// Deletes address with the specified <b>id</b>
		/// </summary>
		/// <param name="id">Id of the address to delete</param>
		public void DeleteId(int id)
		{
			using (context = new AddressContext())
			{
				Address target = context._addresses.Single(e => e.Id == id);
				if (target != null)
				{
					context._addresses.Remove(target);
					context.SaveChanges();
				}
			}
		}

		/// <summary>
		/// Deletes addresses in bulk that apply to the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use</param>
		public void DeleteBulk(Func<Address, bool> filter)
		{
			using (context = new AddressContext())
			{
				List<Address> targets = GetFind(filter);
				if (targets != null)
				{
					foreach (Address target in targets)
					{
						context._addresses.Remove(target);
					}

					context.SaveChanges();
				}
			}
		}
	}
}
