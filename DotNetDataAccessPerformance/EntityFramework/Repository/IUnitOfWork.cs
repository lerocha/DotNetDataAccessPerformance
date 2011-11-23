namespace DotNetDataAccessPerformance.EntityFramework.Repository
{
    public interface IUnitOfWork
    {
        int Commit();
    }
}
