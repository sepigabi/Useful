using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Binarit.Framework.BusinessObject.Domain;
using Microsoft.EntityFrameworkCore;

namespace MobileClient.Portable.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddOrUpdate<T>( this DbSet<T> dbSet, IEnumerable<T> records )
        where T : BusinessObjectBaseWithId
        {
            var dbContext = dbSet.GetContext();
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any( x => x.Id == data.Id );
                if (exists)
                {
                    dbSet.Update( data );
                    dbContext.Entry( data ).State = EntityState.Modified;
                    continue;
                }
                dbSet.Add( data );
            }
        }

        public static async Task AddOrUpdateAsync<T>( this DbSet<T> dbSet, IEnumerable<T> records )
        where T : BusinessObjectBaseWithId
        {
            var dbContext = dbSet.GetContext();
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any( x => x.Id == data.Id );
                if (exists)
                {
                    dbSet.Update ( data );
                    dbContext.Entry( data ).State = EntityState.Modified;
                    continue;
                }
                await dbSet.AddAsync( data );
            }
        }

        public static DbContext GetContext<TEntity>( this DbSet<TEntity> dbSet )
        where TEntity : class
        {
            return (DbContext) dbSet
                .GetType().GetTypeInfo()
                .GetField( "_context", BindingFlags.NonPublic | BindingFlags.Instance )
                .GetValue( dbSet );
        }
    }
}
