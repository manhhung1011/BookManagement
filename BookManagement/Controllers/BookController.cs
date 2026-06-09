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
        public IActionResult Create(Book book)
        {
            _logger?.LogInformation("BookController.Create POST called. ModelState.IsValid={IsValid}", ModelState.IsValid);

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger?.LogWarning("ModelState invalid: {Errors}", errors);
                return View(book);
            }

            _db.Books.Add(book);
            _db.SaveChanges();
            _logger?.LogInformation("Book added (Id={Id}). Showing success on Create page.", book.Id);
            // Show success immediately on the same Create page (no redirect)
            ModelState.Clear();
            ViewBag.Message = "Thêm sách thành công!";
            return View(new Book());
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
        public IActionResult Edit(Book book)
        {
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
