using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using RolePlaying;
using RolePlayingGameData;

namespace tileEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // configure the content manager for the tile engine 
            TileEngine.ContentManager = Content;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            // TODO: use this.Content to load your game content here

            // set the viewport for the tile engine 
            TileEngine.Viewport = graphics.GraphicsDevice.Viewport;
            // load the initial map and set it into the tile engine 
            TileEngine.SetMap(Content.Load<Map>(@"Maps\\Map001"), null);
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
            TileEngine.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // 
            // draw the tile engine 
            // 
            // draw the base and fringe layers 
            TileEngine.DrawLayers(spriteBatch, true, true, false);
            // TODO: draw anything that goes on the map 
            // draw the object layer
            ////////////////////////////////////////////////////////////////////////////////////////
            // -- for now, clear a small rectangle to white
            // -- Clear cannot be called from within a SpriteBatch block
            spriteBatch.End();
            Vector2 position = TileEngine.PartyLeaderPosition.ScreenPosition;
            Rectangle[] clearRects = new Rectangle[1];
            clearRects[0] = new Rectangle((int)position.X, (int)position.Y, 20, 20);
            graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.White, 0f, 0, clearRects);
            spriteBatch.Begin();
            ////////////////////////////////////////////////////////////////////////////////////////
            TileEngine.DrawLayers(spriteBatch, false, false, true);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
