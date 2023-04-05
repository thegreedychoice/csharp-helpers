using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace WordCounter.App.Data
{
    public class SqlClient
    {
        private readonly string _connectionString;

        public SqlClient(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("WordCount");
        }

        public async Task CreateFeedResults(string path, int lineCount, int wordCount, long processingMilliseconds)
        {
            using var sqlConnection = await OpenConnection();
            using (var command = sqlConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    "INSERT INTO BookFeed (Path, LineCount, WordCount, ProcessingMilliseconds) VALUES ('{0}', {1}, {2}, {3});",
                    path, lineCount, wordCount, processingMilliseconds);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateFeedResults(string path, int lineCount, int wordCount, long processingMilliseconds)
        {
            using var sqlConnection = await OpenConnection();
            using (var command = sqlConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = string.Format(
                    "UPDATE BookFeed SET LineCount = {1}, WordCount = {2}, ProcessingMilliseconds = {3} WHERE Path = '{0}';",
                    path, lineCount, wordCount, processingMilliseconds);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveLineResults(string path, int lineNumber, int wordCount, string excerpt)
        {
            int bookFeedId = 0;

            using (var sqlConnection = await OpenConnection())
            {
                using (var selectCommand = sqlConnection.CreateCommand())
                {
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.CommandText = string.Format("SELECT Id FROM BookFeed WHERE Path='{0}'", path);
                    bookFeedId = (int)await selectCommand.ExecuteScalarAsync();
                }

                using (var insertCommand = sqlConnection.CreateCommand())
                {
                    insertCommand.CommandType = CommandType.Text;
                    insertCommand.CommandText = string.Format(
                        "INSERT INTO BookLine (BookFeedId, LineNumber, WordCount, Excerpt) VALUES ('{0}', {1}, {2}, '{3}');",
                        bookFeedId, lineNumber, wordCount, excerpt.Replace("'", "''"));
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task<SqlConnection> OpenConnection()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
