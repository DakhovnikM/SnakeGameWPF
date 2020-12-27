namespace SnakeGameWPF.Models
{
    internal class GameSettings
    {
        public int GameFieldWidth { get; } = 880;

        public int GameFieldHeight { get; } = 680;

        public int StartNomberOfFruits { get;} = 4;

        public int StartNomberOfStones { get;} = 20;

        public int StartNomberOfSnakeElements { get;} = 5;

        public double SnakeStartPositionX { get; } = 250;

        public double SnakeStartPositionY { get; } = 250;

        public int ShiftStep { get; } = 5;

        public int SnakeSpeed { get;} = 70;

        public int Level { get;} = 1;

        public int SnakeLife { get;} = 3;

        public int Score { get; } = 0;
    }
}