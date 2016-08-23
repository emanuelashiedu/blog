namespace Repository
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        int Save(TEntity entity);
        int Delete(TEntity entity);
        int Update(TEntity entity);
        TEntity SaveRecord(TEntity entity);
        TEntity UpdateRecord(TEntity entity);
         
    }
}