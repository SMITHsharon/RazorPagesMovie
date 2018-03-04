using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPagesMovie2.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie2.Models.MovieContext _context;

        public IndexModel(RazorPagesMovie2.Models.MovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }
        /* added the following */
        public SelectList Genres;
        public string MovieGenre { get; set; }

        // added this code 
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public async Task OnGetAsync(string movieGenre, string searchString)
        {
            // use LINQ to get list of genres
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
