using System;
using System.Collections.Generic;
using System.Linq;

using SnakeGameWPF.Models.GameObjects;
using SnakeGameWPF.Models.GameObjectsFactories;

namespace SnakeGameWPF.Models
{
    internal class Scene
    {
        private GameObject _newObject;
        private readonly FruitFactory _fruit;
        private readonly StoneFactory _stone;
        private readonly SnakeFactory _snake;

        public IList<GameObject> Fruits { get; }
        public IList<GameObject> Stones { get; }
        public IList<GameObject> Snake { get; }

        public Scene(GameSettings gameSettings)
        {
            _fruit = new FruitFactory(gameSettings);
            _stone = new StoneFactory(gameSettings);
            _snake = new SnakeFactory(gameSettings);

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
            while (true)
            {
                _newObject = _fruit.GetObject();
                if (ObjectPositionsMatch(_newObject, Fruits)) continue;
                Fruits.Add(_newObject);
                break;
            }
        }

        public void AddNewStoneToStones()
        {
            while (true)
            {
                _newObject = _stone.GetObject();
                if (ObjectPositionsMatch(_newObject, Stones)) continue;
                Stones.Add(_newObject);
                break;
            }
        }

        private bool ObjectPositionsMatch(GameObject gameObject, IList<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => Math.Abs(gameObject.ObjectCoordinateX - item.ObjectCoordinateX) == 0)
                .Any(item => Math.Abs(gameObject.ObjectCoordinateY - item.ObjectCoordinateY) == 0);
        }
    }
}
