using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger<BookController> _logger;
        private readonly BookManagement.Data.ApplicationDbContext _db;

        public BookController(Microsoft.Extensions.Logging.ILogger<BookController> logger, BookManagement.Data.ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string message = null)
        {
            if (!string.IsNullOrEmpty(message)) ViewBag.Message = message;
            var list = _db.Books.ToList();
            return View(list);
        }
        public IActionResult Detail(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }
        // GET: hiển thị form
        public IActionResult Create(bool saved = false)
        {
            if (saved)
            {
                ViewBag.Message = "Thêm sách thành công!";
            }
            return View();
        }

        // POST: xử lý khi submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ModelState.AddModelError("ImagePath", "Chỉ cho phép upload file jpg hoặc png");
                    return View(book);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                book.ImagePath = "/images/books/" + fileName;
            }

            if (ModelState.IsValid)
            {
                _db.Books.Add(book);
                _db.SaveChanges();

                TempData["Message"] = "Thêm sách thành công!";
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    ModelState.AddModelError("ImagePath", "Chỉ cho phép upload file jpg hoặc png");
                    return View(book);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/books");

                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                book.ImagePath = "/images/books/" + fileName;
            }

            if (!ModelState.IsValid) return View(book);
            _db.Books.Update(book);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { message = "Cập nhật thành công" });
        }

        // GET: Delete confirm
        public IActionResult Delete(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _db.Books.Find(id);
            if (book == null) return NotFound();
            _db.Books.Remove(book);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index), new { message = "Xóa thành công" });
        }
    }
}
