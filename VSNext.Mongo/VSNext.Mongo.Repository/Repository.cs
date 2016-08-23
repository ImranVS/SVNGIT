using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;

namespace VSNext.Mongo.Repository
{
    public class Repository<T> : IRepository<T>
         where T : IEntity
    {
        #region MongoSpecific
        public Repository(string connectionString,int? tenantId=null)
        {
            //read from machine
            Collection = Database<T>.GetCollectionFromConnectionString(connectionString);
            TenantId = tenantId;
        }

        public Repository()
        {
        }

        public IMongoCollection<T> Collection
        {
            get; private set;
        }
        public int? TenantId
        {
            get; private set;
        }

        public FilterDefinitionBuilder<T> Filter
        {
            get
            {
                return Builders<T>.Filter;
            }
        }

        public UpdateDefinitionBuilder<T> Updater
        {
            get
            {
                return Builders<T>.Update;
            }
        }

        public ProjectionDefinitionBuilder<T> Project
        {
            get
            {
                return Builders<T>.Projection;
            }
        }

        private IFindFluent<T, T> Query(Expression<Func<T, bool>> filter)
        {
            return Collection.Find(filter);
        }

        private IFindFluent<T, T> Query(FilterDefinition<T> filter)
        {
            return Collection.Find(filter);
        }

        #endregion MongoSpecific

        #region CRUD
        public virtual T Get(string id)
        {
            return Find(i => i.Id == id).FirstOrDefault();
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter)
        {
            return Query(filter).ToEnumerable();
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter)
        {
            return Query(filter).ToEnumerable();
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            return Query(filter).Project<T>(projection).ToEnumerable();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size)
        {
            return Find(filter, i => i.Id, pageIndex, size);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size)
        {
            return Find(filter, order, pageIndex, size, true);
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query(filter).Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query(filter).Skip(pageIndex * size).Limit(size);
            return (isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable();
        }

        public virtual void Insert(T entity)
        {
            entity.TenantId = TenantId;
            Collection.InsertOne(entity);
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            Collection.InsertMany(entities);
        }

        public virtual void Replace(T entity, UpdateOptions updateOptions)
        {
            Collection.ReplaceOne(i => i.Id == entity.Id, entity, updateOptions);
        }

        public void Replace(IEnumerable<T> entities, UpdateOptions updateOptions)
        {
            foreach (T entity in entities)
            {
                Replace(entity, updateOptions);
            }
        }

        public virtual void Replace(T entity)
        {
            Replace(entity, null);
        }

        public void Replace(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                Replace(entity);
            }
        }

        public void BulkUpsert(IEnumerable<UpdateOneModel<T>> entities)
        {
            Collection.BulkWrite(entities);
        }

        public bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value)
        {
            return Update(entity, Updater.Set(field, value));
        }

        public virtual bool Update(T entity, UpdateDefinition<T> update)
        {
            return Update(Filter.Eq(i => i.Id, entity.Id), update);
        }

        public bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value)
        {
            return Update(filter, Updater.Set(field, value));
        }

        public bool Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return Update(filter, update, updateOptions: null);
        }
        public bool Update(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions updateOptions = null)
        {
            return Collection.UpdateMany(filter, update.CurrentDate(i => i.ModifiedOn), updateOptions).ModifiedCount > 0;
        }


        public void Delete(T entity)
        {
            Delete(entity.Id);
        }

        public virtual void Delete(string id)
        {
            Collection.DeleteOne(i => i.Id == id);
        }

        public void Delete(Expression<Func<T, bool>> filter)
        {
            Collection.DeleteMany(filter);
        }

        public void Delete(FilterDefinition<T> filter)
        {
            Collection.DeleteMany(filter);
        }

        public bool Upsert(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return Update(filter, update.SetOnInsert(i => i.CreatedOn, DateTime.Now), updateOptions: new UpdateOptions { IsUpsert = true });
            
        }
        #endregion CRUD

        #region Simplicity
        public bool Any(Expression<Func<T, bool>> filter)
        {
            return Collection.AsQueryable<T>().Any(filter);
        }
        public IEnumerable<T> All()
        {
            return Collection.AsQueryable<T>().AsEnumerable<T>();
        }
        #endregion Simplicity
    }

    public class Repository
    {

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;
        public Repository(string connectionString, string databaseName, string collectionName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
            _collectionName = collectionName;
        }

        public IMongoCollection<BsonDocument> Collection
        {
            get { return _database.GetCollection<BsonDocument>(_collectionName); }
        }
    }
}
