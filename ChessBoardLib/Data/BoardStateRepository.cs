using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class BoardStateRepository
{
	private readonly string _connectionString = "Data Source=ACER_ASPIRE_5;" +
	                                            "Initial Catalog=ChessDB;" +
	                                            "Integrated Security=True;" +
	                                            "Connect Timeout=30;" +
	                                            "Encrypt=True;" +
	                                            "TrustServerCertificate=True;" +
	                                            "ApplicationIntent=ReadWrite;" +
	                                            "MultiSubnetFailover=False";

	public DataTable GetAll()
	{
		throw new NotImplementedException();
	}

	public DataTable GetById(int id)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "SELECT * FROM BoardStates WHERE StateID = @Id";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@Id", id);
		DataTable table = new();
		SqlDataAdapter adapter = new SqlDataAdapter(command);
		adapter.Fill(table);
		return table;
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
}