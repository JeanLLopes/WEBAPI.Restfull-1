using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBAPI.Restful.DataModel
{
    public class UnitOfWork : IDisposable
    { 
        //PRIVATES VARIABLES
        private DB_WEBAPIEntities _context = null;
        private GenericRepository<USER> _userRepository;
        private GenericRepository<PRODUCTS> _productRepository;
        private GenericRepository<TOKENS> _tokenRepository;


        //contructor
        public UnitOfWork()
        {
            _context = new DB_WEBAPIEntities();
        }



        //CREATE REOSITORY PROPERTIES
        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        public GenericRepository<PRODUCTS> ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new GenericRepository<PRODUCTS>(_context);
                return _productRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<USER> UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new GenericRepository<USER>(_context);
                return _userRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<TOKENS> TokenRepository
        {
            get
            {
                if (_tokenRepository == null)
                    _tokenRepository = new GenericRepository<TOKENS>(_context);
                return _tokenRepository;
            }
        }





        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion


        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
