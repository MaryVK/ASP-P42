using ASP_P42.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_P42.Controllers.Api
{
    [Route("api/group")]
    [ApiController]
    public class GroupController(DataContext dataContext) : ControllerBase
    {

        private readonly DataContext _dataContext;
        [HttpGet]  // Это запустится запросом GET /api/group

        public IEnumerable<Data.Entities.ProductGroup> GetAllGroups()
        {
            // возвращаем данные любого типа, они автоматически преобразуются на JSON
            return _dataContext.ProductGroups.Where(g => g.IsHidden == 0); 
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
