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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Rectangle viewPort;
        MouseCursor cursor;
        BuildingPanel buildingPanel;

        //Declaring Plant Buildoptions
        PlantMain mainPlant;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing = true;
            //IsMouseVisible = true;
           //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            
            map = new Map(15);
            cursor = new MouseCursor(this); 
            viewPort = new Rectangle(-400,-200,0,0);
            buildingPanel = new BuildingPanel(-100,0);
            mainPlant = new PlantMain();
            
            
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
            Tile.SetMap(map);
            map.GetTile(0, 0).ReplacePlant(mainPlant);

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
            Tile.SpriteBatch = spriteBatch;
            Root.SetFont(Content.Load<SpriteFont>("FontDescription"));
            Tile.LoadContent(Content);
            Map.LoadContent(Content);
            InfoPanel.LoadContent(Content);
            BuildingPanel.LoadContent(Content);
            MouseCursor.LoadContent(Content);
            Route.LoadContent(Content);

            //Loading Plants
            mainPlant.LoadContent(Content);
            
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
            Tile.Camera = viewPort;
            viewPort.Height = graphics.GraphicsDevice.Viewport.Height;
            viewPort.Width = graphics.GraphicsDevice.Viewport.Width;

            cursor.Update(gameTime);
            buildingPanel.Update(viewPort);
            cursor.Scroll(ref viewPort);

            map.Update(gameTime);
            map.HighlightTile(cursor, viewPort);
            if (!buildingPanel.Select(cursor))
            {
                if (buildingPanel.SelectedOption != null)
                    map.Build(cursor, buildingPanel.SelectedOption);
                else map.SelectTile(cursor);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
           
            
            map.Draw(buildingPanel.SelectedOption);
            buildingPanel.Draw(spriteBatch);
            InfoPanel.Draw(map.SelectedTile);
            cursor.Draw(spriteBatch);
            spriteBatch.End();
             //TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
