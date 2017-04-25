using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using System.IO;
using MigrateVitalSignsData.Model;
using System.Data.SqlClient;
using MongoDB.Bson.Serialization.Attributes;
namespace MigrateVitalSignsData.Mappers
{
    public static class MappingHelper       
    {
        public static string SQLVitalsignsConnectionString;
        public static string SQLStatisticsConnectionString;
        public static string MongoConnectionString;

        public static string GetSourceNames(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName).GetCustomAttributes(false).Where(x => x.GetType() == typeof(BsonElementAttribute)).FirstOrDefault();
            if (property != null)
            {
                return ((BsonElementAttribute)property).ElementName;
            }
            return string.Empty;
        }

        /// <summary>
        /// Currently handles string, int, DateTime, decimal, double
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        public static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
            }
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    int result;
                    if (int.TryParse(value.ToString(), out result))
                    {
                        prop.SetValue(entity, result, null);
                    }
                    else
                    {
                        prop.SetValue(entity, null, null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<DateTime>))
            {
                DateTime date;
                bool isValid = DateTime.TryParse(value.ToString(), out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
                else
                {
                    //Making an assumption here about the format of dates in the source data.
                    isValid = DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.AssumeLocal, out date);
                    if (isValid)
                    {
                        prop.SetValue(entity, date, null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                prop.SetValue(entity, decimal.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                double number;
                bool isValid = double.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, double.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                bool boolVal;
                bool isValid = bool.TryParse(value.ToString(), out boolVal);
                if (isValid)
                {
                    prop.SetValue(entity, bool.Parse(value.ToString()), null);
                }
            }
        }

        public static DataTable ExecuteQuery(string query, bool vitalSignsDatabase = true)
        {
            var dataTable = new DataTable();
            string connectionString = vitalSignsDatabase ? SQLVitalsignsConnectionString : SQLStatisticsConnectionString;
         
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var sqlCommand = new SqlCommand(query, connection))
                {
                    var reader = sqlCommand.ExecuteReader();
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }

        public static MapperData GetMapingData(string fileName)
        {
            var mappingData = (MapperData)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("MappingData/" + fileName), typeof(MapperData));
            return mappingData;
        }
    }

   
}
