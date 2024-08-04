using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class BoardStateRepository
{
	private readonly string _connectionString = DatabaseSettings.ConnectionString;

	public DataTable GetAll()
	{
		throw new NotImplementedException();
	}

	public BoardState GetById(int stateId, int gameId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT * FROM BoardStates WHERE StateID = @stateId AND GameID = @GameId";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@stateId", stateId);
		command.Parameters.AddWithValue("@GameId", gameId);
		DataTable table = new();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		adapter.Fill(table);
		if (table.Rows.Count == 0)
			return null;
		BoardState state = new()
		{
			StateId = Convert.ToInt32(table.Rows[0]["StateID"]),
			WhoseTurn = table.Rows[0]["WhoseTurn"].ToString(),
			WhitePieces = table.Rows[0]["WhitePieces"].ToString(),
			BlackPieces = table.Rows[0]["BlackPieces"].ToString(),
			GameId = Convert.ToInt32(table.Rows[0]["GameID"])
		};
		return state;
	}
	
	public void Clear()
	{
		using SqlConnection  connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "DELETE FROM BoardStates";
		using SqlCommand commandDelete = new SqlCommand(sql, connection);
		commandDelete.ExecuteNonQuery();
		
		string resetIdentityCommand = "DBCC CHECKIDENT ('BoardStates', RESEED, 0)";
		using SqlCommand commandReset = new SqlCommand(resetIdentityCommand, connection) ;
		commandReset.ExecuteNonQuery();
	}
	
	public void SetIdentity(int id)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string resetIdentityCommand = "DBCC CHECKIDENT ('BoardStates', RESEED, @Id)";
		using SqlCommand commandReset = new SqlCommand(resetIdentityCommand, connection) ;
		commandReset.Parameters.AddWithValue("@Id", id);
		commandReset.ExecuteNonQuery();
	}

	public void Save(BoardState state)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "INSERT INTO BoardStates (WhoseTurn, WhitePieces, BlackPieces) " +
		             "VALUES (@WhoseTurn, @WhitePieces, @BlackPieces)";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@WhoseTurn", state.WhoseTurn);
		command.Parameters.AddWithValue("@WhitePieces", state.WhitePieces);
		command.Parameters.AddWithValue("@BlackPieces", state.BlackPieces);
		command.ExecuteNonQuery();
	}

	public void DeleteLogsStartingFrom(int currentStateId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "DELETE FROM BoardStates WHERE StateID >= @Id";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@Id", currentStateId);
		command.ExecuteNonQuery();
	}

	public List<BoardSave> GetByUserId(int userUserId)
	{
		List<BoardSave> boardSaves = new();
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT GameID FROM BoardStates " +
		             "WHERE UserID = @UserId";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@UserId", userUserId);
		DataTable table = new DataTable();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		
		foreach (DataRow row in table.Rows)
		{
			int gid = Convert.ToInt32(row["GameID"]);
			int sid = Convert.ToInt32(row["StateID"]);
			int uid = Convert.ToInt32(row["UserID"]);
			BoardSave boardSave = new()
			{
				GameId = gid,
				StateId = sid,
				UserId = uid
			};
			boardSaves.Add(boardSave);
		}

		return boardSaves;
	}
}