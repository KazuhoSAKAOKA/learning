namespace BlazorWebAppLearnAlignStone.Client.Model;




public enum StoneType
{
	Black,
	White,
}

public record struct CellState(StoneType? Stone, bool Highlights);

public class GameEndedEventArgs : EventArgs
{
	public bool Tie { get; set; }
	public StoneType? Winner { get; init; }
}


public interface IGameBoard
{
	void Reset();
	CellState GetCellState(int x, int y);

	void Operate(Operation operation);

	event EventHandler<GameEndedEventArgs> GameEnded;

}

public record struct Point(int X, int Y);
public record struct Operation(Point Point, StoneType Player);

public enum GameState
{
	TurnOfBlack,
	TurnOfWhite,
	BlackWinner,
	WhiteWinner,
	Tied,
}

public class GameBoard
{
	GameState _gameState;
    public GameState GameState => _gameState;

    public event EventHandler<GameEndedEventArgs>? GameEnded;

	protected virtual void OnGameEnded(GameEndedEventArgs e) => GameEnded?.Invoke(this, e);

	private static readonly IReadOnlyCollection<Point> _directions = new[]
	{
		new Point(-1,-1),
		new Point(0,-1),
		new Point(1,-1),
		new Point(-1,0),
		new Point(1,0),
		new Point(-1,1),
		new Point(0,1),
		new Point(1,1),
	};
	private static readonly IReadOnlyCollection<(Point, Point)> _judgeDirections = new[]
	{
		(new Point(-1,-1),new Point(1,1)),
		(new Point(-1,1),new Point(1,-1)),
		(new Point(-1,0),new Point(1,0)),
		(new Point(0,-1),new Point(0,1)),
	};


	private bool InRange(Point point) => 0 <= point.X && point.X < _size && 0 <= point.Y && point.Y < _size;

	private readonly CellState[][] _cellStates;
	private readonly int _size;
	private readonly int _winningLimit;
	private readonly List<Operation> _history = new List<Operation>();
	private bool _over;
	public GameBoard(int size, int winningLimit)
	{
		_size = size;
		_winningLimit = winningLimit;
		_cellStates = Enumerable.Range(0, _size).Select(_ => Enumerable.Range(0, _size).Select(_ => new CellState()).ToArray()).ToArray();
	}

	private static Point AddPoint(Point v1, Point v2) => new Point(v1.X + v2.X, v1.Y + v2.Y);

	public void Reset()
	{
		_history.Clear();
        _over = false;
        for (var i = 0; i < _cellStates.Length; i++)
		{
			var columns = _cellStates[i];
			for (var j = 0; j < columns.Length; j++)
			{
				columns[j] = new CellState();
			}
		}
		_gameState = GameState.TurnOfBlack;
    }

	public CellState GetCellState(int x, int y) => GetCellState(new Point(x, y));
    public CellState GetCellState(Point p) => _cellStates[p.Y][p.X];

	public void Operate(Operation operation)
	{
		if (operation.Point.X < 0 || _cellStates.Length <= operation.Point.X)
		{
			throw new ArgumentOutOfRangeException(nameof(operation.Point.X));
		}
		var columns = _cellStates[operation.Point.Y];
		if (operation.Point.Y < 0 || columns.Length <= operation.Point.Y)
		{
			throw new ArgumentOutOfRangeException(nameof(operation.Point.Y));
		}
		if (_history.Count > 0 && _history[_history.Count - 1].Player == operation.Player)
		{
			throw new InvalidOperationException();
		}
		if (_over)
		{
			throw new InvalidOperationException();
		}
		var cell = columns[operation.Point.X];
		if (cell.Stone != null)
		{
			throw new InvalidOperationException();
		}
		_gameState = operation.Player == StoneType.Black ? GameState.TurnOfWhite : GameState.TurnOfBlack;
		columns[operation.Point.X] = new CellState(operation.Player, false);
		_history.Add(operation);
		Judge(operation.Point);
	}


	private void Judge(Point point)
	{
		var cell = _cellStates[point.Y][point.X];

        if (!cell.Stone.HasValue) { return; }
		var stone = cell.Stone.Value;
		bool isWin = false;
		foreach (var (dir1, dir2) in _judgeDirections)
		{
			var arr1 = GetStreakPoints(point, dir1, x => x.Stone != null && x.Stone.Value == stone).ToArray();
			var arr2 = GetStreakPoints(AddPoint(point, dir2), dir2, x => x.Stone != null && x.Stone.Value == stone).ToArray();
			if (_winningLimit <= arr1.Length + arr2.Length)
			{
				isWin = true;

				foreach(var arr in arr1.Concat(arr2))
				{
					_cellStates[arr.Y][arr.X].Highlights = true;
				}
			}
		}

		if (!isWin)
		{
			if (_history.Count == _size * _size)
			{
				_over = true;
				_gameState = GameState.Tied;
				OnGameEnded(new GameEndedEventArgs() { Tie = true, Winner = null, });
			}
			return;
		}
		_over = true;

		if(stone == StoneType.Black)
		{
            _gameState = GameState.BlackWinner;
        }
		else
		{
            _gameState = GameState.WhiteWinner;
        }

        OnGameEnded(new GameEndedEventArgs() { Tie = false, Winner = stone, });
	}

	private IEnumerable<Point> GetStreakPoints(Point point, Point direction, Func<CellState, bool> predicate)
	{
		if (!InRange(point)) { yield break; }
		if (!predicate(_cellStates[point.Y][point.X])) { yield break; }
		foreach (var p in GetStreakPoints(AddPoint(point, direction), direction, predicate))
		{
			yield return p;
		}
		yield return point;
	}

	public static Operation CreateOperation(int x, int y, StoneType stone) => new Operation(new Point(x, y), stone);
}