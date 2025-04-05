using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Managers;
using MemoryGame.Model;
using MemoryGame.View;
using MemoryGame.ViewModel.Commands;

namespace MemoryGame.ViewModel
{
    public class MainVM : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
  


        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set { selectedUser = value;
                OnPropertyChanged("SelectedUser");
                OnPropertyChanged("IsUserSelected");
                ((RelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
                ((RelayCommand)PlayCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsUserSelected => SelectedUser != null;

        private string newUsername;
        public string NewUsername
        {
            get => newUsername;
            set { 
                newUsername = value; 
                OnPropertyChanged("NewUsername");
                ((RelayCommand)AddUserCommand).RaiseCanExecuteChanged();
            }
        }

        public List<string> PredefinedImages { get; set; } = new List<string>
        {
            "../Images/Profiles/Image0.png",
            "../Images/Profiles/Image1.png",
            "../Images/Profiles/Image2.png",
            "../Images/Profiles/Image3.png",
            "../Images/Profiles/Image4.png",
            "../Images/Profiles/Image5.png",
            "../Images/Profiles/Image6.png",
            "../Images/Profiles/Image7.png",
            "../Images/Profiles/Image8.png",
            "../Images/Profiles/Image9.png",
            "../Images/Profiles/Image10.png",
            "../Images/Profiles/Image11.png",
            "../Images/Profiles/Image12.png",
            "../Images/Profiles/Image13.png",
            "../Images/Profiles/Image14.png",
            "../Images/Profiles/Image15.png",
            "../Images/Profiles/Image16.png",
            "../Images/Profiles/Image17.png",
            "../Images/Profiles/Image18.png",
        };

        private int currentImageIndex = 0;
        public string CurrentImage
        {
            get => PredefinedImages[currentImageIndex];
        }

        public ICommand NextImageCommand { get; set; }
        public ICommand PreviousImageCommand { get; set; }
        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainVM()
        {
            LoadUsers();
            NextImageCommand = new RelayCommand(o => NextImage());
            PreviousImageCommand = new RelayCommand(o => PreviousImage());
            AddUserCommand = new RelayCommand(o => AddUser(), o => !string.IsNullOrEmpty(NewUsername));
            DeleteUserCommand = new RelayCommand(o => DeleteUser(), o => IsUserSelected);
            PlayCommand = new RelayCommand(o => Play(), o => IsUserSelected);
            ExitCommand=new RelayCommand(o => Environment.Exit(0));
        }
        

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NextImage()
        {
            currentImageIndex = (currentImageIndex + 1) % PredefinedImages.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        private void PreviousImage()
        {
            currentImageIndex = (currentImageIndex - 1 + PredefinedImages.Count) % PredefinedImages.Count;
            OnPropertyChanged(nameof(CurrentImage));
        }

        private void AddUser()
        {
            if (Users.Any(u => u.Username.Equals(NewUsername, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A user with that name already exists. Please choose a different name.",
                                "User Exists", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Users.Add(new User { Username = NewUsername, ImagePath = CurrentImage });
            SaveUsers();
            NewUsername = string.Empty;
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                string username = SelectedUser.Username;

                string filename = $"save_{username}.json";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                Users.Remove(SelectedUser);
                SaveUsers();

                
                StatisticsManager.RemoveUserStatistics(username);
            }
        }

        private void Play()
        {
            int rows = Properties.Settings.Default.Height;
            int columns = Properties.Settings.Default.Width;
            var gameWindow = new MemoryGame.View.GameWindow(SelectedUser,rows,columns);
            gameWindow.Show();
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }

        private void LoadUsers()
        {
            string path = "users.json";
            if (File.Exists(path))
            {
                var users = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(path));
                if (users != null)
                {
                    Users = users;
                }
            }
        }

        private void SaveUsers()
        {
            string path = "users.json";
            File.WriteAllText(path, JsonSerializer.Serialize(Users));
        }
    }

    

}


