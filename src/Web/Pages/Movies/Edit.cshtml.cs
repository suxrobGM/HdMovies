using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HdMovies.Data;
using HdMovies.Models;
using HdMovies.Helpers;

namespace HdMovies.Pages.Movies
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        [BindProperty]
        public IFormFile UploadPoster { get; set; }

        [BindProperty]
        public string[] SelectedGenres { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["genres"] = Enum.GetNames(typeof(Genre));
            Movie = await _context.Movies.Include(m => m.UploadedUser).FirstOrDefaultAsync(m => m.Id == id);
            SelectedGenres = Movie.Genres.Split(',');

            if (Movie == null)
            {
                return NotFound();
            }
           
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var movie = await _context.Movies.FirstAsync(i => i.Id == Movie.Id);
            movie.Title = Movie.Title;
            movie.ReleaseDate = Movie.ReleaseDate;
            movie.Director = Movie.Director;
            movie.Description = Movie.Description;
            movie.GenerateSlug();
            movie.SetGenres(SelectedGenres);

            if (UploadPoster != null)
            {
                var fileName = $"{movie.Slug}_poster.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                movie.PosterPath = $"/db_files/img/{fileName}";
                FileHelper.SaveFileAsync(UploadPoster.OpenReadStream(), fileNameAbsPath);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Movies/Index", new { slug = movie.Slug });
        }

        private bool MovieExists(string id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
