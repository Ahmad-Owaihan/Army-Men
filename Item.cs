using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyMen
{
    class Item
    {
        //fields
        protected int health;
        protected int maxHealth;
        protected int price;
        protected Vector2 hitBoxPosition;
        protected int radius;
        protected Vector2 position;
        public AnimatedSprite anim;
        public static AnimatedSprite[] animations = new AnimatedSprite[10];

        //constuctor
        public Item(Vector2 newPos)
        {
            position = newPos;
            maxHealth = health;
        }

        //properties
        public Vector2 HitBoxPosition { get { return hitBoxPosition; } set { hitBoxPosition = value; } }
        public Vector2 Position { get { return position; } }
        public int Radius { get { return radius; } }
        public int Health { get { return health; }set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int Price { get { return price; } set { price = value; } }
        

        //methods

        public virtual void Update(GameTime gameTime, Vector2 castlePos, SoundEffect[] sounds)
        {

        }

        public static bool CollideWithMyUnit(Projectile proj)
        {
            // my units
            foreach (Unit item in Unit.Items)
            {
                int sum = item.radius + proj.Radius;
                if (Vector2.Distance(item.hitBoxPosition,proj.Position) < sum) //collision with my units
                {
                    if (!proj.MySide) // if its not my bullet
                    {
                        item.health -= proj.GetUnit.AttackDamage;
                        if (item.health <= 0)
                            Player.enemyMoney += (item.price / 10);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CollideWithEnemyUnit(Projectile proj)
        {
            foreach (Item item in Unit.enemyItems)
            {
                int sum = item.radius + proj.Radius;
                if (Vector2.Distance(item.hitBoxPosition, proj.Position) < sum) //collision with enemy units
                {
                    if (proj.MySide) // if its not enemy bullet
                    {
                        item.health -= proj.GetUnit.AttackDamage;
                        if (item.health <= 0)
                            Player.Money += (item.price / 10);
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CollideWithEnemyBase(Projectile proj)
        {
            // enemy building
            int sum = Unit.enemyBase.radius + proj.Radius;
            if (Vector2.Distance(Unit.enemyBase.hitBoxPosition, proj.Position) < sum)
            {
                Unit.enemyBase.Health--;
                return true;
            }
            return false;
        }

        public static bool CollideWithMyBase(Projectile proj)
        {
            // my building
            int sum = Unit.myBase.radius + proj.Radius;
            if (Vector2.Distance(Unit.myBase.hitBoxPosition, proj.Position) < sum)
            {
                Unit.myBase.Health--;
                return true;
            }
            return false;
        }
    }
}
