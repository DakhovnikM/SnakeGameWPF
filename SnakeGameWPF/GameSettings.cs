using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnakeGameWPf
{
    class GameSettings : INotifyPropertyChanged
    {
        public int GameFildWidth { get; } = 680;

        public int GameFildHeight { get; } = 680;

        public int StartNomberOfFruits { get; set; } = 4;

        public int StartNomberOfStones { get; set; } = 15;

        public int StartNomberOfSnakeEllements { get; set; } = 8;

        public double SnakeStartPositionX { get; } = 250;

        public double SnakeStartPositionY { get; } = 250;

        public int ShiftStep { get; } = 5;

        public int SnakeSpeed { get; set; } = 100;

        private int level = 1;
        public int Level
        {
            get { return level; }
            set 
            {
                level = value;
                OnPropertyChanged();
            }
        }

        private int snakeLife = 3;
        public int SnakeLife
        {
            get { return snakeLife; }
            set
            {
                snakeLife = value;
                OnPropertyChanged();
            }
        }

        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
};

