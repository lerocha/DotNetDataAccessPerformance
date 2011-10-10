using DataAccessPlayground.Model;
using DataAccessPlayground.Repository;

namespace DataAccessPlayground.UnitOfWork
{
    public interface IChinookUnitOfWork : IUnitOfWork
    {
        IRepository<Invoice> Invoices { get; }
        IRepository<Customer> Customers { get; } 
    }
}
