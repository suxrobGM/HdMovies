﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SuxrobGM.Sdk.Utils;

namespace HdMovies.Models
{
    public class User : IdentityUser<string>
    {
        [StringLength(32)]
        public override string Id { get; set; } = GeneratorId.GenerateComplex();

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string FirstName { get; set; }

        [StringLength(64, ErrorMessage = "Characters must be less than 64")]
        public string LastName { get; set; }

        [StringLength(64)]
        public string ProfilePhotoUrl { get; set; }

        public ICollection<Movie> UploadedMovies { get; set; } = new List<Movie>();
    }
}
