using ASP_P42.Data;
using ASP_P42.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
            String authKey = "usesrAccessId";
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
            // HttpContext? который доступный из контроллера 
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
                    context.Items.Add(authKey, userAccess);
                }
            }

            await _next(context);
            
        }
    }
}
