using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Views.Admin
{
    public class AdminAddGroupFromModel
    {
        [FromForm(Name = "group-img")]
        public IFormFile Image { get; set; } = null!;


    }
}
