using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public interface IOldMediaService : IEntityService<OldMedia>
    {
        void Clear();
        bool HasOldPath(string path);
    }
}
