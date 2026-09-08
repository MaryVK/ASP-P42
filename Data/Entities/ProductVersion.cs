using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_P42.Data.Entities
{
    public class ProductVersion
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public String? Version { get; set; } = null!;

        [Column(TypeName = "DEECIMAL(18, 2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; } = 1;
        public String? Slug { get; set; }
        public String? ImageUrl { get; set; } = null!;
        public int IsHidden { get; set; } = 0;

        public Product Product { get; set; } = null!;
    }
}
