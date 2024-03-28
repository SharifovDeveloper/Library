using Exam06.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exam06.ViewModels
{
    public class BookViewModel
    {
        public List<Book> Books { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public string Category { get; set; }
    }
}
