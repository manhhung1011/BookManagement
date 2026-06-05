using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger<BookController> _logger;
        private static List<Book> books = new List<Book>
    {
        new Book { Id=1, Name="Clean Code", Price=20 },
        new Book { Id=2, Name="ASP.NET MVC", Price=15 },
        new Book { Id=3, Name="Design Pattern", Price=25 }
    };

        public BookController(Microsoft.Extensions.Logging.ILogger<BookController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string message = null)
        {
            if (!string.IsNullOrEmpty(message)) ViewBag.Message = message;
            return View(books);
        }
        public IActionResult Detail(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
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

            book.Id = books.Count + 1;
            books.Add(book);
            _logger?.LogInformation("Book added (Id={Id}). Showing success on Create page.", book.Id);
            // Show success immediately on the same Create page (no redirect)
            ModelState.Clear();
            ViewBag.Message = "Thêm sách thành công!";
            return View(new Book());
        }
    }
}
