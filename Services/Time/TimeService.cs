namespace ASP_P42.Services.Time
{
    public class TimeService : ITimeService
    {
        public long GetTimestamp()
        {          // получаю текущее время
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();  // преобразует в timestamp (уникальный идентификатор, 
        }                                                           // который показывает точное время определенного события.)
    }
}
