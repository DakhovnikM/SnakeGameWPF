﻿using System.Windows.Media;

namespace SnakeGameWPF.Models.GameObjects
{
    internal abstract class GameObject
    {
        public double ObjectCoordinateX { get; set; }

        public double ObjectCoordinateY { get; set; }

        public ImageSource ObjectImage { get; set; }

        public GameObjectType ObjectType { get; set; }
    }
}
