using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP_P42.Data.Entities
{
    public record Product
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public String Name { get; set; } = null!;
        public String? Description { get; set; } = null!;

        public String? Slug { get; set; } = null!;

        public String ImageUrl { get; set; } = null!;

        public int IsHidden { get; set; } = 0;
        public int OrderInPrice { get; set; } = 100000;

        [JsonIgnore]

        public ProductGroup Group { get; set; } = null!;

        public ICollection<ProductVersion> Versions { get; set; } = [];
    }
}
