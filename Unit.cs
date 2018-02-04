using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ArmyMen
{
    class Unit : Item
    {
        //fields
        private int movementSpeed;
        private float attackSpeed;
        private float attackSpeedTimer;
        private int attackRange;
        private int attackDamage;
        private bool isMoving;
        private bool isStationed;
        private string type;
        private string description;
        public static float enemyTimer = 5;
        public static float innitalEnemyTimer = enemyTimer;
        public static List<Unit> Items = new List<Unit>();
        public static List<Unit> enemyItems = new List<Unit>();

        public static Unit myBase = new Unit(new Vector2(124, 724));
        public static Unit enemyBase = new Unit(new Vector2(2220, 724));

        //properties
        public string Description { get { return description; } }
        public string Type { get { return type; } set { type = value; } }
        public int AttackDamage { get { return attackDamage; } }

        //constructor
        public Unit(Vector2 newPos, string newType, bool stationed) : base(newPos)
        {
            isStationed = stationed;
            type = newType;
    
            if (type == "man")
            {
                description = "A tough soldier with moderate attack range and speed";
                movementSpeed = 100;
                attackSpeed = 10f; // 1 = 0.1hit per sec 
                attackSpeedTimer = attackSpeed;
                radius = 20;
                health = 10;
                attackRange = 400;
                attackDamage = 1;
                price = 40;
            }
            if (type == "man2")
            {
                description = "Squishy Solider with high attack speed but low range";
                movementSpeed = 70;
                attackSpeed = 50f; // 1 = 0.1hit per sec 
                attackSpeedTimer = attackSpeed;
                radius = 20;
                health = 5;
                attackRange = 300;
                attackDamage = 1;
                price = 60;
            }
            if (type == "turrent")
            {
                description = "A stationary turrent with high health and good attack power";
                movementSpeed = 50;
                attackSpeed = 5f; // 1 = 0.1hit per sec 
                attackSpeedTimer = attackSpeed;
                radius = 40;
                health = 20;
                attackRange = 600;
                attackDamage = 5;
                price = 500;
            }

        }

        public Unit(Vector2 newPos) : base(newPos)
        {
            radius = 50;
            health = 100;
            hitBoxPosition = new Vector2(position.X + 100, position.Y + 100);
            maxHealth = health;
        }
        //methods
        public static void SpawnEnemyUnits(GameTime gameTime, SoundEffect[] sounds)
        {
            enemyTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (enemyTimer <= 0)
            {
                Random rand = new Random();
                int x = rand.Next(1, 11);

                while(x != 0)
                {
                    int choice = rand.Next(1, 4);

                    switch (choice)
                    {
                        case 1:
                            Player.enemyMoney = BuyEnemyUnit(Player.enemyMoney, "man", sounds);
                            x--;
                            break;
                        case 2:
                            Player.enemyMoney = BuyEnemyUnit(Player.enemyMoney, "man2", sounds);
                            x--;
                            break;
                        case 3:
                            Player.enemyMoney = BuyEnemyUnit(Player.enemyMoney, "turrent", sounds);
                            x--;
                            break;
                        default:
                            break;
                    }
                }
                if (Unit.enemyBase.Health > 50)
                {
                    enemyTimer = rand.Next(1, (int)innitalEnemyTimer);
                }
                else
                {
                    enemyTimer = 1;
                }
            }

        }

        public static int BuyUnit(int money, string type, SoundEffect[] sounds)
        {
            Random rand = new Random();
            Vector2 basePos = Unit.myBase.HitBoxPosition;
            Unit unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man", false);
            int price = 0;

            if (type == "man")
            {
                unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man", false);
                price = unit.price;
            }
            if (type == "man2")
            {
                unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man2", false);
                price = unit.price;
            }
            if (type == "turrent")
            {
                unit = new Unit(new Vector2(basePos.X+100, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "turrent", true);
                price = unit.price;
            }
            // check funds
            if (price > money)
            {
                return money;
            }
            else
            {
                // play sound
                sounds[3].Play();
                // deduct money
                money -= price;
                // income increases by 10% of purchased unit
                Player.income += (price / 10);
                //spawn
                Items.Add(unit);

                return money;

            }


        }

        public static int BuyEnemyUnit(int money, string type, SoundEffect[] sounds)
        {
            Random rand = new Random();
            Vector2 basePos = Unit.enemyBase.HitBoxPosition;
            Unit unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man", false);
            int price = 0;

            if (type == "man")
            {
                unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man", false);
                price = unit.price;
            }
            if (type == "man2")
            {
                unit = new Unit(new Vector2(basePos.X, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "man2", false);
                price = unit.price;
            }
            if (type == "turrent")
            {
                unit = new Unit(new Vector2(basePos.X - 100, rand.Next((int)basePos.Y - 320, (int)basePos.Y + 320)), "turrent", true);
                price = unit.price;
            }
            // check funds
            if (price > money)
            {
                return money;
            }
            else
            {
                // deduct money
                money -= price;
                // income increases by 10% of purchased unit
                Player.enemyIncome += (price / 10);
                //spawn
                enemyItems.Add(unit);

                return money;

            }


        }

        private void Shoot(Unit target, SoundEffect[] sounds )
        {
            
            Vector2 moveDirection = target.HitBoxPosition - hitBoxPosition;
            moveDirection.Normalize();

            bool mySide = true;
            if (Unit.Items.Contains((Unit)target) || target == Unit.myBase) // target is player unit
            {
                mySide = false;
            }
            else if (Unit.enemyItems.Contains((Unit)target) || target == Unit.enemyBase) // target is enemy unit
            {
                mySide = true;
            }
            Projectile.projectiles.Add(new Projectile(hitBoxPosition, moveDirection, mySide, this));
            if (this.type == "man")
                sounds[0].Play();
            if (this.type == "man2")
                sounds[1].Play();
            if (this.type == "turrent")
                sounds[2].Play();
        }

        public override void Update(GameTime gameTime, Vector2 castlePos, SoundEffect[] sounds)
        {
            if (Items.Contains(this) && isMoving)
            {
                if (type == "man")
                {
                    anim = animations[1];
                }
                if (type == "man2")
                {
                    anim = animations[5];
                }
            }
            else if (Items.Contains(this) && !isMoving)
            {
                if (type == "man")
                {
                    anim = animations[0];
                }
                if (type == "man2")
                {
                    anim = animations[4];
                }
                if (type == "turrent")
                {
                    anim = animations[8];
                }
            }
            else if (enemyItems.Contains(this) && isMoving)
            {
                if (type == "man")
                {
                    anim = animations[3];
                }
                if (type == "man2")
                {
                    anim = animations[7];
                }
            }
            else if (enemyItems.Contains(this) && !isMoving)
            {
                if (type == "man")
                {
                    anim = animations[2];
                }
                if (type == "man2")
                {
                    anim = animations[6];
                }
                if (type == "turrent")
                {
                    anim = animations[9];
                }
            }
            anim.Update(gameTime);

            hitBoxPosition = new Vector2(position.X + 25, position.Y + 25);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 moveDirection = castlePos - position;
            moveDirection.Normalize();

            if (isMoving)
                position += moveDirection * movementSpeed * dt;

            if (Vector2.Distance(position, castlePos) > attackRange) // out of range of opponent base
            {
                if (!isStationed)
                    isMoving = true;

                List<Unit> list = new List<Unit>();

                if (enemyItems.Contains(this)) // enemy unit
                {
                    list = Items;
                }
                else if (Items.Contains(this)) // player unit
                {
                    list = enemyItems;
                }

                List<Unit> tempUnits = new List<Unit>(); // for targetting

                foreach (Unit unit in list) // opponent list 
                {
                    if (Vector2.Distance(HitBoxPosition, unit.HitBoxPosition) < attackRange)
                    {
                        isMoving = false;
                        tempUnits.Add(unit);

                        if (attackSpeedTimer >= 1) // attack 
                        {
                            Shoot(tempUnits[0], sounds);
                            attackSpeedTimer = 0;
                        }
                        else // reloading
                        {
                            attackSpeedTimer += (attackSpeed * 0.1f) * dt;
                        }
                        break;
                    }
                    else
                    {
                        if (!isStationed)
                            isMoving = true;
                    }
                }
            }

            else // in range with building
                {
                    isMoving = false;
                    Unit targetBuilding;

                    if (Items.Contains(this)) // unit is in (my)Items -- attack enemy building
                    {
                        targetBuilding = Unit.enemyBase;
                    }
                    else // unit is in enemyItems -- attack my building
                    {
                        targetBuilding = Unit.myBase;
                    }
                    if (attackSpeedTimer >= 1 && targetBuilding.Health >= 0) // attack 
                    {
                        Shoot(targetBuilding, sounds);
                        attackSpeedTimer = 0;
                    }
                    else // reloading
                    {
                        attackSpeedTimer += (attackSpeed * 0.1f) * dt;
                    }
                }
        }
    }
}
