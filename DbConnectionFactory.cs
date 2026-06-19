using System.Data.Common;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly DatabaseOptions _options;

    public DbConnectionFactory(DatabaseOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ProviderName))
            throw new ArgumentException("Database provider name is required.");

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
            throw new ArgumentException("Database connection string is required.");

        _options = options;
    }

    public DbConnection CreateConnection()
    {
        var factory = DbProviderFactories.GetFactory(_options.ProviderName);

        var connection = factory.CreateConnection();

        if (connection is null)
            throw new InvalidOperationException(
                $"Could not create connection for provider '{_options.ProviderName}'.");

        connection.ConnectionString = _options.ConnectionString;

        return connection;
    }
}
