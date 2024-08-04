using ChessBoardLib.Data;
using ChessPieceLib;
using CoordinatesLib;
using MoveLib;

// hgzraryan@yandex.ru

namespace ChessBoardLib;

public class ChessBoard
{
	private ChessPiece? _activePiece;
	private PieceManager _pieceManager;
	private HashSet<InfluenceCoordinates> _influenceCoordinatesHs;
	private List<InfluenceCoordinates> _influenceCoordinates;
	private GameColor _whoseTurn;
	private Stack<ChessBoardMemento> _undoStack;
	
	//Default Constructor
	public ChessBoard()
	{
		_activePiece = null;
		_pieceManager = new PieceManager();
		_influenceCoordinates = new List<InfluenceCoordinates>();
		_influenceCoordinatesHs = new HashSet<InfluenceCoordinates>();
		_whoseTurn = GameColor.White;
		_undoStack = new Stack<ChessBoardMemento>();
	}

	public ChessPiece ActivePiece
	{
		get { return _activePiece; }
		set { _activePiece = value; }
	}

	public PieceManager PieceManager
	{
		get { return _pieceManager; }
		set { _pieceManager = value; }
	}
	
	public List<InfluenceCoordinates> InfluenceCoordinates
	{
		get { return _influenceCoordinates; }
		set { _influenceCoordinates = value; }
	}
	
	public HashSet<InfluenceCoordinates> InfluenceCoordinatesHs
	{
		get { return _influenceCoordinatesHs; }
		set { _influenceCoordinatesHs = value; }
	}

	public GameColor WhoseTurn
	{
		get { return _whoseTurn; }
		set { _whoseTurn = value; }
	}
		
	/// <summary>
	/// Determines the influence of pieces on the specified position on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to analyze.</param>
	/// <returns>
	/// The color of the pieces influencing the position, or <c>null</c> if no pieces can reach the position.
	/// </returns>
	public GameColor? DeterminePositionInfluence(BaseCoordinates cords)
	{
		List<ChessPiece> piecesThatCanReach = new List<ChessPiece>();
		
		GetPiecesThatCanReachPosition(_pieceManager.WhitePieces, cords, piecesThatCanReach);
		GetPiecesThatCanReachPosition(_pieceManager.BlackPieces, cords, piecesThatCanReach);
		if (piecesThatCanReach.Count != 0)
		{
			GameColor colorToReturn = piecesThatCanReach[0].Color;
			for (int i = 0; i < piecesThatCanReach.Count; i++)
			{
				if (piecesThatCanReach[i].Color != colorToReturn)
					return GameColor.Mixed;
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
				GameColor? color = DeterminePositionInfluence(cords);
				if (color != null)
				{
					_influenceCoordinates.Add(new InfluenceCoordinates(cords, color.Value));
					_influenceCoordinatesHs.Add(new InfluenceCoordinates(cords, color.Value));
				}
			}
		}
	}	
	
	/// <summary>
	/// Updates the influence coordinates of the chessboard and the ChessPieces.
	/// </summary>
	public void UpdateInfluenceCoordinates()
	{
		_influenceCoordinates.Clear();
		_influenceCoordinatesHs.Clear();
		AddInfluenceCoordinates();
	}

	/// <summary>
	/// Checks if the game is in a stalemate.
	/// </summary>
	/// <returns></returns>
	public bool IsStalemate()
	{
		List<ChessPiece> pieces = WhoseTurn == GameColor.White ? _pieceManager.WhitePieces : _pieceManager.BlackPieces;
		List<InfluenceCoordinates> influenceCoordinatesCopy = new List<InfluenceCoordinates>(_influenceCoordinates);
		foreach (var coordinate in influenceCoordinatesCopy)
		{
			if (coordinate.Color != WhoseTurn && coordinate.Color != GameColor.Mixed)
				continue;
			foreach (ChessPiece piece in pieces)
			{
				if (CanPieceGetToPosition(piece, coordinate))
				{
					MakeMove(piece, coordinate);
					bool isCheck = IsUnderAttack(_pieceManager.GetKing(_whoseTurn == GameColor.White ? GameColor.Black : GameColor.White));
					UnmakeMove();
					pieces = WhoseTurn == GameColor.White ? _pieceManager.WhitePieces : _pieceManager.BlackPieces;
					if (!isCheck)
						return false;
				}
			}
		}
		return true;
	}
	
	/// <summary>
	/// Determines if the king at the specified coordinates is in check.
	/// </summary>
	/// <param name="kingCoordinates">The coordinates of the king to check.</param>
	/// <returns>
	///   <c>true</c> if the king is in check; otherwise, <c>false</c>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when the specified king coordinates are null.</exception>
	public bool IsUnderAttack(ChessPiece? king)
	{
		if (_influenceCoordinatesHs.Contains(king.Cord))
			return true;
		return false;
	}
	
	/// <summary>
	/// Determines the current state of the chess game.
	/// </summary>
	/// <returns>The current state of the chess game.</returns>
	public GameState GetGameState()
	{
		bool check = IsUnderAttack(_pieceManager.GetKing(_whoseTurn));
		bool stalemate = IsStalemate();
		
		switch (check)
		{
			case true when stalemate:
				return GameState.Mate;
			case false when stalemate:
				return GameState.Stalemate;
			case true when !stalemate:
				return GameState.Check;
			default:
				return GameState.Normal;
		}
	}
	
