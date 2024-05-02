namespace ChessProject.ChessBoards;

using ChessPieces;
using static Console;

public class ChessBoard
{
	private ChessBoard? _boardBefore;
	private List<ChessPiece> _whitePieces;
	private List<ChessPiece> _blackPieces;
	private List<InfluenceCoordinates> _influenceCoordinates;
	private BoardSize _size;
	private EPieceColor _whoseTurn;
	
	//Default Constructor
	public ChessBoard()
	{
		_boardBefore = null;
		_whitePieces = new List<ChessPiece>();
		_blackPieces = new List<ChessPiece>();
		_influenceCoordinates = new List<InfluenceCoordinates>();
		_size = new BoardSize();
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
		_influenceCoordinates = new	List<InfluenceCoordinates>(toCopyFrom.InfluenceCoordinates);
		_size = new BoardSize();
		_whoseTurn = toCopyFrom.WhoseTurn;
	}

	public BoardSize Size
	{
		get { return _size; }
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
	/// Determines if there is a chess piece on the specified position on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to check.</param>
	/// <returns>
	/// The chess piece occupying the specified position, or <c>null</c> if no piece is present at that position.
	/// </returns>
	public ChessPiece? IsPieceOnPosition(BaseCoordinates cords)
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
		return null;
	}
	
