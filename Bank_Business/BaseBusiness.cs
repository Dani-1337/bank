using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Business
{
    public class BaseBusiness<T, TContext> where TContext : DbContext where T : class
    {
        /// <summary>
		/// Get all objects in the database
		/// </summary>
		/// <returns>All the objects in the database</returns>
		public virtual List<T> GetAll()
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
                {
                    return ((DbSet<T>)(finfo.GetValue(context))).ToList();
                }

                return null; // Should be never hit if the context class is properly written
            }
        }

		/// <summary>
		/// Get an object with the specified <b>id</b>
		/// </summary>
		/// <param name="id">The id to find</param>
		/// <returns>Object with <b>id</b> as its id</returns>
		public virtual T GetId(int id)
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
                {
                    return ((DbSet<T>)(finfo.GetValue(context))).Single(e => (int)typeof(T).GetField("Id").GetValue(e) == id);
                }

                return null; // Should be never hit if the context class is properly written
            }
        }

		/// <summary>
		/// Find all objects with the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use <b><i>(lambda/delegate can be used)</i></b></param>
		/// <returns>Object matching the <b>filter</b><br/> Null if not found or the <b>filter</b> throws ArgumentNullException</returns>
		public virtual List<T> GetFind(Func<T, bool> filter)
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                try
                {
                    foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
                    {
                        return ((DbSet<T>)(finfo.GetValue(context))).Where(filter).ToList();
                    }

                    return null; // Should be never hit if the context class is properly written
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// Adds <b>object</b> to the database
		/// </summary>
		/// <param name="obj">The object to add</param>
		public virtual void Add(T obj)
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
                {
                    ((DbSet<T>)(finfo.GetValue(context))).Add(obj);
                }
                context.SaveChanges();
            }
        }

		/// <summary>
		/// Update <b>object</b> and push changes to database
		/// </summary>
		/// <param name="obj">Object to update, <b><i>id</i></b> is how you specify which object to update</param>
		public virtual void Update(T obj)
		{
			using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
			{
				foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
				{
					T target = ((DbSet<T>)(finfo.GetValue(context))).Single(e => (int)typeof(T).GetField("Id").GetValue(e) == (int)typeof(T).GetField("Id").GetValue(obj));

					if (target != null)
					{
						context.Entry(target).CurrentValues.SetValues(obj);
						context.SaveChanges(); // better to cache it and update database after a while, but whatever
					}
				}
			}
		}

		/// <summary>
		/// Deletes object with the specified <b>id</b>
		/// </summary>
		/// <param name="id">Id of the object to delete</param>
		public virtual void DeleteId(int id)
		{
			using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
			{
				foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
				{
					T target = ((DbSet<T>)(finfo.GetValue(context))).Single(e => (int)typeof(T).GetField("Id").GetValue(e) == id);

					if (target != null)
					{
						((DbSet<T>)(finfo.GetValue(context))).Remove(target);
						context.SaveChanges();
					}
				}
			}
		}

		/// <summary>
		/// Deletes objects in bulk that apply to the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use</param>
		public virtual void DeleteBulk(Func<T, bool> filter)
		{
			using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
			{
				foreach (var finfo in typeof(TContext).GetFields().Where(f => f.IsPublic && f.FieldType == typeof(DbSet<T>)))
				{
					List<T> targets = GetFind(filter);

					if (targets != null)
					{
						foreach (T target in targets)
						{
							((DbSet<T>)(finfo.GetValue(context))).Remove(target);
						}

						context.SaveChanges();
					}
				}	
			}
		}
	}
}
