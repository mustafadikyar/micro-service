using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Micro.Catalog.Models;
using System;

namespace Micro.Catalog.DTOs
{
    [AutoMap(typeof(Course), ReverseMap = true)]
    public class CourseDTO
    {        
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }

    [AutoMap(typeof(Course), ReverseMap = true)]
    public class CourseCreateDTO : CourseDTO
    {
        public DateTime EditedTime { get; set; }
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public FeatureDTO Feature { get; set; }
    }

    [AutoMap(typeof(Course), ReverseMap = true)]
    public class CourseUpdateDTO : CourseCreateDTO
    {
        [SourceMember(nameof(Course.Id))]
        public string CourseId { get; set; }
    }
}