	public bool CanPieceGetToPosition(ChessPiece piece, BaseCoordinates coordinate)
	{
		ChessPiece? pieceOnPosition = this[coordinate];
		if (pieceOnPosition is not null && pieceOnPosition.Color == piece.Color)
			return false;

		if (!piece.IsMoveValid(coordinate))
			return false;
		if (piece is Pawn && !PawnHelper.IsMoveValid(piece, coordinate, this))
			return false;
		BaseCoordinates vector = piece.GetValidVector(coordinate);
		
		BaseCoordinates cordCopy = new BaseCoordinates(piece.Cord + vector);
		while (cordCopy != coordinate)
		{
			if (this[cordCopy] is not null)
				return false;
			cordCopy += vector;
		}
		return true;
	}
	
	private void UpdatePieceValidMoves(ChessPiece piece)
	{
		List<InfluenceCoordinates> coordinatesList = new List<InfluenceCoordinates>(_influenceCoordinates);
		List<BaseCoordinates> validMoves = new List<BaseCoordinates>();
		_activePiece = piece;
		_activePiece.ValidMoves.Clear();
		foreach (InfluenceCoordinates coordinate in coordinatesList)
		{
			if (coordinate.Color != _activePiece.Color && coordinate.Color != GameColor.Mixed)
				continue;
			if (CanPieceGetToPosition(_activePiece, coordinate))
			{
				MakeMove(_activePiece, coordinate);
				if (IsUnderAttack(PieceManager.GetKing(_activePiece.Color)))
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

	public void UpdateValidMoves()
	{
		foreach (ChessPiece piece in _pieceManager.BlackPieces)
			UpdatePieceValidMoves(piece);
		foreach (ChessPiece piece in _pieceManager.WhitePieces)
			UpdatePieceValidMoves(piece);
	}
	
	/// <summary>
	/// Reverts the chessboard to its state before the last move.
	/// </summary>
	public void UnmakeMove()
	{
		if (_undoStack.Count == 0)
			return;
		
		ChessBoardMemento undo = _undoStack.Pop();
		
		_activePiece = undo.ActivePiece;
		_pieceManager = undo.PieceManager;
		_influenceCoordinates = undo.InfluenceCoordinates;
		_influenceCoordinatesHs = undo.InfluenceCoordinatesHs;
		_whoseTurn = undo.WhoseTurn;
	}
	
	public void MakeMove(ChessPiece pieceToMove, BaseCoordinates destination)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == GameColor.White ? _pieceManager.BlackPieces : _pieceManager.WhitePieces;
		_undoStack.Push(new ChessBoardMemento(this));
		ChessPiece? capturedPiece = this[destination];
		if (capturedPiece is not null)
			enemyPieces.Remove(capturedPiece);
		pieceToMove.Move(destination);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == GameColor.White ? GameColor.Black : GameColor.White;
	}

	public void LogCurrentBoardState()
	{
		BoardState state = new BoardState
		{
			WhoseTurn = WhoseTurn == GameColor.White ? "White" : "Black",
			WhitePieces = _pieceManager.ConvertPieceListToString(_pieceManager.WhitePieces),
			BlackPieces = _pieceManager.ConvertPieceListToString(_pieceManager.BlackPieces)
		};
		Logger.Log(state);
	}
	
	/// <summary>
	/// Makes a move on the chessboard, updating the game state accordingly.
	/// </summary>
	/// <param name="move">The move to make.</param>
	public void MakeMoveLog(Move move)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == GameColor.White ? _pieceManager.BlackPieces : _pieceManager.WhitePieces;
		_undoStack.Push(new ChessBoardMemento(this));
		ChessPiece? capturedPiece = this[move.Destination];
		if (capturedPiece is not null)
			enemyPieces.Remove(capturedPiece);
		
		var moveLog = new ChessMoves
		{
			Piece = move.Piece.ToString(),
			Color = move.Piece.Color == GameColor.White ? "White" : "Black",
			WhereFrom = move.Piece.Cord.ToString(),
			WhereTo = move.Destination.ToString(),
			Capture = capturedPiece?.ToString(),
			Date = DateTime.Now
		};
		Logger.Log(moveLog);
		this[move.Piece.Cord]?.Move(move.Destination);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == GameColor.White ? GameColor.Black : GameColor.White;
		LogCurrentBoardState();
	}
	
	/// <summary>
	/// Makes a move on the chessboard, updating the game state accordingly.
	/// </summary>
	/// <param name="move">The move to make.</param>
	public void MakeMove(Move move)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == GameColor.White ? _pieceManager.BlackPieces : _pieceManager.WhitePieces;
		_undoStack.Push(new ChessBoardMemento(this));
		
		ChessPiece? capturedPiece = this[move.Destination];
		if (capturedPiece is not null)
			enemyPieces.Remove(capturedPiece);
		this[move.Piece.Cord]?.Move(move.Destination);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == GameColor.White ? GameColor.Black : GameColor.White;
	}

	public void LoadBoardState(BoardState state)
	{
		string whitePieces = state.WhitePieces;
		string blackPieces = state.BlackPieces;
		_whoseTurn = state.WhoseTurn == "White" ? GameColor.White : GameColor.Black;
		_pieceManager.WhitePieces = _pieceManager.ConvertStringToPieceList(whitePieces, GameColor.White);
		_pieceManager.BlackPieces = _pieceManager.ConvertStringToPieceList(blackPieces, GameColor.Black);
		UpdateInfluenceCoordinates();
		UpdateValidMoves();
	}
	
	public ChessPiece? this [BaseCoordinates cords]
	{
		get { return _pieceManager.FindPieceOnPosition(cords); }
	}
}
