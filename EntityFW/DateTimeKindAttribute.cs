//Sets the DateTimeKind property of the retrieved entity's datetime properties 
    
    
    using System;
    using System.Linq;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeKindAttribute : Attribute
    {
        public DateTimeKind Kind { get; }

        public DateTimeKindAttribute(DateTimeKind kind)
        {
            this.Kind = kind;
        }

        public static void Apply(object entity)
        {
            if (entity == null)
                return;

            //Find all properties that are of type DateTime or DateTime?;
            var properties = entity.GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DateTime)
                         || x.PropertyType == typeof(DateTime?));

            foreach (var property in properties)
            {
                //Check whether these properties have the DateTimeKindAttribute;
                var attr = property.GetCustomAttribute<DateTimeKindAttribute>();
                if (attr == null)
                    continue;

                var datetime = property.PropertyType == typeof(DateTime?)
                    ? (DateTime?)property.GetValue(entity)
                    : (DateTime)property.GetValue(entity);

                //If the value is not null and the property has a set accessor set the appropriate DateTimeKind;
                if (datetime.HasValue && property.CanWrite)
                {
                    property.SetValue(entity, DateTime.SpecifyKind(datetime.Value, attr.Kind));
                }
            }
        }
    }


//In the modelcontext constructor:

    public DbModel(): base("name=ConnectionString")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbModel, DAL.Migrations.Configuration>("ConnectionString"));
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 360;
            objectContext.ObjectMaterialized += (sender, e) => DateTimeKindAttribute.Apply(e.Entity);
        }

    
//Usage:
    [DateTimeKind(DateTimeKind.Utc)]
    public DateTime Updated { get; set; }

    
    
    
