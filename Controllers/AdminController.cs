using ASP_P42.Services.Storage;
using ASP_P42.Views.Admin;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class AdminController(IStorageService storageService) : Controller
    {
        private readonly IStorageService _storageService = storageService;
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGroup(AdminAddGroupFromModel formModel)
        {
            try
            {
                return Ok(_storageService.Save(formModel.Image));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
