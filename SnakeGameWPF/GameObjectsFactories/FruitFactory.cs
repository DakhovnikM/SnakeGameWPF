using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SnakeGameWPF
{
    class FruitFactory : GameObjectFactory
    {
        public FruitFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            var point = new Point()
            {
                X = (new Random().Next(1, 45)) * 15,
                Y = (new Random().Next(1, 45)) * 15
            };

            GameObject fruit = new Fruit()
            {
                ObjectCoordinate = point,
                ObjectImage = new Image
                {
                    Height = 30,
                    Width = 30,
                    Source = new BitmapImage(new Uri(@"D:\Source\Repos\dahovnikM\SnakeWPF_Ver2.0\Resources\apple.png"))
                },
                ObjectType = GameObjectType.Fruit
            };
            return fruit;
        }

        public List<GameObject> GetFruites()
        {
            List<GameObject> fruites = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfFruits; i++)
            {
                GameObject fruit = GetObject();
                fruites.Add(fruit);
            }
            return fruites;
        }
    }
}
