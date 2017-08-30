using System;
using System.Linq;
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
        Player playerLower;
        Player playerUpper;
        Animation PlayerAnimationLower;
        Animation PlayerAnimationUpper;
        Texture2D PlayerTexture;
        Dialogue dialogue;
        List<string> dialogueContent;
        string playerGender;

        //NPC content
        List<npc> npcs;
        List<string> npcID;
        List<Animation> npcUpper;
        List<Animation> npcLower;
        List<Texture2D> npcTexture;

        //Dialogue Content
        Texture2D DialogueTexture;
        Rectangle DialogueRectangle;
        SpriteFont font;
        int dialogueX;
        int dialogueY;
        int dialogueWidth;
        int dialogueHeight;
        int dialoguePage;
        int currentPage;

        //Map
        Texture2D CurrentMapBase;
        Rectangle CurrentMapBaseRect;
        Texture2D CurrentMapOver;
        Rectangle CurrentMapOverRect;
        Texture2D CurrentMapTop;
        Rectangle CurrentMapTopRect;
        int mapX;
        int mapY;
        int MapWidth;
        int MapHeight;
        int MapXTiles;
        int MapYTiles;
        int currentTileX;
        int currentTileY;
        int[][] invisibleCollisionMapData;
        int[][] interactableMapData;
        string mapID = "001";

        //Inputs
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
        int KeyboardDelay = 2000000;//Ticks approx 100 per millisecond
        long previousGameTime;

        //Player direction variables
        string direction;
        int playerMovementSpeed = 1;
        int PlayerAnimationLowerSpeed = 300;
        bool isMoving = false;
        int distanceToTravel = 0;

        //Game state enumerable and reference
        enum GameState
        {
            splash,
            mainMenu,
            ready,
            dialogue
        }
        GameState gameState;

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
            playerLower = new Player();
            playerUpper = new Player();
            direction = "down";
            playerGender = "male";
            gameState = GameState.ready;
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
            //Load NPC Data
            npcs = new List<npc>();
            npcID = new List<string>();
            GetNPCList();
            InitializeNPC();
            //Load Player Data
            PlayerAnimationLower = new Phoenix.Animation();
            PlayerAnimationUpper = new Phoenix.Animation();
            Vector2 PlayerPositionLower = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 PlayerPositionUpper = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 16);
            //Load Dialogue Data
            DialogueTexture = Content.Load<Texture2D>("dialogue/window");
            font = Content.Load<SpriteFont>("dialogue/font");
            dialogueHeight = 100;
            dialogueWidth = 600;
            dialogueX = 100;//100
            dialogueY = 375;//500
            DialogueRectangle = new Rectangle(dialogueX, dialogueY, dialogueWidth, dialogueHeight);
            //Load Map
            CurrentMapBase = Content.Load<Texture2D>("maps/WorldMap_Layer0");
            CurrentMapOver = Content.Load<Texture2D>("maps/WorldMap_Layer3");
            CurrentMapTop = Content.Load<Texture2D>("maps/WorldMap_Layer5");
            MapWidth = CurrentMapBase.Width;
            MapHeight = CurrentMapBase.Height;
            MapXTiles = MapWidth / 16;
            MapYTiles = MapHeight / 16;
            mapX = (-MapWidth / 2 + GraphicsDevice.Viewport.Width / 2) + 8;
            mapY = (-MapHeight / 2 + GraphicsDevice.Viewport.Height / 2) + 8;
            CurrentMapBaseRect = new Rectangle(mapX, mapY, CurrentMapBase.Width, CurrentMapBase.Height);
            CurrentMapOverRect = CurrentMapBaseRect;
            CurrentMapTopRect = CurrentMapBaseRect;
            LoadMap();
            playerLower.Initialize(PlayerAnimationLower, PlayerPositionLower, playerGender);
            playerUpper.Initialize(PlayerAnimationUpper, PlayerPositionUpper, playerGender);
            UpdatePlayerDirection("down");
        }
        
        //Loads map data for collision, interactive, and terrain objects
        public void LoadMap()
        {
            string invisibleCollisionData;
            string interactableData;
            XmlDocument map = new XmlDocument();
            map.Load("Content/maps/WorldMapData.xml");
            XmlNodeList nodes = map.DocumentElement.SelectNodes("/map/layer/data");
            invisibleCollisionData = nodes[8].InnerXml;
            interactableData = nodes[9].InnerXml;
            invisibleCollisionData = invisibleCollisionData.Replace("\r", "");
            invisibleCollisionData = invisibleCollisionData.Replace("\n", "");
            interactableData = interactableData.Replace("\r", "");
            interactableData = interactableData.Replace("\n", "");
            invisibleCollisionMapData = new int[MapYTiles][];
            interactableMapData = new int[MapYTiles][];
            for (int x = 0; x < MapYTiles; x++)
            {
                invisibleCollisionMapData[x] = new int[MapXTiles];
                interactableMapData[x] = new int[MapXTiles];
                for(int y = 0; y < MapXTiles; y++)
                {
                    try
                    {
                        //Parse map value with commas
                        invisibleCollisionMapData[x][y] = int.Parse(invisibleCollisionData.Substring(0, invisibleCollisionData.IndexOf(",")));
                        invisibleCollisionData = invisibleCollisionData.Remove(0, invisibleCollisionData.IndexOf(','));
                        invisibleCollisionData = invisibleCollisionData.TrimStart(',');
                        interactableMapData[x][y] = int.Parse(interactableData.Substring(0, interactableData.IndexOf(",")));
                        interactableData = interactableData.Remove(0, interactableData.IndexOf(','));
                        interactableData = interactableData.TrimStart(',');
                    }
                    catch
                    {
                        //Parse last map data value to prevent trying to parse a comma
                        invisibleCollisionMapData[x][y] = int.Parse(invisibleCollisionData.Substring(0));
                        interactableMapData[x][y] = int.Parse(interactableData.Substring(0));
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
            if(gameState == GameState.ready)UpdatePlayer(gameTime);
            if (gameState == GameState.dialogue) PlayDialogue();
            for(int x = 0; x < npcs.Count; x++)
            {
                npcs.ElementAt(x).Update(gameTime);
            }
            
            base.Update(gameTime);
        }
        //Function to update the player
        public void UpdatePlayer(GameTime gameTime)
        {
            //Save the keyboard states
            UpdateKeyboard();
            //Determine the current tile the player is on
            currentTileX = ((CurrentMapBaseRect.Location.X) - (GraphicsDevice.Viewport.Width / 2));
            currentTileX = Math.Abs(currentTileX / 16);
            currentTileY = ((CurrentMapBaseRect.Location.Y) - (GraphicsDevice.Viewport.Height / 2));
            currentTileY = Math.Abs(currentTileY / 16);

            //Keyboard event handlers

            //Set player to moving
            if ((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S)) && !isMoving)
            {
                if (!IsCollidable(currentTileX, currentTileY + 1))
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }
                if(direction != "down")
                {
                    direction = "down";
                    UpdatePlayerDirection(direction);
                }
                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W)) && !isMoving)
            {
                if (!IsCollidable(currentTileX, currentTileY - 1))
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }
                if (direction != "up")
                {
                    direction = "up";
                    UpdatePlayerDirection(direction);
                }
                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A)) && !isMoving)
            {
                if (!IsCollidable(currentTileX- 1, currentTileY))
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }
                if (direction != "left")
                {
                    direction = "left";
                    UpdatePlayerDirection(direction);
                }
                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D)) && !isMoving)
            {
                if (!IsCollidable(currentTileX + 1, currentTileY))
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }
                if (direction != "right")
                {
                    direction = "right";
                    UpdatePlayerDirection(direction);
                }
                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position
            }
            //Interact Button
            if (currentKeyboardState.IsKeyDown(Keys.E) && gameState != GameState.dialogue && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if(IsInteractable(direction, currentTileX, currentTileY))
                {
                    GetDialogue(direction, currentTileX, currentTileY);
                    previousGameTime = DateTime.Now.Ticks;
                    gameState = GameState.dialogue;
                    currentPage = 5;
                }
            }
            //Sprint
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                playerMovementSpeed = 2;
                PlayerAnimationLowerSpeed = 150;
            }
            else if(currentKeyboardState.IsKeyUp(Keys.Space))
            {
                playerMovementSpeed = 1;
                PlayerAnimationLowerSpeed = 300;
            }

            //Process move
            if (isMoving && distanceToTravel > 0)
            {
                switch (direction)
                {
                    case "down":
                        CurrentMapBaseRect.Y -= playerMovementSpeed;
                        distanceToTravel -= playerMovementSpeed;
                        break;
                    case "up":
                        CurrentMapBaseRect.Y += playerMovementSpeed;
                        distanceToTravel -= playerMovementSpeed;
                        break;
                    case "left":
                        CurrentMapBaseRect.X += playerMovementSpeed;
                        distanceToTravel -= playerMovementSpeed;
                        break;
                    case "right":
                        CurrentMapBaseRect.X -= playerMovementSpeed;
                        distanceToTravel -= playerMovementSpeed;
                        break;
                }
                if(distanceToTravel <= 0)
                {
                    isMoving = false;
                }
            }

            //Update the lower and upper half of the player
            playerUpper.Update(gameTime);
            playerLower.Update(gameTime);
            //Turn off the animation and set the player to default standing position
            if(!isMoving)SetMoving(false);
        }
        //Function to update the direction the player is facing and select the appropriate sprite strip
        public void UpdatePlayerDirection(string direction)
        {
            PlayerTexture = Content.Load<Texture2D>("sprites/player/" + playerUpper.gender + "/walking_" + direction);
            PlayerAnimationLower.Initialize(PlayerTexture, new Vector2(0, 16), 32, 16, 16, 2, PlayerAnimationLowerSpeed, Color.White, 1.0f, true, false);
            PlayerAnimationUpper.Initialize(PlayerTexture, Vector2.Zero, 32, 16, 0, 2, PlayerAnimationLowerSpeed, Color.White, 1.0f, true, false);
        }
        public void UpdateNPCDirection(string direction)
        {
            for(int x = 0; x < npcs.Count; x++)
            {
                switch (direction)
                {
                    case "down":
                        npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathDown);
                        break;
                    case "up":
                        npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathUp);
                        break;
                    case "left":
                        npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathLeft);
                        break;
                    case "right":
                        npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathRight);
                        break;
                }
            }
        }

        //Function to update player movement
        public void SetMoving(bool startMoving)
        {
            if (startMoving)
            {
                PlayerAnimationLower.moving = true;
                PlayerAnimationUpper.moving = true;
                isMoving = true;
            }
            else
            {
                PlayerAnimationLower.moving = false;
                PlayerAnimationUpper.moving = false;
            }

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
            //Draw Base Map
            spriteBatch.Draw(CurrentMapBase, CurrentMapBaseRect, Color.White);//Layer 0
            //Draw player lower half
            playerLower.Draw(spriteBatch);//Player Lower Half
            //Draw NPC lower half
            for(int x = 0; x < npcs.Count; x++)
            {
                npcs.ElementAt(x).Draw(spriteBatch);
            }
            //Draw map layer 3
            spriteBatch.Draw(CurrentMapOver, CurrentMapBaseRect, Color.White);//Layer 3
            //Draw player upper
            playerUpper.Draw(spriteBatch);//Player Upper Half
            //Draw npc upper
            for (int x = 0; x < npcs.Count; x++)
            {
                npcs.ElementAt(x).Draw(spriteBatch);
            }
            spriteBatch.Draw(CurrentMapTop, CurrentMapBaseRect, Color.White);//Layer 5
            if (gameState == GameState.dialogue)
            {
                spriteBatch.Draw(DialogueTexture, DialogueRectangle, Color.White);//Dialogue
                if (currentPage - 5 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 5], new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
                if (currentPage - 4 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 4], new Vector2(dialogueX + 15, dialogueY + 24), Color.Black);
                if (currentPage - 3 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 3], new Vector2(dialogueX + 15, dialogueY + 39), Color.Black);
                if (currentPage - 2 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 2], new Vector2(dialogueX + 15, dialogueY + 54), Color.Black);
                if (currentPage - 1 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 1], new Vector2(dialogueX + 15, dialogueY + 69), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
        //Check if the block the player is facing is collidable
        public bool IsCollidable(int x, int y)
        {
            if (invisibleCollisionMapData[y][x] != 0) return true;
            else return false;
        }
        //Check if the block the player is facing is interactable
        public bool IsInteractable(string direction, int x, int y)
        {
            switch (direction)
            {
                case "up":
                    return (interactableMapData[y - 1][x] != 0);
                case "down":
                    return (interactableMapData[y + 1][x] != 0);
                case "left":
                    return (interactableMapData[y][x - 1] != 0);
                case "right":
                    return (interactableMapData[y][x + 1] != 0);
                default:
                    return false;
            }
        }
        //Gets the event ID for the interaction
        public void GetDialogue(string direction, int x, int y)
        {
            int[] tile = new int[2];
            string tileID;
            switch (direction)
            {
                case "up":
                    tile[0] = x;
                    tile[1] = y - 1;
                    break;
                case "down":
                    tile[0] = x;
                    tile[1] = y + 1;
                    break;
                case "left":
                    tile[0] = x - 1;
                    tile[1] = y;
                    break;
                case "right":
                    tile[0] = x + 1;
                    tile[1] = y;
                    break;
                default:
                    //Shouldn't happen
                    tile[0] = -1;
                    tile[1] = -1;
                    break;
            }
            dialogue = new Dialogue();
            dialogue.initialize(tile[0] + "" + tile[1] + "" + mapID);
            dialogueContent = new List<string>();
            dialogueContent = dialogue.content;
            dialoguePage = dialogueContent.Count;
        }
        //Function to play the dialogue
        public void PlayDialogue()
        {
            UpdateKeyboard();
            if (currentKeyboardState.IsKeyDown(Keys.E) && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (currentPage >= dialoguePage) gameState = GameState.ready;
                else currentPage += 5;
                previousGameTime = DateTime.Now.Ticks;
                
            }
        }
        //Function to update keyboard states
        public void UpdateKeyboard()
        {
            //Save the keyboard states
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
        }
        //Function to get list of NPC ids;
        public void GetNPCList()
        {
            XmlDocument npcData = new XmlDocument();
            npcData.Load("Content/sprites/npc/npc.xml");
            XmlNodeList npcDataList = npcData.DocumentElement.SelectNodes("/npcs/npc");
            for(int x = 0; x < npcDataList.Count; x++)
            {
                npcID.Add(npcDataList[x].Attributes["id"].Value);
            }
        }

        //Function to initialize npcs
        public void InitializeNPC()
        {
            for(int x = 0; x < npcID.Count; x++)
            {
                npcs.Add(new npc());
                npcs.ElementAt(x).Initialize(npcID.ElementAt(x));
                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathDown);
                npcs.ElementAt(x).upper.Initialize(npcs.ElementAt(x).texture, new Vector2(100, 100), 32, 16, 16, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, false);
                npcs.ElementAt(x).lower.Initialize(npcs.ElementAt(x).texture, new Vector2(100, 116), 32, 16, 0, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, false);
            }
        }
    }
}
