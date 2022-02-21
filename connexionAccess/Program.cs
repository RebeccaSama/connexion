using System;
using System.Data.OleDb;

namespace connexionAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            
            OleDbConnection connection = null;
            OleDbCommand command = null;
            var filename = @"F:\TIC_Cours\TI_PAM2\Deuxieme_Trimestre\ADOT.NET\Connexion.mdb";
            const String providerName = "Microsoft.Jet.OLEDB.4.0";

            
            try
            {
                connection = new OleDbConnection("Provider="+ providerName + ";Data Source="+filename);
                connection?.Open();
                Console.WriteLine("Connexion etablie");

                string choice = "n";
                string query = null;

                do
                {

                    Console.WriteLine(" Entrer le Nom");
                    var Name = Console.ReadLine();
                    Console.WriteLine("Numero de Telephone");
                    long phone;
                    long.TryParse(Console.ReadLine(), out phone);

                    query = $"INSERT INTO Personne (Nom, Tel)VALUES(@Nom,@Tel)";
                    command = new OleDbCommand(query, connection);
                    command.Parameters.Add("@Nom", OleDbType.VarChar).Value = Name;
                    command.Parameters.Add("@Tel", OleDbType.BigInt).Value = phone;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Save done");
                    Console.WriteLine("continue? [y/n] : ");
                    choice = Console.ReadLine()[0].ToString().ToLower();
                }
                while (choice == "y");

                query = "SELECT * FROM Personne";
                command.CommandText = query;
                command.Parameters.Clear();
                OleDbDataReader reader = command.ExecuteReader();
                string column = "NOM ".PadRight(100, ' ') + "TELEPHONE";
                Console.WriteLine(column);
                Console.WriteLine("-".PadRight(column.Length, '-'));

                while(reader.Read())
                {
                    Console.WriteLine($"{reader["Nom"].ToString().PadRight(100, ' ')}{reader["Tel"]}");
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                command?.Dispose();
            }
            Console.ReadKey();
        }
    }
}
