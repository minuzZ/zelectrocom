using System.Data.Entity;
using System.Linq;
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
            _context.SaveChanges();
        }
        public bool HasOldPath(string path)
        {
            return _dbset.Any(x => x.OldPath == path);
        }
    }
}
