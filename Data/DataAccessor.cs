namespace ASP_P42.Data
{
    public class DataAccessor(DataContext dataContext)
    {
        private readonly DataContext _dataContext = dataContext;

        public Guid GetDbIdentity() => Guid.NewGuid();
        public List<Entities.ProductGroup> GetAllProductGroups(bool isIncludeHidden = false)
        {
            IQueryable<Entities.ProductGroup> query = _dataContext.ProductGroups;

            if(!isIncludeHidden)
            {
                query = query.Where(g => g.IsHidden == 0);
            }
            return [.. query.OrderBy(g => g.Products)];
                
        }

        internal async Task<Guid> AddNewProductGroup(Entities.ProductGroup group)
        {
            throw new NotImplementedException();
        }
    }
}

/* DAL - Data Access Layer
 * Слой доступа к данным - соединение нескольких DAO
 * (Data Access Object) или обобщённый интерфейс получения 
 * данных
 * 
 */