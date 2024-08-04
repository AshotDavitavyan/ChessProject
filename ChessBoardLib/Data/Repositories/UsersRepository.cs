using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class UsersRepository
{
	private readonly string _connectionString = DatabaseSettings.ConnectionString;
	
	public void Save(User user)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "INSERT INTO Users (UserName, Password) " +
		             "VALUES (@UserName, @Password)";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@UserName", user.UserName);
		command.Parameters.AddWithValue("@Password", user.Password);
		command.ExecuteNonQuery();
	}

	public User GetUser(string username)
	{
		SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT * FROM Users " +
		             "WHERE Username = @UserName";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@UserName", username);
		DataTable table = new();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		adapter.Fill(table);
		if (table.Rows.Count == 0)
			return null;
		DataRow row = table.Rows[0];
		User user = new()
		{
			UserId = Convert.ToInt32(row["UserID"]),
			UserName = row["Username"].ToString(),
			Password = row["Password"].ToString()
		};
		return user;
	}
}