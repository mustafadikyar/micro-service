namespace Micro.Catalog.DTOs
{
    public class CategoryDTO
    {       
        public string Name { get; set; }
    }

    public class CategoryCreateDTO : CategoryDTO
    {

    }

    public class CategoryUpdateDTO : CategoryCreateDTO
    {
        public string Id { get; set; }
    }
}
