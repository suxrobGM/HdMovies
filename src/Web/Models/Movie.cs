using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using SuxrobGM.Sdk.Extensions;
using SuxrobGM.Sdk.Utils;

namespace HdMovies.Models
{
    public enum Genre
    {
        Action,
        Comedy,
        Horror,
        Western,
        Family,
        Thriller,
        Documentary,
        Historical,
        Fantasy,
        Animation
    }

    public class Movie
    {
        [StringLength(8)]
        public string Id { get; set; } = GeneratorId.GenerateShort();

        [StringLength(32, ErrorMessage = "Characters must be less than 32")]
        [Required]
        public string Title { get; set; }

        [StringLength(40)]
        public string Slug { get; private set; }

        [StringLength(4096, ErrorMessage = "Characters must be less than 4096")]
        [Required]
        public string Description { get; set; }

        [StringLength(64)]
        public string PosterPath { get; set; }

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        [Required]
        public string Producer { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int ViewCount { get; set; }

        public User UploadedUser { get; set; }

        [StringLength(32)]
        public string UploadedUserId { get; set; }

        public string Genres { get; private set; }

        public void SetGenres(List<Genre> genres)
        {
            Genres = string.Join(',', genres);
        }

        public void GenerateSlug(bool useHypen = true, bool useLowerLetters = true)
        {
            var slug = this.Title.TranslateToLatin();
            slug = $"{this.Id} {slug}"; // append id before title name

            // invalid chars           
            slug = Regex.Replace(slug, @"[^A-Za-z0-9\s-]", "");

            // convert multiple spaces into one space 
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            var words = slug.Split().Where(str => !string.IsNullOrWhiteSpace(str));
            slug = string.Join(useHypen ? '-' : '_', words);
            this.Slug = useLowerLetters ? slug.ToLower() : slug;
        }
    }
}
