using System.Collections.Generic;

namespace Micro.Shared.DTOs
{
    public class ErrorDTOv2
    {
        public string error { get; set; }
        public string error_description { get; set; }

        
    }

    public class ErrorDTO
    {
        public List<string> Errors { get; set; }
    }
}
