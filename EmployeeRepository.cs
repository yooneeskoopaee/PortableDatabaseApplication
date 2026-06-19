using System.Data.Common;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EmployeeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IEnumerable<Employee> GetEmployees()
    {
        const string sql = """
            SELECT EMPLOYEE_ID, FIRST_NAME, LAST_NAME
            FROM EMPLOYEES
            """;

        using DbConnection connection = _connectionFactory.CreateConnection();

        connection.Open();

        using DbCommand command = connection.CreateCommand();

        command.CommandText = sql;

        using DbDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new Employee
            {
                EmployeeId = Convert.ToInt32(reader["EMPLOYEE_ID"]),
                FirstName = Convert.ToString(reader["FIRST_NAME"]) ?? string.Empty,
                LastName = Convert.ToString(reader["LAST_NAME"]) ?? string.Empty
            };
        }
    }
}
