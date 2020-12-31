using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal class SnakeFactory : GameObjectFactory
    {
        private int _shiftCoordY;

        public SnakeFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject snakeElement = new Snake()
            {
                CoordX = Settings.SnakeStartPositionX,
                CoordY = _shiftCoordY * Settings.ShiftStep + Settings.SnakeStartPositionY,
                Image = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\snakeEll20x20.png")),
                Type = GameObjectType.Snake
            };
            if (_shiftCoordY <= Settings.StartNomberOfSnakeElements) _shiftCoordY++;
            return snakeElement;
        }

        public IList<GameObject> GetSnake()
        {
            var snake = Enumerable
                .Range(1, Settings.StartNomberOfSnakeElements)
                .Select(c => GetObject())
                .ToList();
            return snake;
        }
    }
}
