using System.Text.Json.Serialization;

namespace ASP_P42.Data.Entities
{
    public record ProductGroup
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public String Name { get; set; } = null!;
        public String Description { get; set; } = null!;

        public String Slug { get; set; } = null!;

        public String ImageUrl { get; set; } = null!;

        public int IsHidden { get; set; } = 0;

        public int OrderInPrice { get; set; } = 100000;



        public ICollection<Product> Products { get; set; } = [];

        [JsonIgnore]
        public ProductGroup? ParentGroup { get; set; }

        public ICollection<ProductGroup> Children { get; set; } = [];
    }
}
