using System;
using System.Data.Common;

namespace PortableDatabaseApplication
{
    public class EmployeeRepositorySimple
    {
        private readonly DbProviderFactory _factory;
        private readonly string _connectionString;

        public EmployeeRepositorySimple(DbProviderFactory factory, string connectionString)
        {
            _factory = factory;
            _connectionString = connectionString;
        }

        public void PrintEmployees()
        {
            using var connection = _factory.CreateConnection();

            if (connection == null)
                throw new InvalidOperationException("Connection could not be created.");

            connection.ConnectionString = _connectionString;
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT EMPLOYEE_ID, FIRST_NAME, LAST_NAME FROM EMPLOYEES";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int employeeId = Convert.ToInt32(reader["EMPLOYEE_ID"]);
                string? firstName = reader["FIRST_NAME"].ToString();
                string? lastName = reader["LAST_NAME"].ToString();

                Console.WriteLine($"{employeeId}: {firstName} {lastName}");
            }
        }
    }

    class SimpleProgram
{
    static void SimpleMain(string[] args)
    {
        string providerName = "Oracle.ManagedDataAccess.Client";
        string connectionString = "User Id=myuser;Password=mypass;Data Source=localhost:1521/orclpdb1";

        DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
        var repository = new EmployeeRepositorySimple(factory, connectionString);

        repository.PrintEmployees();
    }
}
}
