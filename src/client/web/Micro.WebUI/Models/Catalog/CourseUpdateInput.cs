﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Micro.WebUI.Models.Catalog
{
    public class CourseUpdateInput
    {
        public string CourseId { get; set; }

        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıklama")]
        public string Description { get; set; }

        [Display(Name = "Kurs fiyat")]
        public decimal Price { get; set; }

        public string UserId { get; set; }

        public string Image { get; set; }
        public FeatureViewModel Feature { get; set; }

        [Display(Name = "Kurs kategori")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs Resim")]
        public IFormFile PhotoFormFile { get; set; }
    }
}
