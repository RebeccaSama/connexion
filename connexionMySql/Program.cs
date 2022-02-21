using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace connexionMySql
{
    class Program
    {
        static void Main(string[] args)
        {
            const string host = "localhost";
            const string port = "3306";
            const string db = "DB1";
            const string user = "root";
            const string pwd = "";
            string conStr = $"Server = {host}; Port = {port}; Database = {db}; Uid = {user}; Pwd = {pwd}";

            try
            {
                using(MySqlConnection connection = new MySqlConnection(conStr))
                {
                    connection.Open();
                    Console.WriteLine("Connexion done!");

                    Console.Write("Enter your name : ");
                    string name = Console.ReadLine();

                    Console.Write("Enter your phone Numer : ");
                    long phone;
                    long.TryParse(Console.ReadLine(), out phone);

                    Console.Write("Enter your Birthday [YYYY-MM-dd]: ");
                    DateTime birthday;
                    DateTime.TryParse(Console.ReadLine(), out birthday);

                    string query = $"INSERT INTO Person (Nom, Tel, Date_nais)VALUES(@Nom, @Tel, @Date_nais)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Nom", MySqlDbType.VarChar).Value = name;
                        command.Parameters.Add("@Tel", MySqlDbType.Int64).Value = phone;
                        command.Parameters.Add("@Date_nais", MySqlDbType.Date).Value = birthday;


                        command.ExecuteNonQuery();
                        Console.WriteLine("Save done !");

                        Console.Write("What do you search ?");
                        string searchValue = Console.ReadLine();
                        query = "SELECT * FROM Person WHERE Nom LIKE @value ";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.Add("@value", MySqlDbType.VarChar).Value = $"%{searchValue}%";
                        string column = "ID".PadRight(10, ' ') +
                                        "NOM".PadRight(20, ' ') +
                                        "TELEPHONE".PadRight(30, ' ') +
                                        "DATE NAISSANCE".PadRight(40, ' ');
                        Console.WriteLine(column);
                        Console.WriteLine("-".PadRight(column.Length, '-'));

                        using(MySqlDataReader reader = command.ExecuteReader())
                        { 
                            while(reader.Read())
                            {
                                Console.Write($"{reader["id"].ToString().PadRight(10, ' ')}");
                                Console.Write($"{reader["Nom"].ToString().PadRight(20, ' ')}");
                                Console.Write($"{reader["Tel"].ToString().PadRight(30, ' ')}");
                                Console.Write($"{DateTime.Parse(reader["Date_nais"].ToString()).ToShortDateString()}\n");

                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
