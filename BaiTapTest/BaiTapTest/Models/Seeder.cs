namespace BaiTapTest.Models
{
    public class Seeder
    {
        public static void Seed(testsanphamContext context)
        {
            // Seed bảng LoaiSanPham (20 loại sản phẩm)
            if (!context.LoaiSanPhams.Any())
            {
                var loaiSanPhams = Enumerable.Range(1, 20).Select(i => new LoaiSanPham
                {
                    Ten = $"Loai San Pham {i}",
                    NgayNhap = DateTime.Now.AddDays(-i) // Ngày nhập ngẫu nhiên
                }).ToList();

                context.LoaiSanPhams.AddRange(loaiSanPhams);
                context.SaveChanges();
            }

            // Seed bảng SanPham (10,000 sản phẩm)
            if (!context.SanPhams.Any())
            {
                var random = new Random();
                var loaiSanPhams = context.LoaiSanPhams.ToList(); // Lấy danh sách loại sản phẩm

                if (!loaiSanPhams.Any())
                {
                    throw new Exception("Danh sách LoaiSanPhams rỗng. Hãy kiểm tra seed dữ liệu cho LoaiSanPhams trước.");
                }

                var sanPhams = Enumerable.Range(1, 10000).Select(i => new SanPham
                {
                    Tensanpham = $"San Pham {i}", // Tên sản phẩm
                    Gia = random.Next(1, 1000) + random.NextDecimal(0, 0.99m), // Giá sản phẩm ngẫu nhiên
                    NgayNhap = DateTime.Now.AddDays(-random.Next(1, 365)), // Ngày nhập ngẫu nhiên
                    IdLoaiSanPham = loaiSanPhams[random.Next(loaiSanPhams.Count)].Id // Gán ngẫu nhiên 1 loại sản phẩm
                }).ToList();

                context.SanPhams.AddRange(sanPhams);
                context.SaveChanges();
            }
        }
    }

    // Hàm mở rộng để tạo giá trị decimal ngẫu nhiên
    public static class RandomExtensions
    {
        public static decimal NextDecimal(this Random random, decimal minValue, decimal maxValue)
        {
            return (decimal)random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}

