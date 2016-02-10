using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class ZDevMap : BaseMap<ZDev>
    {
        public ZDevMap()
        {
            ToTable("ZDev");
        }
    }
}
