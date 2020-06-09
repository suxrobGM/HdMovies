using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HdMovies.Data;
using HdMovies.Models;

namespace HdMovies.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Movie Movie { get;set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var movieSlug = RouteData.Values["slug"].ToString();
            Movie = await _context.Movies.Include(m => m.UploadedUser).FirstAsync(m => m.Slug == movieSlug);

            if (!Request.Headers["User-Agent"].ToString().ToLower().Contains("bot"))
            {
                Movie.ViewCount++;
            }

            await _context.SaveChangesAsync();
            return Page();
        }
    }
}
