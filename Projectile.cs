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
    class Projectile
    {
        private int speed = 1000;
        private int radius = 15;
        private Vector2 position;
        private Vector2 direction;
        private bool mySide;
        private bool collided = false;
        private string type;
        private Unit unit;

        public static List<Projectile> projectiles = new List<Projectile>();

        //constructor
        public Projectile(Vector2 newPos, Vector2 newDir, bool side, Unit newUnit)
        {
            position = newPos;
            direction = newDir;
            mySide = side;
            unit = newUnit;
        }

        // properties
        public bool Collided { get { return collided; } set { collided = value; } }
        public int Radius { get { return radius; } }
        public Vector2 Position { get { return position; } }
        public bool MySide { get { return mySide; } set { mySide = value; } }
        public Unit GetUnit { get { return unit; } }

        //methods
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += direction * speed * dt;
        }
    }
}
