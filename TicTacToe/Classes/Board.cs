using System;

namespace TicTacToe.Classes {

	public enum Mark { None, X, O }

	class Board {

		private Mark[,] _tiles;

		public Board() {
			_tiles = new Mark[3, 3];
		}

		public Mark this[byte x, byte y] {
			get {
				if (x > 2) throw new ArgumentOutOfRangeException("x", "The x position must be between 0 and 2");
				if (y > 2) throw new ArgumentOutOfRangeException("y", "The y position must be between 0 and 2");
				return _tiles[y, x];
			}
			set {
				if (x > 2) throw new ArgumentOutOfRangeException("x", "The x position must be between 0 and 2");
				if (y > 2) throw new ArgumentOutOfRangeException("y", "The y position must be between 0 and 2");
				_tiles[y, x] = value;
			}
		}

		/// <summary>
		/// Checks if a tile is marked.
		/// </summary>
		/// <param name="x">X position</param>
		/// <param name="y">Y position</param>
		public bool IsMarked(byte x, byte y) {
			return _tiles[y, x] != Mark.None;
		}

		/// <summary>
		/// Clears the board.
		/// </summary>
		public void Clear() {
			_tiles = new Mark[3, 3];
		}

		/// <summary>
		/// Checks if there is a draw.
		/// </summary>
		public bool IsFull() {
			foreach (Mark mark in _tiles)
				if (mark == Mark.None)
					return false;
			return true;
		}

		/// <summary>
		/// Returns a copy of the board.
		/// </summary>
		public Board VirtualCopy() {
			Board board = new Board();
			for (byte x = 0; x < 3; x++)
				for (byte y = 0; y < 3; y++)
					board[x, y] = this[x, y];
			return board;
		}

	}

}
