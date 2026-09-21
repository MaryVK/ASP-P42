using ASP_P42.Data;
using ASP_P42.Data.Entities;
using ASP_P42.Models.Admin;
using ASP_P42.Services.Storage;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class AdminController(IStorageService storageService, DataAccessor dataAccessor, DataContext dataContext) : Controller
    {

        // инжекция
        private readonly IStorageService _storageService = storageService;
        private readonly DataAccessor _dataAccessor = dataAccessor;

        public IActionResult Product()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataAccessor.GetAllProductGroups(isIncludeHidden:true),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AdminAddProductFormModel formModel)
        {
            try
            {
                await _dataAccessor.IsProductFormModelValidAsync(formModel);

                // добавление нового товара
                // так как изображение / картинка опциональная, проверяем без исключений
                String? imageUrl = null;
                if (formModel.Image != null)
                {
                    // но если данные передали, то проверяем полностью
                    imageUrl = _storageService.Save(formModel.Image);
                }

                _dataAccessor.AddNewProduct(formModel, imageUrl);

                

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        public IActionResult Group()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataAccessor.GetAllProductGroups(isIncludeHidden: true),
            };
            return View(viewModel);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(AdminAddGroupFormModel formModel)
        {
            try
            {
                Guid newGroupId = await _dataAccessor.AddNewProductGroup(new()
                {
                    ParentId = formModel.ParentId,
                    Name = formModel.Name,
                    Description = formModel.Description,
                    Slug = formModel.Slug,
                    IsHidden = formModel.IsHidden,
                    ImageUrl = "/storage/image/" + _storageService.Save(formModel.Image)
                });
                return Ok(newGroupId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
