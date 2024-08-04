namespace ChessBoardLib.Data;

public static class DatabaseSettings
{
	public static readonly string ConnectionString = "Data Source=ACER_ASPIRE_5;" +
	                                            "Initial Catalog=ChessDB;" +
	                                            "Integrated Security=True;" +
	                                            "Connect Timeout=30;" +
	                                            "Encrypt=True;" +
	                                            "TrustServerCertificate=True;" +
	                                            "ApplicationIntent=ReadWrite;" +
	                                            "MultiSubnetFailover=False";
}