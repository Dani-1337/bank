using Bank_Business.Exceptions;
using Bank_Data.Attributes;
using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Business.Business
{
    public class BaseBusiness<T, TContext> where TContext : DbContext where T : class
    {
		/// <summary>
		/// Get all objects in the database
		/// </summary>
		/// <returns>All the objects in the database</returns>
		/// <exception cref="InvalidClassImplementationException">Throws when the calling method's class was poorly written and missing LocalDbSetAttr attribute</exception>
		public virtual List<T> GetAll()
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
				foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
				{
					return ((DbSet<T>)(finfo.GetValue(context))).ToList();
                }

				throw new InvalidClassImplementationException("Calling method was written poorly, missing LocalDbSetAttr attribute!");
            }
        }

		/// <summary>
		/// Get an object with the specified <b>id</b>
		/// </summary>
		/// <param name="id">The id to find</param>
		/// <returns>Object with <b>id</b> as its id</returns>
		/// <exception cref="InvalidClassImplementationException">Throws when the calling method's class was poorly written and missing LocalDbSetAttr attribute</exception>
		public virtual T GetId(int id)
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
                {
                    return ((DbSet<T>)(finfo.GetValue(context))).Single(e => (int)typeof(T).GetField("Id").GetValue(e) == id);
                }

                throw new InvalidClassImplementationException("Calling method was written poorly, missing LocalDbSetAttr attribute!");
            }
        }

		/// <summary>
		/// Find all objects with the specified <b>filter</b>
		/// </summary>
		/// <param name="filter">Filter to use <b><i>(lambda/delegate can be used)</i></b></param>
		/// <returns>Object matching the <b>filter</b><br/> Null if not found or the <b>filter</b> throws ArgumentNullException</returns>
		/// <exception cref="InvalidClassImplementationException">Throws when the calling method's class was poorly written and missing LocalDbSetAttr attribute</exception>
		public virtual List<T> GetFind(Func<T, bool> filter)
        {
            using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
            {
                try
                {
                    foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
                    {
                        return ((DbSet<T>)(finfo.GetValue(context))).Where(filter).ToList();
                    }

                    throw new InvalidClassImplementationException("Calling method was written poorly, missing LocalDbSetAttr attribute!");
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
                foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
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
				foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
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
				foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
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
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">Throwed when the data you are trying to delete has some other data linked to it, delete that other data first</exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "SecurityIntelliSenseCS:MS Security rules violation", Justification = "<Pending>")]
		public virtual void DeleteBulk(Func<T, bool> filter)
		{
			using (TContext context = (TContext)Activator.CreateInstance(typeof(TContext)))
			{
				foreach (var finfo in typeof(TContext).GetProperties().Where(f => f.CustomAttributes.Where(attr => attr.AttributeType == typeof(LocalDbSetAttr)).ToArray().Length > 0 && f.PropertyType == typeof(DbSet<T>)))
				{
					List<T> targets = GetFind(filter);

					if (targets != null)
					{
						foreach (T target in targets)
						{
							((DbSet<T>)(finfo.GetValue(context))).Attach(target);
							((DbSet<T>)(finfo.GetValue(context))).Remove(target);
						}

						context.SaveChanges();
					}
				}	
			}
		}
	}
}
