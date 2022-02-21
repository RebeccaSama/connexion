using System;
using System.Data.OleDb;
using System.IO;

namespace Connexion.Access
{
    class Program
    {
        static void Main(string[] args)
        {
            OleDbConnection connexion = null;
            var fileName = @"E:\cours\PAM2\ProgrammationMulticouches\ADO_dot_NET\dbtest.mdb";
            const string providerName = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            OleDbCommand command = null;
            try
            {
                connexion = new OleDbConnection(providerName + fileName);
                connexion.Open();
                string choice = "n";
                string query = null;
                do
                {
                    Console.WriteLine("Type your name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Type your phone number:");
                    long phone = 0;
                    long.TryParse(Console.ReadLine(), out phone);

                    query = $"INSERT INTO Personne(nom, tel) VALUES (@nom, @tel)";
                    command = new OleDbCommand(query, connexion);
                    command.Parameters.Add("@nom", OleDbType.VarChar).Value = name;
                    command.Parameters.Add("@tel", OleDbType.BigInt).Value = phone;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Save done!");

                    Console.WriteLine("Continue ? [y/n]");
                    choice = Console.ReadLine()[0].ToString();
                } 
                while (choice == "y");

                //query = "SELECT * FROM Personne";
                //command.CommandText = query;
                //command.Parameters.Clear();
                //OleDbDataReader reader = command.ExecuteReader();
                //int pad = 100;
                //string column = "NOM".PadRight(pad, ' ') + "TELEPHONE";
                //Console.WriteLine(column);
                //Console.WriteLine("-".PadRight(column.Length, '-'));
                //while (reader.Read())
                //{
                //    Console.WriteLine($"{reader["nom"].ToString().PadRight(pad, ' ')}{reader["tel"]}");
                //}
                //reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                command?.Dispose();
                connexion?.Dispose();
            }
            Console.ReadKey();
        }
    }
}
