using System.Collections.Generic;

#nullable disable

namespace ProductCatalouge.EntityFramework.Models.DB
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
