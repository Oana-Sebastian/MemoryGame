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
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace MemoryGame.ViewModel
{
    public class GameVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DispatcherTimer timer;
        private TimeSpan timeLeft;

        public string TimeLeft => timeLeft.ToString(@"mm\:ss");

        public bool IsGameActive => timer != null;
        private ObservableCollection<CardVM> cards;
        public ObservableCollection<CardVM> Cards
        {
            get => cards;
            set
            {
                cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }

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


        public string selectedCategory = "Drivers";
        public string SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory != value)
                {
                    selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public string selectedDifficulty = "Normal";
        public string SelectedDifficulty
        {
            get => selectedDifficulty;
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged(nameof(SelectedDifficulty));
                }
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
        public ICommand DifficultyCommand { get; set; }

        public static User currentUser;
        private CardVM firstSelectedCard;
        private CardVM secondSelectedCard;
        private bool isCheckingMatch = false;
        private bool gameOver = false;
        public GameVM(User user ,int height, int width)
        {
            SelectedCategory = Properties.Settings.Default.Category;
            SelectedDifficulty = Properties.Settings.Default.Difficulty;
            currentUser = user;
            LoadStatistics();
            Rows =Properties.Settings.Default.Height;
            Columns = Properties.Settings.Default.Width;
            Columns = width;
            Cards = new ObservableCollection<CardVM>();
            InitializeCommands();
            StartNewGame();
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
            DifficultyCommand = new RelayCommand(o => SetDifficulty(o.ToString()));
        }
        #region Game
        private void StartNewGame()
        {
            Rows=Properties.Settings.Default.Height;
            Columns= Properties.Settings.Default.Width;
            StopTimer();
            gameOver = false;
            Cards.Clear();
            firstSelectedCard = null;
            secondSelectedCard = null;

            UpdateUserStatistics();

            var newCards = GenerateShuffledCards();
            foreach (var card in newCards)
            {
                Cards.Add(card);
            }

            Cards = GenerateShuffledCards();
            int difficultyTime = GetDifficultyTime(SelectedDifficulty);

            timeLeft = TimeSpan.FromSeconds(Cards.Count()*difficultyTime);
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
            OnPropertyChanged(nameof(IsGameActive));
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
                Properties.Settings.Default.Height = loadedState.Rows;
                Properties.Settings.Default.Width = loadedState.Columns;
                Properties.Settings.Default.Category = loadedState.SelectedCategory;
                Properties.Settings.Default.Save();
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


                StopTimer();
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
            //timer works
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
        public void ExitGame()
        {
            gameOver = true;
            StopTimer();
            timeLeft = TimeSpan.Zero;
            SaveStatistics();
            OnPropertyChanged(nameof(IsGameActive));
            var currentGameWindow = Application.Current.Windows
               .OfType<GameWindow>()
               .FirstOrDefault();

            if (currentGameWindow != null && !currentGameWindow.IsClosing)
            {
                currentGameWindow.Close();
            }
        }
        #endregion

        #region Timer
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (gameOver) return;

            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                OnPropertyChanged(nameof(TimeLeft));
            }
            else
            {
                StopTimer();
                MessageBox.Show("Time's up! You lost.");
                gameOver = true;
            }
        }

        private void StopTimer()
        {
            if (timer != null)
            {
                timer.IsEnabled = false;
                timer.Stop();
                timer.Tick -= Timer_Tick;
                timer = null;
                OnPropertyChanged(nameof(IsGameActive));
            }
        }
        #endregion

        #region Cards
        private void Category(string category)
        {
            SelectedCategory = category;
            Properties.Settings.Default.Category = category;
            Properties.Settings.Default.Save();
            StartNewGame();
            //timer works
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

                        if (timer != null)
                        {
                            timer.Stop();
                            timer = null;
                        }

                        timeLeft = TimeSpan.Zero;
                        var userStat = Statistics.FirstOrDefault(s => s.Username == currentUser.Username);
                        if (userStat != null)
                        {
                            userStat.GamesWon++;
                            SaveStatistics();
                        }

                        StopTimer();
                        MessageBox.Show("Congratulations! You won.");
                        var currentGameWindow = Application.Current.Windows.OfType<GameWindow>().FirstOrDefault();
                        if (currentGameWindow != null)
                        {
                            currentGameWindow.Close();
                        }
                    }

                    ResetSelection();
                }
                else
                {
                    Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        await System.Threading.Tasks.Task.Delay(400);
                        firstSelectedCard.IsFlipped = false;
                        secondSelectedCard.IsFlipped = false;
                        ResetSelection();
                    });
                }
            }
        }

        private ObservableCollection<CardVM> GenerateShuffledCards()
        {
            int totalCards = Rows * Columns;
            int pairCount = totalCards / 2;

            var rnd = new Random();
            var availableImages = Enumerable.Range(0, 20).OrderBy(_ => rnd.Next()).ToList();
            var selectedImages = availableImages.Take(pairCount).ToList();

            List<int> availablePositions = Enumerable.Range(0, totalCards).ToList();
            availablePositions = availablePositions.OrderBy(_ => rnd.Next()).ToList();

           
            CardVM[] cardsArray = new CardVM[totalCards];

            foreach (int imageIndex in selectedImages)
            {
                string imagePath = $"../Images/{SelectedCategory}/Image{imageIndex}.jpg";


                int pos1 = availablePositions[0];
                availablePositions.RemoveAt(0);
                int pos2 = availablePositions[0];
                availablePositions.RemoveAt(0);

               
                cardsArray[pos1] = new CardVM { ImagePath = imagePath, IsFlipped = false, IsMatched = false };
                cardsArray[pos2] = new CardVM { ImagePath = imagePath, IsFlipped = false, IsMatched = false };
            }

           
            return new ObservableCollection<CardVM>(cardsArray);
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


        #endregion

        #region Statistics
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


        private void LoadStatistics()
        {
            string path = "statistics.json";
            if (File.Exists(path))
            {
                var stats = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(path));
                if (stats != null)
                {
                    Statistics = stats;
                }
            }
        }

        private void UpdateUserStatistics()
        {
            var userStat = Statistics.FirstOrDefault(s => s.Username == currentUser.Username);
            if (userStat == null)
            {
                userStat = new User { Username = currentUser.Username, GamesPlayed = 0, GamesWon = 0 };
                Statistics.Add(userStat);
            }

            userStat.GamesPlayed++;
            SaveStatistics();
        }


        private void SaveStatistics()
        {
            string path = "statistics.json";
            File.WriteAllText(path, JsonSerializer.Serialize(Statistics));
        }
        #endregion

        #region Settings
        private void SetStandard()
        {
            Rows = 4;
            Columns = 4;
            Properties.Settings.Default.Height = Rows;
            Properties.Settings.Default.Width = Columns;
            Properties.Settings.Default.Save();
            StopTimer();
            StartNewGame();
            //timer works
        }
        private void SetCustom()
        {
            var customSizeWindow = new CustomSizeWindow();
            var viewModel = (CustomSizeVM)customSizeWindow.DataContext;

            if (customSizeWindow.ShowDialog() == true)
            {
                StopTimer();
                int width = viewModel.Width;
                int height = viewModel.Height;
                Properties.Settings.Default.Height = height;
                Properties.Settings.Default.Width = width;
                Properties.Settings.Default.Save();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                customSizeWindow.Close();

            
                var currentGameWindow = Application.Current.Windows
                    .OfType<GameWindow>()
                    .FirstOrDefault();
                if (currentGameWindow != null)
                {
                    currentGameWindow.SetResizingFlag(true);
                    currentGameWindow.Close();
                }

                var newGameWindow = new GameWindow(currentUser, height, width)
                {
                    DataContext = new GameVM(currentUser, height, width)
                };
                newGameWindow.Show();
            }
        }

        private void SetDifficulty(string difficulty)
        {
            SelectedDifficulty = difficulty;
            Properties.Settings.Default.Difficulty = difficulty;
            Properties.Settings.Default.Save();
            StartNewGame();
        }

        private int GetDifficultyTime(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 5,
                "Normal" => 4,
                "Hard" => 3,
                "Expert" => 2
            };
        }

        #endregion


        private void ShowAbout()
        {
           AboutWindow aboutWindow= new AboutWindow();
           aboutWindow.ShowDialog();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
