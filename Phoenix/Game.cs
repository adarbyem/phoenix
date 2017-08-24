using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlayerNS;

namespace Phoenix
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Global Variables
        //Objects
        Player player;
        Animation PlayerAnimation;
        Texture2D PlayerTexture;

        //Map
        Texture2D CurrentMap;
        Rectangle CurrentMapRect;
        int mapX;
        int mapY;
        int MapWidth;
        int MapHeight;
        int MapXTiles;
        int MapYTiles;
        int currentTileX;
        int currentTileY;
        int[][] mapData;

        //Inputs
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        //Player direction variables
        string direction = "down";
        int playerMovementSpeed = 2;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            player = new Player();

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
            //Load Player Data
            PlayerAnimation = new Phoenix.Animation();
            UpdatePlayerDirection("down");
            Vector2 PlayerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            //Load Map
            CurrentMap = Content.Load<Texture2D>("maps/WorldMap");
            MapWidth = CurrentMap.Width;
            MapHeight = CurrentMap.Height;
            MapXTiles = MapWidth / 16;
            MapYTiles = MapHeight / 16;
            mapX = (-MapWidth / 2 + GraphicsDevice.Viewport.Width / 2);
            mapY = (-MapHeight / 2 + GraphicsDevice.Viewport.Height / 2);
            
            CurrentMapRect = new Rectangle(mapX, mapY, CurrentMap.Width, CurrentMap.Height);
            LoadMap();
            player.Initialize(PlayerAnimation, PlayerPosition);
        }
        
        public void LoadMap()
        {
            string collisionData;
            XmlDocument map = new XmlDocument();
            map.Load("Content/maps/WorldMapData.xml");
            XmlNodeList nodes = map.DocumentElement.SelectNodes("/map/layer/data");
            collisionData = nodes[2].InnerXml;
            collisionData = collisionData.Replace("\r", "");
            collisionData = collisionData.Replace("\n", "");
            mapData = new int[MapYTiles][];
            for(int x = 0; x < MapYTiles; x++)
            {
                mapData[x] = new int[MapXTiles];
                for(int y = 0; y < MapXTiles; y++)
                {
                    try
                    {
                        //Parse map value with commas
                        mapData[x][y] = int.Parse(collisionData.Substring(0, collisionData.IndexOf(",")));
                        collisionData = collisionData.Remove(0, collisionData.IndexOf(','));
                        collisionData = collisionData.TrimStart(',');
                    }
                    catch
                    {
                        //Parse last map data value to prevent trying to parse a comma
                        mapData[x][y] = int.Parse(collisionData.Substring(0));
                    }
                }
            }
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            UpdatePlayer(gameTime);
            base.Update(gameTime);
        }
        //Function to update the player
        public void UpdatePlayer(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            currentTileX = ((CurrentMapRect.Location.X) - (GraphicsDevice.Viewport.Width / 2));
            currentTileX = Math.Abs((int)Math.Round((double)currentTileX / 16));
            currentTileY = ((CurrentMapRect.Location.Y) - (GraphicsDevice.Viewport.Height / 2));
            currentTileY = Math.Abs((int)Math.Round((double)currentTileY / 16));

            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                PlayerAnimation.moving = true;
                CurrentMapRect.Y -= playerMovementSpeed;
                if(direction != "down")
                {
                    direction = "down";
                    UpdatePlayerDirection(direction);
                }
                //Console.WriteLine(mapData[currentTileX][currentTileY]);
                Console.WriteLine(currentTileX + " " + currentTileY);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                PlayerAnimation.moving = true;
                CurrentMapRect.Y += playerMovementSpeed;
                if (direction != "up")
                {
                    direction = "up";
                    UpdatePlayerDirection(direction);
                }
                //Console.WriteLine(mapData[currentTileX][currentTileY]);
                Console.WriteLine(currentTileX + " " + currentTileY);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                CurrentMapRect.X += playerMovementSpeed;
                PlayerAnimation.moving = true;
                if (direction != "left")
                {
                    direction = "left";
                    UpdatePlayerDirection(direction);
                }
                //Console.WriteLine(mapData[currentTileX][currentTileY]);
                Console.WriteLine(currentTileX + " " + currentTileY);
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                CurrentMapRect.X -= playerMovementSpeed;
                PlayerAnimation.moving = true;
                if (direction != "right")
                {
                    direction = "right";
                    UpdatePlayerDirection(direction);
                }
                //Console.WriteLine(mapData[currentTileX][currentTileY]);
                Console.WriteLine(currentTileX + " " + currentTileY);
            }
            player.Update(gameTime);
            PlayerAnimation.moving = false;
            
            
        }
        //Function to update the direction the player is facing and select the appropriate sprite strip
        public void UpdatePlayerDirection(string direction)
        {
            PlayerTexture = Content.Load<Texture2D>("sprites/player/male/walking_" + direction);
            PlayerAnimation.Initialize(PlayerTexture, Vector2.Zero, 32, 32, 2, 300, Color.White, 1.0f, true, false);
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
            //Do Stuff Here
            spriteBatch.Draw(CurrentMap, CurrentMapRect, Color.White);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
