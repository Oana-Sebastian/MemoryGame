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
        public GameWindow(User user, int rows = 4, int columns = 4)
        {
            InitializeComponent();
            DataContext = new GameVM(user, rows, columns);
          //  this.Closing += GameWindow_Closing;
        }

    private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is GameVM viewModel)
            {
                viewModel.ExitGame();
            }
        }
    }

    }