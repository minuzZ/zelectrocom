using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public interface ICustomUrlService : IEntityService<CustomUrl>
    {
        bool IsUniquePath(string path);
    }
}
