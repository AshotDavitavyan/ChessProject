using ChessPieceLib;
using CoordinatesLib;
using MoveLib;

namespace ChessBoardLib;

public class ChessBoard
{
	private ChessPiece? _activePiece;
	private ChessBoard? _boardBefore;
	private List<ChessPiece> _whitePieces;
	private List<ChessPiece> _blackPieces;
	private List<InfluenceCoordinates> _influenceCoordinates;
	private EPieceColor _whoseTurn;
	
	//Default Constructor
	public ChessBoard()
	{
		_boardBefore = null;
		_whitePieces = new List<ChessPiece>();
		_blackPieces = new List<ChessPiece>();
		_influenceCoordinates = new List<InfluenceCoordinates>();
		_whoseTurn = EPieceColor.White;
		_activePiece = null;
	}

	public static ChessPiece Find(List<ChessPiece> whitePieces, List<ChessPiece> blackPieces, ChessPiece piece)
	{
		foreach (ChessPiece whitePiece in whitePieces)
		{
			if (whitePiece.Cord == piece.Cord)
				return whitePiece;
		}
		foreach (ChessPiece blackPiece in blackPieces)
		{
			if (blackPiece.Cord == piece.Cord)
				return blackPiece;
		}
		return null;
	}
	
	//Copy Constructor
	public ChessBoard(ChessBoard toCopyFrom)
	{
		if (toCopyFrom._boardBefore is not null)
			_boardBefore = new ChessBoard(toCopyFrom._boardBefore);
		else
			_boardBefore = null;
		_whitePieces = new List<ChessPiece>();
		foreach (ChessPiece piece in toCopyFrom.WhitePieces)
			_whitePieces.Add(piece.Clone());
		_blackPieces = new List<ChessPiece>();
		foreach (ChessPiece piece in toCopyFrom.BlackPieces)
			_blackPieces.Add(piece.Clone());
		_activePiece = toCopyFrom.ActivePiece is null
			? null
			: Find(_whitePieces, _blackPieces, toCopyFrom._activePiece);
		_influenceCoordinates = new	List<InfluenceCoordinates>();
		foreach (InfluenceCoordinates cord in toCopyFrom.InfluenceCoordinates)
			_influenceCoordinates.Add(new InfluenceCoordinates(cord));
		_whoseTurn = toCopyFrom.WhoseTurn;
	}

	public ChessPiece ActivePiece
	{
		get { return _activePiece; }
		set { _activePiece = value; }
	}

	public List<ChessPiece> WhitePieces
	{
		get { return _whitePieces; }
		set { _whitePieces = value; }
	}
	
	public List<ChessPiece> BlackPieces
	{
		get { return _blackPieces; }
		set { _blackPieces = value; }
	}
	
	public List<InfluenceCoordinates> InfluenceCoordinates
	{
		get { return _influenceCoordinates; }
	}

	public EPieceColor WhoseTurn
	{
		get { return _whoseTurn; }
		set { _whoseTurn = value; }
	}
	
	/// <summary>
	/// Finds the chess piece at the specified coordinates on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to search for.</param>
	/// <returns>The chess piece at the specified coordinates.</returns>
	/// <exception cref="ArgumentException">Thrown when no piece is found at the specified coordinates.</exception>
	public ChessPiece? FindPieceOnPosition(BaseCoordinates cords)
	{
		for (int i = 0; i < _whitePieces.Count; i++)
		{
			if (_whitePieces[i].Cord == cords)
				return _whitePieces[i];
		}
		for (int i = 0; i < _blackPieces.Count; i++)
		{
			if (_blackPieces[i].Cord == cords)
				return _blackPieces[i];
		}

		return null;
	}
	
	public bool CanPieceGetToPosition(ChessPiece piece, BaseCoordinates coordinate)
	{
		ChessPiece? pieceOnPosition = FindPieceOnPosition(coordinate);
		if (pieceOnPosition is not null && pieceOnPosition.Color == piece.Color)
			return false;

		if (!piece.IsMoveValid(coordinate))
			return false;
		BaseCoordinates vector = piece.GetValidVector(coordinate);
		
		BaseCoordinates cordCopy = new BaseCoordinates(piece.Cord + vector);
		while (cordCopy != coordinate)
		{
			if (FindPieceOnPosition(cordCopy) is not null)
				return false;
			cordCopy += vector;
		}
		return true;
	}
	
	public void RemovePiece(ChessPiece piece)
	{
		if (piece.Color == EPieceColor.White)
			_whitePieces.Remove(piece);
		else
			_blackPieces.Remove(piece);
	}