	/// <summary>
	/// Updates the influence coordinates of the chessboard.
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
		if (kingCoordinates is null)
			throw new ArgumentException("King not found.");
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
	public void UndoMove()
	{
		if (_boardBefore is null)
			return;
		_whitePieces = _boardBefore.WhitePieces;
		_blackPieces = _boardBefore.BlackPieces;
		_influenceCoordinates = _boardBefore.InfluenceCoordinates;
		_size = new BoardSize();
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
		List<ChessPiece> pieces = WhoseTurn == EPieceColor.White ? _whitePieces : _blackPieces;
		for (int j = 0; j < _influenceCoordinates.Count; j++)
		{
			if (_influenceCoordinates[j].Color != WhoseTurn && _influenceCoordinates[j].Color != EPieceColor.Mixed)
				continue;
			for (int i = 0; i < pieces.Count; i++)
			{
				if (pieces[i].CanGetToPosition(_influenceCoordinates[j], this))
				{
					MakeMove(pieces[i], _influenceCoordinates[j]);
					bool isCheck = IsCheck(FindTheKing(_whoseTurn == EPieceColor.White ? EPieceColor.Black : EPieceColor.White));
					UndoMove();
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

	public void GetPiecesThatCanReachPosition(List<ChessPiece> pieces, BaseCoordinates cords, List<ChessPiece> piecesThatCanReach)
	{
		for (int i = 0; i < pieces.Count; i++)
		{
			int diffX = Math.Abs(cords.PosX - pieces[i].Cord.PosX);
			int diffY = Math.Abs(cords.PosY - pieces[i].Cord.PosY);
			if (diffX == 0 && diffY == 0)
				continue;
			if (pieces[i].CanGetToPosition(cords, this))
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
		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 8; x++)
			{
				BaseCoordinates cords = new BaseCoordinates(x, y);
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
	
	/// <summary>
	/// Validates a player's move by checking if the destination position is within the influence of the player's color.
	/// </summary>
	/// <param name="piece">The chess piece being moved.</param>
	/// <param name="whereToMove">The coordinates of the destination position.</param>
	/// <exception cref="ArgumentException">Thrown when the destination position is not within the influence of the player's color.</exception>
	public void ValidatePlayerMove(ChessPiece piece, BaseCoordinates whereToMove)
	{
		foreach(InfluenceCoordinates cords in InfluenceCoordinates)
		{
			if (cords == whereToMove && cords.Color != WhoseTurn)
			{
				throw new ArgumentException("Invalid piece, please try again.");
			}
		}
	}
	
	/// <summary>
	/// Checks if the piece at the specified position matches the expected piece type and color.
	/// </summary>
	/// <param name="piece">The chess piece to check.</param>
	/// <param name="cords">The coordinates of the position to check.</param>
	/// <param name="pieceType">The expected type of the piece.</param>
	/// <param name="color">The expected color of the piece.</param>
	/// <exception cref="ArgumentException">Thrown when the piece does not match the expected type or color, or when the piece is null.</exception>
	public void IsExpectedPieceAtPosition(ChessPiece piece, BaseCoordinates cords, char pieceType, EPieceColor color)
	{
		if (piece == null || piece.Type != pieceType || piece.Color != color)
			throw new ArgumentException("Invalid piece, please try again.");
	}
	
	/// <summary>
	/// Parses the player's move and moves the piece on the chessboard.
	/// </summary>
	public void PlayerMove()
	{
		while (true)
		{
			try
			{
				WriteLine("Taking the info of the piece to move...");
				char pieceType = ChessPieceParser.TakePieceType();
				BaseCoordinates piecePosition = new BaseCoordinates(ChessPieceParser.TakePositionOnBoard());
				ChessPiece? piece = IsPieceOnPosition(piecePosition);
				IsExpectedPieceAtPosition(piece, piecePosition, pieceType, WhoseTurn);
				WriteLine("Taking the info of the position to move to...");
				BaseCoordinates whereToMove = new BaseCoordinates(ChessPieceParser.TakePositionOnBoard());
				ValidatePlayerMove(piece, whereToMove);
				MakeMove(piece, whereToMove);
				break;
			}
			catch (Exception ex)
			{
				WriteLine(ex.Message);
			}
		}
	}

	/// <summary>
	/// Finds the chess piece at the specified coordinates on the chessboard.
	/// </summary>
	/// <param name="cords">The coordinates of the position to search for.</param>
	/// <returns>The chess piece at the specified coordinates.</returns>
	/// <exception cref="ArgumentException">Thrown when no piece is found at the specified coordinates.</exception>
	public ChessPiece FindThePiece(BaseCoordinates cords)
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
		throw new ArgumentException("Piece not found.");
	}
	
	/// <summary>
	/// Makes a move on the chessboard, updating the game state accordingly.
	/// </summary>
	/// <param name="pieceToMove">The chess piece to move.</param>
	/// <param name="whereToMove">The coordinates of the destination position.</param>
	/// <exception cref="ArgumentException">Thrown when attempting to move a piece to an invalid position or when the piece to move is null.</exception>
	public void MakeMove(ChessPiece pieceToMove, BaseCoordinates whereToMove)
	{
		List<ChessPiece> enemyPieces = WhoseTurn == EPieceColor.White ? BlackPieces : WhitePieces;
		_boardBefore = new ChessBoard(this);
		
		ChessPiece? capturedPiece = IsPieceOnPosition(whereToMove);
		if (capturedPiece != null)
			enemyPieces.Remove(capturedPiece);
		pieceToMove = FindThePiece(pieceToMove.Cord);
		pieceToMove.Move(whereToMove, this);
		UpdateInfluenceCoordinates();
		WhoseTurn = WhoseTurn == EPieceColor.White ? EPieceColor.Black : EPieceColor.White;
	}

	/// <summary>
	/// The main game loop.
	/// </summary>
	public void TheGame()
	{
		ChessBot bot = new ChessBot();
		while (true)
		{
			EGameState gameState = GetGameState();
			switch (gameState)
			{
				case EGameState.Mate:
					WriteLine($"Mate for {_whoseTurn}s, game over.");
					return;
				case EGameState.Stalemate:
					WriteLine($"Stalemate for {WhoseTurn}s, game over.");
					return;
				case EGameState.Check:
					WriteLine($"Check for {WhoseTurn}s.");
					break;
			}
			ChessBoardDisplayer.DisplayChessBoard(this);
			switch (WhoseTurn)
			{
				case EPieceColor.Black:
					WriteLine("Black's turn");
					bot.MakeMove(this);
					break;
				case EPieceColor.White:
					WriteLine("White's turn");
					PlayerMove();
					break;
			}
		}
	}
}

