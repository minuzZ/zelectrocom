using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNet.Identity.EntityFramework;
using ZelectroCom.Data.Mapping;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Data
{
    public interface IContext
    {
        IDbSet<ApplicationUser> ApplicationUsers { get; set;}
        IDbSet<Section> Sections { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }

    public class AppDbContext : IdentityDbContext, IContext
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
        }

        public IDbSet<ApplicationUser> ApplicationUsers { get; set; }
        public IDbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            List<Type> typesToRegister = new List<Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!String.IsNullOrEmpty(type.Namespace) &&
                    (type.IsGenericType && type.GetGenericTypeDefinition() != typeof (BaseMap<>) || !type.IsGenericType) &&
                    type.BaseType != null && type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof (EntityTypeConfiguration<>)
                     || type.BaseType.GetGenericTypeDefinition() == typeof (BaseMap<>)))
                    typesToRegister.Add(type);
            }

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

    }
}