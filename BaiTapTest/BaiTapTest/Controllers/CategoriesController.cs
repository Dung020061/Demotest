using BaiTapTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTapTest.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly testsanphamContext _context;

        public CategoriesController(testsanphamContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? keyword, int page = 1, int pageSize = 10)
        {
            var query = _context.LoaiSanPhams.AsQueryable();

            // Kiểm tra từ khóa và lọc dữ liệu theo tên loại sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Ten.Contains(keyword)); // Dùng Contains để tìm kiếm phần chuỗi
            }

            // Tổng số loại sản phẩm
            var totalItems = query.Count();

            // Phân trang dữ liệu
            var products = query
                .OrderBy(p => p.Id) // Sắp xếp theo ID
                .Skip((page - 1) * pageSize) // Bỏ qua các mục ở các trang trước
                .Take(pageSize) // Lấy dữ liệu cho trang hiện tại
                .ToList();

            // Truyền dữ liệu qua ViewData
            ViewData["TotalItems"] = totalItems; // Tổng số mục
            ViewData["Page"] = page; // Trang hiện tại
            ViewData["PageSize"] = pageSize; // Số lượng sản phẩm trên mỗi trang
            ViewData["Keyword"] = keyword; // Từ khóa tìm kiếm

            return View(products); // Trả về view với dữ liệu tìm kiếm và phân trang
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(LoaiSanPham model)
        {
            // Kiểm tra xem tên sản phẩm đã tồn tại chưa
            if (_context.LoaiSanPhams.Any(p => p.Ten == model.Ten))
            {
                ModelState.AddModelError("Ten", "Tên loại sản phẩm đã tồn tại.");
            }

            // Kiểm tra dữ liệu nhập vào
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Lưu sản phẩm mới
            _context.LoaiSanPhams.Add(model);
            _context.SaveChanges();

            // Thông báo thành công và quay lại danh sách sản phẩm
            TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Tìm sản phẩm theo ID
            var product = _context.LoaiSanPhams.Find(id);

            if (product == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404
                return NotFound();
            }



            // Truyền sản phẩm vào View để chỉnh sửa
            return View(product);
        }

        [HttpPost]

        public IActionResult Edit(LoaiSanPham updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả lại form với thông báo lỗi
                return View(updatedProduct);
            }

            // Tìm sản phẩm cũ từ cơ sở dữ liệu
            var existingProduct = _context.LoaiSanPhams.Find(updatedProduct.Id);
            if (existingProduct == null)
            {
                // Nếu không tìm thấy, trả về lỗi 404
                return NotFound();
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Ten = updatedProduct.Ten;

            existingProduct.NgayNhap = updatedProduct.NgayNhap;


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
            // Tìm loại sản phẩm theo ID
            var category = _context.LoaiSanPhams.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound(new { success = false, message = "Loại sản phẩm không tồn tại." });
            }

            // Kiểm tra số lượng sản phẩm thuộc loại này
            var productCount = _context.SanPhams.Count(x => x.IdLoaiSanPham == id);

            if (productCount > 0)
            {

                return Json(new
                {
                    success = false,
                    message = $"Xóa thất bại: Còn {productCount} sản phẩm thuộc loại này."
                });
            }

            // Tiến hành xóa loại sản phẩm
            _context.LoaiSanPhams.Remove(category);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Xóa thành công: Loại sản phẩm đã được loại bỏ."
            });
        }

    }
}