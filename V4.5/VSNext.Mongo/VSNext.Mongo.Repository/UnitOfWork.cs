using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNext.Mongo.Repository
{
  public  class UnitOfWork
    {
        private string _connectionString;
        private readonly int? _tenantId;
        public UnitOfWork(string connectionString, int? tenantId)
        {
            _connectionString = connectionString;
            _tenantId = tenantId;
        }
        private Dictionary<string, object> repositories;
        public Repository<T> Repository<T>() where T : IEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _connectionString,_tenantId);
                repositories.Add(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }
    }
}
