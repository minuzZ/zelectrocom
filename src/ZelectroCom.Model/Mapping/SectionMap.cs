using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class SectionMap : BaseMap<Section>
    {
        public SectionMap()
        {
            Property(x => x.Name).IsRequired().HasMaxLength(50);

            ToTable("Section");
        }
    }
}
