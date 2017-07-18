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
        private TimeSpan dateTimeOffset = new TimeSpan(100, 100, 100);
        private Boolean isService = false; 

        public List<String> serviceProcessNames = new List<String>() {
            "VitalSignsAlertService",
            "VitalSignsConsoleCommandProcessor",
            "VitalSignsCore64Bit",
            "VitalSignsDailyTasks",
            "VitalSignsDBHealth",
            "VitalSignsMasterService",
            "VitalSignsMicrosoft",
            "VitalSignsPlusCore",
            "VitalSignsPlusDomino",
            "VitalSignsPlusEXJournal",
            "TestMicrosoftServices.vshost"
        };

        public Repository(string connectionString,int? tenantId=null)
        {
            //read from machine
            Collection = Database<T>.GetCollectionFromConnectionString(connectionString);
            TenantId = tenantId;


            if(dateTimeOffset == new TimeSpan(100, 100, 100))
            {
                DateTime now = DateTime.Now;
                dateTimeOffset = new TimeSpan(now.ToLocalTime().Ticks - now.ToUniversalTime().Ticks);

            }
            string currProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            isService = serviceProcessNames.Contains(System.Diagnostics.Process.GetCurrentProcess().ProcessName);// System.Environment.StackTrace.Contains("RPRWyatt.VitalSigns.Services.VSServices.OnStart") || System.Environment.StackTrace.Contains("TestMicrosoftServices");
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
            return ConvertDateTimes(Query(filter).ToEnumerable().Select(x => (Object)x)).Select(x => (T)x);
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter)
        {
            return ConvertDateTimes(Query(filter).ToEnumerable().Select(x => (Object)x)).Select(x => (T)x);
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter, ProjectionDefinition<T> projection)
        {
            return ConvertDateTimes(Query(filter).Project<T>(projection).ToEnumerable().Select(x => (Object)x)).Select(x => (T)x);
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
            return ConvertDateTimes((isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable().Select(x => (Object)x)).Select(x => (T)x);
        }

        public virtual IEnumerable<T> Find(FilterDefinition<T> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending)
        {
            var query = Query(filter).Skip(pageIndex * size).Limit(size);
            return ConvertDateTimes((isDescending ? query.SortByDescending(order) : query.SortBy(order)).ToEnumerable().Select(x => (Object)x)).Select(x => (T)x);
        }

        public virtual string Insert(T entity)
        {
            entity.TenantId = TenantId;
            Collection.InsertOne(entity);
            return entity.Id;
        }

        public virtual void Insert(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.TenantId = TenantId;
            Collection.InsertMany(entities);
        }

        public virtual void Replace(T entity, UpdateOptions updateOptions, FilterDefinition<T> filterDef)
        {
            Collection.ReplaceOne(filterDef, entity, updateOptions);
        }

        public virtual void Replace(T entity, UpdateOptions updateOptions)
        {
            Replace(entity, updateOptions, Filter.Eq(x => x.Id, entity.Id));
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
        public void Delete()
        {
            Delete();
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

        #region SupportingClasses

        private IEnumerable<Object> ConvertDateTimes(IEnumerable<Object> entities)
        {
            //return entities;
            if (!isService) return entities;
            if (entities.Count() == 0)
                return entities;

            System.Reflection.PropertyInfo[] properties = entities.First().GetType().GetProperties().Where(i => (((Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType) == typeof(DateTime)) && i.GetSetMethod() != null) || i.PropertyType.Namespace == "System.Collections.Generic").ToArray();
            var g = entities.First().GetType().GetProperties();
            List<Object> listOfEntities = entities.ToList();
            foreach(Object entity in listOfEntities)
            {
                foreach(System.Reflection.PropertyInfo prop in properties)
                {
                    if (prop.PropertyType.Namespace == "System.Collections.Generic")
                    {
                        try
                        {
                            //working!!!! ?
                            var val =  prop.GetValue(entity) as System.Collections.IEnumerable;
                            if (val != null)
                            {
                                var valEnumerable = val.Cast<Object>().AsEnumerable();
                                ConvertDateTimes(valEnumerable);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        DateTime? dt = (DateTime?)(prop.GetValue(entity));
                        if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
                        {
                            prop.SetValue(entity, dt.Value.ToLocalTime());
                        }
                        /*
                        DateTime? dt = (DateTime?)(prop.GetValue(entity));
                        if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
                        {
                            prop.SetValue(entity, dt.Value.Add(dateTimeOffset));
                        }
                        DateTime? dt2 = (DateTime?)(prop.GetValue(entity));
                        */
                    }
                }
            }
            return listOfEntities;
        }

        private Object ConvertDateTimesForReplace(T entity)
        {
            //return entities;
            if (!isService) return entity;

            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties().Where(i => (((Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType) == typeof(DateTime)) && i.GetSetMethod() != null) || i.PropertyType.Namespace == "System.Collections.Generic").ToArray();

            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (prop.PropertyType.Namespace == "System.Collections.Generic")
                {
                    try
                    {
                        //working!!!! ?
                        var val = prop.GetValue(entity) as System.Collections.IEnumerable;
                        if (val != null)
                        {
                            var valEnumerable = val.Cast<Object>().AsEnumerable();
                            ConvertDateTimesForReplace(valEnumerable);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {

                    DateTime? dt = (DateTime?)(prop.GetValue(entity));
                    if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
                    {
                        prop.SetValue(entity, dt.Value.ToLocalTime());
                    }
                    /*
                    DateTime? dt = (DateTime?)(prop.GetValue(entity));
                    if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
                    {
                        prop.SetValue(entity, dt.Value.Subtract(dateTimeOffset));
                    }
                    DateTime? dt2 = (DateTime?)(prop.GetValue(entity));
                    */
                }
            }
            
            return entity;

        }

        private IEnumerable<Object> ConvertDateTimesForReplace(IEnumerable<Object> entities)
        {
            //return entities;
            if (!isService) return entities;
            if (entities.Count() == 0)
                return entities;

            System.Reflection.PropertyInfo[] properties = entities.First().GetType().GetProperties().Where(i => (((Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType) == typeof(DateTime)) && i.GetSetMethod() != null) || i.PropertyType.Namespace == "System.Collections.Generic").ToArray();
            var g = entities.First().GetType().GetProperties();
            List<Object> listOfEntities = entities.ToList();
            foreach (Object entity in listOfEntities)
            {
                foreach (System.Reflection.PropertyInfo prop in properties)
                {
                    if (prop.PropertyType.Namespace == "System.Collections.Generic")
                    {
                        try
                        {
                            //working!!!! ?
                            var val = prop.GetValue(entity) as System.Collections.IEnumerable;
                            if (val != null)
                            {
                                var valEnumerable = val.Cast<Object>().AsEnumerable();
                                ConvertDateTimesForReplace(valEnumerable);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        DateTime? dt = (DateTime?)(prop.GetValue(entity));
                        if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
                        {
                            prop.SetValue(entity, dt.Value.Subtract(dateTimeOffset));
                        }
                        DateTime? dt2 = (DateTime?)(prop.GetValue(entity));
                    }
                }
            }
            return listOfEntities;
        }


        //private T ConvertDateTimesForReplace(T entity)
        //{
        //    //return entities;
        //    if (!isService) return entity;

        //    System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties().Where(i => ((Nullable.GetUnderlyingType(i.PropertyType) ?? i.PropertyType) == typeof(DateTime)) && i.GetSetMethod() != null).ToArray();
        //    foreach (System.Reflection.PropertyInfo prop in properties)
        //    {

        //        DateTime? dt = (DateTime?)(prop.GetValue(entity));
        //        if (dt.HasValue && !dt.Value.Equals(DateTime.MinValue))
        //        {
        //            prop.SetValue(entity, dt.Value.Subtract(dateTimeOffset));
        //        }
        //        DateTime? dt2 = (DateTime?)(prop.GetValue(entity));

        //    }

        //    return entity;
        //}


        #endregion
    }

    public class Repository
    {

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;
        public Repository(string connectionString, string databaseName, string collectionName)
        {
            _client = new MongoClient(connectionString.EndsWith("/") ? connectionString + databaseName : connectionString + "/" + databaseName);
            _database = _client.GetDatabase(databaseName);
            _collectionName = collectionName;
        }

        public IMongoCollection<BsonDocument> Collection
        {
            get { return _database.GetCollection<BsonDocument>(_collectionName); }
        }
    }
}
