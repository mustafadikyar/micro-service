using System;

namespace Micro.Catalog.DTOs
{
    public class CourseDTO
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }

    public class CourseCreateDTO : CourseDTO
    {
        public DateTime EditedTime { get; set; }
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public FeatureDTO Feature { get; set; }
    }

    public class CourseUpdateDTO : CourseCreateDTO
    {
        public string Id { get; set; }
    }
}
