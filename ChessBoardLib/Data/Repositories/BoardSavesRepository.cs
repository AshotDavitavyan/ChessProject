using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class BoardSavesRepository
{
	private readonly string _connectionString = DatabaseSettings.ConnectionString;

	public DataTable GetAll()
	{
		throw new NotImplementedException();
	}
	
	public DataTable GetByUserName()
	{
		throw new NotImplementedException();
	}

	public void Save(BoardSave save)
	{
		using SqlConnection connection = new(_connectionString);
		connection.Open();
		string sql = "INSERT INTO GameSaves (StateID, UserID) " +
		             "VALUES (@StateID, @UserID)";
		SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@StateID", save.StateId);
		command.Parameters.AddWithValue("@UserID", save.UserId);
		// command.Parameters.AddWithValue("@SaveDate", save.SaveDate);
		command.ExecuteNonQuery();
	}

	public int GetLastId()
	{
		using SqlConnection connection = new(_connectionString);
		connection.Open();
		string sql = "SELECT IDENT_CURRENT('GameSaves') AS Current_Identity";
		using SqlCommand command = new SqlCommand(sql, connection);
		object result = command.ExecuteScalar();
		if (result == DBNull.Value)
			return 0;
		return Convert.ToInt32(result);
	}

	public void DeleteGameLog(int currentGameId)
	{
		using SqlConnection connection = new(_connectionString);
		connection.Open();
		string sql = "DELETE FROM GameSaves WHERE GameID = @GameID";
		SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@GameID", currentGameId);
		command.ExecuteNonQuery();
	}

	public void DeleteGameSave(int currentGameId)
	{
		using SqlConnection connection = new(_connectionString);
		connection.Open();
		string sql = "DELETE FROM GameSaves WHERE GameID = @GameID";
		SqlCommand command = new(sql, connection);
		command.Parameters.AddWithValue("@GameID", currentGameId);
		command.ExecuteNonQuery();
	}
}