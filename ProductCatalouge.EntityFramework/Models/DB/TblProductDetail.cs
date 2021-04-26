using System;

#nullable disable

namespace ProductCatalouge.EntityFramework.Models.DB
{
    public partial class TblProductDetail
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
