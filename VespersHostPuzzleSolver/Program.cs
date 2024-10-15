using System.ComponentModel;
using System.Numerics;
using static PuzzleSolver;

PuzzleSolver ps = new PuzzleSolver();

if (ps.Solve())
{
	Console.WriteLine("Solved");
}
else
{
	Console.WriteLine("Not Solved");
}

public abstract class Terminal : ICloneable
{
	public string Name = "foo";
	public List<int> ValidInputs = new();
	public List<int> PossibleOutputs = new();
	public bool Completed = false; // This becomes true when the terminal is successfully completed.

	public abstract bool ProcessInput(int input);
	public abstract bool ProcessOutput(int output);

	public abstract object Clone();
}

public class MazeTerminal : Terminal
{
	// The Maze terminal has three phases.
	private int Phase = 1; // The current phase. This goes up to 3.

	public override object Clone()
	{
		return new MazeTerminal(this);
	}
	
	public MazeTerminal(MazeTerminal original)
	{
		ValidInputs = new List<int>(original.ValidInputs);
		PossibleOutputs = new List<int>(original.PossibleOutputs);
		Name = original.Name;
		Completed = original.Completed;
		Phase = original.Phase;
	}

	public MazeTerminal()
	{
		ValidInputs = [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
		PossibleOutputs = [];
		Name = "Top Right";
	}

	public override bool ProcessInput(int input)
	{
		if (!ValidInputs.Contains(input))
		{
			return false;
		}
		else
		{
			ValidInputs.Clear();
			switch (Phase)
			{
				case 1:
					if (input == 5 || input == 6)
						PossibleOutputs = [11];
					if (input == 7 || input == 8)
						PossibleOutputs = [11, 15, 17];
					if (input == 9 || input == 10)
						PossibleOutputs = [11, 15, 17, 19, 21, 23];
					if (input == 11 | input == 12)
						PossibleOutputs = [11, 15, 17, 19, 21, 23, 25, 27, 29];
					if (input == 13 || input == 14)
						PossibleOutputs = [11, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35];
					if (input == 15)
						PossibleOutputs = [11, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41];
					break;
				case 2:
					if (input == 3 || input == 4)
						PossibleOutputs = [6];
					if (input == 5 || input == 6)
						PossibleOutputs = [6, 10];
					if (input == 7 || input == 8)
						PossibleOutputs = [6, 10, 14];
					if (input == 9 || input == 10)
						PossibleOutputs = [6, 10, 14, 18];
					if (input == 11 || input == 12)
						PossibleOutputs = [6, 10, 14, 18, 22];
					if (input == 13 || input == 14)
						PossibleOutputs = [6, 10, 14, 18, 22, 26];
					if (input == 15)
						PossibleOutputs = [6, 10, 14, 18, 22, 26, 30];
					break;
				case 3:
					if (input == 5 || input == 6)
						PossibleOutputs = [9];
					if (input == 7 || input == 8)
						PossibleOutputs = [9, 13, 15];
					if (input == 9 || input == 10)
						PossibleOutputs = [9, 13, 15, 17, 19, 21];
					if (input == 11 || input == 12)
						PossibleOutputs = [9, 13, 15, 17, 19, 21, 23, 25, 27];
					if (input == 13 || input == 14)
						PossibleOutputs = [9, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33];
					if (input == 15)
						PossibleOutputs = [9, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39];
					break;
			}

			return true;
		}
	}

	public override bool ProcessOutput(int output)
	{
		if (!PossibleOutputs.Contains(output))
		{
			return false;
		}
		else
		{
			PossibleOutputs.Clear();
			Phase++;
			switch (Phase)
			{
				case 1:
					ValidInputs = [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
					break;
				case 2:
					ValidInputs = [3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
					break;
				case 3:
					ValidInputs = [5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
					break;
				default:
					ValidInputs.Clear();
					Completed = true;
					break;
			}

			return true;
		}
	}
}

public class TicTacToeTerminal : Terminal
{

	// The TicTacToe terminal has only one phase. 1s must win.
	private int[][] Grid = [[-1, -1, -1], [-1, -1, -1], [-1, -1, 0]]; // The tic-tac-toe grid.

	private int[] rowCountsZeros = [0, 0, 1];
	private int[] columnCountsZeros = [0, 0, 1];
	private int[] rowCountsOnes = [0, 0, 0];
	private int[] columnCountsOnes = [0, 0, 0];
	private int forwardDiagonalZeros = 1;
	private int forwardDiagonalOnes = 0;
	private int backwardDiagonalZeros = 0;
	private int backwardDiagonalOnes = 0;

	public override object Clone()
	{
		return new TicTacToeTerminal(this);
	}

	public TicTacToeTerminal(TicTacToeTerminal original)
	{
		ValidInputs = new List<int>(original.ValidInputs);
		PossibleOutputs = new List<int>(original.PossibleOutputs);
		Name = original.Name;
		Completed = original.Completed;

		for (int i = 0; i < original.Grid.Length; ++i)
		{
			for (int j = 0; j < original.Grid[i].Length; ++j)
			{
				Grid[i][j] = original.Grid[i][j];
			}
		}

		original.rowCountsOnes.CopyTo(rowCountsOnes, 0);
		original.rowCountsZeros.CopyTo(rowCountsZeros, 0);
		original.columnCountsZeros.CopyTo(columnCountsZeros, 0);
		original.columnCountsOnes.CopyTo(columnCountsOnes, 0);

		forwardDiagonalOnes = original.forwardDiagonalOnes;
		forwardDiagonalZeros = original.forwardDiagonalZeros;
		backwardDiagonalOnes = original.backwardDiagonalOnes;
		backwardDiagonalZeros = original.backwardDiagonalZeros;
	}

	public TicTacToeTerminal()
	{
		ValidInputs = [0, 1, 2, 4, 5, 6, 8, 9];
		PossibleOutputs = [0, 1, 2, 4, 5, 6, 8, 9];

		Name = "Top Left";
	}

	private int CheckIfSolved()
	{
		foreach (int count in rowCountsZeros)
		{
			if (count >= 3)
			{
				return 0;
			}
		}

		foreach (int count in rowCountsOnes)
		{
			if (count >= 3)
			{
				return 1;
			}
		}

		foreach (int count in columnCountsZeros)
		{
			if (count >= 3)
			{
				return 0;
			}
		}

		foreach (int count in columnCountsOnes)
		{
			if (count >= 3)
			{
				return 1;
			}
		}

		if (forwardDiagonalZeros >= 3)
		{
			return 0;
		}

		if (forwardDiagonalOnes >= 3)
		{
			return 1;
		}

		if (backwardDiagonalZeros >= 3)
		{
			return 0;
		}

		if (backwardDiagonalOnes >= 3)
		{
			return 1;
		}

		return -1;
	}

	public override bool ProcessInput(int input)
	{
		if (!ValidInputs.Contains(input))
		{
			return false;
		}
		else
		{
			// The x-coordinate comes from the 2 least significant bits, and the y-coordinate comes from the next 2.
			int x = input % 4;
			int y = (input / 4) % 4;

			Grid[x][y] = 0;
			PossibleOutputs = ValidInputs;
			ValidInputs = [];
			PossibleOutputs.Remove(input);
			rowCountsZeros[x]++;
			columnCountsZeros[y]++;

			if (x == y)
			{
				forwardDiagonalZeros++;
			}

			if (x + y == 2)
			{
				backwardDiagonalZeros++;
			}

			if (CheckIfSolved() == 0)
			{
				return false;
			}

			return true;
		}
	}

	public override bool ProcessOutput(int output)
	{
		if (!PossibleOutputs.Contains(output))
		{
			return false;
		}
		else
		{
			// The x-coordinate comes from the 2 least significant bits, and the y-coordinate comes from the next 2.
			int x = output % 4;
			int y = (output / 4) % 4;

			Grid[x][y] = 1;
			ValidInputs = PossibleOutputs;
			PossibleOutputs = [];
			ValidInputs.Remove(output);
			rowCountsOnes[x]++;
			columnCountsOnes[y]++;

			if (x == y)
			{
				forwardDiagonalOnes++;
			}

			if (x + y == 2)
			{
				backwardDiagonalOnes++;
			}

			if (CheckIfSolved() == 1)
			{
				Completed = true;
				ValidInputs.Clear();
				PossibleOutputs.Clear();
			}

			return true;
		}
	}
}

public class RockPaperScissorsTerminal : Terminal
{
	// The Rock Paper Scissors terminal has three phases.
	private int Phase = 1; // The current phase. This goes up to 3.

	private int Player = -1;
	private int Opponent = -1;

	public override object Clone()
	{
		return new RockPaperScissorsTerminal(this);
	}

	public RockPaperScissorsTerminal(RockPaperScissorsTerminal original)
	{
		ValidInputs = new List<int>(original.ValidInputs);
		PossibleOutputs = new List<int>(original.PossibleOutputs);
		Name = original.Name;
		Completed = original.Completed;

		Phase = original.Phase;
		Player = original.Player;
		Opponent = original.Opponent;
	}

	public RockPaperScissorsTerminal()
	{
		ValidInputs = [1, 2, 3];
		PossibleOutputs = [1, 2, 3];
		Name = "Bottom Left";
	}

	public override bool ProcessInput(int input)
	{
		if (!ValidInputs.Contains(input))
		{
			return false;
		}
		else
		{
			switch (Player)
			{
				case 1:
					if (input == 3)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case 2:
					if (input == 1)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case 3:
					if (input == 2)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case -1:
					Opponent = input;
					ValidInputs.Clear();
					PossibleOutputs = [input, (input == 3) ? 1 : input + 1];
					break;
			}

			if (Phase > 3)
			{
				Completed = true;
				ValidInputs.Clear();
				PossibleOutputs.Clear();
			}

			return true;
		}
	}

	public override bool ProcessOutput(int output)
	{
		if (!PossibleOutputs.Contains(output))
		{
			return false;
		}
		else
		{
			switch (Opponent)
			{
				case 1:
					if (output == 2)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case 2:
					if (output == 3)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case 3:
					if (output == 1)
						Phase++;
					Player = Opponent = -1;
					ValidInputs = [1, 2, 3];
					PossibleOutputs = [1, 2, 3];
					break;
				case -1:
					Player = output;
					PossibleOutputs.Clear();
					ValidInputs = [output, (output == 1) ? 3 : output - 1];
					break;
			}

			if (Phase > 3)
			{
				Completed = true;
				ValidInputs.Clear();
				PossibleOutputs.Clear();
			}

			return true;
		}
	}
}

public class BooleanTerminal : Terminal
{
	// The Boolean terminal has three phases.
	private int Phase = 1; // The current phase. This goes up to 3.

	private int FirstNum = 7;
	private int SecondNum = -1;
	private int ExpectedResult = -1;
	private int ProvidedResult = -1;
	public override object Clone()
	{
		return new BooleanTerminal(this);
	}

	public BooleanTerminal(BooleanTerminal original)
	{
		ValidInputs = new List<int>(original.ValidInputs);
		PossibleOutputs = new List<int>(original.PossibleOutputs);
		Name = original.Name;
		Completed = original.Completed;

		Phase = original.Phase;
		FirstNum = original.FirstNum;
		SecondNum = original.SecondNum;
		ExpectedResult = original.ExpectedResult;
		ProvidedResult = original.ProvidedResult;
	}

	public BooleanTerminal()
	{
		ValidInputs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
		PossibleOutputs = [0, 1, 2, 3, 4, 5, 6, 7];
		Name = "Bottom Right";
	}

	public override bool ProcessInput(int input)
	{
		if (!ValidInputs.Contains(input))
		{
			return false;
		}
		else
		{
			SecondNum = input;
			switch (Phase)
			{
				case 1:
					ExpectedResult = FirstNum & SecondNum;
					break;
				case 2:
					ExpectedResult = FirstNum ^ SecondNum;
					break;
				case 3:
					ExpectedResult = FirstNum | SecondNum;
					break;
			}

			if (ProvidedResult != -1 && ExpectedResult == ProvidedResult)
			{
				Phase++;
				ValidInputs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
				switch (Phase)
				{
					case 2:
						PossibleOutputs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
						break;
					case 3:
						PossibleOutputs = [6, 7, 14, 15];
						break;
					default:
						PossibleOutputs.Clear();
						ValidInputs.Clear();
						Completed = true;
						break;

				}

				FirstNum = 6; // First num will always be 6 after the first phase.
				SecondNum = -1;
				ExpectedResult = -1;
				ProvidedResult = -1;
				return true;
			}
			else if (ProvidedResult == -1)
			{
				ValidInputs.Clear();
				PossibleOutputs = [ExpectedResult];
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public override bool ProcessOutput(int output)
	{
		if (!PossibleOutputs.Contains(output))
		{
			return false;
		}
		else
		{
			ProvidedResult = output;
			if (SecondNum != -1)
			{
				switch (Phase)
				{
					case 1:
						ExpectedResult = FirstNum & SecondNum;
						break;
					case 2:
						ExpectedResult = FirstNum ^ SecondNum;
						break;
					case 3:
						ExpectedResult = FirstNum | SecondNum;
						break;
				}

				if (ExpectedResult == ProvidedResult)
				{
					Phase++;
					ValidInputs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
					switch (Phase)
					{
						case 2:
							PossibleOutputs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
							break;
						case 3:
							PossibleOutputs = [6, 7, 14, 15];
							break;
						default:
							PossibleOutputs.Clear();
							ValidInputs.Clear();
							Completed = true;
							break;

					}

					FirstNum = 6; // First num will always be 6 after the first phase.
					SecondNum = -1;
					ExpectedResult = -1;
					ProvidedResult = -1;
					return true;
				}
				else if (ProvidedResult == -1)
				{
					PossibleOutputs = [ExpectedResult];
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				ValidInputs.Clear();
				PossibleOutputs.Clear();

				for (int i = 0; i <= 15; ++i)
				{
					switch (Phase)
					{
						case 1:
							if ((FirstNum & i) == ProvidedResult)
							{
								ValidInputs.Add(i);
							}
							break;
						case 2:
							if ((FirstNum ^ i) == ProvidedResult)
							{
								ValidInputs.Add(i);
							}
							break;
						case 3:
							if ((FirstNum | i) == ProvidedResult)
							{
								ValidInputs.Add(i);
							}
							break;
					}
				}

				return true;
			}
		}
	}
}

public class TerminalState(MazeTerminal maze, TicTacToeTerminal ttt, RockPaperScissorsTerminal rps, BooleanTerminal boolean)
{
	public MazeTerminal Maze = maze;
	public TicTacToeTerminal Ttt = ttt;
	public RockPaperScissorsTerminal Rps = rps;
	public BooleanTerminal Boolean = boolean;
	public int LastIndex = -1;
}

public class PuzzleSolver
{
	public class Move
	{
		public Move(Terminal source, Terminal? destination, int value)
		{
			Source = source; Destination = destination; Value = value;
		}

		public Terminal Source;
		public Terminal? Destination;
		public int Value;
	}

	public Stack<Move> moveStack = new Stack<Move>();
	public Stack<TerminalState> terminalStates = new Stack<TerminalState>();
	public Stack<List<Move>> moveListStack = new Stack<List<Move>>();
	public int minSolutionLength = 50;

	public bool Solve()
	{
		MazeTerminal maze = new();
		TicTacToeTerminal ttt = new();
		RockPaperScissorsTerminal rps = new();
		BooleanTerminal boolean = new();

		bool restartWhile = true;

		terminalStates.Push(new(new MazeTerminal(maze), new TicTacToeTerminal(ttt), new RockPaperScissorsTerminal(rps), new BooleanTerminal(boolean)));

		while (terminalStates.Count > 0)
		{
			maze = terminalStates.Peek().Maze;
			ttt = terminalStates.Peek().Ttt;
			rps = terminalStates.Peek().Rps;
			boolean = terminalStates.Peek().Boolean;

			int numCompleted = 0;

			if (maze.Completed)
			{
				numCompleted++;
			}
			if (ttt.Completed)
			{
				numCompleted++;
			}
			if (boolean.Completed)
			{
				numCompleted++;
			}
			if (rps.Completed)
			{
				numCompleted++;
			}

			if (numCompleted == 4 && moveStack.Count <= minSolutionLength)
			{
				minSolutionLength = moveStack.Count;
				Stack<Move> tempStack = new Stack<Move>();
				Console.WriteLine();
				while (moveStack.Count > 0)
				{
					Move move = moveStack.Pop();
					tempStack.Push(move);
				}

				while (tempStack.Count > 0)
				{
					Move move = tempStack.Pop();
					moveStack.Push(move);
					Console.WriteLine((moveStack.Count) + ". "
						+ "Source: " + move.Source.Name + ((move.Destination is not null) ? " Dest: " + move.Destination?.Name : "")
						+ " Value: " + move.Value);
				}
			}

			List<Move> possibleMoves = (!restartWhile) ? moveListStack.Pop() : new List<Move>();
			if (restartWhile)
			{
				for (int i = 0; i < maze.PossibleOutputs.Count; ++i)
				{
					if (numCompleted == 3)
					{
						possibleMoves.Add(new(maze, null, maze.PossibleOutputs[i]));
					}

					if (rps.ValidInputs.Contains(maze.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(maze, rps, maze.PossibleOutputs[i]));
					}

					if (ttt.ValidInputs.Contains(maze.PossibleOutputs[i] % 16))
					{
						possibleMoves.Add(new(maze, ttt, maze.PossibleOutputs[i] % 16));
					}

					if (boolean.ValidInputs.Contains(maze.PossibleOutputs[i] % 16))
					{
						possibleMoves.Add(new(maze, boolean, maze.PossibleOutputs[i] % 16));
					}
				}

				for (int i = 0; i < rps.PossibleOutputs.Count; ++i)
				{
					if (numCompleted == 3)
					{
						possibleMoves.Add(new(rps, null, rps.PossibleOutputs[i]));
					}

					if (maze.ValidInputs.Contains(rps.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(rps, maze, rps.PossibleOutputs[i]));
					}

					if (ttt.ValidInputs.Contains(rps.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(rps, ttt, rps.PossibleOutputs[i]));
					}

					if (boolean.ValidInputs.Contains(rps.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(rps, boolean, rps.PossibleOutputs[i]));
					}
				}

				for (int i = 0; i < ttt.PossibleOutputs.Count; ++i)
				{
					if (numCompleted == 3)
					{
						possibleMoves.Add(new(ttt, null, ttt.PossibleOutputs[i]));
					}

					if (maze.ValidInputs.Contains(ttt.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(ttt, maze, ttt.PossibleOutputs[i]));
					}

					if (rps.ValidInputs.Contains(ttt.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(ttt, rps, ttt.PossibleOutputs[i]));
					}

					if (boolean.ValidInputs.Contains(ttt.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(ttt, boolean, ttt.PossibleOutputs[i]));
					}
				}

				for (int i = 0; i < boolean.PossibleOutputs.Count; ++i)
				{
					if (numCompleted == 3)
					{
						possibleMoves.Add(new(boolean, null, boolean.PossibleOutputs[i]));
					}

					if (maze.ValidInputs.Contains(boolean.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(boolean, maze, boolean.PossibleOutputs[i]));
					}

					if (rps.ValidInputs.Contains(boolean.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(boolean, rps, boolean.PossibleOutputs[i]));
					}

					if (ttt.ValidInputs.Contains(boolean.PossibleOutputs[i]))
					{
						possibleMoves.Add(new(boolean, ttt, boolean.PossibleOutputs[i]));
					}
				}
			}

			// 2. Pursue each valid move.
			restartWhile = false;
			for (int i = terminalStates.Peek().LastIndex + 1; i < possibleMoves.Count; ++i)
			{
				Move move = possibleMoves[i];
				TerminalState newState = new(new MazeTerminal(maze), new TicTacToeTerminal(ttt), new RockPaperScissorsTerminal(rps), new BooleanTerminal(boolean));
				if (move.Source.GetType() == typeof(TicTacToeTerminal))
				{
					move.Source = newState.Ttt;
				}
				if (move.Source.GetType() == typeof(BooleanTerminal))
				{
					move.Source = newState.Boolean;
				}
				if (move.Source.GetType() == typeof(RockPaperScissorsTerminal))
				{
					move.Source = newState.Rps;
				}
				if (move.Source.GetType() == typeof(MazeTerminal))
				{
					move.Source = newState.Maze;
				}

				if (move.Destination is null)
				{
					if (move.Source.ProcessOutput(move.Value))
					{
						// This was a valid move. Let's repeat.
						moveStack.Push(move);
						moveListStack.Push(possibleMoves);
						terminalStates.Peek().LastIndex = i;
						terminalStates.Push(newState);
						//Console.WriteLine(moveStack.Count + ". Attempting Source: " + move.Source.Name + " Value: " + move.Value);
						restartWhile = true;
						break;
					}
				}
				else
				{
					if (move.Destination.GetType() == typeof(TicTacToeTerminal))
					{
						move.Destination = newState.Ttt;
					}
					if (move.Destination.GetType() == typeof(BooleanTerminal))
					{
						move.Destination = newState.Boolean;
					}
					if (move.Destination.GetType() == typeof(RockPaperScissorsTerminal))
					{
						move.Destination = newState.Rps;
					}
					if (move.Destination.GetType() == typeof(MazeTerminal))
					{
						move.Destination = newState.Maze;
					}

					if (move.Source.ProcessOutput(move.Value) && move.Destination.ProcessInput(move.Value))
					{
						// This was a valid move. Let's repeat.
						moveStack.Push(move);
						moveListStack.Push(possibleMoves);
						terminalStates.Peek().LastIndex = i;
						terminalStates.Push(newState);
						/*Console.WriteLine(moveStack.Count + ". Attempting Source: " + move.Source.Name + " Destination: " 
							+ move.Destination.Name + " Value: " + move.Value);*/
						restartWhile = true;
						break;
					}
				}
			}

			if (!restartWhile)
			{
				if (moveStack.Count <= 0)
				{
					return true;
				}
				moveStack.Pop();// We couldn't make a move from here, so pop the top off the stack. Same with the terminal states.
				terminalStates.Pop();

				//Console.WriteLine("Undoing.");
			}
		}

		return true;
	}
}