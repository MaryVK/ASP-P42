namespace ASP_P42.Services.Hash
{
    public static class HashServiceExtension
    {
        public static IServiceCollection AddHash(
            this IServiceCollection services)
        {
            return services.AddSingleton<IHashService, Md5HashService>();
            // return services.AddScoped<IHashService, Md5HashService>();
            // return services.AddTransient<IHashService, Md5HashService>();
        }
    }
}

/* Д.З. Реалізувати службу (сервіс) часу, яка дозволяє
 * запитати поточну мітку часу (timestamp)
 * Вивести на сторінку сайту саму мітку часу та хеш-код служби
 * Переконатись, що з оновленням сторінки час змінюється, 
 * а служба - ні. (додати скріншоти)
 */