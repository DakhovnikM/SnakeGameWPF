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
                ObjectCoordinateX = GameSettings.SnakeStartPositionX,
                ObjectCoordinateY = _shiftCoordY * GameSettings.ShiftStep + GameSettings.SnakeStartPositionY,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\snakeEll20x20.png")),
                ObjectType = GameObjectType.Snake
            };
            if (_shiftCoordY <= GameSettings.StartNomberOfSnakeElements) _shiftCoordY++;
            return snakeElement;
        }

        public IList<GameObject> GetSnake()
        {
            var snake = Enumerable
                .Range(1, GameSettings.StartNomberOfSnakeElements)
                .Select(c => GetObject())
                .ToList();
            return snake;
        }
    }
}
