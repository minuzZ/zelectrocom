using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class SectionMap : BaseMap<Section>
    {
        public SectionMap()
        {
            Property(x => x.Name).IsRequired().HasMaxLength(50);
            Property(x => x.Path).IsRequired().HasMaxLength(50);

            ToTable("Section");
        }
    }
}
