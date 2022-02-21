using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Connexion.DAL
{
    public class Sql
    {
        private readonly string connectionString;
        private readonly string providerName;
        private readonly DbProviderFactory factory;
        private readonly string connectionStringName;

        public Sql(string connectionString, string providerName)
        {
            this.connectionString = connectionString;
            this.providerName = providerName;

            factory = DbProviderFactories.GetFactory(providerName);
        }
        public Sql (string connectionStringName):
            this(ConfigurationManager.ConnectionString[connectionStringName].ConnectionString, ConfigurationManager.ConnectionString[connectionStringName].providerName)
        {
           
        }
        /// <summary>
        /// retourne un objet de connexion ouvert
        /// </summary>
        /// <returns>Dbconnection</returns>
         private DbConnection GetConnexion()
         {
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            return connection;
         }
        private DbCommand GetCommand(string query, IEnumerable<Parameter> parameters, bool isStoredProcedure = false)
        {
            var command = factory.CreateCommand();
            command.CommandText = query;
            if (isStoredProcedure)
                command.CommandType = CommandType.StoredProcedure;
            
                    if(parameters != null)
                    { 
                        foreach(var parameter in parameters)
                        {
                            var p = factory.CreateParameter();
                            p.ParameterName = parameter.Name;
                            p.DbType = parameter.Type;
                            p.Value = parameter.Value;
                            command.Parameters.Add(p);
                        }

                    }
            return command;
        }
        public void Execute(string query, IEnumerable<Parameter> parameters, bool isStoredProcedure = false)
        {
            using (var connection = GetConnexion())
            {
                using(var command = GetCommand(query, parameters, isStoredProcedure))
                {
                    command.Connection = connection;
                    command.ExecuteNonQuery();

                }
            }
        }
        public IEnumerable<T> Read<T>(string query, IEnumerable<Parameter> parameters, Func<DbDataReader, T> callBack, bool isStoredProcedure = false)
        {
            List<T> datas = new List<T>();
            using (var connection = GetConnexion())
            {
                using (var command = GetCommand(query, parameters, isStoredProcedure))
                {
                    command.Connection = connection;
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            callBack?.Invoke(reader);
                        }
                        reader.Close();
                    }
                }
            }
            return datas;
        }
        public class Parameter
        {
            public string Name { get; set; }
            public DbType Type {get; set;}
            public object Value { get; set; }
        }
    }
}
