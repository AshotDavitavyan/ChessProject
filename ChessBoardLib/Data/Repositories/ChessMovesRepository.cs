using System.Data;
using System.Data.SqlClient;

namespace ChessBoardLib.Data;

public class ChessMovesRepository
{
	private readonly string _connectionString = DatabaseSettings.ConnectionString;

	public DataTable GetAll()
	{
		string sql = "SELECT * FROM ChessMoves";
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
		
		DataSet ds = new DataSet();
		adapter.Fill(ds);
		
		DataTable dt = ds.Tables[0];
		return (dt);
	}
	
	public void Clear()
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "DELETE FROM ChessMoves";
		using SqlCommand commandDelete = new SqlCommand(sql, connection);
		commandDelete.ExecuteNonQuery();
		
		string resetIdentityCommand = "DBCC CHECKIDENT ('ChessMoves', RESEED, 0)";
		using SqlCommand commandReset = new SqlCommand(resetIdentityCommand, connection) ;
		commandReset.ExecuteNonQuery();
	}
	
	public void Save(ChessMoves moves)
	{
		using SqlConnection connection = new SqlConnection(_connectionString);
		connection.Open();
		string sql = "INSERT INTO ChessMoves (ChessPiece, Color, [From], [To], Capture, Date) " +
		             "VALUES (@Piece, @Color, @WhereFrom, @WhereTo, @Capture, @Date)";
		using SqlCommand command = new SqlCommand(sql, connection);
		command.Parameters.AddWithValue("@Piece", moves.Piece);
		command.Parameters.AddWithValue("@Color", moves.Color);
		command.Parameters.AddWithValue("@WhereFrom", moves.WhereFrom);
		command.Parameters.AddWithValue("@WhereTo", moves.WhereTo);
		command.Parameters.AddWithValue("@Capture", moves.Capture??(object)DBNull.Value);
		command.Parameters.AddWithValue("@Date", moves.Date);
		command.ExecuteNonQuery();
	}
}