using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SnakeGameWPf
{
    class SnakeFactory : GameObjectFactory
    {
        public SnakeFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            Point point = new Point()
            {
                X = GameSettings.SnakeStartPositionX,
                Y = GameSettings.SnakeStartPositionY
            };

            GameObject snakeEllement = new Snake()
            {
                ObjectCoordinate = point,
                ObjectImage = new Image()
                {
                    Height = 30,
                    Width = 30,
                    Source = new BitmapImage(new Uri(@"D:\Source\Repos\dahovnikM\SnakeWPF_Ver2.0\Resources\snakeBody.png"))
                },
                ObjectType = GameObjectType.Snake
            };
            return snakeEllement;
        }

        public List<GameObject> GetSnake()
        {
            List<GameObject> snake = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfSnakeEllements; i++)
            {
                GameObject snakeEllement = GetObject();
                snakeEllement.ObjectCoordinate.Y += i * 5;
                snake.Add(snakeEllement);
            }
            return snake;
        }
    }
}
