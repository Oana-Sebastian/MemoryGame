using System.ComponentModel;
using System.Windows;
using MemoryGame.Model;
using MemoryGame.ViewModel;

namespace MemoryGame.View
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private bool isClosing = false;
        private bool isResizing = false;
        public GameWindow(User user, int rows = 4, int columns = 4)
        {
            InitializeComponent();
            DataContext = new GameVM(user, rows, columns);
            this.Closing += GameWindow_Closing;
        }

        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isClosing) return;
            if (isResizing) return;

            isClosing = true;

            if (DataContext is GameVM viewModel)
            {
                viewModel.ExitGame();
            }
            MainWindow mainWindow =new MainWindow();
            if (mainWindow != null && !isResizing)
            {
                mainWindow.Show();
            }
        }
        public void SetResizingFlag(bool value)
        {
            isResizing = value;
        }
        public bool IsClosing => isClosing;
    }

    }