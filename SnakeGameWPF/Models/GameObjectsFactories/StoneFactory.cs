using System;
using System.Collections.Generic;
using System.Linq;
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
                CoordX = (new Random().Next(1, 44)) * 20,
                CoordY = (new Random().Next(1, 34)) * 20,
                Image = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\stone.png")),
                Type = GameObjectType.Stone
            };
            return stone;
        }

        public IList<GameObject> GetStones()
        {
            var stones = Enumerable
                .Range(1, Settings.StartNumberOfStones)
                .Select(c => GetObject())
                .ToList();
            
            return stones;
        }
    }
}
