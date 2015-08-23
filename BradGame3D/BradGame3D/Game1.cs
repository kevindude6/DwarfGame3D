using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Text;

namespace BradGame3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        enum Gamemode {MainMenu, Game, TerrainMenu };
        int currentScreen = (int) Gamemode.MainMenu;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GameScreen gameObject;
        private MenuThings.MainMenu mainMenuObject;
        private MenuThings.TerrainMenu terrainObject;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            graphics.IsFullScreen = true;
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            graphics.ApplyChanges();
            


            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            this.IsMouseVisible = true;
            mainMenuObject = new MenuThings.MainMenu(this);
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //tex = Content.Load<Texture2D>("grass");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
    
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            switch (currentScreen)
            {
                case (int)Gamemode.Game: gameObject.updateGame(gameTime); break;
                case (int)Gamemode.MainMenu: mainMenuObject.updateMenu(gameTime); break;
                case (int)Gamemode.TerrainMenu: terrainObject.updateMenu(gameTime); break;
            }

            base.Update(gameTime);
        }
   


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GraphicsDevice.Clear(Color.SkyBlue);
            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            RasterizerState rs = new RasterizerState();
            
            rs.CullMode = CullMode.CullCounterClockwiseFace;
            rs.FillMode = FillMode.Solid;
            rs.ScissorTestEnable = false;

            rs.MultiSampleAntiAlias = false;
            rs.DepthBias = 8;

            switch (currentScreen)
            {
                case (int)Gamemode.Game: gameObject.draw(gameTime); break;
                case (int)Gamemode.MainMenu: mainMenuObject.drawMenu(gameTime); break;
                case (int)Gamemode.TerrainMenu: terrainObject.drawMenu(gameTime); break;
            }

            base.Draw(gameTime);
        }


        public void startGame()
        {
            gameObject = new GameScreen(this);
            currentScreen = (int)Gamemode.Game;
            mainMenuObject = null; 
        }
        public void enterMainMenu()
        {
            mainMenuObject = new MenuThings.MainMenu(this);
            currentScreen = (int)Gamemode.MainMenu;
            gameObject = null;
            terrainObject = null;
        }
        public void enterTerrainMenu()
        {
            terrainObject = new MenuThings.TerrainMenu(this);
            currentScreen = (int)Gamemode.TerrainMenu;
            mainMenuObject = null;
            
        }



     
    }
}
