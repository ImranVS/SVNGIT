using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MigrateVitalSignsData.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace MigrateVitalSignsData.Mappers
{
    public class Mapper<TEntity>  where TEntity : class, new()
    {
        public readonly MapperData MapperData;
        public readonly bool VitalSignsDatabase;
        public Mapper(string fileName, bool vitalsignsDatabase = true)
        {
            MapperData = MappingHelper.GetMapingData(fileName);
            VitalSignsDatabase = vitalsignsDatabase;
        }
        public List<TEntity> Map()
        {
            DataTable table = MappingHelper.ExecuteQuery(MapperData.SQLQuery, VitalSignsDatabase);
            List<TEntity> entities = new List<TEntity>();
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var properties = (typeof(TEntity)).GetProperties().Where(x => x.GetCustomAttributes(typeof(BsonElementAttribute), true).Any())
                                              .ToList(); //Only get properties that have the SourceNamesAttribute; ignore others
            foreach (DataRow row in table.Rows)
            {
                TEntity entity = new TEntity();
                foreach (var prop in properties)
                {
                    Map(typeof(TEntity), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }

        public void Map(Type type, DataRow row, PropertyInfo prop, object entity)
        {
            string fieldName = MappingHelper.GetSourceNames(type, prop.Name);
         
            var result = MapperData.Mappings.FirstOrDefault(x => x.MongoDbColumn == fieldName);
            if(result!=null)
          
                if (!string.IsNullOrEmpty(result.SQLColumn) && row.Table.Columns.Contains(result.SQLColumn))
                {
                    var propertyValue = row[result.SQLColumn];
                    if (propertyValue != DBNull.Value)
                    {
                        MappingHelper.ParsePrimitive(prop, entity, row[result.SQLColumn]);
                    }
               }
            
        }
    }
}
