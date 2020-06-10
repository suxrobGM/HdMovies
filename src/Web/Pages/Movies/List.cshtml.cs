using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuxrobGM.Sdk.Pagination;
using HdMovies.Data;
using HdMovies.Models;

namespace HdMovies.Pages.Movies
{
    public class ListModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Movie> Movies { get;set; }

        public async Task OnGetAsync(int pageIndex)
        {
            var movies = _context.Movies.Include(m => m.UploadedUser).AsNoTracking();
            Movies = await PaginatedList<Movie>.CreateAsync(movies.OrderByDescending(i => i.Timestamp), pageIndex, 12);
        }
    }
}
