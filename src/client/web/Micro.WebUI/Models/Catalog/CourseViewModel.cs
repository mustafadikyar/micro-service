﻿using System;

namespace Micro.WebUI.Models.Catalog
{
    public class CourseViewModel
    {
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string StockPictureUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string ShortDescription
        {
            get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;
        }


        public string UserId { get; set; }
        public DateTime EditedTime { get; set; }
        public FeatureViewModel Feature { get; set; }
        public string CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
