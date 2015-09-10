using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelectroCom.Data;
using ZelectroCom.Data.Models;

namespace ZelectroCom.Service
{
    public class SectionService : EntityService<Section>, ISectionService
    {
        public SectionService(IContext context) : base(context) { }

        public IEnumerable<Section> GetActiveSections()
        {
            var activeSections = GetAll().Where(IsActiveSection);
            return activeSections;
        }

        public bool IsActiveSection(Section item)
        {
            return item.SectionState == SectionState.Active;
        }
    }
}
