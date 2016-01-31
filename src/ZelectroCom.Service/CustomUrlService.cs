using System.Linq;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public class CustomUrlService : EntityService<CustomUrl>, ICustomUrlService
    {
        public CustomUrlService(IContext context) : base(context) { }

        public bool IsUniquePath(string path)
        {
            return !_dbset.Any(x => x.Url == path);
        }
    }
}
