using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace PortableDatabaseApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                    .Build();

                DatabaseOptions databaseOptions = configuration
                    .GetSection("Database")
                    .Get<DatabaseOptions>()
                    ?? throw new InvalidOperationException("Database configuration is missing.");

                RegisterDatabaseProvider(databaseOptions.ProviderName);

                IDbConnectionFactory connectionFactory = new DbConnectionFactory(databaseOptions);

                IEmployeeRepository employeeRepository = new EmployeeRepository(connectionFactory);

                IEnumerable<Employee> employees = employeeRepository.GetEmployees();

                foreach (Employee employee in employees)
                {
                    Console.WriteLine($"{employee.EmployeeId}: {employee.FirstName} {employee.LastName}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Application failed.");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
        }

        private static void RegisterDatabaseProvider(string providerName)
        {
            if (string.Equals(
                    providerName,
                    "Microsoft.Data.SqlClient",
                    StringComparison.OrdinalIgnoreCase))
            {
                DbProviderFactories.RegisterFactory(
                    providerName,
                    "Microsoft.Data.SqlClient");

                return;
            }
            else if (string.Equals(
                    providerName,
                    "Oracle.ManagedDataAccess.Client",
                    StringComparison.OrdinalIgnoreCase))
            {
                    DbProviderFactories.RegisterFactory(
                    providerName,
                    "Oracle.ManagedDataAccess.Client");

                return;
            }

            /*
            * For other databases, register their provider factory here.
            *
            * Example:
            *
            * DbProviderFactories.RegisterFactory(
            *     "Some.Other.Database.Provider",
            *     SomeOtherProviderFactory.Instance);
            *
            * This keeps provider-specific code isolated in one place.
            */
        }
    }
}