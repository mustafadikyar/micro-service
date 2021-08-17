using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Micro.Catalog.Models;

namespace Micro.Catalog.DTOs
{
    [AutoMap(typeof(Category), ReverseMap = true)]
    public class CategoryDTO
    {       
        public string Name { get; set; }
    }

    [AutoMap(typeof(Category), ReverseMap = true)]
    public class CategoryCreateDTO : CategoryDTO
    {

    }

    [AutoMap(typeof(Category), ReverseMap = true)]
    public class CategoryUpdateDTO : CategoryCreateDTO
    {
        [SourceMember(nameof(Category.Id))]
        public string CategoryId { get; set; }
    }
}
