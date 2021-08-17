namespace Micro.Catalog.DTOs
{
    public class FeatureDTO
    {
        public int Duration { get; set; }
    }

    public class FeatureCreateDTO : FeatureDTO
    {
    }

    public class FeatureUpdateDTO : FeatureCreateDTO
    {
    }
}
