using ASP_P42.Data;
using ASP_P42.Data.Middleware.AuthSession;
using ASP_P42.Services.Hash;
using ASP_P42.Services.Kdf;
using ASP_P42.Services.Storage;
using ASP_P42.Services.Time;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHash();
builder.Services.AddKdf();
builder.Services.AddTimeService();
builder.Services.AddStorage();
// БД додається як сервіс, але специфічним методом-розширенням
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LocalDB")
    )
);
builder.Services.AddScoped<DataAccessor>();

// настройка сессий 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Настройка CORS
// CORS (Cross-Origin Resource Sharing) - «обмен ресурсами между разными источниками»
// это механизм, который управляет тем, может ли один сайт
// обращаться к серверу, который находится на другом адресе(origin)

// POSTMAN игнорирует CORS, потому что CORS - это в первую очередь ограничение 
//                          ограничение браузера, а не запрет самого
//                          HTTP-запроса
// POSTMAN - программа для отправки HTTP-запросов к бэкенд и проверка ответа
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy => 
        policy
        .AllowAnyOrigin()    // открытый API - для всех пользователей
        .AllowAnyHeader()    // разрешаем все заголовки 
        .AllowAnyMethod()    // все методы запроса
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseAuthorization();
app.MapStaticAssets();

app.UseSession();  // включаем сессии
// добавляем custom middleware

app.UseAuthSession(); //комментирую для подтверждения
//                      работы                     
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

/*
 * REST - (Representational State Transfer) - это архитектурный стиль для
 * разработки веб-сервисов.
 * Это набор принципов и ограничений, и т.д
 * 1) Структура проекта
 * 2) взаимодействия между клиентом и сервером
 * 
 * - Stateless - отсутствует положение на сервере (нет сохранения данных) - 
 *   нет сессии
 * 
 * все данные, которые должны быть связаны с пользователем, переходят
 * из сервера на клиентский бок (к запросам).
 * Это приводит к появлению стандартов токенов, которые передают указанные данные.
 * 
 * -  Cacheable - кеширование данных на стороне клиента (браузера) - 
 *    ответ сервера должен содержать информацию про то, можно ли кешировать данные или нет.
 * 
 * - Layered system - не должна обязательно определять, происходит ли подключение ДО сервера,
 * или через промежуточный сервер (прокси)
 * 
 * [client] <---------> [proxy} <--------->[server]
 * GET /item/123        GET /item/123 <---------> 404 Not Found   
 *                      ошибка запроса
 *              <--------  500 International Server Error
 *              
 *                    заголовки ответа
 *                       Server: nginx             Server: Kestrel
 *                       Data: 1234567891          Date: 1234567890
 * 
 * Server? 
 * Date ?
 * ========= Данное требование нуждается в пересмотре принципов формирования ответов.
 * 
 * - Uniform interface - унифицированный интерфейс взаимодействия между клиентом и сервером
 *     Все запросы клиента и ответы сервера должны быть унифицированными,
 *     тоесть иметь однотипную структуру.
 *   = Resourse identification in requests - адрес запроса должен указыывать на ресурс
 *   = Resourse manipulation throught representations - ответ должен содержать метаданные,
 *         которые указывают возможность манипуляций с ресурсом (например, GET, POST, PUT, DELETE)
 *   = Self-descriptive messages - ответ должен содержать информацию про тип уведомления
 *        (про тип данных, которые передаются)
 *   = Hypermedia as the engine of application state (HATEOAS) - ответ должен содержать
 *        ссылку на внутренние ресурсы, если такие есть (показывать своё содержание)
 *   
 *   
 *   -- позитивный момент - добавлять данные про параметры, которые возможны для запроса
 *            "parameters": {
 *                "id": {"type": "string", value: "123"},
 *                "sort": {"type": "string", value: "asc"},
 *                "region": {"type": "string", value: "Odessa"},
 *            }
 *   -- если поддерживается локализация, то добавляется информация про язык ответа
 *            "locale": "en_US"
 *        },
 *        data: { "id": 123, "name": "item name" }
 *  }
*/         





























