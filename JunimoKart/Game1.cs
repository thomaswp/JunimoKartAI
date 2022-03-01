using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using StardewValley.Minigames;
using StardewValley;
using Microsoft.Xna.Framework.Input;

namespace JunimoKart
{
    class Game1 : Game
    {
        public static Random random = new Random();

        public static void playSound(string path) { }

        public static int minecartHighScore = 0;
        internal static Texture2D debrisSpriteSheet;
        internal static Texture2D staminaRect;
        internal static Texture2D mouseCursors;
        internal static GameTime currentGameTime;
        internal static SpriteFont dialogueFont;
        internal static Viewport viewport;
        //internal static 
        internal static Microsoft.Xna.Framework.Content.ContentManager content;
        internal static Game1 game1;
        internal static InputState input;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MineCart cartGame;
        internal static PlayerIndex playerOneIndex = PlayerIndex.One;
        internal static ICue minecartLoop = new ICue();
        internal static object currentMinigame;
        internal static bool isUsingBackToFrontSorting;

        //public static AI AI = new RandomAI();
        //public static AI AI = new PlayerAI();
        public static AI AI = new SimpleAI();

        public bool IsMainInstance { get { return true; } }

        public static long ticks { get { return currentGameTime.TotalGameTime.Ticks; } }

        internal static Rectangle getSourceRectForStandardTileSheet(Texture2D debrisSpriteSheet, int v)
        {
            throw new NotImplementedException();
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            content = Content;
            Content.RootDirectory = "Content";
            game1 = this;
            input = new InputState();
        }

        internal bool HasKeyboardFocus()
        {
            return true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            dialogueFont = content.Load<SpriteFont>("SpriteFont1");
            debrisSpriteSheet = content.Load<Texture2D>("debris");
            mouseCursors = content.Load<Texture2D>("Cursors");
            viewport = GraphicsDevice.Viewport;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            var white3 = new Color[1];
            staminaRect = new Texture2D(GraphicsDevice, 1, 1, mipMap: false, SurfaceFormat.Color);
            for (int k = 0; k < white3.Length; k++)
            {
                white3[k] = new Color(255, 255, 255, 255);
            }
            staminaRect.SetData(white3);


            if (cartGame == null)
            {
                cartGame = new MineCart(0, 2);
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            currentGameTime = gameTime;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            currentGameTime = gameTime;
            cartGame.draw(spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            currentGameTime = gameTime;

            int speed = 100;
            for (int i = 0; i < speed; i++)
            {
                input.UpdateStates();
                cartGame.tick(gameTime);
            }
        }

        internal static void playSoundPitched(string v1, int v2) { }

        internal static bool isOneOfTheseKeysDown(KeyboardState keyboardState, Keys useToolButton)
        {
            return keyboardState.IsKeyDown(useToolButton);
        }
    }
}
