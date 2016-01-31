using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZelectroCom.Data.Models
{
    public enum ContentType
    {
        Section,
        Article
    }

    public class CustomUrl : Entity
    {
        public string Url { get; set; }
        public ContentType ContentType { get; set; }
        public int ContentId { get; set; }
        public string ContentPath { get; set; }
    }
}
