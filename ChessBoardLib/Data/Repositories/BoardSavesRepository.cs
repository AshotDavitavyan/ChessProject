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
		throw new NotImplementedException();
	}
}