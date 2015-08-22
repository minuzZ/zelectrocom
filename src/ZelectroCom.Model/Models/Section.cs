using System.Collections.Generic;

namespace ZelectroCom.Data.Models
{
    public class Section : Entity
    {
        public virtual ICollection<Article> Articles { get; set; }
        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsHidden { get; set; }

        public int Order { get; set; }
    }
}
