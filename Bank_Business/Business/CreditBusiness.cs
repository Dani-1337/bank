using Bank_Data.Context;
using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Business.Business
{
	public class CreditBusiness : BaseBusiness<Credit, CreditContext>
	{
		/// <summary>
		/// Same as <b>GetAll</b> but it returns all credits as a group of credits, for example: first element will have <i>all the credits</i> that the first user has, second element will have all of second's user credits, etc...
		/// </summary>
		/// <returns>All credits of every user, grouped</returns>
		public List<List<Credit>> GetAllGrouped()
		{
			List<List<Credit>> credits = new List<List<Credit>>();
			Dictionary<Credit, int> credits_sorted = new Dictionary<Credit, int>();
			List<Credit> allCredits = GetAll();

			foreach (Credit credit in allCredits) // Add every credit in a "sorted" way
			{
				credits_sorted.Add(credit, credit.ParentId);
			}

			int last_id = int.MinValue;

			foreach (var credit in credits_sorted)
			{
				if (last_id != credit.Key.ParentId)
				{
					if (last_id != int.MinValue)
						credits.Add(allCredits); // allCredits is reused, ignore the name
					last_id = credit.Value;

					// Reusing object's memory
					allCredits = new List<Credit>();
					allCredits.Add(credit.Key);
				}
				else
				{
					allCredits.Add(credit.Key);
				}
			}

			return credits;
		}

		public override List<Credit> GetAll()
		{
			using (CreditContext context = new CreditContext())
			{
				return context._credits.Include("Histories").ToList();
			}
		}

		/// <summary>
		/// Same as <b>GetId</b> but it returns all credits that the <b>parent_id</b> (which is the id of the user) has.
		/// </summary>
		/// <param name="parent_id">Id of the parent, which is the user</param>
		/// <returns>All of the credits a user has</returns>
		public List<Credit> GetIdGrouped(int parent_id)
		{
			Dictionary<Credit, int> credits_sorted = new Dictionary<Credit, int>();
			List<Credit> allCredits = GetAll();

			foreach (Credit credit in allCredits) // Add every credit in a "sorted" way
			{
				credits_sorted.Add(credit, credit.ParentId);
			}

			return credits_sorted.Where(e => e.Value == parent_id).Select(e => e.Key).ToList();
		}

		public override Credit GetId(int id)
		{
			using (CreditContext context = new CreditContext())
			{
				return context._credits.Include("Histories").Single(e => e.Id == id);
			}
		}

		/// <summary>
		/// Same as <b>GetFind</b> but it returns all credits that match <b>filter</b> as a group of credits, for example: first element will have <i>all the credits</i> that the first user has, second element will have all of second's user credits, etc...
		/// </summary>
		/// <param name="filter">FIlter to apply to each collection of credits, per user</param>
		/// <returns>List of all the credits of each user, with respect to the <b>filter</b></returns>
		public List<List<Credit>> GetFindGrouped(Func<List<Credit>, bool> filter)
		{
			try
			{
				List<List<Credit>> credits = new List<List<Credit>>();
				Dictionary<Credit, int> credits_sorted = new Dictionary<Credit, int>();
				List<Credit> allCredits = GetAll();

				foreach (Credit credit in allCredits) // Add every credit in a "sorted" way
				{
					credits_sorted.Add(credit, credit.ParentId);
				}

				int last_id = int.MinValue;

				foreach (var credit in credits_sorted)
				{
					if (last_id != credit.Key.ParentId)
					{
						if (last_id != int.MinValue)
							credits.Add(allCredits); // allCredits is reused, ignore the name
						last_id = credit.Value;

						// Reusing object's memory
						allCredits = new List<Credit>();
						allCredits.Add(credit.Key);
					}
					else
					{
						allCredits.Add(credit.Key);
					}
				}

				return credits.Where(filter).ToList();
			}
			catch (ArgumentNullException)
			{
				return null;
			}
		}

		public override List<Credit> GetFind(Func<Credit, bool> filter)
		{
			using (CreditContext context = new CreditContext())
			{
				try
				{
					return context._credits.Include("Histories").Where(filter).ToList();
				}
				catch (ArgumentNullException)
				{
					return null;
				}
			}
		}

		public override void Add(Credit obj)
		{
			using (CreditContext context = new CreditContext())
			{
				foreach (History history in obj.Histories)
				{
					history.ParentId = obj.Id; // Not auto updating, TODO: ......................... fuck htis!!!!!! !1!
					context._histories.Add(history);
				}

				context._credits.Add(obj);
				context.SaveChanges();
			}
		}

		public override void Update(Credit obj)
		{
			using (CreditContext context = new CreditContext())
			{
				Credit target = context._credits.Include("Histories").Single(e => e.Id == obj.Id);
				if (target != null)
				{
					context.Entry(target).CurrentValues.SetValues(obj);
				}


				List<History> target2 = context._histories.Where(e => e.ParentId == obj.Id).ToList();

				if (target2 != null)
				{
					foreach (History history in target2)
					{
						context.Entry(history).CurrentValues.SetValues(obj.Histories.Single(e => e.Id == history.Id));
					}
				}

				context.SaveChanges();
			}
		}

		public override void DeleteId(int id)
		{
			using (CreditContext context = new CreditContext())
			{
				Credit target = context._credits.Include("Histories").Single(e => e.Id == id);
				if (target != null)
				{
					List<History> target2 = context._histories.Where(e => e.ParentId == id).ToList();
					if (target2 != null)
					{
						foreach (History history in target2)
						{
							context._histories.Remove(history);
						}
					}

					context._credits.Remove(target);
					context.SaveChanges();
				}
			}
		}

		public override void DeleteBulk(Func<Credit, bool> filter)
		{
			using (CreditContext context = new CreditContext())
			{
				List<Credit> targets = GetFind(filter);
				if (targets != null)
				{
					foreach (Credit target in targets)
					{
						DeleteId(target.Id);
					}

					context.SaveChanges();
				}
			}
		}
	}
}
