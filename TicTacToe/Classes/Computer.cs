using System;

namespace TicTacToe.Classes {

	static class Computer {

		/// <summary>
		/// Gets the best move for the computer.
		/// </summary>
		/// <param name="board">The board to analyze</param>
		public static (byte x, byte y) GetBestMove(Board board) {
			Board virtualBoard = board.VirtualCopy();

			byte[] bestMove = new byte[2];
			int bestScore = -1000;

			for (byte x = 0; x < 3; x++) {
				for (byte y = 0; y < 3; y++) {
					if (virtualBoard[x, y] == Mark.None) {
						virtualBoard[x, y] = Mark.O;
						int score = Minimax(virtualBoard, 0, false);
						virtualBoard[x, y] = Mark.None;
						if (score > bestScore) {
							bestScore = score;
							bestMove[0] = x;
							bestMove[1] = y;
						}
					}
				}
			}

			return (bestMove[0], bestMove[1]);
		}

		private static int Minimax(Board board, int depth, bool isMaximizing) {
			(WinResult? winner, byte[,]? winnerTiles) = GameEngine.CheckWin(board);
			if (winner is WinResult winResult) {
				if (winResult == WinResult.Player) {
					return -10;
				} else if (winResult == WinResult.Computer) {
					return 10;
				} else if (winResult == WinResult.Draw) {
					return 0;
				}
			}

			if (isMaximizing) {
				int bestScore = -1000;
				for (byte x = 0; x < 3; x++) {
					for (byte y = 0; y < 3; y++) {
						if (board[x, y] == Mark.None) {
							board[x, y] = Mark.O;
							int score = Minimax(board, depth + 1, false);
							board[x, y] = Mark.None;
							bestScore = Math.Max(score, bestScore);
						}
					}
				}
				return bestScore;
			} else {
				int bestScore = 1000;
				for (byte x = 0; x < 3; x++) {
					for (byte y = 0; y < 3; y++) {
						if (board[x, y] == Mark.None) {
							board[x, y] = Mark.X;
							int score = Minimax(board, depth + 1, true);
							board[x, y] = Mark.None;
							bestScore = Math.Min(score, bestScore);
						}
					}
				}
				return bestScore;
			}
		}

	}

}
