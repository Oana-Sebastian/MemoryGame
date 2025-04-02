using System.Collections.ObjectModel;
using System.ComponentModel;
using MemoryGame.Model;
using MemoryGame.ViewModel.Commands;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows;
using MemoryGame.View;
using System.Text.Json;
using System.IO;

namespace MemoryGame.ViewModel
{
    public class GameVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DispatcherTimer timer;
        private TimeSpan timeLeft;

        public string TimeLeft => timeLeft.ToString(@"mm\:ss");

        public bool IsGameActive => timer != null;
        public ObservableCollection<CardVM> Cards { get; set; }
        public ObservableCollection<User> Statistics { get; set; } = new();

        private const double CardWidth = 140; 
        private const double CardHeight = 140;


        public double WindowWidth => Columns * CardWidth;
        public double WindowHeight => Rows * CardHeight + 160;

        public int Columns
        {
            get => _columns;
            set 
            { 
                _columns = value;
                OnPropertyChanged(nameof(Columns));
                OnPropertyChanged(nameof(WindowWidth));
      
            }
        }

        private int _rows;
        private int _columns;
        public int Rows
        {
            get => _rows;
            set 
            { 
                _rows = value; 
                OnPropertyChanged(nameof(Rows));
                OnPropertyChanged(nameof(WindowHeight));
            }
        }


        private string selectedCategory = "Drivers";
        public string SelectedCategory
        {
            get => selectedCategory;
            set { 
                selectedCategory = value; 
                }
        }


        public ICommand NewGameCommand { get; set; }
        public ICommand OpenGameCommand { get; set; }
        public ICommand SaveGameCommand { get; set; }
        public ICommand StatisticsCommand { get; set; }
        public ICommand CategoryCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand SetStandardCommand { get; set; }
        public ICommand SetCustomCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand CardSelectedCommand { get; set; }

