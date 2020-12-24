using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace SnakeGameWPF.Models
{
    class FruitFactory : GameObjectFactory
    {
        public FruitFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject fruit = new Fruit()
            {
                ObjectCoordinateX = (new Random().Next(1, 45)) * 15,
                ObjectCoordinateY = (new Random().Next(1, 45)) * 15,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\apple.png")),
                ObjectType = GameObjectType.Fruit
            };
            return fruit;
        }

        public IList<GameObject> GetFruites()
        {
            var fruites = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfFruits; i++)
            {
                GameObject fruit = GetObject();
                fruites.Add(fruit);
            }
            return fruites;
        }
    }
}
