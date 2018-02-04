using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyMen
{
    class Player
    {
        // fields
        private Vector2 position = new Vector2(300, 300);
        private Direction direction;
        private bool isMoving = false;
        private int speed = 800;
        public static int Money;
        public static int income = 10;
        public static int enemyMoney;
        public static int enemyIncome = 10;

        // constructors
        public Vector2 Position { get { return position; } set { position = value; } }

        // methods
        public void Update(GameTime gameTime)
        {
            isMoving = false;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Up))
            {
                isMoving = true;
                direction = Direction.Up;
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                isMoving = true;
                direction = Direction.Down;
            }
            if (kState.IsKeyDown(Keys.Left))
            {
                isMoving = true;
                direction = Direction.Left;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                isMoving = true;
                direction = Direction.Right;
            }

            if (isMoving)
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (position.Y >= 0)
                        {
                            position.Y -= speed * dt;
                            break;
                        }
                        else
                            break;

                    case Direction.Down:
                        if (position.Y <= 1792)
                        {
                            position.Y += speed * dt;
                            break;
                        }
                        else
                            break;

                    case Direction.Left:
                        if (position.X >= 0)
                        {
                            position.X -= speed * dt;
                            break;
                        }
                        else
                            break;

                    case Direction.Right:
                        if (position.X <= 2560)
                        {
                            position.X += speed * dt;
                            break;
                        }
                        else
                            break;
                }
            }
        }
    }
}
