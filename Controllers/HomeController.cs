using System.Diagnostics;
using ASP_P42.Models;
using ASP_P42.Models.Home.Models;
using ASP_P42.Services.Hash;
using ASP_P42.Services.Kdf;
using Microsoft.AspNetCore.Mvc;


// принимаем форму
namespace ASP_P42.Controllers
{
    // primary constructor - прямо при оголошенні класу
    public class HomeController(IHashService hashService, IKdfService kdfService) : Controller
    {
        // Інжекція через конструктор у формі Primary
        private readonly IHashService _hashService = hashService;
        private readonly IKdfService _kdfService = kdfService;
        public IActionResult IoC()
        {
            String digest = _kdfService.Dk("96DCBBBA",
    "96DCBBBA-9AEE-44A2-8835-72DFE4E1A710");
            // передача даних до представлення
            // варіанти спільних ресурсів
            ViewBag.Hash = _hashService.GetHashCode();
            ViewData["digest"] = digest;
            return View();
        }

        public IActionResult Models(String? id)  // id - опціональний параметр маршруту
        {
            // Одна з центральних задач контролерів - підготовка і 
            // трансформація моделей
            HomeModelsViewModel viewModel = new()
            {
                PageTitle = "Моделі в ASP",
                Intro = "Модель (у MVC) - архітектурна частина проєкту, яка відповідає за \r\n    взаємодію з даними.\r\n    Модель (в ASP) - клас (об'єкт), призначений для передачі даних \r\n    (DTO - Data Transfer Object, Entity)",
                ClassificationHeader = "Розрізняють декілька типів моделей за призначенням:",
                ClassificationList = [
                    "Модель представлення (ViewModel або PageModel) - дані, з яких будується сторінка (або її частина - представлення)",
                    "Модель форми (FormModel) - дані, що заповнюються користувачем на сторінці і передаються на обробку.",
                    "Модель даних (DTO - Data Transfer Object, Entity) - дані, що  зберігаються на постійній основі, частіше за все у БД. ",
                ],
                ExampleHeader = "Наприклад, для моделі \"користувач\":",
                ExampleList = [
                    "Модель форми (реєстрація) - логін, пароль, повтор пароля, ...",
                    "Модель даних (у БД) - логін, DK(хеш паролю), сіль, ..., дата створення",
                    "Модель представлення (профіль або кабінет) - логін, ..., дата створення \r\n        (паролів немає)",
                ],
            };

            return id == "json" ? Json(viewModel) : View(viewModel);   // передаємо модель (об'єкт) до представлення
        }

        [HttpPost]  // Обмежуємо використання сторінки лише методом POST
        public IActionResult ModelsForm(HomeModelsFormModel formModel)
        {
            return View(formModel);
        }
        public IActionResult Index()
        {
            // ViewBag - способ передать данные из Controller во View 
            // ViewBag - свойство, которое есть у Controller, которое наследуется HomeController-ом
            // Я записала  context.Items.Add("itemKey", "Item Value") в AuthSessionMiddleware.cs , 
            // поэтому HomeController может взять это значение из HttpContext.Items
            ViewBag.ItemValue = HttpContext.Items["itemKey"];

            return View();
        }

        public IActionResult EntityFramework()
        {
            return View();
        }
        public IActionResult RazorPractice()
        {
            return View();
        }
        public IActionResult Razor()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
