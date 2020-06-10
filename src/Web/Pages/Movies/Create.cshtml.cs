using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using HdMovies.Data;
using HdMovies.Models;
using HdMovies.Helpers;

namespace HdMovies.Pages.Movies
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult OnGet()
        {
            ViewData["genres"] = Enum.GetNames(typeof(Genre));

            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        [BindProperty]
        public IFormFile UploadPoster { get; set; }

        [BindProperty]
        public string[] SelectedGenres { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _context.Users.FirstAsync(i => i.UserName == User.Identity.Name);
            Movie.UploadedUser = currentUser;
            Movie.GenerateSlug();
            Movie.SetGenres(SelectedGenres);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UploadPoster != null)
            {
                var fileName = $"{Movie.Slug}_poster.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                Movie.PosterPath = $"/db_files/img/{fileName}";
                FileHelper.SaveFileAsync(UploadPoster.OpenReadStream(), fileNameAbsPath);
            }

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./List");
        }
    }
}
