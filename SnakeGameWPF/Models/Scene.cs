using System;
using System.Collections.Generic;
using System.Linq;
using SnakeGameWPF.Models.GameObjects;
using SnakeGameWPF.Models.GameObjectsFactories;

namespace SnakeGameWPF.Models
{
    internal class Scene
    {
        private readonly GameSettings _gameSettings;

        private GameObject _newObject;

        private static Scene _scene;
        public IList<GameObject> Fruits { get; }

        public IList<GameObject> Stones { get; }

        public IList<GameObject> Snake { get; }


        private Scene(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            Stones = new StoneFactory(_gameSettings).GetStones();
            Fruits = new FruitFactory(_gameSettings).GetFruits();
            Snake = new SnakeFactory(_gameSettings).GetSnake();;
        }

        public static Scene GetScene(GameSettings gameSettings)
        {
            return _scene ??= new Scene(gameSettings);
        }

        public void AddNewSnakeBodyElement()
        {
            var snakeFactory = new SnakeFactory(_gameSettings);
            var snakeElement = snakeFactory.GetObject();
            Snake.Add(snakeElement);
        }

        public void AddNewFruitToFruits() //TODO Реализовать универсальный метод
        {
            do
            {
                var fruitFactory = new FruitFactory(_gameSettings);
                _newObject = fruitFactory.GetObject();

            } while (ObjectsPositionsMatched(_newObject, Fruits));
            Fruits.Add(_newObject);
        }

        public void AddNewStoneToStones()
        {
            do
            {
                var stoneFactory = new StoneFactory(_gameSettings);
                _newObject = stoneFactory.GetObject();

            } while (ObjectsPositionsMatched(_newObject, Stones));
            Stones.Add(_newObject);
        }

        private bool ObjectsPositionsMatched(GameObject gameObject, IList<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => Math.Abs(gameObject.ObjectCoordinateX - item.ObjectCoordinateX) <= _gameSettings.ShiftStep)
                .Any(item => Math.Abs(gameObject.ObjectCoordinateY - item.ObjectCoordinateY) <= _gameSettings.ShiftStep);
        }
    }
}
