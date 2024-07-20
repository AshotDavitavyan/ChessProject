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
		List<ChessPiece> allyPieces = board.WhoseTurn == EPieceColor.Black ? board.BlackPieces : board.WhitePieces;
		List<ChessPiece> enemyPieces = board.WhoseTurn == EPieceColor.Black ? board.WhitePieces : board.BlackPieces; 
		foreach (ChessPiece piece in allyPieces)
			sum += piece.Value;
		foreach (ChessPiece piece in enemyPieces)
			sum -= piece.Value;

		return sum;
	}

	private static List<Move> GenerateMoves(ChessBoard board)
	{
		List<InfluenceCoordinates> coordinatesList = new List<InfluenceCoordinates>(board.InfluenceCoordinates);
		List<Move> moves = new List<Move>();
		foreach (InfluenceCoordinates coordinate in coordinatesList)
		{
			if (coordinate.Color != board.WhoseTurn && coordinate.Color != EPieceColor.Mixed)
				continue;
			ChessPiece? possibleKing = board.FindPieceOnPosition(coordinate);
			if (possibleKing is not null && possibleKing is King)
				continue;
			List<ChessPiece> pieces = board.WhoseTurn == EPieceColor.Black ? board.BlackPieces : board.WhitePieces;
			foreach (ChessPiece piece in pieces)
			{
				board.ActivePiece = piece.Clone();
				if (piece is King && coordinate.Color != piece.Color)
					continue;
				if (board.CanPieceGetToPosition(piece, coordinate))
				{
					board.MakeMove(piece, coordinate);
					if (board.IsCheck(board.FindTheKing(board.WhoseTurn == EPieceColor.Black ? EPieceColor.White : EPieceColor.Black)))
					{
						board.UnmakeMove();
						continue;
					}
					board.UnmakeMove();
					Move move = new Move(board.ActivePiece, coordinate, board.FindPieceOnPosition(coordinate));
					moves.Add(move);
				}
			}
		}
		 
		moves = moves.OrderByDescending(move => move.MoveValue).ToList();
		return moves;
	}
	
	private static int Minimax(ChessBoard board, int depth, int alpha, int beta)
	{
		int bestValue = -int.MaxValue;
		if (depth == 0)
		{
			int rawBoard = Evaluate(board);
			int kingToCenter = ForceKingToCenterEval(board.FindTheKing(EPieceColor.White),
				board.FindTheKing(EPieceColor.Black), 5);
			return rawBoard + kingToCenter;
		}

		List<Move> moves = GenerateMoves(board);

		if (moves.Count == 0)
		{
			if (board.GetGameState() == EGameState.Mate)
				return -int.MaxValue;
			return 0;
		}

		foreach (Move move in moves)
		{
			try
			{
				board.MakeMove(move);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
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
	public static void MakeMove(ChessBoard board)
	{
		Minimax(board, 3, -int.MaxValue, int.MaxValue);
		board.MakeMove(chosenMove);
	}
}