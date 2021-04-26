#nullable disable

namespace ProductCatalouge.EntityFramework.Models.DB
{
    public partial class TblUserRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }

        public virtual TblUser User { get; set; }
    }
}
