using System;

namespace Application.Features._categories.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconIdentifier { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#000000";
        public bool IsGlobal { get; set; }
    }
}
