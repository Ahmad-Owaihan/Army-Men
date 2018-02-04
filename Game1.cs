using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using System.Collections.Generic;

namespace ArmyMen
{
    enum GameState
    {
        MainMenu,
        Paused,
        InGame,
        InBattle,
        InShop
    }
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Controls
        SoundEffect[] sounds;

        // units
        Texture2D leftTurrent_Sprite;
        Texture2D rightTurrent_Sprite;

        Texture2D leftMan_Sprite;
        Texture2D rightMan_Sprite;
        Texture2D leftMen_Sprite;
        Texture2D rightMen_Sprite;

        Texture2D leftMan2_Sprite;
        Texture2D rightMan2_Sprite;
        Texture2D leftMen2_Sprite;
        Texture2D rightMen2_Sprite;

        Texture2D buyMan;
        Texture2D buyManNo;
        Texture2D buyMan2;
        Texture2D buyMan2No;
        Texture2D buyTurrent;
        Texture2D buyTurrentNo;



        //Castle
        Texture2D castle100_Sprite;
        Texture2D castle50_Sprite;
        Texture2D castle0_Sprite;

        //Bullets
        Texture2D manBullet_Sprite;
        Texture2D man2Bullet_Sprite;
        Texture2D turrentBullet_Sprite;

        SpriteFont money_Font;
        SpriteFont income_Font;

        Rectangle newGameRectangle = new Rectangle(100, 40, 300, 80);
        Rectangle buySoldier = new Rectangle(250, 5, 50, 50);
        Rectangle buySoldier2 = new Rectangle(305, 5, 50, 50);
        Rectangle buySoldier3 = new Rectangle(360, 5, 50, 50);

        Texture2D pointer;

        MouseState mouse;
        bool isReleased = true;
        Point mousePosition;
        
        TiledMapRenderer mapRenderer;
        TiledMap myMap;

        Player player = new Player();
        float incomeTimer;
        float incomeCount;

        Camera2D cam;

        // audio
        SoundEffect manShot_Sound;
        SoundEffect man2Shot_Sound;
        SoundEffect turrentShot_Sound;
        SoundEffect buyUnit_Sound;
        Song song;

        //other
        

        GameState gameState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            mapRenderer = new TiledMapRenderer(GraphicsDevice);

