using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame.ViewModel.Commands;
using System.Windows.Input;
using MemoryGame.Model;
using MemoryGame.View;

namespace MemoryGame.ViewModel
{
    public class CustomSizeVM : INotifyPropertyChanged
    {
        private int width = Properties.Settings.Default.Width;
        private int height = Properties.Settings.Default.Height;
        private bool? _dialogResult;
        private User user;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Width
        {
            get => width;
            set
            {
                width = value;
                Properties.Settings.Default.Width = value;
                OnPropertyChanged(nameof(Width));
                OnPropertyChanged(nameof(CanConfirm));
            }
        }

        public int Height
        {
            get => height;
            set
            {
                height = value;
                Properties.Settings.Default.Height = value;
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(CanConfirm));
            }
        }

        public bool CanConfirm => Width >= 2 && Width <= 6 && Height >= 2 && Height <= 6 && (Width * Height) % 2 == 0;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public bool? DialogResult
        {
            get => _dialogResult;
            private set
            {
                _dialogResult = value;
                OnPropertyChanged(nameof(DialogResult));
            }
        }

       public CustomSizeVM()
        {
            ConfirmCommand = new RelayCommand(_ => Confirm(), _ => CanConfirm);
            CancelCommand = new RelayCommand(_ => Cancel());
            user=GameVM.currentUser;
        }

        private void Confirm()
        {
            DialogResult = true;
            Properties.Settings.Default.Save();
            var newGameVM = new GameVM(user, Height, Width);

            var gameWindow = new GameWindow(user, Height, Width);
            gameWindow.Show();

            var currentGameWindow = System.Windows.Application.Current.Windows
                .OfType<MemoryGame.View.GameWindow>()
                .FirstOrDefault();
            currentGameWindow?.Close();
            var mainWindow = System.Windows.Application.Current.Windows
                .OfType<MemoryGame.View.MainWindow>()
                .FirstOrDefault();
            mainWindow?.Close();
            var currentCustomWindow = System.Windows.Application.Current.Windows
               .OfType<MemoryGame.View.CustomSizeWindow>()
               .FirstOrDefault();
            currentCustomWindow?.Close();

        }

        private void Cancel()
        {
            DialogResult = false;
            var currentCustomWindow = System.Windows.Application.Current.Windows
               .OfType<MemoryGame.View.CustomSizeWindow>()
               .FirstOrDefault();
            currentCustomWindow?.Close();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
