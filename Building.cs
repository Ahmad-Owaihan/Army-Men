using Microsoft.Xna.Framework;

namespace ArmyMen
{
    class Building : Item
    {


        public Building(Vector2 newPos) : base(newPos)
        {
            radius = 50;
            health = 100;
            hitBoxPosition = new Vector2(position.X + 100, position.Y + 100);
            maxHealth = health;
        }

    }
}