            cam = new Camera2D(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            castle100_Sprite = Content.Load<Texture2D>("Buildings/castle100");
            castle50_Sprite = Content.Load<Texture2D>("Buildings/castle50");
            castle0_Sprite = Content.Load<Texture2D>("Buildings/castle0");

            //units
            leftMan_Sprite = Content.Load<Texture2D>("Units/SLeft");
            rightMan_Sprite = Content.Load<Texture2D>("Units/SRight");
            leftMen_Sprite = Content.Load<Texture2D>("Units/menLeft");
            rightMen_Sprite = Content.Load<Texture2D>("Units/menRight");

            leftMan2_Sprite = Content.Load<Texture2D>("Units/man2Left");
            rightMan2_Sprite = Content.Load<Texture2D>("Units/man2Right");
            leftMen2_Sprite = Content.Load<Texture2D>("Units/men2Left");
            rightMen2_Sprite = Content.Load<Texture2D>("Units/men2Right");

            leftTurrent_Sprite = Content.Load<Texture2D>("Turrents/turrentLeft");
            rightTurrent_Sprite = Content.Load<Texture2D>("Turrents/turrentRight");

            buyMan = Content.Load<Texture2D>("Units/buyMan");
            buyManNo = Content.Load<Texture2D>("Units/buyManNo");
            buyMan2 = Content.Load<Texture2D>("Units/buyMan2");
            buyMan2No = Content.Load<Texture2D>("Units/buyMan2No");
            buyTurrent = Content.Load<Texture2D>("Units/buyTurrent");
            buyTurrentNo = Content.Load<Texture2D>("Units/buyTurrentNo");

            // unit animation
            Item.animations[0] = new AnimatedSprite(leftMan_Sprite, 1, 1);
            Item.animations[1] = new AnimatedSprite(leftMen_Sprite, 1, 4);
            Item.animations[2] = new AnimatedSprite(rightMan_Sprite, 1, 1);
            Item.animations[3] = new AnimatedSprite(rightMen_Sprite, 1, 4);

            Item.animations[4] = new AnimatedSprite(leftMan2_Sprite, 1, 1);
            Item.animations[5] = new AnimatedSprite(leftMen2_Sprite, 1, 4);
            Item.animations[6] = new AnimatedSprite(rightMan2_Sprite, 1, 1);
            Item.animations[7] = new AnimatedSprite(rightMen2_Sprite, 1, 4);

            Item.animations[8] = new AnimatedSprite(leftTurrent_Sprite, 1, 1);
            Item.animations[9] = new AnimatedSprite(rightTurrent_Sprite, 1, 1);

            //bullets
            manBullet_Sprite = Content.Load<Texture2D>("Bullet/manBullet");
            man2Bullet_Sprite = Content.Load<Texture2D>("Bullet/man2Bullet");
            turrentBullet_Sprite = Content.Load<Texture2D>("Bullet/turrentBullet");

            //tiled map
            myMap = Content.Load<TiledMap>("Misc/gameMap");

            //audio
            manShot_Sound = Content.Load<SoundEffect>("Audio/manShot");
            man2Shot_Sound = Content.Load<SoundEffect>("Audio/man2Shot");
            turrentShot_Sound = Content.Load<SoundEffect>("Audio/turrentShot");
            buyUnit_Sound = Content.Load<SoundEffect>("Audio/buyUnit");

            song = Content.Load<Song>("Audio/background1");

            sounds = new SoundEffect[4];
            sounds[0] = manShot_Sound;
            sounds[1] = man2Shot_Sound;
            sounds[2] = turrentShot_Sound;
            sounds[3] = buyUnit_Sound;


            //pointer
            pointer = Content.Load<Texture2D>("Misc/pointer");

            //fonts
            money_Font = Content.Load<SpriteFont>("Fonts/moneyFont");
            income_Font = Content.Load<SpriteFont>("Fonts/incomeFont");




            Player.Money = 500;
            Player.enemyMoney = 700;
            incomeTimer = 10;
            incomeCount = 0;


            gameState = GameState.InBattle;



            //MediaPlayer.Play(song);
            //MediaPlayer.IsRepeating = true;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float td = (float)gameTime.ElapsedGameTime.TotalSeconds;
            incomeCount += td;
            if (incomeCount >= incomeTimer)
            {
                Player.Money += Player.income;
                Player.enemyMoney += Player.enemyIncome;
                incomeCount = 0;
            }

            mouse = Mouse.GetState();
            mousePosition = new Point(mouse.X, mouse.Y);


            if (gameState == GameState.MainMenu)
            {
                // new game
                if (mouse.LeftButton == ButtonState.Pressed && isReleased == true && newGameRectangle.Contains(mousePosition))
                {
                    gameState = GameState.InBattle;
                    isReleased = false;
                }

                if (mouse.LeftButton == ButtonState.Released)
                {
                    isReleased = true;
                }
            }


            if (gameState == GameState.InBattle)
            {
                Unit.SpawnEnemyUnits(gameTime, sounds);

                if (mouse.LeftButton == ButtonState.Pressed && isReleased == true && buySoldier.Contains(mousePosition))
                {
                    Player.Money = Unit.BuyUnit(Player.Money, "man", sounds);
                    isReleased = false;
                }
                if (mouse.LeftButton == ButtonState.Pressed && isReleased == true && buySoldier2.Contains(mousePosition))
                {
                    Player.Money = Unit.BuyUnit(Player.Money, "man2", sounds);
                    isReleased = false;
                }
                if (mouse.LeftButton == ButtonState.Pressed && isReleased == true && buySoldier3.Contains(mousePosition))
                {
                    Player.Money = Unit.BuyUnit(Player.Money, "turrent", sounds);
                    isReleased = false;
                }

                if (mouse.LeftButton == ButtonState.Released)
                {
                    isReleased = true;
                }

                player.Update(gameTime);
                cam.LookAt(player.Position);

                foreach (Unit unit in Unit.Items)
                {
                    int rad = Unit.enemyBase.Radius;

                    if (unit.GetType() == typeof(Unit))
                    {
                        unit.Update(gameTime, new Vector2(Unit.enemyBase.Position.X + rad, Unit.enemyBase.Position.Y + rad), sounds);
                    }
                }

                foreach (Unit unit in Unit.enemyItems)
                {
                    int rad = Unit.myBase.Radius;

                    if (unit.GetType() == typeof(Unit))
                    {
                        unit.Update(gameTime, new Vector2(Unit.myBase.Position.X + rad, Unit.myBase.Position.Y + rad), sounds);
                    }
                }

                foreach (Projectile proj in Projectile.projectiles)
                {
                    proj.Update(gameTime);

                    if (Item.CollideWithEnemyBase(proj))
                    {
                        proj.Collided = true;
                    }
                    if (Item.CollideWithMyBase(proj))
                    {
                        proj.Collided = true;
                    }
                    if (Item.CollideWithEnemyUnit(proj))
                    {
                        proj.Collided = true;
                    }
                    if (Item.CollideWithMyUnit(proj))
                    {
                        proj.Collided = true;
                    }
                }

                Projectile.projectiles.RemoveAll(p => p.Collided);
                Unit.Items.RemoveAll(i => i.Health <= 0);
                Unit.enemyItems.RemoveAll(i => i.Health <= 0);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameState == GameState.InBattle)
            {
                GraphicsDevice.Clear(Color.ForestGreen);

                mapRenderer.Draw(myMap, cam.GetViewMatrix());

                spriteBatch.Begin(transformMatrix: cam.GetViewMatrix());

                //pointer
                spriteBatch.Draw(pointer, new Vector2(player.Position.X, player.Position.Y), Color.White);

                //projectile
                foreach (Projectile proj in Projectile.projectiles)
                {
                    if (proj.GetUnit.Type == "man")
                        spriteBatch.Draw(manBullet_Sprite, new Vector2(proj.Position.X, proj.Position.Y), Color.White);
                    if (proj.GetUnit.Type == "man2")
                        spriteBatch.Draw(man2Bullet_Sprite, new Vector2(proj.Position.X, proj.Position.Y), Color.White);
                    if (proj.GetUnit.Type == "turrent")
                        spriteBatch.Draw(turrentBullet_Sprite, new Vector2(proj.Position.X, proj.Position.Y), Color.White);
                }

                // castle health monitor
                // player castle
                if (Unit.myBase.Health > Unit.myBase.MaxHealth / 2)
                {
                    spriteBatch.Draw(castle100_Sprite, Unit.myBase.Position, Color.White);
                }
                else if (Unit.myBase.Health <= Unit.myBase.MaxHealth / 2 && Unit.myBase.Health > 0)
                {
                    spriteBatch.Draw(castle50_Sprite, Unit.myBase.Position, Color.White);
                }
                else
                {
                    spriteBatch.Draw(castle0_Sprite, Unit.myBase.Position, Color.White);
                }

                //enemy castle
                if (Unit.enemyBase.Health > Unit.enemyBase.MaxHealth / 2)
                {
                    spriteBatch.Draw(castle100_Sprite, Unit.enemyBase.Position, Color.White);
                }
                else if (Unit.enemyBase.Health <= Unit.enemyBase.MaxHealth / 2 && Unit.enemyBase.Health > 0)
                {
                    spriteBatch.Draw(castle50_Sprite, Unit.enemyBase.Position, Color.White);
                }
                else
                {
                    spriteBatch.Draw(castle0_Sprite, Unit.enemyBase.Position, Color.White);
                }

                foreach (Unit unit in Unit.Items)
                {
                    unit.anim.Draw(spriteBatch, unit.Position);
                }

                foreach (Unit unit in Unit.enemyItems)
                {
                    unit.anim.Draw(spriteBatch, unit.Position);
                }

                //health bar for castles
                if (Unit.myBase.Health > 0)
                    spriteBatch.DrawRectangle(new Rectangle((int)Unit.myBase.Position.X + 20, (int)Unit.myBase.Position.Y - 10, Unit.myBase.Health, 10), Color.Red, 5);

                if (Unit.enemyBase.Health > 0)
                    spriteBatch.DrawRectangle(new Rectangle((int)Unit.enemyBase.Position.X + 20, (int)Unit.enemyBase.Position.Y - 10, Unit.enemyBase.Health, 10), Color.Red, 5);

                spriteBatch.End();

                //locked screen
                spriteBatch.Begin();

                spriteBatch.DrawString(money_Font, "$" + Player.Money, new Vector2(0, 0), Color.Gold);
                spriteBatch.DrawString(money_Font, "+$" + Player.income, new Vector2(0, 50), Color.Brown);

                spriteBatch.DrawString(money_Font, "$" + Player.enemyMoney, new Vector2(0, 100), Color.Gold);
                spriteBatch.DrawString(money_Font, "+$" + Player.enemyIncome, new Vector2(0, 150), Color.Brown);

                spriteBatch.DrawRectangle(buySoldier, Color.White);
                if (Player.Money >= 40)
                {
                    spriteBatch.Draw(buyMan, buySoldier, Color.White);
                }
                else
                {
                    spriteBatch.Draw(buyManNo, buySoldier, Color.White);
                }

                spriteBatch.DrawRectangle(buySoldier2, Color.White);
                if (Player.Money >= 60)
                {
                    spriteBatch.Draw(buyMan2, buySoldier2, Color.White);
                }
                else
                {
                    spriteBatch.Draw(buyMan2No, buySoldier2, Color.White);
                }

                spriteBatch.DrawRectangle(buySoldier3, Color.White);
                if (Player.Money >= 500)
                {
                    spriteBatch.Draw(buyTurrent, buySoldier3, Color.White);
                }
                else
                {
                    spriteBatch.Draw(buyTurrentNo, buySoldier3, Color.White);
                }

                spriteBatch.End();
            }

            if (gameState == GameState.MainMenu)
            {
                GraphicsDevice.Clear(Color.DarkCyan);

                spriteBatch.Begin();

                spriteBatch.DrawRectangle(newGameRectangle, Color.Black, 1);
                spriteBatch.DrawString(money_Font, "New Game", new Vector2(125, 48), Color.Black);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