        public static User currentUser;
        private CardVM firstSelectedCard;
        private CardVM secondSelectedCard;
        private bool isCheckingMatch = false;
        private bool gameOver = false;
        public GameVM(User user ,int height, int width)
        {
            currentUser = user;
            LoadStatistics();
            Rows = height;
            Columns = width;
            Cards = new ObservableCollection<CardVM>();
            InitializeCommands();
            StartNewGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameOver)
                return;
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged("TimeLeft");
            }
            else
            {
                timer.Stop();
                timer = null;
                MessageBox.Show("Time's up! You lost.");
                gameOver = true;
            }
        }

        private void InitializeCommands()
        {
            NewGameCommand = new RelayCommand(o => StartNewGame());
            OpenGameCommand = new RelayCommand(o => OpenGame());
            SaveGameCommand = new RelayCommand(o => SaveGame());
            StatisticsCommand = new RelayCommand(o => ShowStatistics());
            CategoryCommand = new RelayCommand(o => Category(o.ToString()));
            ExitCommand = new RelayCommand(o => ExitGame());
            SetStandardCommand = new RelayCommand(o => SetStandard());
            SetCustomCommand = new RelayCommand(o => SetCustom());
            AboutCommand = new RelayCommand(o => ShowAbout());
            CardSelectedCommand = new RelayCommand(o => CardSelected(o), o => IsGameActive);
        }

        private void StartNewGame()
        {
            timer?.Stop();
            timer = null;
            gameOver = false;
            Cards.Clear();
            firstSelectedCard = null;
            secondSelectedCard = null;

            var userStat = Statistics.FirstOrDefault(s => s.Username == currentUser.Username);
            if (userStat == null)
            {
                userStat = new User { Username = currentUser.Username, GamesPlayed = 0, GamesWon = 0 };
                Statistics.Add(userStat);
            }

            userStat.GamesPlayed++;
            SaveStatistics();
            int totalCards = Rows * Columns;
            int pairCount = totalCards / 2;

            
            for (int i = 0; i < pairCount; i++)
            {
                string imagePath = $"../Images/{SelectedCategory}/Image{i % 20}.jpg";
                Cards.Add(new CardVM { ImagePath = imagePath, IsFlipped = false, IsMatched = false });
                Cards.Add(new CardVM { ImagePath = imagePath, IsFlipped = false, IsMatched = false });
            }
            
            var rnd = new Random();
            for (int i = Cards.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }


            timeLeft = TimeSpan.FromMinutes(2);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            OnPropertyChanged(nameof(IsGameActive));
        }

        private void Category(string category)
        {
            SelectedCategory = category;
            StartNewGame();
        }

        private void CardSelected(object parameter)
        {
            if (!(parameter is CardVM selectedCard))
                return;
            if (selectedCard.IsFlipped || selectedCard.IsMatched || isCheckingMatch)
                return;

            selectedCard.IsFlipped = true;

            if (firstSelectedCard == null)
            {
                firstSelectedCard = selectedCard;
            }
            else if (secondSelectedCard == null && selectedCard != firstSelectedCard)
            {
                secondSelectedCard = selectedCard;
                isCheckingMatch = true; 

                
                if (firstSelectedCard.ImagePath == secondSelectedCard.ImagePath)
                {
                    firstSelectedCard.IsMatched = true;
                    secondSelectedCard.IsMatched = true;
                        if (AllCardsMatched())
                        {
                        gameOver = true;
                        timer?.Stop();
                        timer = null;
                        timeLeft = TimeSpan.Zero;
                        var userStat = Statistics.FirstOrDefault(s => s.Username == currentUser.Username);
                            userStat.GamesWon++;
                            SaveStatistics();
                            MessageBox.Show("Congratulations! You won.");
                        }
                    ResetSelection();
                }
                else
                {
                   
                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        await System.Threading.Tasks.Task.Delay(500);
                        firstSelectedCard.IsFlipped = false;
                        secondSelectedCard.IsFlipped = false;
                        ResetSelection();
                    });
                }
            }
        }

       
        private void ResetSelection()
        {
            firstSelectedCard = null;
            secondSelectedCard = null;
            isCheckingMatch = false;
        }

        private bool AllCardsMatched()
        {
            foreach (var card in Cards)
            {
                if (!card.IsMatched)
                    return false;
            }
            return true;
        }

        private void OpenGame()
        {
           
            string filename = $"save_{currentUser.Username}.json";

            if (!File.Exists(filename))
            {
                MessageBox.Show("There is no save for this user.");
                return;
            }

            string json = File.ReadAllText(filename);
            GameState loadedState = JsonSerializer.Deserialize<GameState>(json);
            if (loadedState != null)
            {
              
                Rows = loadedState.Rows;
                Columns = loadedState.Columns;
                SelectedCategory = loadedState.SelectedCategory;
                timeLeft = TimeSpan.FromSeconds(loadedState.TimeLeftSeconds);

                Cards.Clear();
                foreach (var cs in loadedState.Cards)
                {
                    Cards.Add(new CardVM
                    {
                        ImagePath = cs.ImagePath,
                        IsFlipped = cs.IsFlipped,
                        IsMatched = cs.IsMatched
                    });
                }

                
                timer?.Stop();
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
                OnPropertyChanged(nameof(TimeLeft));

                MessageBox.Show("Game loaded succesfully!");
            }
            else
            {
                MessageBox.Show("Error with loading the save");
            }
        }

        private void SaveGame()
        {
            var gameState = new GameState
            {
                Rows = this.Rows,
                Columns = this.Columns,
                SelectedCategory = this.SelectedCategory,
                TimeLeftSeconds = timeLeft.TotalSeconds,
                Cards = Cards.Select(c => new CardState
                {
                    ImagePath = c.ImagePath,
                    IsFlipped = c.IsFlipped,
                    IsMatched = c.IsMatched
                }).ToList()
            };

           
            string filename = $"save_{currentUser.Username}.json";

            string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filename, json);

            MessageBox.Show("Game saved");
        }

        private void ShowStatistics()
        {
            LoadStatistics();
            string message = "Users - Games - Wins\n";
            foreach (var stat in Statistics)
            {
                message += $"{stat.Username} – {stat.GamesPlayed} – {stat.GamesWon}\n";
            }
            MessageBox.Show(message, "Statistics");
        }

        private void ExitGame()
        {
            gameOver = true;
            timer?.Stop();
            timer = null;
            timeLeft = TimeSpan.Zero;
            SaveStatistics();
            OnPropertyChanged(nameof(IsGameActive));
            Application.Current.Windows[1].Close();
        }

        private void SetStandard()
        {
            Rows = 4;
            Columns = 4;
            StartNewGame();
        }

        private bool UserExists(string username)
        {
            return currentUser != null && username == currentUser.Username;
        }

        private void LoadStatistics()
        {
            string path = "statistics.json";
            if (File.Exists(path))
            {
                var stats = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(path));
                if (stats != null)
                {
                    Statistics = new ObservableCollection<User>(stats.Where(s => UserExists(s.Username)));
                }
            }
        }

        private void SaveStatistics()
        {
            string path = "statistics.json";
            Statistics = new ObservableCollection<User>(Statistics.Where(s => UserExists(s.Username))); 
            File.WriteAllText(path, JsonSerializer.Serialize(Statistics));
        }

        private void SetCustom()
        {
            timer?.Stop();
            timer = null;
            timeLeft = TimeSpan.Zero;
            var customSizeWindow = new CustomSizeWindow();
            var viewModel = (CustomSizeVM)customSizeWindow.DataContext;

            if (customSizeWindow.ShowDialog() == true)
            {
                int width = viewModel.Width;
                int height = viewModel.Height;

               
                var gameWindow = new GameWindow(currentUser, height, width)
                {
                    DataContext = new GameVM(currentUser, height, width)
                };
                customSizeWindow.Close();
                gameWindow.Show();
            }
        }

        private void ShowAbout()
        {
            MessageBox.Show("Name: Oană Sebastian\nEmail: sebastian.oana@student.unitbv.ro\nGroup: 10LF233\nSpecialization: Informatics");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
