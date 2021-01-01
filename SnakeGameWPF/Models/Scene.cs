using System;
using System.Collections.Generic;
using System.Linq;

using SnakeGameWPF.Models.GameObjects;
using SnakeGameWPF.Models.GameObjectsFactories;

namespace SnakeGameWPF.Models
{
    /// <summary>
    /// Класс формирует готовую для отображения сцену со всеми обьектами.
    /// </summary>
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

        public void AddNewSnakeElement()
        {
            var snakeElement = _snake.GetObject();
            Snake.Add(snakeElement);
        }

        public void AddNewFruit()
        {
            while (true)
            {
                _newObject = _fruit.GetObject();
                if (ObjectsMatch(_newObject, Fruits) || ObjectsMatch(_newObject, Stones)) continue;
                Fruits.Add(_newObject);
                break;
            }
        }

        public void AddNewStone()
        {
            while (true)
            {
                _newObject = _stone.GetObject();
                if (ObjectsMatch(_newObject, Stones) || ObjectsMatch(_newObject, Fruits)) continue;
                Stones.Add(_newObject);
                break;
            }
        }

        private bool ObjectsMatch(GameObject gameObject, IList<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => Math.Abs(gameObject.CoordX - item.CoordX) == 0)
                .Any(item => Math.Abs(gameObject.CoordY - item.CoordY) == 0);
        }
    }
}
