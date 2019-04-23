using System.Collections.Generic;
using System.Linq;
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
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any( x => x.Id == data.Id );
                if (exists)
                {
                    dbSet.Update( data );
                    continue;
                }
                dbSet.Add( data );
            }
        }

        public static async Task AddOrUpdateAsync<T>( this DbSet<T> dbSet, IEnumerable<T> records )
        where T : BusinessObjectBaseWithId
        {
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any( x => x.Id == data.Id );
                if (exists)
                {
                    dbSet.Update ( data );
                    continue;
                }
                await dbSet.AddAsync( data );
            }
        }
    }
}
