using System.ComponentModel.DataAnnotations;

namespace BaiTapTest.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        public string Ten { get; set; } = null!;
        [Required(ErrorMessage = "Ngày nhập không được để trống.")]
        [DataType(DataType.Date, ErrorMessage = "Ngày nhập không đúng định dạng.")]
        public DateTime? NgayNhap { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
