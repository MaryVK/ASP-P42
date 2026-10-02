using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP_P42.Data.Entities
{
    public record ProductVersion
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public String? Version { get; set; } = null!;

        [Column(TypeName = "DECIMAL(18, 2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; } = 1;
        public String? Slug { get; set; }
        public String? ImageUrl { get; set; } = null!;
        public int IsHidden { get; set; } = 0;
        public int OrderInPrice { get; set; } = 100000;

        [JsonIgnore]
        public Product Product { get; set; } = null!;
    }
}
