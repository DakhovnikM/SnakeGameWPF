using SnakeGameWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SnakeGameWPF
{
    class GameEngine
    {
        private readonly Scene scene;

        private readonly SceneRender sceneRender;

        public GameSettings gameSettings;

        readonly MainWindow mainWindow;

        readonly DialogWindow dialogWindow;

        private readonly DispatcherTimer timer;

        Direction direction;

        #region CTOR
        public GameEngine(MainWindow mainWindow, DialogWindow dialogWindow)
        {
            this.mainWindow = mainWindow;
            this.dialogWindow = dialogWindow;

            gameSettings = new GameSettings();
            scene = Scene.GetScene(gameSettings);
            sceneRender = SceneRender.GetScenRender(gameSettings, mainWindow);

            sceneRender.Render(scene);

            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, gameSettings.SnakeSpeed) };
            timer.Tick += GameCycle;

            direction = Direction.Pause;
            mainWindow.KeyDown += MainWindow_KeyDown; ;
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
                if (timer.IsEnabled)
                {
                    //mainWindow.dialogWindow.Visibility = Visibility.Visible;
                    //mainWindow.dialogWindow.Owner = mainWindow;
                    //window3.Owner = mainWindow;
                    //mainWindow.ApplyBlur();
                    //window3.Visibility = Visibility.Visible;
                    timer.Stop();
                }
                else
                {
                    timer.Start();
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
            if (snakeBitItSelf) gameSettings.SnakeLife--;

            bool canAddStone = false;
            GameObject objToRemove = GetObjectToRemove(scene.Fruits);
            if (objToRemove is Fruit fruit)
            {
                scene.Fruits.Remove(fruit);
                scene.AddNewFruitToFruits();
                scene.AddNewSnakeBodyEllement();

                gameSettings.Score++;
                canAddStone = true;

                IncreaseSpeed();
            }

            objToRemove = GetObjectToRemove(scene.Stones);
            if (objToRemove is Stone stone)
            {
                scene.Stones.Remove(stone);
                gameSettings.SnakeLife--;
                if (gameSettings.SnakeLife == 0) GameOver();
            }

            if (gameSettings.Score % 2 == 0 && canAddStone)
                scene.AddNewStoneToStones();

            mainWindow.canvas.Children.Clear();
            SetNewSnakePosition();
            sceneRender.Render(scene);
        }

        /// <summary>
        /// Перемещает змею на новуюпозицию.
        /// </summary>
        public void SetNewSnakePosition()
        {
            var snakeEllement = scene.Snake[0];
            Point subPosition = snakeEllement.ObjectCoordinate;
            switch (direction)
            {
                case Direction.Up:
                    snakeEllement.ObjectCoordinate.Y -= gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinate.Y = snakeEllement.ObjectCoordinate.Y < 0
                        ? gameSettings.GameFildWidth
                        : snakeEllement.ObjectCoordinate.Y;
                    break;

                case Direction.Down:
                    snakeEllement.ObjectCoordinate.Y += gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinate.Y = snakeEllement.ObjectCoordinate.Y > gameSettings.GameFildWidth
                        ? 0
                        : snakeEllement.ObjectCoordinate.Y;
                    break;

                case Direction.Right:
                    snakeEllement.ObjectCoordinate.X += gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinate.X = snakeEllement.ObjectCoordinate.X > gameSettings.GameFildWidth
                        ? 0
                        : snakeEllement.ObjectCoordinate.X;
                    break;

                case Direction.Left:
                    snakeEllement.ObjectCoordinate.X -= gameSettings.ShiftStep;
                    snakeEllement.ObjectCoordinate.X = snakeEllement.ObjectCoordinate.X < 0
                        ? gameSettings.GameFildWidth
                        : snakeEllement.ObjectCoordinate.X;
                    break;

                case Direction.Pause:
                    return;
            }

            for (int i = scene.Snake.Count - 1; i > 1; i--)
                scene.Snake[i].ObjectCoordinate = scene.Snake[i - 1].ObjectCoordinate;

            scene.Snake[1].ObjectCoordinate = subPosition;
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
                if (Math.Abs(scene.Snake[0].ObjectCoordinate.X - item.ObjectCoordinate.X) <= gameSettings.ShiftStep * 4)
                    if (Math.Abs(scene.Snake[0].ObjectCoordinate.Y - item.ObjectCoordinate.Y) <= gameSettings.ShiftStep * 4)
                        return item;
            return null;
        }

        /// <summary>
        /// Проверяет на совпадение координат головы змеи и элементов ее тела.
        /// </summary>
        /// <returns>true, false</returns>
        public bool SnakeHeadPositionMatchBody()
        {
            if (scene.Snake.Count < 5)
                return false;

            for (var i = 4; i < scene.Snake.Count; i++)
                if (Math.Abs(scene.Snake[0].ObjectCoordinate.X - scene.Snake[i].ObjectCoordinate.X) <= gameSettings.ShiftStep)
                    if (Math.Abs(scene.Snake[0].ObjectCoordinate.Y - scene.Snake[i].ObjectCoordinate.Y) <= gameSettings.ShiftStep)
                        return true;
            return false;
        }

        ///<summary>
        ///Увеличивает скорость.
        ///</summary>
        private void IncreaseSpeed()
        {
            if (gameSettings.Score % 3 == 0)
            {
                if (gameSettings.SnakeSpeed > 10)
                {
                    gameSettings.SnakeSpeed -= 2;
                    gameSettings.Level++;
                    timer.Interval = new TimeSpan(0, 0, 0, 0, gameSettings.SnakeSpeed);
                }
            }
        }

        private void GameOver()
        {
            timer.Stop();
            mainWindow.ApplyBlur();
            //window2.Owner = mainWindow;
            dialogWindow.Label.Content = "У вас не осталось жизней.\nКонец игры !!!";
            dialogWindow.Button.Content = "Заново";
            dialogWindow.Show();
            //window2.TextBlock_W2.Text = Window2DisplayInfo();
        }

        #region Beta



        //    /// <summary>
        //    /// Проверяет столкнулась ли змея со своим телом.
        //    /// </summary>
        //    /// <returns></returns>
        //    private bool DidSnakeBiteItSelf() => snake.HeadCoordMatchBody();



        //    private void MainWindowDisplayInfo()
        //    {
        //        mainWindow.TextBlock.Text = $"Фрукты   {Score}";
        //        mainWindow.TextBlock1.Text = $"Уровень  {GameLevel}";
        //        mainWindow.TextBlock2.Text = $"Камни    {stonesCollection.Count}";
        //        mainWindow.TextBlock3.Text = $"Жизни    {Life}";
        //        mainWindow.TextBlock4.Text = $"Время    {timeSpan.Minutes}:{timeSpan.Seconds}";
        //    }

        //    private string Window2DisplayInfo()
        //    {
        //        return Score == 0
        //                     ? "К сожалению вы не превзошли предыдущий рекорд. Попробуйте еще раз."
        //                     : $"ПОЗДРАВЛЯЕМ!!! Вы установили новый рекорд!!! {Score}";
        //    }

        //    private void GameInit()
        //    {
        //        snakeBitItSelf = false;
        //        snakeCollideStone = false;
        //        GameLevel = 0;
        //        Score = 0;
        //        Life = 3;
        //        stopWatch.Reset();
        //        mainWindow.canvas.Children.Clear();
        //        ObjectsInit();
        //        PlacementObjectsOnCanvas();
        //    }

        //    private void ObjectsInit()
        //    {
        //        fruitCollection = new FruitsCollection(3);
        //        snake = new Snake(fieldSize, shiftStep);
        //        stonesCollection = new StonesCollection();
        //    }

        //    #region Events
        //    private void MainWindowLoaded(object sender, EventArgs e)
        //    {
        //        GameInit();
        //    }

        //    private void MainWindowKeyDown(object sender, KeyEventArgs e)
        //    {

        //        if (e.Key == Key.Space)
        //        {
        //            if (stopWatch.IsRunning)
        //                stopWatch.Stop();
        //            else
        //                stopWatch.Start();

        //            if (timer.IsEnabled)
        //            {
        //                window3.Owner = mainWindow;
        //                mainWindow.ApplyBlur();
        //                window3.Visibility = Visibility.Visible;
        //                timer.Stop();
        //            }
        //            else
        //            {
        //                mainWindow.ClearBlur();
        //                timer.Start();
        //            }
        //        }
        //        snake.SetMoveDirection(e);
        //    }

        //    private void Window3KeyDown(object sender, KeyEventArgs e)
        //    {
        //        if (e.Key == Key.Space)
        //        {
        //            window3.Hide();
        //            mainWindow.ClearBlur();
        //            timer.Start();
        //            stopWatch.Start();
        //        }
        //    }

        //    private void Window2B1Click(object sender, EventArgs e)
        //    {
        //        GameInit();
        //        mainWindow.ClearBlur();
        //        window2.Hide();
        //    }
        #endregion
    }
}
