using ASP_P42.Data;
using ASP_P42.Models.Admin;
using ASP_P42.Services.Storage;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers
{
    public class AdminController(IStorageService storageService, DataContext dataContext) : Controller
    {

        // инжекция
        private readonly IStorageService _storageService = storageService;
        private readonly DataContext _dataContext = dataContext;

        public IActionResult Product()
        {
            AdminGroupViewModel viewModel = new()
            {
                Groups = _dataContext.ProductGroups.OrderBy(g => g.OrderInPrice).ToList(),
            };
            return View(viewModel);
        }
        public IActionResult AddProduct(AdminAddProductFormModel formModel)
        {
            try
            {
                // так как товары ИМЕЮТ определённые группы, проверяем её на правильность 
                Data.Entities.ProductGroup group = _dataContext
                    .ProductGroups
                    .FirstOrDefault(g => g.Id == formModel.GroupId)
                    ?? throw new Exception($"Group not found with id='{formModel.GroupId}'");
                if(formModel.ProductId != null)
                {
                    Data.Entities.Product? product = _dataContext
                        .Products
                        .FirstOrDefault(p => p.Id == formModel.ProductId)
                    ?? throw new Exception($"Product not found with id='{formModel.ProductId}'");
                }
                else
                {
                    // добавление нового товара
                    // так как изображение / картинка опциональная, проверяем без исключений
                    String? imageUrl = null;
                    if(formModel.Image != null)
                    {
                        // но если данные передали, то проверяем полностью
                        imageUrl = _storageService.Save(formModel.Image);
                    }
                    // Разбираем данные на Товар и Версию
                    Guid productId = Guid.NewGuid();
                    _dataContext.Products.Add(new()
                    {
                        Id = productId,
                        GroupId = group.Id,
                        Name = formModel.Name,
                        Description = formModel.Description,
                        ImageUrl = imageUrl,
                        IsHidden = formModel.IsHidden,
                        OrderInPrice = formModel.Order,
                        Slug = formModel.Slug,
                    });
                    _dataContext.ProductVersions.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        ImageUrl = imageUrl,
                        Price = (decimal)formModel.Price,
                        Stock = formModel.Stock,
                        OrderInPrice = 1,
                        Slug = formModel.Slug,
                        IsHidden = formModel.IsHidden,
                    });
                    _dataContext.SaveChanges();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddGroup(AdminAddGroupFormModel formModel)
        {
            try
            {
                _dataContext.ProductGroups.Add(new()
                {
                    Id = Guid.NewGuid(),
                    ParentId = formModel.ParentId,
                    Name = formModel.Name,
                    Description = formModel.Description,
                    Slug = formModel.Slug,
                    IsHidden = formModel.IsHidden,
                    ImageUrl = "/storage/image/" + _storageService.Save(formModel.Image)
                });
                _dataContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
