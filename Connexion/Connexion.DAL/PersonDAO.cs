using System;
using System.Collections.Generic;
using System.Data.Common;

using Connexion.BO;
namespace Connexion.DAL
{
    public class PersonDAO
    {
        private readonly Sql sql;

        public PersonDAO(string connectionStringName)
        {
            sql = new Sql(connectionStringName);
        }
        public void Add(Person person)
        {
            sql.Execute
            (
                "Sp_Person_Insert",
                new Sql.Parameter[]
                {
                    new Sql.Parameter("@name", System.Data.DbType.String, person.Name),
                    new Sql.Parameter("@phone", System.Data.DbType.Int64, person.PhoneNumber),
                    new Sql.Parameter("@birth_day", System.Data.DbType.Date, person.BirthDay),
                    new Sql.Parameter("@picture", System.Data.DbType.Binary, person.Photo)
                },
                true
            );
        }

        public IEnumerable<Person> GetAll()
        {
            return sql.Read<Person>("Sp_Person_Select", null, GetPerson, true);
        }

        private Person GetPerson(DbDataReader reader)
        {
            return new Person
            (
                int.Parse(reader["id"].ToString()),
                reader["name"].ToString(),
                long.Parse(reader["phone"].ToString()),
                DateTime.Parse(reader["birth_day"].ToString()),
                reader["picture"] != DBNull.Value ? (byte[])reader["picture"] : null
            );
        }
    }
}
