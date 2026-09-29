using ASP_P42.Data;
using ASP_P42.Data.Entities;
using ASP_P42.Models.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers.Api
{
    [Route("api/group")]
    [ApiController]
    public class GroupController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;

        [HttpGet]  // Это запустится запросом GET /api/group

        
       
        public RestResponse GetAllGroups(int page = 1, int pageSize = 10)
        {

            var query = _dataContext
                .ProductGroups
                .Where(g => g.IsHidden == 0 && g.ParentId == null)
                .OrderBy(g => g.OrderInPrice);

            int cnt = query.Count();

            RestMetaPagination pagination = new()
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = cnt,
                TotalPages = (int)Math.Ceiling((float)cnt / pageSize),
            };

            ProductGroup[] groups = query.Skip(pageSize * (page - 1)).Take(pageSize).ToArray();

            foreach(var group in groups)
            {
                if(group.ImageUrl.StartsWith('/'))
                {
                    group.ImageUrl = $"{Request.Scheme}://{Request.Host}{group.ImageUrl}";
                }
            }
            // возвращаем данные любого типа, они автоматически преобразуются на JSON
            return new()
            {
                Meta = new()
                {
                    ApiName = "Product Groups",
                    DataType = "json/array",
                    CacheTime = 86_400_000,
                    Manipulations = ["GET"],
                    Links =
                    {
                        { "self", "/api/group" },
                        { "sub", "/api/group/{slug}" },
                    },
                    Pagination = pagination,
                },
                Data = groups,
            };
        }

        [HttpPost]  // это запустится запросом POST /api/group
        public bool CreateNewGroup(Data.Entities.ProductGroup group)
        {
            return true;
        } 
    }
}

/*
 API контроллеры, отличия от MVC

API - Application Program Interface
(интерфейс взаимодействия программы с приложением)
Проект (программный комплекс) мы условно делим на 
Program - "центральную" часть, которая отвечает за 
сохрание данных
Application (приложения) - отдельные программы, которые 
взаимодействуют с пользователем и центральной Программой 


                   Program (ASP)
               /           |           \
              API         API         API
   React(web-frontend)   Mobile       Desktop


Так как Программа напрямую не контактирует с человеком,
API предвидит передачу "сырых" данных, тогда как MVC
имеет представление (View), призначенные для человека.
- MVC представляет IActionResult, среди которых есть JSON, но основой является представление
- API сразу возвращает JSON и не может создавать представления

Маршрутизация:
- MVC делит запрос по адрсу -- pattern: "{controller=Home}/{action=Index}/{id?}"
    независимо от метода запроса (GET, POST, ...) -- GET /path u POST 




 */
