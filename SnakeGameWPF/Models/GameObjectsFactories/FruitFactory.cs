using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.Models.GameObjectsFactories
{
    internal class FruitFactory : GameObjectFactory
    {
        public FruitFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject fruit = new Fruit()
            {
                ObjectCoordinateX = (new Random().Next(1, 44)) * 20,
                ObjectCoordinateY = (new Random().Next(1, 34)) * 20,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\fruit20x20.png")),
                ObjectType = GameObjectType.Fruit
            };
            return fruit;
        }

        public IList<GameObject> GetFruits()
        {
            var fruits = Enumerable
                .Range(1, GameSettings.StartNomberOfFruits)
                .Select(c => GetObject())
                .ToList();

            return fruits;
        }
    }
}
