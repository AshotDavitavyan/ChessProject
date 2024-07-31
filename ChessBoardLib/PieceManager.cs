using System.Text;
using ChessPieceLib;
using CoordinatesLib;

namespace ChessBoardLib;

public class PieceManager
{
	private Dictionary<BaseCoordinates, ChessPiece> _whitePiecesD = new();
	private Dictionary<BaseCoordinates, ChessPiece> _blackPiecesD = new();
	private List<ChessPiece> _whitePieces = new ();
	private List<ChessPiece> _blackPieces = new ();
	private ChessPiece? _whiteKing;
	private ChessPiece? _blackKing;

	public List<ChessPiece> WhitePieces
	{
		get => _whitePieces;
		set => _whitePieces = value;
	}
	
	public List<ChessPiece> BlackPieces
	{
		get => _blackPieces;
		set => _blackPieces = value;
	}

	public ChessPiece? WhiteKing
	{
		get => _whiteKing;
		set => _whiteKing = value;
	}

	public ChessPiece? BlackKing
	{
		get => _blackKing;
		set => _blackKing = value;
	}
	
	public PieceManager()
	{
	}

	public PieceManager(PieceManager copy)
	{
		ChessPiece tmp;
		foreach (ChessPiece piece in copy.WhitePieces)
		{
			tmp = piece.Clone();
			_whitePieces.Add(tmp);
			if (piece is King)
				WhiteKing = tmp;
		}
		foreach (ChessPiece piece in copy.BlackPieces)
		{
			tmp = piece.Clone();
			_blackPieces.Add(tmp);
			if (piece is King)
				BlackKing = tmp;
		}
	}

	public ChessPiece? FindPiece(ChessPiece toFind)
	{
		foreach (var piece in WhitePieces)
		{
			if (piece == toFind)
				return piece;
		}

		foreach (var piece in BlackPieces)
		{
			if (piece == toFind)
				return piece;
		}

		return null;
	}
	
	/// <summary>
	/// Finds the chess piece at the specified coordinates on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to search for.</param>
	/// <returns>The chess piece at the specified coordinates.</returns>
	/// <exception cref="ArgumentException">Thrown when no piece is found at the specified coordinates.</exception>
	public ChessPiece? FindPieceOnPosition(BaseCoordinates cords)
	{
		foreach (var piece in WhitePieces)
		{
			if (piece.Cord == cords)
				return piece;
		}
		foreach (var piece in BlackPieces)
		{
			if (piece.Cord == cords)
				return piece;
		}

		return null;
	}
	
	public void RemovePiece(ChessPiece piece)
	{
		if (piece.Color == GameColor.White)
			_whitePieces.Remove(piece);
		else
			_blackPieces.Remove(piece);
	}
	
	/// <summary>
	/// Adds a chess piece to the collection of chess pieces.
	/// </summary>
	/// <param name="piece"> The piece to add. </param>
	public void AddPiece(ChessPiece piece)
	{
		switch (piece.Color)
		{
			case GameColor.Black:
				_blackPieces.Add(piece);
				if (piece is King)
					_blackKing = piece;
				break;
			case GameColor.White:
				_whitePieces.Add(piece);
				if (piece is King)
					_whiteKing = piece;
				break;
		}
	}

	public ChessPiece? GetKing(GameColor color)
	{
		return color == GameColor.Black ? _blackKing : _whiteKing;
	}

	public string ConvertPieceListToString(List<ChessPiece> pieces)
	{
		StringBuilder sb = new();
		foreach (var piece in pieces)
		{
			sb.Append(piece.Type);
			sb.Append(piece.Cord);
		}
		return sb.ToString();
	}

	public List<ChessPiece> ConvertStringToPieceList(string piecesString, GameColor color)
	{
		List<ChessPiece> pieces = new();
		List<string> pieceStrings = new();
		
		for (int i = 0; i < piecesString.Length; i += 3)
			pieceStrings.Add(piecesString.Substring(i, 3));
		foreach (var pieceString in pieceStrings)
		{
			string type = pieceString[0].ToString();
			string position = pieceString[1].ToString() + pieceString[2].ToString();
			ChessPiece piece = ChessPieceParser.CreatePiece(new BaseCoordinates(position), type, color);
			pieces.Add(piece);
		}
		return pieces;
	}
}
