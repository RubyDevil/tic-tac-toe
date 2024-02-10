using System;

namespace TicTacToe.Classes {

	public enum State { InProgress, Paused, Over }
	public enum WinResult { Player, Computer, Draw }

    class GameEngine {

		private Board _board;
		private State _state;
		private WinResult? _winner;
		private byte[,]? _winnerTiles;

		public Board Board {
			get => _board;
			private set => _board = value;
		}

		public State State {
			get => _state;
			private set => _state = value;
		}

		public WinResult? Winner {
			get => _winner;
			private set => _winner = value;
		}

		public byte[,]? WinnerTiles {
			get => _winnerTiles;
			set => _winnerTiles = value;
		}

		public GameEngine(bool hardMode = true) {
			_board = new Board();
			_state = State.Over;
		}

		/// <summary>
		/// Restarts the game.
		/// </summary>
		public void Restart() {
			Board = new Board();
			State = State.InProgress;
		}

		/// <summary>
		/// Pauses the game.
		/// </summary>
		public void Pause() {
			if (State == State.InProgress)
				State = State.Paused;
		}

		/// <summary>
		/// Resumes the game.
		/// </summary>
		public void Resume() {
			if (State == State.Paused)
				State = State.InProgress;
		}

		/// <summary>
		/// Checks if someone won.
		/// </summary>
		public void Update() {
			(WinResult? winner, byte[,]? winnerTiles) = CheckWin(Board);
			if (winner is WinResult winResult) {
				Winner = winResult;
				WinnerTiles = winnerTiles;
				State = State.Over;
			}
		}

		/// <summary>
		/// Checks if someone won.
		/// </summary>
		/// <param name="board">The board to analyze</param>
		public static (WinResult? winner, byte[,]? winnerTiles) CheckWin(Board board) {
			// Check rows
			for (byte y = 0; y < 3; y++)
				if (board[0, y] != Mark.None && board[0, y] == board[1, y] && board[0, y] == board[2, y])
					return (board[0, y] == Mark.X ? WinResult.Player : WinResult.Computer, new byte[,] { { 0, y }, { 1, y }, { 2, y } });
			
			// Check columns
			for (byte x = 0; x < 3; x++)
				if (board[x, 0] != Mark.None && board[x, 0] == board[x, 1] && board[x, 0] == board[x, 2])
					return (board[x, 0] == Mark.X ? WinResult.Player : WinResult.Computer, new byte[,] { { x, 0 }, { x, 1 }, { x, 2 } });
			
			// Check diagonals
			if (board[0, 0] != Mark.None && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
				return (board[0, 0] == Mark.X ? WinResult.Player : WinResult.Computer, new byte[,] { { 0, 0 }, { 1, 1 }, { 2, 2 } });
			
			if (board[2, 0] != Mark.None && board[2, 0] == board[1, 1] && board[2, 0] == board[0, 2])
				return (board[2, 0] == Mark.X ? WinResult.Player : WinResult.Computer, new byte[,] { { 2, 0 }, { 1, 1 }, { 0, 2 } });
			
			// Check draw
			return board.IsFull()
				? (WinResult.Draw, null)
				: (null, null);
		}

	}

}
