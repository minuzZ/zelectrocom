using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class OldMediaMap : BaseMap<OldMedia>
    {
        public OldMediaMap()
        {
            ToTable("OldMedia");
        }
    }
}
