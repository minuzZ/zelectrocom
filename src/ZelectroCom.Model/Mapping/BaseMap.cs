using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ZelectroCom.Data.Mapping
{
    public class BaseMap<T> : EntityTypeConfiguration<T> where T : Entity
    {
        public BaseMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
