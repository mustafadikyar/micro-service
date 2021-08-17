using AutoMapper;
using Micro.Catalog.Models;

namespace Micro.Catalog.DTOs
{
    [AutoMap(typeof(Feature), ReverseMap = true)]
    public class FeatureDTO
    {
        public int Duration { get; set; }
    }

    [AutoMap(typeof(Course), ReverseMap = true)]
    public class FeatureCreateDTO : FeatureDTO
    {
    }

    [AutoMap(typeof(Course), ReverseMap = true)]
    public class FeatureUpdateDTO : FeatureCreateDTO
    {
    }
}
