using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal class SnakeFactory : GameObjectFactory
    {
        public SnakeFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject snakeElement = new Snake()
            {
                ObjectCoordinateX = GameSettings.SnakeStartPositionX,
                ObjectCoordinateY = GameSettings.SnakeStartPositionY,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\snakeBody.png")),
                ObjectType = GameObjectType.Snake
            };
            return snakeElement;
        }

        public IList<GameObject> GetSnake()
        {
            var snake = new List<GameObject>();

            for (var i = 0; i < GameSettings.StartNomberOfSnakeElements; i++)
            {
                GameObject snakeElement = GetObject();
                snakeElement.ObjectCoordinateY += i * 5;
                snake.Add(snakeElement);
            }
            return snake;
        }
    }
}