	/// <summary>
	/// Finds the coordinates of the king of the specified color on the chessboard.
	/// </summary>
	/// <param name="whichKing">The color of the king to find.</param>
	/// <returns>
	/// The coordinates of the king, or <c>null</c> if the king of the specified color is not found.
	/// </returns>
	public BaseCoordinates? FindTheKing(EPieceColor whichKing)
	{
		List<ChessPiece> pieces = whichKing == EPieceColor.White ? _whitePieces : _blackPieces;
		for (int i = 0; i < pieces.Count; i++)
		{
			if (pieces[i].Type == 'K' && pieces[i].Color == whichKing)
				return pieces[i].Cord;
		}

		throw new InvalidOperationException("King not found");
	}

	
	private void UpdatePieceValidMoves(ChessPiece piece)
	{
		List<InfluenceCoordinates> coordinatesList = new List<InfluenceCoordinates>(_influenceCoordinates);
		List<BaseCoordinates> validMoves = new List<BaseCoordinates>();
		_activePiece = piece;
		_activePiece.ValidMoves.Clear();
		foreach (InfluenceCoordinates coordinate in coordinatesList)
		{
			if (coordinate.Color != _activePiece.Color && coordinate.Color != EPieceColor.Mixed)
				continue;
			if (CanPieceGetToPosition(_activePiece, coordinate))
			{
				MakeMove(_activePiece, coordinate);
				if (IsCheck(FindTheKing(_activePiece.Color)))
				{
					UnmakeMove();
					continue;
				}
				validMoves.Add(new BaseCoordinates(coordinate));
				UnmakeMove();
			}
		}
		_activePiece.ValidMoves = validMoves;
	}
	
	/// <summary>
	/// Updates the influence coordinates of the chessboard and the ChessPieces.
	/// </summary>
	public void UpdateInfluenceCoordinates()
	{
		_influenceCoordinates.Clear();
		AddInfluenceCoordinates();
	}
	
	/// <summary>
	/// Determines if the king at the specified coordinates is in check.
	/// </summary>
	/// <param name="kingCoordinates">The coordinates of the king to check.</param>
	/// <returns>
	///   <c>true</c> if the king is in check; otherwise, <c>false</c>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when the specified king coordinates are null.</exception>
	public bool IsCheck(BaseCoordinates? kingCoordinates)
	{
		for (int i = 0; i < _influenceCoordinates.Count; i++)
		{
			if (_influenceCoordinates[i] == kingCoordinates)
				return true;
		}
		return false;
	}
	
	/// <summary>
	/// Reverts the chessboard to its state before the last move.
	/// </summary>
	public void UnmakeMove()
	{
		if (_boardBefore is null)
			return;
		_activePiece = _boardBefore.ActivePiece;
		_whitePieces = _boardBefore.WhitePieces;
		_blackPieces = _boardBefore.BlackPieces;
		_influenceCoordinates = _boardBefore.InfluenceCoordinates;
		_whoseTurn = _boardBefore.WhoseTurn;
		if (_boardBefore._boardBefore is not null)
			_boardBefore = _boardBefore._boardBefore;
		else
			_boardBefore = null;
	}
	
	/// <summary>
	/// Checks if the game is in a stalemate.
	/// </summary>
	/// <returns></returns>
	public bool IsStalemate()
	{
		int iteration = 0;
		List<ChessPiece> pieces = WhoseTurn == EPieceColor.White ? _whitePieces : _blackPieces;
		List<InfluenceCoordinates> influenceCoordinatesCopy = new List<InfluenceCoordinates>(_influenceCoordinates);
		foreach (var coordinate in influenceCoordinatesCopy)
		{
			iteration++;
			if (coordinate.Color != WhoseTurn && coordinate.Color != EPieceColor.Mixed)
				continue;
			foreach (ChessPiece piece in pieces)
			{
				if (CanPieceGetToPosition(piece, coordinate))
				{
					MakeMove(piece, coordinate);
					bool isCheck = IsCheck(FindTheKing(_whoseTurn == EPieceColor.White ? EPieceColor.Black : EPieceColor.White));
					UnmakeMove();
					pieces = WhoseTurn == EPieceColor.White ? _whitePieces : _blackPieces;
					if (!isCheck)
						return false;
				}
			}
		}
		return true;
	}
	
