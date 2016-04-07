using Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IDisposable
    {
        private DataBaseContext _context = new DataBaseContext();
        private MasterRepository<Client> clientRepository;
        public MasterRepository<PropertyToMap> propertyToMapRepository;
        private bool disposed = false;

        public MasterRepository<Client> ClientRepository
        {
            get
            {

                if (this.clientRepository == null)
                {
                    this.clientRepository = new MasterRepository<Client>(_context);
                }
                return clientRepository;
            }
        }

        public MasterRepository<PropertyToMap> PropertyToMapRepository
        {
            get
            {

                if (this.propertyToMapRepository == null)
                {
                    this.propertyToMapRepository = new MasterRepository<PropertyToMap>(_context);
                }
                return propertyToMapRepository;
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
