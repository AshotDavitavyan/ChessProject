using ChessPieceLib;
using CoordinatesLib;

namespace ChessBoardLib;

public class ChessBoardMemento
{
	private ChessPiece? _activePiece;
	private PieceManager _pieceManager;
	private List<InfluenceCoordinates> _influenceCoordinates;
	private HashSet<InfluenceCoordinates> _influenceCoordinatesHs;
	private GameColor _whoseTurn;

	public ChessPiece? ActivePiece
	{
		get => _activePiece;
		set => _activePiece = value;
	}

	public PieceManager PieceManager
	{
		get => _pieceManager;
		set => _pieceManager = value;
	}
	
	public List<InfluenceCoordinates> InfluenceCoordinates
	{
		get => _influenceCoordinates;
		set => _influenceCoordinates = value;
	}
	
	public HashSet<InfluenceCoordinates> InfluenceCoordinatesHs
	{
		get => _influenceCoordinatesHs;
		set => _influenceCoordinatesHs = value;
	}
	
	public GameColor WhoseTurn
	{
		get => _whoseTurn;
		set => _whoseTurn = value;
	}
	
	public ChessBoardMemento(ChessBoard memento)
	{
		_pieceManager = new PieceManager(memento.PieceManager);
		_influenceCoordinates = new List<InfluenceCoordinates>();
		_influenceCoordinatesHs = new HashSet<InfluenceCoordinates>();
		foreach (InfluenceCoordinates cord in memento.InfluenceCoordinates)
			_influenceCoordinates.Add(new InfluenceCoordinates(cord));
		foreach (InfluenceCoordinates cord in memento.InfluenceCoordinatesHs)
			_influenceCoordinatesHs.Add(new InfluenceCoordinates(cord));
		_activePiece = memento.ActivePiece is null
			? null
			: _pieceManager.FindPiece(memento.ActivePiece);
		_whoseTurn = memento.WhoseTurn;
	}
}