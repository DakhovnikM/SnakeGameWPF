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
        private Direction _direction;
        private readonly Scene _scene;
        private readonly GameSettings _gameSettings;
        private readonly DispatcherTimer _timer;

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
                    MessageBox.Show("У вас не осталось жизней !!!", "КОНЕЦ ИГРЫ", MessageBoxButton.OKCancel);
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

        public ObservableCollection<GameObject> GameObjectsCollection { get; private set; }
        #endregion

        #region Команды
        public ICommand KeyUpCommand { get; set; }
        private bool CanExecuteKeyUpCommand(object p) => true;
        private void OnExecutedKeyUpCommand(object p)
        {
            if (_direction == Direction.Down) return;
            _direction = Direction.Up;
        }

        public ICommand KeyDownCommand { get; set; }
        private bool CanExecuteKeyDownCommand(object p) => true;
        private void OnExecutedKeyDownCommand(object p)
        {
            if (_direction == Direction.Up) return;
            _direction = Direction.Down;
        }

        public ICommand KeyRightCommand { get; set; }
        private bool CanExecuteKeyRightCommand(object p) => true;
        private void OnExecutedKeyRightCommand(object p)
        {
            if (_direction == Direction.Left) return;
            _direction = Direction.Right;
        }

        public ICommand KeyLeftCommand { get; set; }
        private bool CanExecuteKeyLeftCommand(object p) => true;
        private void OnExecutedKeyLeftCommand(object p)
        {
            if (_direction == Direction.Right) return;
            _direction = Direction.Left;
        }

        public ICommand KeySpaceCommand { get; set; }
        private bool CanExecuteKeySpaceCommand(object p) => true;
        private void OnExecutedKeySpaceCommand(object p)
        {
            if (_timer.IsEnabled) _timer.Stop();
            else _timer.Start();
        }
        public ICommand BtnExitCommand { get; set; }
        private bool CanExecuteBtnExitCommand(object p) => true;
        private void OnExecutedBtnExitCommand(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region CTOR
        public MainWindowViewModel()
        {
            _gameSettings = new GameSettings();
            _scene = Scene.GetScene(_gameSettings);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, _gameSettings.SnakeSpeed) };
            _timer.Tick += GameEngine;

            _direction = Direction.Up;

            Score = _gameSettings.Score;
            Life = _gameSettings.SnakeLife;
            Level = _gameSettings.Level;

            GameObjectsCollection = new ObservableCollection<GameObject>();
            GetGameObjectsCollection();

            #region Создание команд
            KeyUpCommand = new RelayCommand(OnExecutedKeyUpCommand, CanExecuteKeyUpCommand);
            KeyDownCommand = new RelayCommand(OnExecutedKeyDownCommand, CanExecuteKeyDownCommand);
            KeyRightCommand = new RelayCommand(OnExecutedKeyRightCommand, CanExecuteKeyRightCommand);
            KeyLeftCommand = new RelayCommand(OnExecutedKeyLeftCommand, CanExecuteKeyLeftCommand);
            KeySpaceCommand = new RelayCommand(OnExecutedKeySpaceCommand, CanExecuteKeySpaceCommand);
            BtnExitCommand = new RelayCommand(OnExecutedBtnExitCommand, CanExecuteBtnExitCommand);
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
            bool snakeBitItSelf = SnakeHeadPositionMatchBody();
            if (snakeBitItSelf)
            {
                Life = 0;
                GameOver();
            }

            var canAddStone = false;

            GameObject objToRemove = GetObjectToRemove(_scene.Fruits);
            if (objToRemove is Fruit fruit)
            {
                _scene.Fruits.Remove(fruit);
                _scene.AddNewFruitToFruits();
                _scene.AddNewSnakeBodyElement();

                Score++;
                canAddStone = true;
                IncreaseSpeed();
            }

            objToRemove = GetObjectToRemove(_scene.Stones);
            if (objToRemove is Stone stone)
            {
                _scene.Stones.Remove(stone);
                Life--;
                if (Life == 0) GameOver();
            }

            if (Score % 2 == 0 && canAddStone)
                _scene.AddNewStoneToStones();

            MoveSnake();
            GameObjectsCollection.Clear();
            GetGameObjectsCollection();
        }

        /// <summary>
        /// Перемещает змею на новую позицию.
        /// </summary>
        private void MoveSnake()
        {
            var snakeElement = _scene.Snake[0];
            var subPositionX = snakeElement.ObjectCoordinateX;
            var subPositionY = snakeElement.ObjectCoordinateY;
            switch (_direction)
            {
                case Direction.Up:
                    snakeElement.ObjectCoordinateY -= _gameSettings.ShiftStep;
                    snakeElement.ObjectCoordinateY = snakeElement.ObjectCoordinateY < 0
                        ? _gameSettings.GameFieldHeight
                        : snakeElement.ObjectCoordinateY;
                    break;

                case Direction.Down:
                    snakeElement.ObjectCoordinateY += _gameSettings.ShiftStep;
                    snakeElement.ObjectCoordinateY = snakeElement.ObjectCoordinateY > _gameSettings.GameFieldHeight
                        ? 0
                        : snakeElement.ObjectCoordinateY;
                    break;

                case Direction.Right:
                    snakeElement.ObjectCoordinateX += _gameSettings.ShiftStep;
                    snakeElement.ObjectCoordinateX = snakeElement.ObjectCoordinateX > _gameSettings.GameFieldWidth
                        ? 0
                        : snakeElement.ObjectCoordinateX;
                    break;

                case Direction.Left:
                    snakeElement.ObjectCoordinateX -= _gameSettings.ShiftStep;
                    snakeElement.ObjectCoordinateX = snakeElement.ObjectCoordinateX < 0
                        ? _gameSettings.GameFieldWidth
                        : snakeElement.ObjectCoordinateX;
                    break;

                case Direction.Pause:
                    return;
            }

            for (var i = _scene.Snake.Count - 1; i > 1; i--)
            {
                _scene.Snake[i].ObjectCoordinateX = _scene.Snake[i - 1].ObjectCoordinateX;
                _scene.Snake[i].ObjectCoordinateY = _scene.Snake[i - 1].ObjectCoordinateY;
            }

            _scene.Snake[1].ObjectCoordinateX = subPositionX;
            _scene.Snake[1].ObjectCoordinateY = subPositionY;
        }

        /// <summary>
        /// Объединяет все игровые элементы в одну коллекцию.
        /// </summary>
        private void GetGameObjectsCollection()
        {
            foreach (var fruit in _scene.Fruits) GameObjectsCollection.Add(fruit);
            foreach (var stone in _scene.Stones) GameObjectsCollection.Add(stone);
            foreach (var snake in _scene.Snake) GameObjectsCollection.Add(snake);
        }

        /// <summary>
        /// Проверяет на совпадение координаты змеи и обьектов,
        /// если такой обьект имеется он возвращается для дальнейшего удаления из коллекции.
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        private GameObject GetObjectToRemove(IEnumerable<GameObject> gameObjects)
        {
            return gameObjects
                .Where(item => Math.Abs(_scene.Snake[0].ObjectCoordinateX - item.ObjectCoordinateX) <= _gameSettings.ShiftStep * 2)
                .FirstOrDefault(item => Math.Abs(_scene.Snake[0].ObjectCoordinateY - item.ObjectCoordinateY) <= _gameSettings.ShiftStep * 2);
        }

        /// <summary>
        /// Проверяет на совпадение координат головы змеи и элементов ее тела.
        /// </summary>
        /// <returns>true, false</returns>
        private bool SnakeHeadPositionMatchBody()
        {
            if (_scene.Snake.Count < 5)
                return false;

            for (var i = 4; i < _scene.Snake.Count; i++)
                if (Math.Abs(_scene.Snake[0].ObjectCoordinateX - _scene.Snake[i].ObjectCoordinateX) <= _gameSettings.ShiftStep)
                    if (Math.Abs(_scene.Snake[0].ObjectCoordinateY - _scene.Snake[i].ObjectCoordinateY) <= _gameSettings.ShiftStep)
                        return true;
            return false;
        }

        ///<summary>
        ///Увеличивает скорость.
        ///</summary>
        private void IncreaseSpeed()
        {
            if (Score % 3 != 0) return;
            if (_gameSettings.SnakeSpeed <= 10) return;

            _gameSettings.SnakeSpeed -= 2;
            Level++;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, _gameSettings.SnakeSpeed);
        }

        private void GameOver()
        {
            _timer.Stop();
            //mainWindow.ApplyBlur();
            //window2.Owner = mainWindow;
            //dialogWindow.Label.Content = "У вас не осталось жизней.\nКонец игры !!!";
            //dialogWindow.Button.Content = "Заново";
            //dialogWindow.Show();
            //window2.TextBlock_W2.Text = Window2DisplayInfo();
        }
    }
}
