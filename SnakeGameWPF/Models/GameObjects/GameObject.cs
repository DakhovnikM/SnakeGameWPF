using System.Windows.Media;

namespace SnakeGameWPF.Models.GameObjects
{
    internal abstract class GameObject
    {
        public double CoordX { get; set; }

        public double CoordY { get; set; }

        public ImageSource Image { get; set; }

        public GameObjectType Type { get; set; }
    }
}
