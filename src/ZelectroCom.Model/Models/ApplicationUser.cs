using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ZelectroCom.Data.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    //UserName = Nickname

    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Article> Articles { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Description { get; set; }
        public virtual double Rating { get; set; }
        public virtual bool HasAvatar { get; set; }
    }
}