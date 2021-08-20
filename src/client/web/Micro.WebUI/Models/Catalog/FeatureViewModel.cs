using System.ComponentModel.DataAnnotations;

namespace Micro.WebUI.Models.Catalog
{
    public class FeatureViewModel
    {
        [Display(Name = "Kurs süre")]
        public int Duration { get; set; }
    }
}
