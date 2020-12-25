using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal class StoneFactory : GameObjectFactory
    {
        public StoneFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject stone = new Stone()
            {
                ObjectCoordinateX = (new Random().Next(1, 44)) * 20,
                ObjectCoordinateY = (new Random().Next(1, 34)) * 20,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\stone.png")),
                ObjectType = GameObjectType.Stone
            };
            return stone;
        }

        public IList<GameObject> GetStones()
        {
            var stones = new List<GameObject>();

            for (var i = 0; i < GameSettings.StartNomberOfStones; i++)
            {
                GameObject stone = GetObject();
                stones.Add(stone);
            }
            return stones;
        }
    }
}
