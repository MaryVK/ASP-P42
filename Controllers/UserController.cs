using Microsoft.AspNetCore.Mvc;
using ASP_P42.Data;
using ASP_P42.Services.Kdf;
using System.Text;
using ASP_P42.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ASP_P42.Models.User;
using System.Text.RegularExpressions;


namespace ASP_P42.Controllers
{
    public class UserController(
        DataContext dataContext,
        IKdfService kdfService
        ) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;



        // Регистрация по данным, которые поступают из фронтенда (JSON)
        public IActionResult SignUp([FromBody]UserSignUpFormModel formModel)
        {   
            // Валидация модели - проверка данных на допустимость
            // Добавляем нового юзера

            if(formModel == null)
            {
                return BadRequest("Data structure non-bindable to model");
            }
            if (! formModel.IsAgree)
            {
                return BadRequest("You should confirm site policy (agreement)");
            }
            String requiredMessage = " could not be empty";
            if (String.IsNullOrEmpty(formModel.Login))
            {
                return BadRequest(nameof(formModel.Login) + requiredMessage);
            }

            if (String.IsNullOrEmpty(formModel.FullName))
            {
                return BadRequest(nameof(formModel.FullName) + requiredMessage);
            }

            if (String.IsNullOrEmpty(formModel.Email))
            {
                return BadRequest(nameof(formModel.Email) + requiredMessage);
            }

            if (String.IsNullOrEmpty(formModel.Phone))
            {
                return BadRequest(nameof(formModel.Phone) + requiredMessage);
            }

            if(formModel.Password != formModel.Repeat)
            {
                return BadRequest("Password and Repeat mismatch");
            }

            // проверка следующей сложности - соответствие форматаm, а также предыдущая обработка
            formModel.FullName = formModel.FullName.Trim();
            if(formModel.FullName.Length < 2)
            {
                return BadRequest(nameof(formModel.FullName) + " too short (2 symbols at least)");
            }

            formModel.Login = formModel.Login.Trim();
            if (formModel.Login.Length < 2)
            {
                return BadRequest(nameof(formModel.Login) + " too short (2 symbols at least)");
            }

  
            if (formModel.Login.Contains (':'))
            {
                return BadRequest(nameof(formModel.Login) + " could not contain colon (':')");
            }


            formModel.Email = formModel.Email.Trim();
            if (!Regex.IsMatch(
                formModel.Email,
                @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
            ))
            {
                return BadRequest(nameof(formModel.Email) + " has invalid format");
            }

            // самая сложная проверка - с привлечением БД

            if(_dataContext.UserAccess.Any(ua => ua.Login == formModel.Login))
            {
                BadRequest(nameof(formModel.Login));
            }

            Guid userId = Guid.NewGuid();
            _dataContext.UsersData.Add(new()
            {
                Id = userId,
                FullName = formModel.FullName,
                Email = formModel.Email,
                Phone = formModel.Phone,
                RegisteredAt = DateTime.Now,
                BirthDate = default,
            });
            String salt = Guid.NewGuid().ToString();
            _dataContext.UserAccess.Add(new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = _dataContext.UsersRoles.First(r => r.Name == "User").Id,
                Login = formModel.Login,
                Salt = salt,
                Dk = _kdfService.Dk(formModel.Password, salt),
            });
            _dataContext.SaveChanges();                
            return Json(formModel);
        }

       
        // Аутенфикация - проверка логина и пароля
        public IActionResult BasicAuth()
        {
            UserAccess? userAccess;
            try
            {
                userAccess = AuthenticateUser();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if(userAccess == null)
            {
                return Unauthorized(
                    "Credentials rejected: check login and password");
            }
            //// обратные действия к стандарту RFC 7617 'Basic' HTTP Authentication 
            //String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            //if (authHeader == String.Empty)
            //{
            //    return Unauthorized("Missing Authorization header");
            //}
            //String scheme = "Basic ";
            //if (!authHeader.StartsWith(scheme))
            //{
            //    return Unauthorized("Authorization scheme must be 'Basic'");
            //}
            //String credentials = authHeader[scheme.Length..];
            //byte[] rawData;

            //try
            //{
            //    rawData = Convert.FromBase64String(credentials);
            //}
            //catch
            //{
            //    return Unauthorized(
            //        "Authorization credentials must be valid Base64::section 4");
            //}
            //String userPass;
            //try
            //{
            //    userPass = Encoding.UTF8.GetString(rawData);
            //}
            //catch
            //{
            //    return Unauthorized(
            //       "User-pass must be valid UTF9 string");
            //}

            //String[] parts = userPass.Split(':', 2);
            //if (parts.Length != 2)
            //{
            //    return Unauthorized(
            //      "User-pass must be concatenated by ':'");
            //}

            //String login = parts[0];
            //String password = parts[1];
            //// так как пароль в БД не сохраняется, юолее того,
            //// средствами БД нельзя вычислить DK, проверка
            //// происходит в 2 этапа:
            //// 1. ищем в БД пользователя по логину (он уникальный)
            //// 2. забираем соль данного пользователя и запускаем 
            ////    вычисления ДК с переданным паролем и солью
            ////    Результат вычисления должен совпадать с сохранением 
            ////    ДК в БД
            ///
            ////    

            //if (_dataContext
            //    .UserAccess
            //    .FirstOrDefault(ua => ua.Login == login)
            //    is UserAccess userAccess)
            //{
            //    String dk = _kdfService.Dk(password, userAccess.Salt);
            //    if (dk == userAccess.Dk)
            //    {
            //        // точка положительного решения про аутенфикацию
            //        // тут следует переходить к авторизации.
            //        // Рассмотрим 2 способа: Автоматический через сессии и
            //        // отдельный через токены.

            //        // Серверная сессия (сеанс) - способ сохранения данных
            //        // про запросы со стороны сервера с установлением для 
            //        // запроса Cookie , которая играет роль токена доступа к
            //        // сохранению данных.
            //        // От клиента не требуется особенных действий, только 
            //        // обеспечить стандартную работу кукки, если приложение
            //        // вне браузера.
            //        // Данные сохраняем в сессию
                    HttpContext.Session.SetString(
                        "userAccessId",
                        userAccess.Id.ToString()
                        );
                    // ответ может быть пустым 
                    return Ok();
                }
            

            
        

        public IActionResult BasicAuthJwt()
        {
            UserAccess? userAccess;
            try
            {
                userAccess = AuthenticateUser();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (userAccess == null)
            {
                return Unauthorized(
                    "Credentials rejected: check login and password");
            }

            // Формируем токен 
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            long time = (DateTime.Now.Ticks - DateTime.UnixEpoch.Ticks) / 100000;

            var payload = new
            {
                sub = userAccess.Login,
                iat = time,
                exp = time + 1000000,
                name = userAccess.UserData.FullName,
                email = userAccess.UserData.Email
            };
            String body = Base64UrlTextEncoder.Encode(
                Encoding.UTF8.GetBytes(
                    JsonSerializer.Serialize(header)))
                + "." +
                Base64UrlTextEncoder.Encode(
                    Encoding.UTF8.GetBytes(
                        JsonSerializer.Serialize(payload)));

            String signature = Base64UrlTextEncoder.Encode(
                System.Security.Cryptography.HMACSHA256.HashData(
                    Encoding.UTF8.GetBytes("secret"),
                    Encoding.UTF8.GetBytes(body)
            ));

            return Ok(body + "." + signature);
        }

        private UserAccess? AuthenticateUser()
        {
            // обратные действия к стандарту RFC 7617 'Basic' HTTP Authentication 
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if (authHeader == String.Empty)
            {
                throw new Exception("Missing Authorization header");
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                throw new Exception("Authorization scheme must be 'Basic'");
            }
            String credentials = authHeader[scheme.Length..];
            byte[] rawData;

            try
            {
                rawData = Convert.FromBase64String(credentials);
            }
            catch
            {
                throw new Exception(
                    "Authorization credentials must be valid Base64::section 4");
            }
            String userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(rawData);
            }
            catch
            {
                throw new Exception(
                   "User-pass must be valid UTF9 string");
            }

            String[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new Exception(
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

            if (_dataContext
                .UserAccess
                .Include( ua => ua.UserData)
                .Include(ua => ua.UserRole)
                .AsNoTracking()
                .FirstOrDefault(ua => ua.Login == login)
                is UserAccess userAccess)
            {
                String dk = _kdfService.Dk(password, userAccess.Salt);
                if (dk == userAccess.Dk)
                {
                    _dataContext.AuthJournals.Add(new AuthJournal
                    {
                        DateTime = DateTime.Now,
                        Login = login,
                        Dk = dk,
                        IsOk = true

                    });

                    _dataContext.SaveChanges();

                    return userAccess;
                }

                _dataContext.AuthJournals.Add(new AuthJournal
                {
                    DateTime = DateTime.Now,
                    Login = login,
                    Dk = dk,
                    IsOk = false
                });

                _dataContext.SaveChanges();
            }
            return null;
        }
    }
}
