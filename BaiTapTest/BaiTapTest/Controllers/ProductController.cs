using BaiTapTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTest.Controllers
{

    public class ProductController : Controller
    {
        private readonly testsanphamContext _context;

        public ProductController(testsanphamContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? keyword, int? categoryId, int page = 1, int pageSize = 10)
        {
            // Truy vấn sản phẩm
            var query = _context.SanPhams
                .Include(p => p.IdLoaiSanPhamNavigation) // Tham chiếu tới LoaiSanPham
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword) && categoryId.HasValue)
            {
                query = query.Where(p =>
                    p.Tensanpham.Contains(keyword) && // Tìm theo tên sản phẩm
                    p.IdLoaiSanPham == categoryId);  // Tìm theo loại sản phẩm
            }
            else if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Tensanpham.Contains(keyword)); // Chỉ tìm theo tên
            }
            else if (categoryId.HasValue)
            {
                query = query.Where(p => p.IdLoaiSanPham == categoryId); // Chỉ tìm theo loại
            }
            // Tổng số sản phẩm
            var totalItems = query.Count();

            // Phân trang
            var products = query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền dữ liệu qua ViewData
            ViewData["TotalItems"] = totalItems;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["Categories"] = _context.LoaiSanPhams.ToList();
            ViewData["Keyword"] = keyword;
            ViewData["CategoryId"] = categoryId;

            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = _context.LoaiSanPhams.ToList(); // Load danh sách loại sản phẩm
            return View();
        }
        [HttpPost]
        public IActionResult Create(SanPham model)
        {
            // Kiểm tra xem tên sản phẩm đã tồn tại chưa
            if (_context.SanPhams.Any(p => p.Tensanpham == model.Tensanpham))
            {
                ModelState.AddModelError("Tensanpham", "Tên sản phẩm đã tồn tại.");
            }

            // Kiểm tra dữ liệu nhập vào
            if (!ModelState.IsValid)
            {
                ViewData["Categories"] = _context.LoaiSanPhams.ToList(); // Load danh sách loại sản phẩm
                return View(model);
            }

            // Lưu sản phẩm mới
            _context.SanPhams.Add(model);
            _context.SaveChanges();

            // Thông báo thành công và quay lại danh sách sản phẩm
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tìm sản phẩm theo ID
            var product = _context.SanPhams.Find(id);

            if (product == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404
                return NotFound();
            }

            // Truyền danh sách loại sản phẩm vào ViewData
            ViewData["Categories"] = _context.LoaiSanPhams.ToList();

            // Truyền sản phẩm vào View để chỉnh sửa
            return View(product);
        }

        [HttpPost]

        public IActionResult Edit(SanPham updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả lại form với thông báo lỗi
                return View(updatedProduct);
            }

            // Tìm sản phẩm cũ từ cơ sở dữ liệu
            var existingProduct = _context.SanPhams.Find(updatedProduct.Id);
            if (existingProduct == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404
                return NotFound();
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Tensanpham = updatedProduct.Tensanpham;
            existingProduct.Gia = updatedProduct.Gia;
            existingProduct.NgayNhap = updatedProduct.NgayNhap;
            existingProduct.IdLoaiSanPham = updatedProduct.IdLoaiSanPham;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Thông báo thành công
            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";

            // Quay lại trang danh sách sản phẩm
            return RedirectToAction("Index");
        }
        [HttpPost]

        public IActionResult Delete(int id)
        {
            var product = _context.SanPhams.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Xóa sản phẩm
            _context.SanPhams.Remove(product);
            _context.SaveChanges();

            return Json(new { success = true }); // Trả về kết quả thành công
        }


    }
}
