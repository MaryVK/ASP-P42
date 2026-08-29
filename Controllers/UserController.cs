using Microsoft.AspNetCore.Mvc;
using ASP_P42.Data;
using ASP_P42.Services.Kdf;
using System.Text;
using ASP_P42.Data.Entities;


namespace ASP_P42.Controllers
{
    public class UserController(
        DataContext dataContext,
        IKdfService kdfService
        ) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;
        // Аутенфикация - проверка логина и пароля
        public IActionResult BasicAuth()
        {
            // обратные действия к стандарту RFC 7617 'Basic' HTTP Authentication 
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if(authHeader == String.Empty)
            {
                return Unauthorized("Missing Authorization header");
            }
            String scheme = "Basic ";
            if(!authHeader.StartsWith(scheme))
            {
                return Unauthorized("Authorization scheme must be 'Basic'");
            }
            String credentials = authHeader[scheme.Length..];
            byte[] rawData;

            try
            {
                rawData = Convert.FromBase64String(credentials);
            }
            catch
            {
                return Unauthorized(
                    "Authorization credentials must be valid Base64::section 4");
            }
            String userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(rawData);
            }
            catch
            {
                return Unauthorized(
                   "User-pass must be valid UTF9 string");
            }

            String[] parts = userPass.Split(':', 2);
            if(parts.Length != 2)
            {
                return Unauthorized(
                  "User-pass must be concatenated by ':'");
            }

            String login = parts[0];
            String password = parts[1];
            // так как пароль в БД не сохраняется, юолее того,
            // средствами БД нельзя вычислить DK, проверка
            // происходит в 2 этапа:
            // 1. ищем в БД пользователя по логину (он уникальный)
            // 2. забираем соль данного пользователя и запускаем 
            //    вычисления ДК с переданным паролем и солью
            //    Результат вычисления должен совпадать с сохранением 
            //    ДК в БД
            //    

            if(_dataContext
                .UserAccess
                .FirstOrDefault(ua => ua.Login == login)
                is UserAccess userAccess)
            {
                String dk = _kdfService.Dk(password, userAccess.Salt);
                if (dk == userAccess.Dk)
                {
                    // точка положительного решения про аутенфикацию
                    // тут следует переходить к авторизации.
                    // Рассмотрим 2 способа: Автоматический через сессии и
                    // отдельный через токены.

                    // Серверная сессия (сеанс) - способ сохранения данных
                    // про запросы со стороны сервера с установлением для 
                    // запроса Cookie , которая играет роль токена доступа к
                    // сохранению данных.
                    // От клиента не требуется особенных действий, только 
                    // обеспечить стандартную работу кукки, если приложение
                    // вне браузера.
                    // Данные сохраняем в сессию
                    HttpContext.Session.SetString(
                        "userAccessId",
                        userAccess.Id.ToString()
                        );
                    // ответ может быть пустым 
                    return Ok();
                }
            }
            
            return Unauthorized(
                    "Credentials rejected: check login and password");
        }
    }
}
