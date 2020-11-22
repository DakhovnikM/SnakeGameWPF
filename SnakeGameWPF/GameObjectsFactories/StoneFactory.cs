using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SnakeGameWPF
{
    class StoneFactory : GameObjectFactory
    {
        public StoneFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            var point = new Point()
            {
                X = (new Random().Next(1, 45)) * 15,
                Y = (new Random().Next(1, 45)) * 15
            };

            GameObject stone = new Stone()
            {
                ObjectCoordinate = point,
                ObjectImage = new Image
                {
                    Height = 30,
                    Width = 30,
                    Source = new BitmapImage(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\stone.png"))
                },
                ObjectType = GameObjectType.Stone
            };
            return stone;
        }

        public List<GameObject> GetStones()
        {
            List<GameObject> stones = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfStones; i++)
            {
                GameObject stone = GetObject();
                stones.Add(stone);
            }
            return stones;
        }
    }
}
