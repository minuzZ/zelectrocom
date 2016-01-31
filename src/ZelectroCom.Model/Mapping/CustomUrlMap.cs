using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class CustomUrlMap : BaseMap<CustomUrl>
    {
        public CustomUrlMap()
        {
            ToTable("CustomUrl");
        }
    }
}
