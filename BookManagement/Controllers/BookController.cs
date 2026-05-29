using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        private static List<Book> books = new List<Book>
    {
        new Book { Id=1, Name="Clean Code", Price=20 },
        new Book { Id=2, Name="ASP.NET MVC", Price=15 },
        new Book { Id=3, Name="Design Pattern", Price=25 }
    };

        public IActionResult Index()
        {
            return View(books);
        }
        public IActionResult Detail(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }
        // GET: hiển thị form
        public IActionResult Create()
        {
            return View();
        }

        // POST: xử lý khi submit
        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            book.Id = books.Count + 1;
            books.Add(book);
            TempData["Message"] = "Thêm sách thành công!";
            return RedirectToAction("Index");
        }
    }
}
