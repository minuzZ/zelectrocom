
namespace ZelectroCom.Data.Models
{
    public enum ZDevState
    {
        Active,
        Deleted
    }

    public class ZDev : Entity
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public ZDevState ZDevState { get; set; }
    }
}
