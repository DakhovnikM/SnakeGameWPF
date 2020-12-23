using SnakeGameWPF.ViewModels;
using SnakeGameWPF.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeGameWPF.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        Direction direction;
        private readonly Scene _scene;
        private GameSettings _gameSettings;
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

        public ObservableCollection<GameObject> GameCoolection { get; set; }
        #endregion

        #region CTOR
        public MainWindowViewModel()
        {

        }
        public MainWindowViewModel(MainWindow mainWindow):this()
        {
            _gameSettings = new GameSettings();
            _scene = Scene.GetScene(_gameSettings);

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, _gameSettings.SnakeSpeed) };
            _timer.Tick += GameCycle;

            direction = Direction.Pause;
            mainWindow.KeyDown += MainWindow_KeyDown; ;

            GameCoolection = new ObservableCollection<GameObject>();
            Score = _gameSettings.Score;
            Life = _gameSettings.SnakeLife;
            Level = _gameSettings.Level;
            GetGameColection();
        }
        #endregion

        /// <summary>
        /// обработчик нажатия клавиш главного окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            direction = e.Key switch
            {
                Key.Up => direction == Direction.Down ? direction : Direction.Up,
                Key.Down => direction == Direction.Up ? direction : Direction.Down,
                Key.Right => direction == Direction.Left ? direction : Direction.Right,
                Key.Left => direction == Direction.Right ? direction : Direction.Left,
                _ => direction
            };

            if (e.Key == Key.Space)
            {
                if (_timer.IsEnabled) _timer.Stop();
                else
                {
                    _timer.Start();
                    direction = Direction.Up;
                }
            }
        }

        /// <summary>
        /// Основной цикл игры.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameCycle(object sender, EventArgs e)
        {
            bool snakeBitItSelf = SnakeHeadPositionMatchBody();
            if (snakeBitItSelf) Life--;

            bool canAddStone = false;
            
            GameObject objToRemove = GetObjectToRemove(_scene.Fruits);
            if (objToRemove is Fruit fruit)
            {
                _scene.Fruits.Remove(fruit);
                _scene.AddNewFruitToFruits();
                _scene.AddNewSnakeBodyEllement();

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

            SetNewSnakePosition();
            GetGameColection();
        }

        /// <summary>
        /// Перемещает змею на новуюпозицию.
        /// </summary>
        public void SetNewSnakePosition()
        {
            var snakeEllement = _scene.Snake[0];
            var subPositionX = snakeEllement.ObjectCoordinateX;
            var subPositionY = snakeEllement.ObjectCoordinateY;
            switch (direction)
            {
                case Direction.Up:
                    snakeEllement.ObjectCoordinateY -= _gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinateY = snakeEllement.ObjectCoordinateY < 0
                        ? _gameSettings.GameFildWidth
                        : snakeEllement.ObjectCoordinateY;
                    break;

                case Direction.Down:
                    snakeEllement.ObjectCoordinateY += _gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinateY = snakeEllement.ObjectCoordinateY > _gameSettings.GameFildWidth
                        ? 0
                        : snakeEllement.ObjectCoordinateY;
                    break;

                case Direction.Right:
                    snakeEllement.ObjectCoordinateX += _gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinateX = snakeEllement.ObjectCoordinateX > _gameSettings.GameFildWidth
                        ? 0
                        : snakeEllement.ObjectCoordinateX;
                    break;

                case Direction.Left:
                    snakeEllement.ObjectCoordinateX -= _gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinateX = snakeEllement.ObjectCoordinateX < 0
                        ? _gameSettings.GameFildWidth
                        : snakeEllement.ObjectCoordinateX;
                    break;

                case Direction.Pause:
                    return;
            }

            for (int i = _scene.Snake.Count - 1; i > 1; i--)
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
        public void GetGameColection()
        {
            GameCoolection.Clear();

            foreach (var fruit in _scene.Fruits) GameCoolection.Add(fruit);

            foreach (var stone in _scene.Stones) GameCoolection.Add(stone);

            foreach (var snake in _scene.Snake) GameCoolection.Add(snake);
        }

        /// <summary>
        /// Проверяет на совпадение координаты змеи и всех обьектов.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        private GameObject GetObjectToRemove(List<GameObject> gameObjects)
        {
            foreach (var item in gameObjects)
                if (Math.Abs(_scene.Snake[0].ObjectCoordinateX - item.ObjectCoordinateX) <= _gameSettings.ShiftStep * 4)
                    if (Math.Abs(_scene.Snake[0].ObjectCoordinateY - item.ObjectCoordinateY) <= _gameSettings.ShiftStep * 4)
                        return item;
            return null;
        }

        /// <summary>
        /// Проверяет на совпадение координат головы змеи и элементов ее тела.
        /// </summary>
        /// <returns>true, false</returns>
        public bool SnakeHeadPositionMatchBody()
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
            if (Score % 3 == 0)
            {
                if (_gameSettings.SnakeSpeed > 10)
                {
                    _gameSettings.SnakeSpeed -= 2;
                    Level++;
                    _timer.Interval = new TimeSpan(0, 0, 0, 0, _gameSettings.SnakeSpeed);
                }
            }
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
