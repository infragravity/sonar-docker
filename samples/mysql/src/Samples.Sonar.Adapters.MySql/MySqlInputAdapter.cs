using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infragravity.Sonar.Abstractions;
using MySql;
using MySql.Data.MySqlClient;

namespace Samples.Sonar.Adapters.MySql
{
    public class MySqlInputAdapter : IInputAdapter
    {
        private readonly string _connectionString;
        private readonly string _address;
         private readonly string _alias;
        public MySqlInputAdapter(ConnectionStringSettings connectionSettings)
        {    
            if(null == connectionSettings)
                throw new ArgumentNullException(nameof(connectionSettings));        
            _connectionString = connectionSettings.ConnectionString;
            if(string.IsNullOrEmpty(_connectionString))
                throw new ApplicationException(String.Format("Connection string is empty in {0}",connectionSettings.Name));
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connectionString);
            if(string.IsNullOrEmpty(builder.DataSource))
                throw new ApplicationException(String.Format("Data Source is empty in {0}",connectionSettings.Name));
            _address = builder.DataSource.Split(',').FirstOrDefault();
            if(string.IsNullOrEmpty(_address))
                throw new ApplicationException(String.Format("Invalid address property in {0}",connectionSettings.Name));
            _alias = connectionSettings.Name;
        }
        public string Address
        {
            get{return _address;}
        }
        public string Alias
        {
            get{return _alias;}
        }
        public IInputAdapterResult<EnumerationObject> Enumerate(string resource, string namespaceName, string filter, int maxItems, HybridDictionary valuesMap)
        {
            return new InputAdapterResult(GetTableResponse(filter, _connectionString));
        }
        public Task<IEnumerable<EnumerationObject>> EnumerateAsync(string resource, string filter, int maxItems)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<EnumerationObject> GetTableResponse(string sql, string connectionString)
        {
            List<EnumerationObject> result = new List<EnumerationObject>();
            MySqlConnection connection = null;
            try
            {
                List<object[]> points = new List<object[]>();                             
                connection = new MySqlConnection(connectionString);
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;                        
                        using (var reader = cmd.ExecuteReader())
                        {                            
                            if (reader.HasRows)
                            {
                                //get list of values
                                while (reader.Read())
                                {
                                    int length = reader.FieldCount;
                                    HybridDictionary row = new HybridDictionary(length);
                                    for (int i = 0; i < length; i++)
                                    {
                                        if (reader.IsDBNull(i))
                                        {
                                            Type type = reader.GetFieldType(i);
                                            TypeInfo typeInfo = type.GetTypeInfo();

                                            bool isNullable = typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
                                            var value = typeInfo.IsValueType && !isNullable ? Activator.CreateInstance(type) : null;
                                            row.Add(reader.GetName(i), value);
                                        }
                                        else
                                        {
                                            row.Add(reader.GetName(i), reader.GetValue(i));
                                        }
                                    }
                                    result.Add(new EnumerationObject("MySql", row));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (null != connection)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Dispose();
                }
            }
            return result;
        }
        public void Dispose(){}
    }
}