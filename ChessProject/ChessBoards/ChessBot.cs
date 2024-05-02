using ChessProject.ChessPieces;

namespace ChessProject.ChessBoards;

/// <summary>
/// Represents a chess bot that can play chess using the Minimax algorithm.
/// </summary>
public class ChessBot
{
	private Move _chosenMove;
	
	/// <summary>
	/// Calculates the combined value of pieces on the chessboard for the given player.
	/// </summary>
	/// <param name="whitePieces">The list of white pieces on the chessboard.</param>
	/// <param name="blackPieces">The list of black pieces on the chessboard.</param>
	/// <returns>The combined value of pieces for the given player.</returns>
	public  int CalculateCombinedValue(List<ChessPiece> whitePieces, List<ChessPiece> blackPieces)
	{
		int combinedValue = 0;
		foreach (ChessPiece piece in blackPieces)
		{
			combinedValue += piece.Value;
		}

		foreach (ChessPiece piece in whitePieces)
		{
			combinedValue -= piece.Value;
		}

		return combinedValue;
	}

	/// <summary>
	/// Gets the valid moves for black pieces on the chessboard.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	/// <returns>The list of valid moves for black pieces.</returns>
	public List<Move> GetValidMovesForBlacks(ChessBoard board)
	{
		List<Move> moves = new List<Move>();
		foreach (InfluenceCoordinates position in board.InfluenceCoordinates)
		{
			if (position.Color == EPieceColor.White)
				continue;
			for (int i = 0; i < board.BlackPieces.Count; i++)
			{
				ChessPiece piece = board.BlackPieces[i];
				if (piece.CanGetToPosition(position, board))
				{
					Move move = new Move(piece.Clone(), piece.Cord, position);
					moves.Add(move);
				}
			}
		}
		return (moves);
	}

	/// <summary>
	/// Gets the valid moves for white pieces on the chessboard.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	/// <returns>The list of valid moves for white pieces.</returns>
	public List<Move> GetValidMovesForWhites(ChessBoard board)
	{
		List<Move> moves = new List<Move>();
		foreach (InfluenceCoordinates position in board.InfluenceCoordinates)
		{
			if (position.Color == EPieceColor.Black)
				continue;
			for (int i = 0; i < board.WhitePieces.Count; i++)
			{
				ChessPiece piece = board.WhitePieces[i];
				if (piece.CanGetToPosition(position, board))
				{
					Move move = new Move(piece.Clone(), piece.Cord, position);
					moves.Add(move);
				}
			}
		}
		return (moves);
	}

	/// <summary>
	/// Performs the Minimax algorithm to determine the best move for the current player.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	/// <param name="depth">The depth of the Minimax search tree.</param>
	/// <param name="isMaximizing">A value indicating whether the algorithm is maximizing the score.</param>
	/// <returns>The value of the best move found by the Minimax algorithm.</returns>
	public int Minimax(ChessBoard board, int depth, bool isMaximizing)
	{
		if (depth == 0)
			return (CalculateCombinedValue(board.WhitePieces, board.BlackPieces));
		if (isMaximizing)
		{
			int bestMoveValue = int.MinValue;
			List<Move> moves = GetValidMovesForBlacks(board);
			Move bestMove = moves[0];
			foreach (Move move in GetValidMovesForBlacks(board))
			{
				board.MakeMove(move.Piece, move.Destination);
				int value = Minimax(board, depth - 1, false);
				if (value > bestMoveValue)
				{
					bestMoveValue = value;
					bestMove = move;
				}
				board.UndoMove();
			}
			_chosenMove = bestMove;
			return bestMoveValue;
		}
		else if (!isMaximizing)
		{
			int bestMoveValue = int.MaxValue;
			List<Move> moves = GetValidMovesForWhites(board);
			Move bestMove = moves[0];
			foreach (Move move in moves)
			{
				board.MakeMove(move.Piece, move.Destination);
				int value = Minimax(board, depth - 1, true);
				if (value < bestMoveValue)
				{
					bestMoveValue = value;
					bestMove = move;
				}
				board.UndoMove();
			}
			_chosenMove = bestMove;
			return bestMoveValue;
		}
		return CalculateCombinedValue(board.WhitePieces, board.BlackPieces);
	}
	
	/// <summary>
	/// Makes a move on the chessboard using the Minimax algorithm.
	/// </summary>
	/// <param name="board">The current chessboard state.</param>
	public void MakeMove(ChessBoard board)
	{
		Minimax(board, 3, true);
		board.MakeMove(_chosenMove.Piece, _chosenMove.Destination);
	}
}