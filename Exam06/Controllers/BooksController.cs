using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exam06.Data;
using Exam06.Models;
using Exam06.ViewModels;

namespace Exam06.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSort"] = sortOrder == "id_asc" ? "id_desc" : "id_asc";
            ViewData["TittleSort"] = sortOrder == "tittle_asc" ? "tittle_desc" : "tittle_asc";
            ViewData["DescriptionSort"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewData["PriceSort"] = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewData["CategoryIdSort"] = sortOrder == "categoryId_asc" ? "categoryId_desc" : "categoryId_asc";
            ViewData["AuthorIdSort"] = sortOrder == "authorId_asc" ? "authorId_desc" : "authorId_asc";


            var books = _context.Books.AsQueryable();

            books = sortOrder switch
            {
                "id_asc" => books.OrderBy(x => x.Id),
                "id_desc" => books.OrderByDescending(x => x.Id),
                "tittle_asc" => books.OrderBy(x => x.Title),
                "tittle_desc" => books.OrderByDescending(x => x.Title),
                "description_asc" => books.OrderBy(x => x.Description),
                "description_desc" => books.OrderByDescending(x => x.Description),
                "price_asc" => books.OrderBy(x => x.Price),
                "price_desc" => books.OrderByDescending(x => x.Price),
                "categoryId_asc" => books.OrderBy(x => x.Category.Id),
                "categoryId_desc" => books.OrderByDescending(x => x.Category.Id),
                "authorId_asc" => books.OrderBy(x => x.Author.Id),
                "authorId_desc" => books.OrderByDescending(x => x.Author.Id),
                _ => books.OrderBy(x => x.Title)
            };

            var categories = await _context.Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();

            var booksViewModel = new BookViewModel()
            {
                Books = await books.ToListAsync(),
                Categories = categories
            };

            return View(booksViewModel);
        }
            [HttpPost]
            public async Task<IActionResult> Index(string? searchString, string category)
            {
                int categoryId;

                if (int.TryParse(category, out categoryId)&& searchString!=null)
                {
                    var books = await _context.Books
                        .Include(b => b.Category)
                        .Include(b => b.Author)
                        .Where(b => b.Title.ToLower().Contains(searchString.ToLower()))
                        .Where(c => c.CategoryId == categoryId)
                        .ToListAsync();

                    var categories = await _context.Categories
                        .Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        })
                        .ToListAsync();

                    var bookViewModel = new BookViewModel
                    {
                        Books = books,
                        Categories = categories
                    };

                    return View(bookViewModel);
                }
                else
                {
                   
                    var books = await _context.Books
                        .Include(b => b.Category)
                        .ToListAsync();

                    var categories = await _context.Categories
                        .Select(x => new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        })
                        .ToListAsync();

                    var bookViewModel = new BookViewModel
                    {
                        Books = books,
                        Categories = categories
                    };

                    return View(bookViewModel);
                }
            }

            // GET: Books/Details/5
            public async Task<IActionResult> Details(int? id)
            {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
            }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,CategoryId,AuthorId")] Book book)
        {
            if (book!=null)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FUllName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,CategoryId,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (book!=null)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'LibraryDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
