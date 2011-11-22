namespace DataAccessPlayground.EntityFramework.Repository
{
    public interface IChinookUnitOfWork : IUnitOfWork
    {
        IRepository<Invoice> Invoices { get; }
        IRepository<Customer> Customers { get; } 
    }
}
