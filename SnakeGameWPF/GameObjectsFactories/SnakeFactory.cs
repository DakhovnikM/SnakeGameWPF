﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SnakeGameWPF
{
    class SnakeFactory : GameObjectFactory
    {
        public SnakeFactory(GameSettings gameSettings) : base(gameSettings)
        {

        }

        public override GameObject GetObject()
        {
            GameObject snakeEllement = new Snake()
            {
                ObjectCoordinateX = GameSettings.SnakeStartPositionX,
                ObjectCoordinateY = GameSettings.SnakeStartPositionY,
                ObjectImage = BitmapFrame.Create(new Uri(@"D:\Source\Repos\dahovnikM\SnakeGameWPF\SnakeGameWPF\Resources\snakeBody.png")),
                ObjectType = GameObjectType.Snake
            };
            return snakeEllement;
        }

        public List<GameObject> GetSnake()
        {
            var snake = new List<GameObject>();

            for (int i = 0; i < GameSettings.StartNomberOfSnakeEllements; i++)
            {
                GameObject snakeEllement = GetObject();
                snakeEllement.ObjectCoordinateY += i * 5;
                snake.Add(snakeEllement);
            }
            return snake;
        }
    }
}
