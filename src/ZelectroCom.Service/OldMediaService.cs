using System.Data.Entity;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public class OldMediaService : EntityService<OldMedia>, IOldMediaService
    {
        public OldMediaService(IContext context) : base(context) { }

        public void Clear()
        {
            foreach (var p in _context.Set<OldMedia>())
            {
                _context.Entry(p).State = EntityState.Deleted;
            }
        }
    }
}
