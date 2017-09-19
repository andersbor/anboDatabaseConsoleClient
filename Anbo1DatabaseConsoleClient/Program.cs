using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Anbo1DatabaseConsoleClient
{
    class Program
    {
        private const string ConnectionString =
            "Server=tcp:anbo-databaseserver.database.windows.net,1433;Initial Catalog=anbobase;Persist Security Info=False;User ID=anbo;Password=Secret12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string SelectAllStudents = "select * from student";
        private const string InsertStudent = "insert into student (name, semester) values (@name, @semester)";

        static void Main()
        {
            //string connString = GetConnectionStringFromAppConfig();
            string connString = ConnectionString;
            using (SqlConnection databaseConnection = new SqlConnection(connString))
            {
                databaseConnection.Open();
                IList<Student> allStudents = GetAllStudents(databaseConnection);
                Console.WriteLine(string.Join("\n", allStudents));
                InsertStudents(databaseConnection);
                allStudents = GetAllStudents(databaseConnection);
                Console.WriteLine(string.Join("\n", allStudents));
            }
        }

        private static string GetConnectionStringFromAppConfig()
        {
            // https://msdn.microsoft.com/en-us/library/ms254494.aspx
            ConnectionStringSettingsCollection connectionStringSettingsCollection = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings connStringSettings = connectionStringSettingsCollection["anbo1databaseAzure"];
            string connString = connStringSettings.ConnectionString;
            return connString;
        }

        private static IList<Student> GetAllStudents(SqlConnection databaseConnection)
        {
            using (SqlCommand selectCommand = new SqlCommand(SelectAllStudents, databaseConnection))
            {
                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    IList<Student> students = new List<Student>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            int semester = reader.GetInt32(2);
                            DateTime timeStamp = reader.GetDateTime(3);
                            // normally you would not show the ID to the user
                            //Console.WriteLine(id + " " + name + " " + semester + " " + timeStamp);
                            Student st = new Student()
                            {
                                Id = id,
                                Name = name,
                                Semester = semester,
                                TimeStamp = timeStamp
                            };
                            students.Add(st);
                        }
                    }
                    //else
                    //{
                    //    Console.WriteLine("No rows");
                    //}
                    return students;
                }
            }
        }
        private static void InsertStudents(SqlConnection databaseConnection)
        {
            while (true)
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name)) break;
                Console.Write("Semester: ");
                string semester = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(semester)) break;
                InsertOneStudent(databaseConnection, name, semester);
            }
        }

        private static void InsertOneStudent(SqlConnection databaseConnection, string name, string semester)
        {
            using (SqlCommand insertCommand = new SqlCommand(InsertStudent, databaseConnection))
            {
                insertCommand.Parameters.AddWithValue("@name", name);
                insertCommand.Parameters.AddWithValue("@semester", semester);
                int rowsAffected = insertCommand.ExecuteNonQuery();
                Console.WriteLine(rowsAffected + " row(s) affected");
            }
        }
    }
}