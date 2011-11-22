namespace DataAccessPlayground.EntityFramework.Repository
{
    public interface IUnitOfWork
    {
        int Commit();
    }
}
