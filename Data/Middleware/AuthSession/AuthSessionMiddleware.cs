using ASP_P42.Data;
using ASP_P42.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASP_P42.Data.Middleware.AuthSession
{
    // Классы Middleware должны иметь полную структуру:
    // - конструктор должен принимать ссылку на следующий Middleware - next
    // - для дальнейшего использования ссылки должно быть сохранено
    // - по условиям (или безусловно) работа должна передаваться дальше
    public class AuthSessionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(
            HttpContext context,    // инжекция через метод
            DataContext dataContext   // 
        )
        {
            String authKey = "userAccessId";
            // сначала спрашиваем, не запрошен ли выход (из авториз. режима)
            // про это свидетельствует наличие query-параметра "logout"
            //if (context.Request.Query.ContainsKey("logout"))
            //{
            //    // удаляем из сессии сохранённые данные 
            //    context.Session.Remove(authKey);
            //    // переадресовываем ответ на тот же адрес, с 
            //    // которого пришёл запрос
            //    // с целью "уборки" имеющегося query-параметра
            //    context.Response.Redirect(context.Request.Path);
            //    // останавливаем дальнейшую обработку данного запроса
            //    return;
            //}

            // context, который передаётся параметром, это тот самый
            // HttpContext? который доступен из контроллера 
            context.Items.Add("itemKey", "Item Value");

            // проверяем, есть ли в сессии элемент с ключом "userAccessId"
            //String authKey = "userAccessId";
            if (context.Session.Keys.Contains(authKey))
            {
                String userAccessId = context.Session.GetString(authKey)!;
                // єто должна быь валидная строка с БД
                UserAccess? userAccess = dataContext
                    .UserAccess
                    .Include(ua => ua.UserData)  // инструкция для заполнения
                    .Include(ua => ua.UserRole)  // навигационных особенностей
                    .AsNoTracking()              // Отключнение 
                    .FirstOrDefault(ua => ua.Id.ToString() == userAccessId);
                if (userAccess != null)
                {
                    // найдено подтверждение допуска, передаём к контексту 
                    // context.Items.Add(authKey, userAccess);
                    // Данный подход не рекомендованный, т.к 
                    // привязывается к типам данных сущностей.
                    // Реомендовано использовать унифицированный
                    // интерфейс с помощью Claims - набора атрибутов
                    // типового предназначения
                    context.User = new ClaimsPrincipal(  // представитель текущего пользователя в приложении
                        new ClaimsIdentity(  // набор информации об 1 авторизации
                            [
                            // 1 Claim - 1 характеристика пользователя
                                new Claim(ClaimTypes.Name, userAccess.UserData.FullName),
                                new (ClaimTypes.Email, userAccess.UserData.Email),
                                new (ClaimTypes.NameIdentifier, userAccess.Login),
                                new (ClaimTypes.Sid, userAccess.Id.ToString()),
                            ],
                            nameof(AuthSessionMiddleware)
                        )
                    );
                }
            }

            await _next(context);
            
        }
    }
}
