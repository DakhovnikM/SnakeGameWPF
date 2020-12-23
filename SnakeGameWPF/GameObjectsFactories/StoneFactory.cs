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
            GameObject stone = new Stone()
            {
                ObjectCoordinateX = (new Random().Next(1, 45)) * 15,
                ObjectCoordinateY = (new Random().Next(1, 45)) * 15,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\stone.png")),
                ObjectType = GameObjectType.Stone
            };
            return stone;
        }

        public IList<GameObject> GetStones()
        {
            var stones = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfStones; i++)
            {
                GameObject stone = GetObject();
                stones.Add(stone);
            }
           return stones;
        }
    }
}
