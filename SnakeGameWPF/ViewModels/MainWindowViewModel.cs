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
        private readonly DispatcherTimer _timer;
        private readonly GameEngine _gameEngine;
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
            _gameEngine = new GameEngine();
            _timer = new DispatcherTimer { Interval = _gameEngine.Interval };

            _timer.Tick += _gameEngine.GameLoop;
            _timer.Tick += Changed;
            _gameEngine.Over += GameEngine_Over;

            Score = _gameEngine.Score;
            Life = _gameEngine.Life;
            Level = _gameEngine.Level;

            GameObjectCollection = _gameEngine.GameObjectCollection;

            #region Создание команд

            KeyUpCommand = new RelayCommand((object p) => _gameEngine.Direction = Direction.Up, (object p) => _gameEngine.Direction != Direction.Down);

            KeyDownCommand = new RelayCommand((object p) => _gameEngine.Direction = Direction.Down, (object p) => _gameEngine.Direction != Direction.Up);

            KeyRightCommand = new RelayCommand((object p) => _gameEngine.Direction = Direction.Right, (object p) => _gameEngine.Direction != Direction.Left);

            KeyLeftCommand = new RelayCommand((object p) => _gameEngine.Direction = Direction.Left, (object p) => _gameEngine.Direction != Direction.Right);

            KeySpaceCommand = new RelayCommand(OnExecutedKeySpaceCommand);

            BtnExitCommand = new RelayCommand((object p) => Application.Current.Shutdown(), (object p) => true);
            #endregion
        }

        private void Changed(object sender, EventArgs e)
        {
            Score = _gameEngine.Score;
            Life = _gameEngine.Life;
            Level = _gameEngine.Level;
        }
        #endregion

        private void GameEngine_Over(object sender, bool e)
        {
            _timer.Stop();
        }
    }
}
