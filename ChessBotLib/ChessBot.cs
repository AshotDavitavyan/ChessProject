using System.Data;
using ChessBoardLib;
using ChessPieceLib;
using CoordinatesLib;
using MoveLib;

namespace ChessBotLib;

public static class ChessBot
{
	private static Move? chosenMove;

	private static int ForceKingToCenterEval(BaseCoordinates allyKingCoordinate, BaseCoordinates enemyKingCoordinate, int endgameWeight)
	{
		int evaluation = 0;

		int enemyKingX = enemyKingCoordinate.PosX;
		int enemyKingY = enemyKingCoordinate.PosY;

		int enemyKingDistanceFromCenterX = Math.Max(3 - enemyKingX, enemyKingX - 4);
		int enemyKingDistanceFromCenterY = Math.Max(3 - enemyKingY, enemyKingY - 4);

		int enemyKingDistanceFromCenter = enemyKingDistanceFromCenterX + enemyKingDistanceFromCenterY;
		evaluation += enemyKingDistanceFromCenter;

		int allyKingX = allyKingCoordinate.PosX;
		int allyKingY = allyKingCoordinate.PosY;
		int distanceBetweenKingsX = Math.Abs(allyKingX - enemyKingX);
		int distanceBetweenKingsY = Math.Abs(allyKingY - enemyKingY);

		int distanceBetweenKings = distanceBetweenKingsX + distanceBetweenKingsY;
		evaluation += 14 - distanceBetweenKings;
		
		return evaluation * endgameWeight;
	}
	
	private static int Evaluate(ChessBoard board)
	{
		int sum = 0;
		List<ChessPiece> allyPieces = board.WhoseTurn == GameColor.Black ? board.PieceManager.BlackPieces : board.PieceManager.WhitePieces;
		List<ChessPiece> enemyPieces = board.WhoseTurn == GameColor.Black ? board.PieceManager.WhitePieces : board.PieceManager.BlackPieces; 
		foreach (ChessPiece piece in allyPieces)
			sum += piece.Value;
		foreach (ChessPiece piece in enemyPieces)
			sum -= piece.Value;

		return sum;
	}

	private static List<Move> GenerateMoves(ChessBoard board, GameColor whoseTurn)
	{
		var coordinatesList = 
			new List<InfluenceCoordinates>(board.InfluenceCoordinates);
		var moves = new List<Move>();
		foreach (InfluenceCoordinates coordinate in coordinatesList)
		{
			if (coordinate.Color != whoseTurn && coordinate.Color != GameColor.Mixed)
				continue;
			ChessPiece? possibleKing = board[coordinate];
			if (possibleKing is not null && possibleKing is King)
				continue;
			var pieces = whoseTurn == GameColor.Black ? board.PieceManager.BlackPieces : board.PieceManager.WhitePieces;
			AddMovesForCoordinate(board, whoseTurn, pieces, coordinate, moves);
		}
		 
		moves = moves.OrderByDescending(move => move.MoveValue).ToList();
		return moves;
	}

	private static void AddMovesForCoordinate(ChessBoard board, GameColor whoseTurn, List<ChessPiece> pieces,
		InfluenceCoordinates coordinate, List<Move> moves)
	{
		foreach (ChessPiece piece in pieces)
		{
			board.ActivePiece = piece.Clone();
			if (piece is King && coordinate.Color != piece.Color)
				continue;
			if (board.CanPieceGetToPosition(piece, coordinate))
			{
				board.MakeMove(piece, coordinate);
				if (board.IsUnderAttack(board.PieceManager.GetKing(whoseTurn)))
				{
					board.UnmakeMove();
					continue;
				}
				board.UnmakeMove();
				Move move = new Move(board.ActivePiece, coordinate, board[coordinate]);
				moves.Add(move);
			}
		}
	}

	private static int Minimax(ChessBoard board, int depth, int alpha, int beta)
	{
		int bestValue = -int.MaxValue;
		if (depth == 0)
		{
			int rawBoard = Evaluate(board);
			int kingToCenter = ForceKingToCenterEval(board.PieceManager.WhiteKing.Cord,
				board.PieceManager.BlackKing.Cord, 5);
			return rawBoard + kingToCenter;
		}

		List<Move> moves = GenerateMoves(board, board.WhoseTurn);

		if (moves.Count == 0)
		{
			if (board.GetGameState() == GameState.Mate)
				return -int.MaxValue + 1;
			return 0;
		}

		foreach (Move move in moves)
		{
			board.MakeMove(move);
			int moveValue = -Minimax(board, depth - 1, -beta, -alpha);
			board.UnmakeMove();
			if (moveValue > bestValue)
			{
				bestValue = moveValue;
				if (depth == 3)
					chosenMove = move;
			}

			alpha = Math.Max(alpha, moveValue);
			
			if (alpha >= beta)
				return beta;
		}
			
		return alpha;
	}
	
	/// <summary>
	/// Makes a move on the chessboard using the Minimax algorithm.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	public static void Think(ChessBoard board)
	{
		Minimax(board, 3, -int.MaxValue, int.MaxValue);
		board.MakeMoveLog(chosenMove);
	}
}