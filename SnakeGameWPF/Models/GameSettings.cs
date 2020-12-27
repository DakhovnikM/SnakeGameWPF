namespace SnakeGameWPF.Models
{
    internal class GameSettings
    {
        public int GameFieldWidth { get; } = 880;

        public int GameFieldHeight { get; } = 680;

        public int StartNomberOfFruits { get; set; } = 4;

        public int StartNomberOfStones { get; set; } = 20;

        public int StartNomberOfSnakeElements { get; set; } = 5;

        public double SnakeStartPositionX { get; } = 250;

        public double SnakeStartPositionY { get; } = 250;

        public int ShiftStep { get; } = 5;

        public int SnakeSpeed { get; set; } = 70;

        public int Level { get; set; } = 1;

        public int SnakeLife { get; set; } = 3;

        public int Score { get; set; }
    }
}