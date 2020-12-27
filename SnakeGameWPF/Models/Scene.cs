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
        private readonly FruitFactory _fruit;
        private readonly StoneFactory _stone;
        private readonly SnakeFactory _snake;

        public IList<GameObject> Fruits { get; }
        public IList<GameObject> Stones { get; }
        public IList<GameObject> Snake { get; }

        public Scene(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;

            _fruit = new FruitFactory(_gameSettings);
            _stone = new StoneFactory(_gameSettings);
            _snake = new SnakeFactory(_gameSettings);

            Fruits = _fruit.GetFruits();
            Stones = _stone.GetStones();
            Snake = _snake.GetSnake();
        }

        public void AddNewSnakeBodyElement()
        {
            var snakeElement = _snake.GetObject();
            Snake.Add(snakeElement);
        }

        public void AddNewFruitToFruits() //TODO Реализовать универсальный метод
        {
            do _newObject = _fruit.GetObject();
            while (ObjectPositionsMatch(_newObject, Fruits));

            Fruits.Add(_newObject);
        }

        public void AddNewStoneToStones()
        {
            do _newObject = _stone.GetObject();
            while (ObjectPositionsMatch(_newObject, Stones));

            Stones.Add(_newObject);
        }

        private bool ObjectPositionsMatch(GameObject gameObject, IList<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => gameObject.ObjectCoordinateX == item.ObjectCoordinateX)
                .Any(item => gameObject.ObjectCoordinateY == item.ObjectCoordinateY);
        }
    }
}
