using SnakeGameWPF.Commands;
using SnakeGameWPF.Models;
using SnakeGameWPF.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SnakeGameWPF.Models.GameObjects;

namespace SnakeGameWPF.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Поля
        private Direction _direction;
        private Scene _scene;
        private readonly GameSettings _settings;
        private readonly DispatcherTimer _timer;
        private int _speed;
        #endregion

        #region Свойства
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        private int _life;
        public int Life
        {
            get => _life;
            set
            {
                _life = value;
                if (_life == 0)
                {
                    //DialogWindow;

                }
                OnPropertyChanged("Life");
            }
        }

        private int _level;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }

        public ObservableCollection<GameObject> GameObjectCollection { get; private set; }
        #endregion

        #region Команды
        public ICommand KeyUpCommand { get; set; }
        public ICommand KeyDownCommand { get; set; }
        public ICommand KeyRightCommand { get; set; }
        public ICommand KeyLeftCommand { get; set; }
        public ICommand KeySpaceCommand { get; set; }
        private void OnExecutedKeySpaceCommand(object p)
        {
            if (_timer.IsEnabled) _timer.Stop();
            else _timer.Start();
        }

        public ICommand BtnExitCommand { get; set; }

        #endregion

        #region CTOR
        public MainWindowViewModel()
        {
            _settings = new GameSettings();
            _scene = new Scene(_settings);
            _direction = Direction.Up;
            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, _settings.SnakeSpeed) };
            _timer.Tick += GameEngine;

            _speed = _settings.SnakeSpeed;
            Score = _settings.Score;
            Life = _settings.SnakeLife;
            Level = _settings.Level;

            GameObjectCollection = new ObservableCollection<GameObject>();
            GetGameObjectCollection();

            #region Создание команд
            KeyUpCommand = new RelayCommand((object p) => _direction = Direction.Up, (object p) => _direction != Direction.Down);

            KeyDownCommand = new RelayCommand((object p) => _direction = Direction.Down, (object p) => _direction != Direction.Up);

            KeyRightCommand = new RelayCommand((object p) => _direction = Direction.Right, (object p) => _direction != Direction.Left);

            KeyLeftCommand = new RelayCommand((object p) => _direction = Direction.Left, (object p) => _direction != Direction.Right);

            KeySpaceCommand = new RelayCommand(OnExecutedKeySpaceCommand);

            BtnExitCommand = new RelayCommand((object p) => Application.Current.Shutdown(), (object p) => true);
            #endregion
        }
        #endregion

        /// <summary>
        /// Основной цикл игры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameEngine(object sender, EventArgs e)
        {
            bool collided = SnakeCollidedItSelf();
            if (collided) GameOver();

            var canAddStone = false;
            GameObject objToRemove = GetObjectToRemove(GameObjectCollection);

            if (objToRemove is Fruit fruit)
            {
                _scene.Fruits.Remove(fruit);
                _scene.AddNewFruit();
                _scene.AddNewSnakeElement();

                Score++;
                canAddStone = true;
                SpeedUp();
            }

            if (objToRemove is Stone stone)
            {
                _scene.Stones.Remove(stone);
                Life--;
                if (Life == 0) GameOver();
            }

            if (Score % 2 == 0 && canAddStone) _scene.AddNewStone();

            MoveSnake();
            GameObjectCollection.Clear();
            GetGameObjectCollection();
        }

        /// <summary>
        /// Перемещает змею на новую позицию.
        /// </summary>
        private void MoveSnake()
        {
            //Сохраняем координаты головы змеи во временных переменных
            var snakeElement = _scene.Snake[0];
            var tmpPositionX = snakeElement.CoordX;
            var tmpPositionY = snakeElement.CoordY;

            //В зависимости от направления изменяем координаты головы змеи на шаг сдвига
            switch (_direction)
            {
                case Direction.Up:
                    snakeElement.CoordY -= _settings.ShiftStep;
                    snakeElement.CoordY = snakeElement.CoordY < 0
                        ? _settings.GameFieldHeight
                        : snakeElement.CoordY;
                    break;

                case Direction.Down:
                    snakeElement.CoordY += _settings.ShiftStep;
                    snakeElement.CoordY = snakeElement.CoordY > _settings.GameFieldHeight
                        ? 0
                        : snakeElement.CoordY;
                    break;

                case Direction.Right:
                    snakeElement.CoordX += _settings.ShiftStep;
                    snakeElement.CoordX = snakeElement.CoordX > _settings.GameFieldWidth
                        ? 0
                        : snakeElement.CoordX;
                    break;

                case Direction.Left:
                    snakeElement.CoordX -= _settings.ShiftStep;
                    snakeElement.CoordX = snakeElement.CoordX < 0
                        ? _settings.GameFieldWidth
                        : snakeElement.CoordX;
                    break;

                default:
                    throw new ArgumentException(nameof(_direction));
            }

            //Проходим по телу змеи (начия с 3го элемента), перекидывая координаты текущего элемента на следующий.
            for (var i = _scene.Snake.Count - 1; i > 1; i--)
            {
                _scene.Snake[i].CoordX = _scene.Snake[i - 1].CoordX;
                _scene.Snake[i].CoordY = _scene.Snake[i - 1].CoordY;
            }

            //Пробрасываем временно сохраненные координаты головы во второй элемент.
            _scene.Snake[1].CoordX = tmpPositionX;
            _scene.Snake[1].CoordY = tmpPositionY;
        }

        /// <summary>
        /// Объединяет все игровые элементы в одну коллекцию.
        /// </summary>
        private void GetGameObjectCollection()
        {
            foreach (var fruit in _scene.Fruits) GameObjectCollection.Add(fruit);
            foreach (var stone in _scene.Stones) GameObjectCollection.Add(stone);
            foreach (var snake in _scene.Snake) GameObjectCollection.Add(snake);
        }

        /// <summary>
        /// Проверяет на совпадение координаты змеи и обьектов,
        /// если такой обьект имеется он возвращается для дальнейшего удаления из коллекции.
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        private GameObject GetObjectToRemove(ObservableCollection<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => Math.Abs(_scene.Snake[0].CoordX - item.CoordX) <= _settings.ShiftStep)
                .FirstOrDefault(item => Math.Abs(_scene.Snake[0].CoordY - item.CoordY) <= _settings.ShiftStep);
        }

        /// <summary>
        /// Проверяет на совпадение координат головы змеи и элементов ее тела.
        /// </summary>
        /// <returns>true, false</returns>
        private bool SnakeCollidedItSelf()
        {
            if (_scene.Snake.Count < 5)
                return false;

            for (var i = 4; i < _scene.Snake.Count; i++)
                if (Math.Abs(_scene.Snake[0].CoordX - _scene.Snake[i].CoordX) <= _settings.ShiftStep)
                    if (Math.Abs(_scene.Snake[0].CoordY - _scene.Snake[i].CoordY) <= _settings.ShiftStep)
                        return true;
            return false;
        }

        ///<summary>
        ///Увеличивает скорость.
        ///</summary>
        private void SpeedUp()
        {
            if (Score % 3 != 0 || _speed <= 5) return;

            _speed -= 2;
            Level++;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, _speed);
        }

        private void GameOver()
        {
            _timer.Stop();
            _scene = new Scene(_settings);
            _direction = Direction.Up;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, _settings.SnakeSpeed);
            _speed = _settings.SnakeSpeed;
            Score = _settings.Score;
            Life = _settings.SnakeLife;
            Level = _settings.Level;
            GetGameObjectCollection();
        }
    }
}
