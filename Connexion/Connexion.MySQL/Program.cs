using System;
using MySql.Data.MySqlClient;

namespace Connexion.MySQL
{
    class Program
    {
        static void Main(string[] args)
        {
            const string host = "localhost";
            const string port = "3306";
            const string db = "db1";
            const string user = "root";
            const string password = "";
            String contString = $"Server = {host}; Port = {port}; Database = {db}; Uid = {user}; Pwd = {password}";

            try
            {
                using(MySqlConnection connection = new MySqlConnection(contString))
                {
                    connection.Open();
                    Console.WriteLine("Connection done!");

                    Console.Write("Enter your name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter your phone number: ");
                    long phone;
                    long.TryParse(Console.ReadLine(), out phone);
                    Console.Write("Enter your birthday [yyyy-MM-dd]: ");
                    DateTime birthDay;
                    DateTime.TryParse(Console.ReadLine(), out birthDay);

                    string query = "INSERT INTO person(name, phone, birth_day)  VALUES(@name, @phone, @birth_day)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@birth_day", birthDay);

                        command.ExecuteNonQuery();
                        Console.WriteLine("save done!");

                        Console.WriteLine("What do you search ?");
                        string searchValue = Console.ReadLine();

                        query = "SELECT * FROM person WHERE name LIKE @name";
                        command.CommandText = query;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@name", searchValue);

                        string column = "NAME".PadRight(50, ' ') + "PHONE".PadRight(25, ' ') + "BIRTHDAY".PadRight(50, ' ');
                        Console.WriteLine(column);
                        Console.WriteLine("-".PadRight(column.Length, '-'));

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"|{reader["name"].ToString().PadRight(50, ' ')}"+
                                    $"|{reader["phone"].ToString().PadRight(25, ' ')}"+
                                    $"|{reader["birth_day"].ToString().PadRight(49, ' ')}|");
                                Console.WriteLine("-".PadRight(column.Length, '-'));
                            }
                        }

                    }
                    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
