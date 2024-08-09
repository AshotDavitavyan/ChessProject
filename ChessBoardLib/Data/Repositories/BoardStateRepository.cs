using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class BoardStateRepository
{
	private readonly string _connectionString = DatabaseSettings.ConnectionString;

	public List<BoardState> GetAllByGameId(int gameId)
	{
		List<BoardState> states = new();
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT * FROM BoardStates WHERE GameID = @GameId";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@GameId", gameId);
		DataTable table = new();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		adapter.Fill(table);
		if (table.Rows.Count == 0)
			return null;
		foreach (DataRow row in table.Rows)
		{
			BoardState state = new BoardState
			{
				WhoseTurn = row["WhoseTurn"].ToString(),
				BlackPieces = row["BlackPieces"].ToString(),
				WhitePieces = row["WhitePieces"].ToString(),
				GameId = Convert.ToInt32(row["GameId"]),
				StateId = Convert.ToInt32(row["StateId"])
			};
			states.Add(state);
		}

		return states;
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
	
	// public void Clear()
	// {
	// 	using SqlConnection  connection = new SqlConnection(_connectionString);
	// 	connection.Open();
	// 	string sql = "DELETE FROM BoardStates";
	// 	using SqlCommand commandDelete = new SqlCommand(sql, connection);
	// 	commandDelete.ExecuteNonQuery();
	// 	
	// 	string resetIdentityCommand = "DBCC CHECKIDENT ('BoardStates', RESEED, 0)";
	// 	using SqlCommand commandReset = new SqlCommand(resetIdentityCommand, connection) ;
	// 	commandReset.ExecuteNonQuery();
	// }
	
	// public void SetIdentity(int id)
	// {
	// 	using SqlConnection connection = new SqlConnection(_connectionString);
	// 	connection.Open();
	// 	string resetIdentityCommand = "DBCC CHECKIDENT ('BoardStates', RESEED, @Id)";
	// 	using SqlCommand commandReset = new SqlCommand(resetIdentityCommand, connection) ;
	// 	commandReset.Parameters.AddWithValue("@Id", id);
	// 	commandReset.ExecuteNonQuery();
	// }

	public void Save(BoardState state)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "INSERT INTO BoardStates (WhoseTurn, WhitePieces, BlackPieces, GameID) " +
		             "VALUES (@WhoseTurn, @WhitePieces, @BlackPieces, @GameId)";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@WhoseTurn", state.WhoseTurn);
		command.Parameters.AddWithValue("@WhitePieces", state.WhitePieces);
		command.Parameters.AddWithValue("@BlackPieces", state.BlackPieces);
		command.Parameters.AddWithValue("@GameId", state.GameId);
		command.ExecuteNonQuery();
	}

	public void DeleteGameLogsStartingFrom(int currentStateId, int GameId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "DELETE FROM BoardStates WHERE StateID >= @Id AND GameID = @GameId";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@Id", currentStateId);
		command.Parameters.AddWithValue("@GameId", GameId);
		command.ExecuteNonQuery();
	}

	public List<BoardSave> GetByUserId(int userUserId)
	{
		List<BoardSave> boardSaves = new();
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT * FROM GameSaves " +
		             "WHERE UserID = @UserId";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@UserId", userUserId);
		DataTable table = new DataTable();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		adapter.Fill(table);
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

	public BoardState? GetLastStateForGame(int gameId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT TOP 1 * FROM BoardStates " +
		             "WHERE GameID = @GameId ORDER BY StateID DESC";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("GameId", gameId);
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		DataTable table = new DataTable();
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
	
	public BoardState? GetFirstStateForGame(int gameId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT TOP 1 * FROM BoardStates " +
		             "WHERE GameID = @GameId ORDER BY StateID ASC";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("GameId", gameId);
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		DataTable table = new DataTable();
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

	public int GetLastStateId()
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();

		string sql = "SELECT IDENT_CURRENT('BoardStates') AS Current_Identity";
		using SqlCommand command = new SqlCommand(sql, connection);
		object result = command.ExecuteScalar();
		if (result == DBNull.Value)
			return 0;
		return Convert.ToInt32(result);
	}

	public Dictionary<int, BoardState>? GetWhiteStates(int gameId)
	{
		Dictionary<int, BoardState> states = new Dictionary<int, BoardState>();
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();

		String sql = "SELECT * FROM BoardStates WHERE GameID = @GameId AND WhoseTurn = 'White'";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@GameId", gameId);
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		DataTable table = new DataTable();
		adapter.Fill(table);
		if (table.Rows.Count == 0 || table.Rows[0][0] == DBNull.Value)
			return null;
		foreach (DataRow row in table.Rows)
		{
			BoardState state = new BoardState
			{
				StateId = Convert.ToInt32(row["StateID"]),
				WhoseTurn = row["WhoseTurn"].ToString(),
				WhitePieces = row["WhitePieces"].ToString(),
				BlackPieces = row["BlackPieces"].ToString(),
				GameId = Convert.ToInt32(row["GameID"])
			};
			states.Add(state.StateId, state);
		}
		return states;
	}

	public void DeleteGameStates(int currentGameId)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		
		String sql = "DELETE FROM BoardStates WHERE GameID = @GameId";
		SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@GameId", currentGameId);
		command.ExecuteNonQuery();
	}
}