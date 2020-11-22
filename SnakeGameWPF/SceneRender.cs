using SnakeGameWPF.Views;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SnakeGameWPF
{
    class SceneRender
    {
        int SceneWidth { get; set; }

        int SceneHeidht { get; set; }

        readonly MainWindow mainWindow;

        private static SceneRender sceneRender;

        private SceneRender() { }

        private SceneRender(GameSettings gameSettings, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            SceneWidth = gameSettings.GameFildWidth;
            SceneHeidht = gameSettings.GameFildHeight;
        }

        public static SceneRender GetScenRender(GameSettings gameSettings, MainWindow mainWindow)
        {
            if (sceneRender == null)
                sceneRender = new SceneRender(gameSettings, mainWindow);

            return sceneRender;
        }

        public void Render(Scene scene)
        {
            RenderingObjectsCollection(scene.Snake, mainWindow);
            RenderingObjectsCollection(scene.Stones, mainWindow);
            RenderingObjectsCollection(scene.Fruits, mainWindow);
        }

        /// <summary>
        /// Размещает элементы на поле, перебирая коллекцию элементов.
        /// </summary>
        private void RenderingObjectsCollection(IList<GameObject> gameObjects, MainWindow mainWindow)
        {
            foreach (var item in gameObjects)
                RenderingObject(item, mainWindow);
        }

        /// <summary>
        /// Размещает один элемент на поле по его координатам.
        /// </summary>
        /// <param name="gameObject"></param>
        private void RenderingObject(GameObject gameObject, MainWindow mainWindow)
        {
            Canvas.SetLeft(gameObject.ObjectImage, gameObject.ObjectCoordinate.X);
            Canvas.SetTop(gameObject.ObjectImage, gameObject.ObjectCoordinate.Y);
            mainWindow.canvas.Children.Add(gameObject.ObjectImage);
        }
    }
}
