using System.ComponentModel.DataAnnotations;

namespace BaiTapTest.Models
{
    public partial class SanPham
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        public string Tensanpham { get; set; } = null!;
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Gia { get; set; }
        [Required(ErrorMessage = "Ngày nhập không được để trống.")]
        [DataType(DataType.Date, ErrorMessage = "Ngày nhập không đúng định dạng.")]
        public DateTime? NgayNhap { get; set; }
        [Required(ErrorMessage = "Loại sản phẩm không được để trống.")]
        public int? IdLoaiSanPham { get; set; }

        public virtual LoaiSanPham? IdLoaiSanPhamNavigation { get; set; }
    }
}
