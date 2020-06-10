using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HdMovies.Data;
using HdMovies.Models;
using HdMovies.Helpers;

namespace HdMovies.Pages.Movies
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies
                .Include(m => m.UploadedUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FindAsync(id);

            if (Movie != null)
            {
                _context.Movies.Remove(Movie);
                var imgFileName = Path.GetFileName(Movie.PosterPath);
                var posterFullPath = Path.Combine(_env.WebRootPath, "db_files", "img", imgFileName);
                FileHelper.DeleteFile(posterFullPath);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Movies/List");
        }
    }
}