	/// <summary>
	/// Retrieves the list of chess pieces from the given collection that can reach the specified position on the chessboard.
	/// </summary>
	/// <param name="pieces">The collection of chess pieces to search.</param>
	/// <param name="cords">The coordinates of the position to reach.</param>
	/// <param name="piecesThatCanReach">The list to populate with pieces that can reach the specified position.</param>
	private void GetPiecesThatCanReachPosition(List<ChessPiece> pieces, BaseCoordinates cords, List<ChessPiece> piecesThatCanReach)
	{
		for (int i = 0; i < pieces.Count; i++)
		{
			int diffX = Math.Abs(cords.PosX - pieces[i].Cord.PosX);
			int diffY = Math.Abs(cords.PosY - pieces[i].Cord.PosY);
			if (diffX == 0 && diffY == 0)
				return;
			if (CanPieceGetToPosition(pieces[i], cords))
				piecesThatCanReach.Add(pieces[i]);
		}
	}
	
	/// <summary>
	/// Determines the influence of pieces on the specified position on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to analyze.</param>
	/// <returns>
	/// The color of the pieces influencing the position, or <c>null</c> if no pieces can reach the position.
	/// </returns>
	public EPieceColor? DeterminePositionInfluence(BaseCoordinates cords)
	{
		List<ChessPiece> piecesThatCanReach = new List<ChessPiece>();
		
		GetPiecesThatCanReachPosition(_whitePieces, cords, piecesThatCanReach);
		GetPiecesThatCanReachPosition(_blackPieces, cords, piecesThatCanReach);
		if (piecesThatCanReach.Count != 0)
		{
			EPieceColor colorToReturn = piecesThatCanReach[0].Color;
			for (int i = 0; i < piecesThatCanReach.Count; i++)
			{
				if (piecesThatCanReach[i].Color != colorToReturn)
					return EPieceColor.Mixed;
			}
			return colorToReturn;
		}
		return null;
	}
	
	/// <summary>
	/// Adds the influence coordinates of the chessboard to the collection of influence coordinates.
	/// </summary>
	public void AddInfluenceCoordinates()
	{
		BaseCoordinates cords = new BaseCoordinates();
		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				cords.PosX = x;
				cords.PosY = y;
				EPieceColor? color = DeterminePositionInfluence(cords);
				if (color != null)
					_influenceCoordinates.Add(new InfluenceCoordinates(cords, color.Value));
			}
		}
	}
	
	/// <summary>
	/// Adds a chess piece to the collection of chess pieces.
	/// </summary>
	/// <param name="piece"> The piece to add. </param>
	public void AddPiece(ChessPiece piece)
	{
		switch (piece.Color)
		{
			case EPieceColor.Black:
				_blackPieces.Add(piece);
				break;
			case EPieceColor.White:
				_whitePieces.Add(piece);
				break;
		}
	}
	
	/// <summary>
	/// Determines the current state of the chess game.
	/// </summary>
	/// <returns>The current state of the chess game.</returns>
	public EGameState GetGameState()
	{
		bool check = IsCheck(FindTheKing(_whoseTurn));
		bool stalemate = IsStalemate();
		
		switch (check)
		{
			case true when stalemate:
				return EGameState.Mate;
			case false when stalemate:
				return EGameState.Stalemate;
			case true when !stalemate:
				return EGameState.Check;
			default:
				return EGameState.Normal;
		}
	}
	
	public void MakeMove(ChessPiece pieceToMove, BaseCoordinates destination)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == EPieceColor.White ? BlackPieces : WhitePieces;
		_boardBefore = new ChessBoard(this);
		
		ChessPiece? capturedPiece = FindPieceOnPosition(destination);
		if (capturedPiece != null)
			enemyPieces.Remove(capturedPiece);
		pieceToMove.Move(destination);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == EPieceColor.White ? EPieceColor.Black : EPieceColor.White;
	}

	public void UpdateValidMoves()
	{
		foreach (ChessPiece piece in BlackPieces)
			UpdatePieceValidMoves(piece);
		foreach (ChessPiece piece in WhitePieces)
			UpdatePieceValidMoves(piece);
	}
	
	/// <summary>
	/// Makes a move on the chessboard, updating the game state accordingly.
	/// </summary>
	/// <param name="pieceToMove">The chess piece to move.</param>
	/// <param name="whereToMove">The coordinates of the destination position.</param>
	/// <exception cref="ArgumentException">Thrown when attempting to move a piece to an invalid position or when the piece to move is null.</exception>
	public void MakeMove(Move move)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == EPieceColor.White ? BlackPieces : WhitePieces;
		_boardBefore = new ChessBoard(this);
		
		ChessPiece? capturedPiece = FindPieceOnPosition(move.Destination);
		if (capturedPiece != null)
			enemyPieces.Remove(capturedPiece);
		FindPieceOnPosition(move.Piece.Cord).Move(move.Destination);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == EPieceColor.White ? EPieceColor.Black : EPieceColor.White;
	}
}