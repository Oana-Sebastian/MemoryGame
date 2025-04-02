using System.ComponentModel;

namespace MemoryGame.ViewModel
{
    public class CardVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isFlipped;
        public bool IsFlipped
        {
            get => isFlipped;
            set { isFlipped = value; OnPropertyChanged("IsFlipped"); OnPropertyChanged("DisplayImage"); }
        }

        private bool isMatched;
        public bool IsMatched
        {
            get => isMatched;
            set { isMatched = value; OnPropertyChanged("IsMatched"); }
        }

        public string ImagePath { get; set; }
        public string BackImagePath { get; set; } = "../Images/back.png";

        public string DisplayImage => IsFlipped || IsMatched ? ImagePath : BackImagePath;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
