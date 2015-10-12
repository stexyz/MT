using System.Data;
using System.Data.SqlClient;

namespace mt_web_api.Services
{
    public class SqlService
    {
        protected const string OrdersTableName = "dbo.Orders";
        protected const string ProductsTableName = "dbo.Products";
        protected const string ErrorsTableName = "dbo.Errors";
        
        protected static void ExecuteCommand(SqlConnection connection, string commandText) {
            SqlCommand command = new SqlCommand {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = commandText
            };
            command.ExecuteNonQuery();
        }
    }
}