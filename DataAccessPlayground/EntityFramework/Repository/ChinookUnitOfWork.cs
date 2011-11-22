using System;

namespace DataAccessPlayground.EntityFramework.Repository
{
    public class ChinookUnitOfWork : IChinookUnitOfWork, IDisposable
    {
        private readonly ChinookEntities _context;
        private IRepository<Invoice> _invoices;
        private IRepository<Customer> _customers;

        ChinookUnitOfWork()
        {
            _context = new ChinookEntities();
        }

        ChinookUnitOfWork(ChinookEntities context)
        {
            _context = context;
        }

        #region Implementation of IChinookUnitOfWork

        public IRepository<Invoice> Invoices
        {
            get { return (_invoices ?? (_invoices = new ObjectSetRepository<Invoice>(_context.Invoices))); }
        }

        public IRepository<Customer> Customers
        {
            get { return (_customers ?? (_customers = new ObjectSetRepository<Customer>(_context.Customers))); }
        }

        #endregion

        #region Implementation of IUnitOfWork

        public int Commit()
        {
            return _context.SaveChanges();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
