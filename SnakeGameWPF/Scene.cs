using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace SnakeGameWPF
{
    class Scene
    {
        public List<GameObject> Fruits { get; private set; }

        public List<GameObject> Stones { get; private set; }

        public List<GameObject> Snake { get; private set; }

        readonly GameSettings _gameSettings;

        private GameObject newObject;

        private static Scene scene;

        private Scene() { }

        private Scene(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            Stones = new StoneFactory(_gameSettings).GetStones();
            Fruits = new FruitFactory(_gameSettings).GetFruites();
            Snake = new SnakeFactory(_gameSettings).GetSnake();
        }

        public static Scene GetScene(GameSettings gameSettings)
        {
            if (scene == null)
                scene = new Scene(gameSettings);

            return scene;
        }

        public void AddNewSnakeBodyEllement()
        {
            var snakeFactory = new SnakeFactory(_gameSettings);
            var snakeEllement = snakeFactory.GetObject();
            Snake.Add(snakeEllement);
        }

        public void AddNewFruitToFruits()
        {
            do
            {
                var fruitFactory = new FruitFactory(_gameSettings);
                newObject = fruitFactory.GetObject();

            } while (ObjectsPositionsMatched(newObject, Fruits));
            Fruits.Add(newObject);
        }

        public void AddNewStoneToStones()
        {
            do
            {
                var stoneFactory = new StoneFactory(_gameSettings);
                newObject = stoneFactory.GetObject();

            } while (ObjectsPositionsMatched(newObject, Stones));
            Stones.Add(newObject);
        }

        private bool ObjectsPositionsMatched(GameObject gameObject, IList<GameObject> gameObjects)
        {
            foreach (var item in gameObjects)
            {
                if (Math.Abs(gameObject.ObjectCoordinateX - item.ObjectCoordinateX) <= _gameSettings.ShiftStep)
                    if (Math.Abs(gameObject.ObjectCoordinateY - item.ObjectCoordinateY) <= _gameSettings.ShiftStep)
                        return true;
            }
            return false;
        }
    }
}
