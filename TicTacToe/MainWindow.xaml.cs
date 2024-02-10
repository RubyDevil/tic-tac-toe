using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TicTacToe.Classes;

namespace TicTacToe {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private GameEngine _gameEngine = new GameEngine();

		private Image[,] _tiles;

		public MainWindow() {
			InitializeComponent();
			// Aggregate the tiles
			_tiles = new Image[3, 3] {
				{ Tile00, Tile10, Tile20 },
				{ Tile01, Tile11, Tile21 },
				{ Tile02, Tile12, Tile22 }
			};
			RenderGameBoard();
			RefreshControls();
		}

		private void RefreshControls() {
			gameBoard.IsEnabled = _gameEngine.State == State.InProgress;
			btnRestart.IsEnabled = _gameEngine.State != State.Over;
			btnPlay.IsEnabled = _gameEngine.State == State.Over;
			switch (_gameEngine.State) {
				case State.InProgress:
					btnPlay.Content = "Pause";
					break;
				case State.Over:
					btnPlay.Content = "Play";
					break;
				case State.Paused:
					btnPlay.Content = "Resume";
					break;
			}
		}

		private void RenderGameBoard() {
			for (byte x = 0; x < 3; x++) {
				for (byte y = 0; y < 3; y++) {
					Mark mark = _gameEngine.Board[x, y];
					_tiles[y, x].Source = new BitmapImage(new Uri($"pack://application:,,,/Resources/Images/{mark}.png"));
				}
			}
		}

		private void CheckWin() {
			if (_gameEngine.State == State.Over) return;
			_gameEngine.Update();
			if (_gameEngine.Winner is WinResult result) {
				switch (result) {
					case WinResult.Player:
						TraceManager.Log("Game over, Player won.", txtLog);
						// MessageBox.Show("You won!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
						break;
					case WinResult.Computer:
						TraceManager.Log("Game over, Computer won.", txtLog);
						// MessageBox.Show("You lost!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
						break;
					case WinResult.Draw:
						TraceManager.Log("Game over, its a draw.", txtLog);
						// MessageBox.Show("It's a draw!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
						break;
				}
			}
		}

		private void Tile_Click(object sender, RoutedEventArgs e) {
			if (_gameEngine.State != State.InProgress) return;
			
			if (sender is Image img) {
				try {
					// Get the position of the tile
					byte x = (byte)Grid.GetColumn(img);
					byte y = (byte)Grid.GetRow(img);
					// Check if the tile is already marked
					if (_gameEngine.Board.IsMarked(x, y)) return;
					// Mark the tile
					_gameEngine.Board[x, y] = Mark.X;
					TraceManager.Log($"Player marked ({x},{y}).", txtLog);
					// Check for win
					CheckWin();
					// Computer's turn
					if (_gameEngine.State == State.InProgress) {
						// Get the best move
						(byte moveX, byte moveY) = Computer.GetBestMove(_gameEngine.Board);
						// Mark the tile
						_gameEngine.Board[moveX, moveY] = Mark.O;
						TraceManager.Log($"Computer marked ({moveX},{moveY}).", txtLog);
						// Check for win
						CheckWin();
					}
				} catch (Exception exception) {
					// Log and display the error
					MessageBox.Show(exception.Message, "Internal Error", MessageBoxButton.OK, MessageBoxImage.Error);
					TraceManager.Log(exception.Message, txtLog);
				}
				RenderGameBoard();
				RefreshControls();
			}
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e) {
			switch (_gameEngine.State) {
				case State.Over:
					_gameEngine.Restart();
					RenderGameBoard();
					break;
				case State.InProgress:
					_gameEngine.Pause();
					break;
				case State.Paused:
					_gameEngine.Resume();
					break;
			}
			RefreshControls();
		}

		private void RestartButton_Click(object sender, RoutedEventArgs e) {
			MessageBoxResult selection = MessageBox.Show(
				"Are you sure you want to restart the game?",
				"Restart Game",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question
			);

			if (selection == MessageBoxResult.Yes) {
				_gameEngine.Restart();
				RenderGameBoard();
				RefreshControls();
			}
		}

		private void Log_SizeChanged(object sender, SizeChangedEventArgs e) {
			(sender as ScrollViewer)?.ScrollToBottom();
		}
	}

}
