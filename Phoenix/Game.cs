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
        //Constants
        const int BATTLECHANCE = 2;//Default 12 Higher = less chance
        const int BATTLEMODIFIER = 2;//Default 2 Higher = more chance in areas without tall grass
        const int BATTLESPEED = 5;//Default 10 battle message speed
        const int ANIMATIONFRAMETIME = 10;//Default X animation frame speed

        //Menu Globals
        Texture2D menuSelector;
        Rectangle menuSelectorRect;
        Texture2D menuSelectorMoves;
        Rectangle menuSelectorMovesRect;

        //Battle Globals
        string[] mapPaths;
        int[][] encounters;
        Texture2D battleBG;
        Rectangle battleBGRect;
        Texture2D battlePlace;
        Rectangle battlePlaceRect;
        Texture2D enemyPokemon;
        Rectangle enemyPokemonRect;
        Texture2D playerPokemon;
        Rectangle playerPokemonRect;
        Texture2D battleMenu0;
        Rectangle battleMenu0Rect;
        Texture2D battleMenuMoves;
        Rectangle battleMenuMovesRect;
        Texture2D battleAnimationTexture;
        Texture2D enemyHPPanel;
        Rectangle enemyHPPanelRect;
        Texture2D enemyHP;
        Rectangle enemyHPRect;
        Texture2D playerHPPanel;
        Rectangle playerHPPanelRect;
        Texture2D playerHP;
        Rectangle playerHPRect;
        Texture2D hpBack;
        Rectangle enemyHPBackRect;
        Rectangle playerHPBackRect;
        Pokemon.pokemon enemyPokemonObject;
        Pokemon.pokemon playerPokemonObject;
        Vector2 move1pos;
        Vector2 move2pos;
        Vector2 move3pos;
        Vector2 move4pos;
        Vector2 enemyNamePos;
        Vector2 playerNamePos;
        Vector2 playerHPNumbers;
        Pokemon.moves moveToUse;
        Pokemon.moves selectedEnemyMove;
        Rectangle enemyBattleAnimationRect;
        Rectangle playerBattleAnimationRect;
        Rectangle animationFrameSourceRect;
        string selectedMove;
        double netDamage;
        int type;
        bool didDisplayEffectiveness;
        string encounterText = "";
        bool playerFirst = true;
        string battleMenuSelection;
        bool playerAnimationComplete;
        bool enemyAnimationComplete;
        int animationCycle;
        bool reverseAnimation;
        long animationTime;
        long animationDelay;
        int battleAnimationX = 0;
        int battleAnimationY = 0;
        int finishFrames;
        int xframes;
        int yframes;
        int currentPokemon;
        bool battleAnimationFinisher = false;
        bool battleAnimationStarted = false;
        int playerHPWidth;
        int enemyHPWidth;

        //Pokemon Globals
        Pokemon pokemon;
        string shinyString;

        //Player Pokemon
        List<Pokemon.pokemon> playerBox;
        List<Pokemon.pokemon> pcBox0;

        //RNG
        Random rng;

        //Player Globals
        Player playerLower;
        Player playerUpper;
        Animation PlayerAnimationLower;
        Animation PlayerAnimationUpper;
        Texture2D PlayerTexture;
        Dialogue dialogue;
        List<string> dialogueContent;
        string playerGender;

        //NPC Globals
        List<npc> npcs;
        List<string> npcID;
        int[] npcOffset;

        //Dialogue Globals
        Texture2D DialogueTexture;
        Rectangle DialogueRectangle;
        SpriteFont font;
        SpriteFont font6;
        int dialogueX;
        int dialogueY;
        int dialogueWidth;
        int dialogueHeight;
        int dialoguePage;
        int currentPage;

        //Splash Screen Globals
        Texture2D SplashScreen;
        Rectangle SplashRect;

        //Map Globals
        Maps maps;
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
        double[][] invisibleCollisionMapData;
        int[][] interactableMapData;
        int[][] tallGrassMapData;
        string mapID;
        string mapEncountersID;
        string mapEnvironmentType;

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
        bool isInTallGrass = false;
        bool playerIsFalling = false;
        bool playerIsRunning = false;

        //Game state and player environment enumerables and reference
        enum GameState
        {
            splash,
            mainMenu,
            ready,
            dialogue,
            battle
        }
        GameState gameState;
        enum PlayerEnvironment
        {
            outside,
            swimming,
            indoors,
            cave,
            repel
        }
        PlayerEnvironment playerEnvironment;

        enum MapEnvironment
        {
            grass,
            forest,
            cave,
            beach,
            surf,
            desert
        }
        MapEnvironment mapEnvironment;

        enum BattleState
        {
            begin,
            playerTurn,
            enemyTurn,
            runSucceed,
            runFail,
            displayingResult,
            animatingPlayer,
            animatingEnemy,
            end
        }
        BattleState battleState;
        BattleState previousBattleState;

        enum BattleMenuDepth
        {
            initial,
            fight,
            pokemon,
            item,
            run,
            waiting,
        }
        BattleMenuDepth battleMenuDepth;

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
            playerEnvironment = PlayerEnvironment.outside;
            mapEnvironment = MapEnvironment.grass;
            battleState = BattleState.begin;
            maps = new Maps();
            mapPaths = new string[2];
            rng = new Random();
            pokemon = new Pokemon();
            battleBGRect = new Rectangle(140, 0, 512, 384);
            battlePlaceRect = new Rectangle(140, 0, 512, 384);
            enemyPokemonRect = new Rectangle(428, 45, 192, 192);
            playerPokemonRect = new Rectangle(160, 140, 288, 288);
            menuSelectorRect = new Rectangle(110, 274, 102, 31);
            battleMenu0Rect = new Rectangle(100, 255, 233, 117);
            battleMenuMovesRect = new Rectangle(200, 315, 395, 102);
            menuSelectorMovesRect = new Rectangle(210, 322, 191, 44);
            playerBattleAnimationRect = new Rectangle(160, 140, 288, 288);
            enemyBattleAnimationRect = new Rectangle(428, 45, 192, 192);
            animationFrameSourceRect = new Rectangle(0, 0, 192, 192);
            enemyHPPanelRect = new Rectangle(165, 70, 232, 63);
            playerHPPanelRect = new Rectangle(420, 225, 232, 63);
            enemyHPRect = new Rectangle(221, 100, 142, 16);
            enemyHPBackRect = new Rectangle(221, 100, 142, 16);
            playerHPRect = new Rectangle(476, 256, 142, 16);
            playerHPBackRect = new Rectangle(476, 256, 142, 16);
            move1pos = new Vector2(225, 330);
            move2pos = new Vector2(415, 330);
            move3pos = new Vector2(225, 375);
            move4pos = new Vector2(415, 375);
            enemyNamePos = new Vector2(208, 78);
            playerNamePos = new Vector2(460, 233);
            playerHPNumbers = new Vector2(469, 272);
            playerBox = new List<Pokemon.pokemon>();
            pcBox0 = new List<Pokemon.pokemon>();
            shinyString = "";
            selectedMove = "move1";
            type = -1;
            didDisplayEffectiveness = false;
            playerAnimationComplete = true;
            enemyAnimationComplete = true;
            animationDelay = 100000;
            animationCycle = 0;
            currentPokemon = 0;
            finishFrames = 6;//Must be an even number or the sprite will get stuck on it's inverse texture
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

            //Load Textures for Battle Menu
            battleMenu0 = Content.Load<Texture2D>("menu/battle_0");
            menuSelector = Content.Load<Texture2D>("menu/selector");
            battleMenuMoves = Content.Load<Texture2D>("menu/battle_moves");
            menuSelectorMoves = Content.Load<Texture2D>("menu/moveselector");
            enemyHPPanel = Content.Load<Texture2D>("battle/components/hp");
            playerHPPanel = Content.Load<Texture2D>("battle/components/hp");
            enemyHP = Content.Load<Texture2D>("battle/components/hp_full");
            playerHP = Content.Load<Texture2D>("battle/components/hp_full");
            hpBack = Content.Load<Texture2D>("battle/components/hp_back");
            //Load Pokemon
            pokemon.InitializeMoves();
            playerBox.Add(pokemon.generatePokemon("1", 1));//INITIAL PLAYER POKEMON
            //Load Player Data
            PlayerAnimationLower = new Phoenix.Animation();
            PlayerAnimationUpper = new Phoenix.Animation();
            Vector2 PlayerPositionLower = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 PlayerPositionUpper = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2 - 16);
            //Load Dialogue Data
            DialogueTexture = Content.Load<Texture2D>("dialogue/window");
            font = Content.Load<SpriteFont>("dialogue/font");
            font6 = Content.Load<SpriteFont>("dialogue/font6");
            dialogueHeight = 100;
            dialogueWidth = 600;
            dialogueX = 100;//100
            dialogueY = 375;//500
            DialogueRectangle = new Rectangle(dialogueX, dialogueY, dialogueWidth, dialogueHeight);
            //Load Splash
            SplashScreen = Content.Load<Texture2D>("splash/splash");
            SplashRect = new Rectangle(0, 0, 800, 600);
            //Load Map
            mapID = "001";
            mapEncountersID = "Route 1";
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
            //Load NPC Data
            npcs = new List<npc>();
            npcID = new List<string>();
            GetNPCList();
            InitializeNPC();


        }

        //Loads map data for collision, interactive, and terrain objects
        public void LoadMap()
        {
            string invisibleCollisionData;
            string interactableData;
            string tallGrassData;
            XmlDocument map = new XmlDocument();
            map.Load("Content/maps/WorldMapData.xml");
            XmlNodeList nodes = map.DocumentElement.SelectNodes("/map/layer/data");
            invisibleCollisionData = nodes[8].InnerXml;
            interactableData = nodes[9].InnerXml;
            tallGrassData = nodes[11].InnerXml;
            invisibleCollisionData = invisibleCollisionData.Replace("\r", "");
            invisibleCollisionData = invisibleCollisionData.Replace("\n", "");
            tallGrassData = tallGrassData.Replace("\r", "");
            tallGrassData = tallGrassData.Replace("\n", "");
            interactableData = interactableData.Replace("\r", "");
            interactableData = interactableData.Replace("\n", "");
            invisibleCollisionMapData = new double[MapYTiles][];
            interactableMapData = new int[MapYTiles][];
            tallGrassMapData = new int[MapYTiles][];
            for (int x = 0; x < MapYTiles; x++)
            {
                invisibleCollisionMapData[x] = new double[MapXTiles];
                interactableMapData[x] = new int[MapXTiles];
                tallGrassMapData[x] = new int[MapXTiles];
                for (int y = 0; y < MapXTiles; y++)
                {
                    try
                    {
                        //Parse map value with commas
                        invisibleCollisionMapData[x][y] = double.Parse(invisibleCollisionData.Substring(0, invisibleCollisionData.IndexOf(",")));
                        invisibleCollisionData = invisibleCollisionData.Remove(0, invisibleCollisionData.IndexOf(','));
                        invisibleCollisionData = invisibleCollisionData.TrimStart(',');
                        interactableMapData[x][y] = int.Parse(interactableData.Substring(0, interactableData.IndexOf(",")));
                        interactableData = interactableData.Remove(0, interactableData.IndexOf(','));
                        interactableData = interactableData.TrimStart(',');
                        tallGrassMapData[x][y] = int.Parse(tallGrassData.Substring(0, tallGrassData.IndexOf(",")));
                        tallGrassData = tallGrassData.Remove(0, tallGrassData.IndexOf(','));
                        tallGrassData = tallGrassData.TrimStart(',');
                    }
                    catch
                    {
                        //Parse last map data value to prevent trying to parse a comma
                        invisibleCollisionMapData[x][y] = double.Parse(invisibleCollisionData.Substring(0));
                        interactableMapData[x][y] = int.Parse(interactableData.Substring(0));
                        tallGrassMapData[x][y] = int.Parse(tallGrassData.Substring(0));
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

            // TODO: Add your update logic here
            if (gameState == GameState.ready)
            {
                UpdatePlayer(gameTime);
                MoveNPC();
                for (int x = 0; x < npcs.Count; x++)
                {
                    npcs.ElementAt(x).Update(gameTime, CurrentMapBaseRect.X, CurrentMapBaseRect.Y);
                }
            }
            //Updates the map environment
            switch (mapEnvironment)
            {
                case MapEnvironment.grass:
                    mapEnvironmentType = "grass";
                    break;

            }
            if (gameState == GameState.dialogue) PlayDialogue();
            if (gameState == GameState.splash) UpdateSplash(gameTime);
            if (gameState == GameState.battle && battleState != BattleState.displayingResult) UpdateBattle(gameTime);
            base.Update(gameTime);
        }

        //Function to update the battle feature
        public void UpdateBattle(GameTime gameTime)
        {
            //Update keyboard
            UpdateKeyboard();

            //Key presses

            //Positioning and other tests
            if (currentKeyboardState.IsKeyDown(Keys.P))
            {
                playerPokemonObject.train("all");
                Console.WriteLine(playerPokemonObject.atk + " " + playerPokemonObject.satk + " " + playerPokemonObject.def + " " + playerPokemonObject.sdef + " " + playerPokemonObject.evade + " " + playerPokemonObject.spd);
            }
            /*
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                playerHPNumbers.Y--;
                Console.WriteLine(playerHPNumbers.X + " " + playerHPNumbers.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                playerHPNumbers.Y++;
                Console.WriteLine(playerHPNumbers.X + " " + playerHPNumbers.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                playerHPNumbers.X--;
                Console.WriteLine(playerHPNumbers.X + " " + playerHPNumbers.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                playerHPNumbers.X++;
                Console.WriteLine(playerHPNumbers.X + " " + playerHPNumbers.Y);
            }
            */
            //End Positioning

            if (currentKeyboardState.IsKeyDown(Keys.R) && enemyAnimationComplete)
            {
                gameState = GameState.ready;
                battleState = BattleState.begin;
            }
            if (currentKeyboardState.IsKeyDown(Keys.E) && battleState == BattleState.begin && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (playerFirst) battleState = BattleState.playerTurn;
                else battleState = BattleState.enemyTurn;
                previousGameTime = DateTime.Now.Ticks;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.E) && battleMenuDepth == BattleMenuDepth.initial && battleState == BattleState.playerTurn && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (battleMenuDepth == BattleMenuDepth.initial)
                {
                    if (battleMenuSelection == "run") processRun();
                    if (battleMenuSelection == "fight") battleMenuDepth = BattleMenuDepth.fight;
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.E) && battleState == BattleState.runSucceed && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                gameState = GameState.ready;
                battleMenuDepth = BattleMenuDepth.initial;
                battleState = BattleState.begin;
                battleMenuSelection = "fight";
                menuSelectorRect.X = 110;
                menuSelectorRect.Y = 274;
                previousGameTime = DateTime.Now.Ticks;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.E) && battleState == BattleState.runFail && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                battleMenuDepth = BattleMenuDepth.initial;
                battleState = BattleState.enemyTurn;//SET TO ENEMY TURN
                previousGameTime = DateTime.Now.Ticks;
            }

            //Movement keys for battlemenustate of fight
            if (currentKeyboardState.IsKeyDown(Keys.Escape) && battleMenuDepth == BattleMenuDepth.fight)
            {
                battleMenuDepth = BattleMenuDepth.initial;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move3" && selectedMove != "move4" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (playerPokemonObject.move3.name != "NONE" && selectedMove == "move1")
                {
                    menuSelectorMovesRect.X = 210;
                    menuSelectorMovesRect.Y = 368;
                    selectedMove = "move3";
                }
                if (playerPokemonObject.move4.name != "NONE" && selectedMove == "move2")
                {
                    menuSelectorMovesRect.X = 400;
                    menuSelectorMovesRect.Y = 368;
                    selectedMove = "move4";
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move1" && selectedMove != "move2" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (playerPokemonObject.move1.name != "NONE" && selectedMove == "move3")
                {
                    menuSelectorMovesRect.X = 210;
                    menuSelectorMovesRect.Y = 322;
                    selectedMove = "move1";
                }
                if (playerPokemonObject.move2.name != "NONE" && selectedMove == "move4")
                {
                    menuSelectorMovesRect.X = 400;
                    menuSelectorMovesRect.Y = 322;
                    selectedMove = "move2";
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move1" && selectedMove != "move3" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (playerPokemonObject.move1.name != "NONE" && selectedMove == "move2")
                {
                    menuSelectorMovesRect.X = 210;
                    menuSelectorMovesRect.Y = 322;
                    selectedMove = "move1";
                }
                if (playerPokemonObject.move3.name != "NONE" && selectedMove == "move4")
                {
                    menuSelectorMovesRect.X = 210;
                    menuSelectorMovesRect.Y = 368;
                    selectedMove = "move3";
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move2" && selectedMove != "move4" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (playerPokemonObject.move2.name != "NONE" && selectedMove == "move1")
                {
                    menuSelectorMovesRect.X = 400;
                    menuSelectorMovesRect.Y = 322;
                    selectedMove = "move2";
                }
                if (playerPokemonObject.move4.name != "NONE" && selectedMove == "move3")
                {
                    menuSelectorMovesRect.X = 400;
                    menuSelectorMovesRect.Y = 368;
                    selectedMove = "move4";
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            if (currentKeyboardState.IsKeyDown(Keys.E) && battleMenuDepth == BattleMenuDepth.fight && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                processMoveSelection(selectedMove, gameTime);
                previousGameTime = DateTime.Now.Ticks;
            }
            if (battleState == BattleState.enemyTurn)
            {
                battleMenuDepth = BattleMenuDepth.waiting;
                processEnemyMoves();
            }
            //Movement keys for initial battle menu
            if (currentKeyboardState.IsKeyDown(Keys.Down) && battleMenuSelection != "item" && battleMenuSelection != "run" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.Y += 41;
                if (battleMenuSelection == "fight") battleMenuSelection = "item";
                else battleMenuSelection = "run";
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up) && battleMenuSelection != "fight" && battleMenuSelection != "pokemon" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.Y -= 41;
                if (battleMenuSelection == "item") battleMenuSelection = "fight";
                else battleMenuSelection = "pokemon";
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Left) && battleMenuSelection != "fight" && battleMenuSelection != "item" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.X -= 105;
                if (battleMenuSelection == "pokemon") battleMenuSelection = "fight";
                else battleMenuSelection = "item";
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right) && battleMenuSelection != "pokemon" && battleMenuSelection != "run" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.X += 105;
                if (battleMenuSelection == "fight") battleMenuSelection = "pokemon";
                else battleMenuSelection = "run";
            }
        }
        //Function to update the splash screen
        public void UpdateSplash(GameTime gameTime)
        {
            //Update keyboard
            UpdateKeyboard();
            //Button Event Handlers
            if (currentKeyboardState.IsKeyDown(Keys.E))
            {
                gameState = GameState.ready;
            }
        }

        //Function to update the player during ready game state
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
            if (((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S)) && !isMoving) || (playerIsFalling && !isMoving))
            {

                if (direction != "down")
                {
                    direction = "down";
                    UpdatePlayerDirection(direction);
                }
                if ((!IsCollidable(currentTileX, currentTileY + 1, false) && IsNPCCollide(currentTileX, currentTileY)[0] == -99999))
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                    if (playerIsFalling) distanceToTravel += 16;
                }


                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position DEBUG CODE
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W)) && !isMoving)
            {
                if (direction != "up")
                {
                    direction = "up";
                    UpdatePlayerDirection(direction);
                }
                if (!IsCollidable(currentTileX, currentTileY - 1, false) && IsNPCCollide(currentTileX, currentTileY - 2)[0] == -99999)
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }

                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position DEBUG CODE
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A)) && !isMoving)
            {
                if (direction != "left")
                {
                    direction = "left";
                    UpdatePlayerDirection(direction);
                }
                if (!IsCollidable(currentTileX - 1, currentTileY, false) && IsNPCCollide(currentTileX - 1, currentTileY - 1)[0] == -99999)
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }

                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position DEBUG CODE
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D)) && !isMoving)
            {
                if (direction != "right")
                {
                    direction = "right";
                    UpdatePlayerDirection(direction);
                }
                if (!IsCollidable(currentTileX + 1, currentTileY, false) && IsNPCCollide(currentTileX + 1, currentTileY - 1)[0] == -99999)
                {
                    SetMoving(true);
                    distanceToTravel = 16;
                }

                Console.WriteLine(currentTileX + " " + currentTileY);//Display current tile position DEBUG CODE
            }

            //Interact Button (to interact with objects and NPCs)
            if (currentKeyboardState.IsKeyDown(Keys.E) && gameState != GameState.dialogue && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                if (IsInteractable(direction, currentTileX, currentTileY))
                {
                    GetDialogue(direction, currentTileX, currentTileY, false);
                    previousGameTime = DateTime.Now.Ticks;
                    gameState = GameState.dialogue;
                    currentPage = 5;
                }
                else
                {
                    switch (direction)
                    {
                        case "left":
                            npcOffset = IsNPCCollide(currentTileX - 1, currentTileY - 1);
                            if (npcOffset[0] != -99999)
                            {
                                GetDialogue(direction, currentTileX - npcOffset[0], currentTileY - npcOffset[1], true);
                                previousGameTime = DateTime.Now.Ticks;
                                gameState = GameState.dialogue;
                                currentPage = 5;
                            }
                            break;
                        case "right":
                            npcOffset = IsNPCCollide(currentTileX + 1, currentTileY - 1);
                            if (npcOffset[0] != -99999)
                            {
                                GetDialogue(direction, currentTileX - npcOffset[0], currentTileY - npcOffset[1], true);
                                previousGameTime = DateTime.Now.Ticks;
                                gameState = GameState.dialogue;
                                currentPage = 5;
                            }
                            break;
                        case "down":
                            npcOffset = IsNPCCollide(currentTileX, currentTileY);
                            if (npcOffset[0] != -99999)
                            {
                                GetDialogue(direction, currentTileX - npcOffset[0], currentTileY - npcOffset[1], true);
                                previousGameTime = DateTime.Now.Ticks;
                                gameState = GameState.dialogue;
                                currentPage = 5;
                            }
                            break;
                        case "up":
                            npcOffset = IsNPCCollide(currentTileX, currentTileY - 2);
                            if (npcOffset[0] != -99999)
                            {
                                GetDialogue(direction, currentTileX - npcOffset[0], currentTileY - npcOffset[1], true);
                                previousGameTime = DateTime.Now.Ticks;
                                gameState = GameState.dialogue;
                                currentPage = 5;
                            }
                            break;

                    }
                }

            }
            //Sprint
            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                playerMovementSpeed = 2;
                PlayerAnimationLowerSpeed = 150;
                if (!playerIsRunning)
                {
                    playerIsRunning = true;
                    UpdatePlayerDirection(direction);
                }
            }
            else if (currentKeyboardState.IsKeyUp(Keys.Space))
            {
                playerMovementSpeed = 1;
                PlayerAnimationLowerSpeed = 300;
                if (playerIsRunning)
                {
                    playerIsRunning = false;
                    UpdatePlayerDirection(direction);
                }
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

                if (distanceToTravel <= 0)
                {
                    if (playerIsFalling)
                    {
                        playerIsFalling = false;
                        UpdatePlayerDirection(direction);
                    }
                    isMoving = false;
                    if (tallGrassMapData[currentTileY][currentTileX] != 0)
                    {
                        isInTallGrass = true;
                    }
                    else if (tallGrassMapData[currentTileY][currentTileX] == 0 && isInTallGrass)
                    {
                        isInTallGrass = false;
                    }
                    //Determine if player entered battle
                    if (playerEnvironment == PlayerEnvironment.outside && isInTallGrass)
                    {
                        //Random battle no modifiers
                        if (rng.Next(1, BATTLECHANCE) + 1 == BATTLECHANCE)//Default 1,10
                        {
                            //local variables
                            string playerShinyString = "";
                            battleMenuSelection = "fight";
                            //Set the battle state

                            //Get a list of the encounters for this map id
                            encounters = pokemon.getPokemonID(mapEncountersID);
                            int encounterID = 0;
                            enemyHPRect.Width = 142;
                            enemyHP = Content.Load<Texture2D>("battle/components/hp_full");
                            //Determine encounter based on weights
                            int totalWeight = 0;
                            for (int x = 0; x < encounters.Length; x++)
                            {
                                totalWeight += encounters[x][1];
                            }
                            totalWeight = rng.Next(0, totalWeight);
                            for (int x = 0; x < encounters.Length; x++)
                            {
                                if (totalWeight < encounters[x][1])
                                {
                                    encounterID = encounters[x][0];
                                    break;
                                }
                                else totalWeight -= encounters[x][1];
                            }
                            enemyPokemonObject = pokemon.generatePokemon(encounterID.ToString(), 1);
                            playerPokemonObject = playerBox.ElementAt(currentPokemon);
                            //Determine if player's pokemon is shiny
                            if (playerPokemonObject.isShiny) playerShinyString = "_shiny";
                            if (enemyPokemonObject.isShiny) shinyString = "_shiny";
                            else shinyString = "";
                            playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                            enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                            mapPaths = maps.getMap(mapEnvironmentType, false);
                            battleBG = Content.Load<Texture2D>("battle/backgrounds/" + mapPaths[0]);
                            battlePlace = Content.Load<Texture2D>("battle/backgrounds/" + mapPaths[1]);
                            gameState = GameState.battle;
                            //Determine who goes first
                            if (playerPokemonObject.spd >= enemyPokemonObject.spd || true)//Delete boolean after testing
                            {
                                playerFirst = true;
                                battleMenuDepth = BattleMenuDepth.initial;
                            }
                            else
                            {
                                playerFirst = false;
                                battleMenuDepth = BattleMenuDepth.waiting;
                            }
                            //Set initial encounter text
                            previousGameTime = DateTime.Now.Ticks;
                            encounterText = "A wild " + enemyPokemonObject.name + " appeared!";
                            //Console.WriteLine(enemyPokemonObject.atk + " " + enemyPokemonObject.satk + " " + enemyPokemonObject.def + " " + enemyPokemonObject.sdef + " " + enemyPokemonObject.evade + " " + enemyPokemonObject.spd);
                        }
                    }
                    if (playerEnvironment == PlayerEnvironment.swimming || playerEnvironment == PlayerEnvironment.cave)
                    {
                        //Random battle cave and swim modifiers
                        if (rng.Next(BATTLEMODIFIER, BATTLECHANCE) == 10)
                        {
                            Console.WriteLine("Cave or Swim Battle");
                        }
                    }
                }
            }

            //Update the lower and upper half of the player
            playerUpper.Update(gameTime);
            playerLower.Update(gameTime);
            //Turn off the animation and set the player to default standing position
            if (!isMoving) SetMoving(false);
        }
        //Function to update the direction the player is facing and select the appropriate sprite strip
        public void UpdatePlayerDirection(string direction)
        {
            if (!playerIsFalling && !playerIsRunning) PlayerTexture = Content.Load<Texture2D>("sprites/player/" + playerUpper.gender + "/walking_" + direction);
            if (playerIsFalling) PlayerTexture = Content.Load<Texture2D>("sprites/player/" + playerUpper.gender + "/falling_" + direction);
            if (playerIsRunning && !playerIsFalling) PlayerTexture = Content.Load<Texture2D>("sprites/player/" + playerUpper.gender + "/running_" + direction);
            PlayerAnimationLower.Initialize(PlayerTexture, new Vector2(0, 16), 32, 16, 16, 2, PlayerAnimationLowerSpeed, Color.White, 1.0f, true, false);
            PlayerAnimationUpper.Initialize(PlayerTexture, Vector2.Zero, 32, 16, 0, 2, PlayerAnimationLowerSpeed, Color.White, 1.0f, true, false);
        }
        //Function to randomly move npcs
        public void MoveNPC()
        {
            Random rng = new Random();
            long timestamp = DateTime.Now.Ticks;
            int direction;

            for (int x = 0; x < npcs.Count; x++)
            {
                if (timestamp > npcs.ElementAt(x).lastMoveTime + npcs.ElementAt(x).mfreq * 100000 && npcs.ElementAt(x).moving == false && npcs.ElementAt(x).mfreq != -1)
                {
                    direction = rng.Next(1, 16);//Default 1, 16

                    switch (direction)
                    {

                        case 1://Up
                            if (!IsCollidable((int)npcs.ElementAt(x).defaultLocation[0], (int)npcs.ElementAt(x).defaultLocation[1], true))
                            {
                                npcs.ElementAt(x).moving = true;
                                npcs.ElementAt(x).distanceToTravel = 1;
                                npcs.ElementAt(x).changeFromDefaultLocation[1]--;
                                npcs.ElementAt(x).direction = "up";
                                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathUp);
                            }
                            break;
                        case 2://Down
                            if (!IsCollidable((int)npcs.ElementAt(x).defaultLocation[0], (int)npcs.ElementAt(x).defaultLocation[1] + 2, true))
                            {
                                npcs.ElementAt(x).moving = true;
                                npcs.ElementAt(x).distanceToTravel = 1;
                                npcs.ElementAt(x).changeFromDefaultLocation[1]++;
                                npcs.ElementAt(x).direction = "down";
                                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathDown);
                            }
                            break;
                        case 3://Left
                            if (!IsCollidable((int)npcs.ElementAt(x).defaultLocation[0] - 1, (int)npcs.ElementAt(x).defaultLocation[1] + 1, true))
                            {
                                npcs.ElementAt(x).moving = true;
                                npcs.ElementAt(x).distanceToTravel = 1;
                                npcs.ElementAt(x).changeFromDefaultLocation[0]--;
                                npcs.ElementAt(x).direction = "left";
                                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathLeft);
                            }
                            break;
                        case 4://Right
                            if (!IsCollidable((int)npcs.ElementAt(x).defaultLocation[0] + 1, (int)npcs.ElementAt(x).defaultLocation[1] + 1, true))
                            {
                                npcs.ElementAt(x).moving = true;
                                npcs.ElementAt(x).distanceToTravel = 1;
                                npcs.ElementAt(x).changeFromDefaultLocation[0]++;
                                npcs.ElementAt(x).direction = "right";
                                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathRight);
                            }
                            break;
                        default:
                            //No move at all 33% chance
                            break;
                    }
                    npcs.ElementAt(x).upper.Initialize(npcs.ElementAt(x).texture, Vector2.Zero, 32, 16, 16, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, npcs.ElementAt(x).moving);
                    npcs.ElementAt(x).lower.Initialize(npcs.ElementAt(x).texture, new Vector2(100, 116), 32, 16, 0, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, npcs.ElementAt(x).moving);
                    npcs.ElementAt(x).lastMoveTime = DateTime.Now.Ticks;
                }
                else if (npcs.ElementAt(x).moving)
                {
                    switch (npcs.ElementAt(x).direction)
                    {
                        case "up":
                            npcs.ElementAt(x).defaultLocation[1] -= 0.02m;
                            npcs.ElementAt(x).distanceToTravel -= 0.02m;
                            break;
                        case "down":
                            npcs.ElementAt(x).defaultLocation[1] += 0.02m;
                            npcs.ElementAt(x).distanceToTravel -= 0.02m;
                            break;
                        case "left":
                            npcs.ElementAt(x).defaultLocation[0] -= 0.02m;
                            npcs.ElementAt(x).distanceToTravel -= 0.02m;
                            break;
                        case "right":
                            npcs.ElementAt(x).defaultLocation[0] += 0.02m;
                            npcs.ElementAt(x).distanceToTravel -= 0.02m;
                            break;
                    }
                    if (npcs.ElementAt(x).distanceToTravel <= 0) npcs.ElementAt(x).moving = false;
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //Gamestate splash
            if (gameState == GameState.splash)
            {
                spriteBatch.Draw(SplashScreen, SplashRect, Color.White);
            }
            if (gameState == GameState.battle) drawBattle(gameTime);
            if (gameState == GameState.ready || gameState == GameState.dialogue)
            {
                drawGame();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Check if the tile the player is facing is a collidable object

        public bool IsCollidable(int x, int y, bool isNPC)
        {
            if (direction == "down" && invisibleCollisionMapData[y][x] == 23843 && !isNPC)
            {
                playerIsFalling = true;
                UpdatePlayerDirection(direction);
                return false;
            }
            else if (invisibleCollisionMapData[y][x] != 0) return true;
            else return false;
        }

        //Check if the tile the player is facing is a collidable NPC
        public int[] IsNPCCollide(int x, int y)
        {
            for (int z = 0; z < npcs.Count; z++)
            {
                if (x == npcs.ElementAt(z).defaultLocation[0] && y == npcs.ElementAt(z).defaultLocation[1])
                {
                    return npcs.ElementAt(z).changeFromDefaultLocation;
                }
            }
            return new int[2] { -99999, -99999 };
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

        //Gets the event ID for the interaction and changes the npc's direction to face the player and initialized the dialogue
        public void GetDialogue(string direction, int x, int y, bool isNPC)
        {
            int[] tile = new int[2];
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
                    if (isNPC) tile[0] = x - 1;
                    break;
                case "right":
                    tile[0] = x + 1;
                    tile[1] = y;
                    break;
                default:
                    //Shouldn't happen
                    break;
            }
            dialogue = new Dialogue();
            if (!isNPC) dialogue.initialize(tile[0] + "" + tile[1] + "" + mapID);
            else
            {
                dialogue.initialize(tile[0] + "" + tile[1] + "" + mapID + "N");
                for (int z = 0; z < npcs.Count; z++)
                {
                    if (tile[0] + "" + tile[1] + "" + mapID + "N" == npcs.ElementAt(z).dialogueID)
                    {
                        switch (direction)
                        {
                            case "left":
                                npcs.ElementAt(z).direction = "right";
                                npcs.ElementAt(z).texture = Content.Load<Texture2D>(npcs.ElementAt(z).spritePathRight);
                                break;
                            case "right":
                                npcs.ElementAt(z).direction = "left";
                                npcs.ElementAt(z).texture = Content.Load<Texture2D>(npcs.ElementAt(z).spritePathLeft);
                                break;
                            case "up":
                                npcs.ElementAt(z).direction = "down";
                                npcs.ElementAt(z).texture = Content.Load<Texture2D>(npcs.ElementAt(z).spritePathDown);
                                break;
                            case "down":
                                npcs.ElementAt(z).direction = "up";
                                npcs.ElementAt(z).texture = Content.Load<Texture2D>(npcs.ElementAt(z).spritePathUp);
                                break;
                        }
                        npcs.ElementAt(z).upper.Initialize(npcs.ElementAt(z).texture, Vector2.Zero, 32, 16, 16, 2, npcs.ElementAt(z).speed, Color.White, 1.0f, true, false);
                        npcs.ElementAt(z).lower.Initialize(npcs.ElementAt(z).texture, new Vector2(100, 116), 32, 16, 0, 2, npcs.ElementAt(z).speed, Color.White, 1.0f, true, false);
                    }
                }
            }
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
            for (int x = 0; x < npcDataList.Count; x++)
            {
                npcID.Add(npcDataList[x].Attributes["id"].Value);
            }
        }

        //Function to initialize npcs
        public void InitializeNPC()
        {
            for (int x = 0; x < npcID.Count; x++)
            {
                npcs.Add(new npc());
                npcs.ElementAt(x).Initialize(npcID.ElementAt(x));
                npcs.ElementAt(x).texture = Content.Load<Texture2D>(npcs.ElementAt(x).spritePathDown);
                npcs.ElementAt(x).upper.Initialize(npcs.ElementAt(x).texture, Vector2.Zero, 32, 16, 16, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, false);
                npcs.ElementAt(x).lower.Initialize(npcs.ElementAt(x).texture, new Vector2(100, 116), 32, 16, 0, 2, npcs.ElementAt(x).speed, Color.White, 1.0f, true, false);
            }
        }

        //Process the Run Away command
        public void processRun()
        {
            if (playerPokemonObject.spd >= enemyPokemonObject.spd)
            {
                int run = rng.Next(1, 100) + 1;
                if (run > 10)
                {
                    encounterText = playerPokemonObject.name + " ran away successfully!";
                    savePokemon();
                    battleState = BattleState.runSucceed;
                }
                else
                {
                    encounterText = playerPokemonObject.name + " failed to run away!";
                    battleState = BattleState.runFail;
                }
            }
            else
            {
                int run = rng.Next(1, 100) + 1;
                if (run < (100 - (enemyPokemonObject.spd - playerPokemonObject.spd - 1) * 10))
                {
                    encounterText = playerPokemonObject.name + " ran away successfully!";
                    savePokemon();
                    battleState = BattleState.runSucceed;
                }
                else
                {
                    encounterText = playerPokemonObject.name + " failed to run away!";
                    battleState = BattleState.runFail;
                }
            }
        }
        //Process to save the current pokemon in the correct "bag" slot
        //Should be used when pokemon faints, is changed out, or the battle ends
        public void savePokemon()
        {
            //Save current pokemon
            playerBox.RemoveAt(currentPokemon);
            playerBox.Insert(currentPokemon, playerPokemonObject);
        }

        //Process move command from battle
        public void processMoveSelection(string move, GameTime gameTime)
        {
            moveToUse = new Pokemon.moves();
            switch (move)
            {
                case "move1":
                    moveToUse = playerPokemonObject.move1;
                    break;
                case "move2":
                    moveToUse = playerPokemonObject.move2;
                    break;
                case "move3":
                    moveToUse = playerPokemonObject.move3;
                    break;
                case "move4":
                    moveToUse = playerPokemonObject.move4;
                    break;
                default:
                    //Should not happen
                    break;
            }
            if (!didHit()) Console.WriteLine("Missed!");
            else
            {
                playerMoveResult();
            }
            applyPlayerConditions();
            applyEnemyConditions();
            //PROCESS ADDITIONAL PLAYER COMBAT ACTIONS HERE


            reverseAnimation = false;
            playerAnimationComplete = false;
            previousBattleState = battleState;
            animationTime = DateTime.Now.Ticks;
            battleState = BattleState.displayingResult;
            previousGameTime = DateTime.Now.Ticks;
            didDisplayEffectiveness = false;
        }

        //Process enemy move commands here
        public void processEnemyMoves()
        {
            int numMoves = 4;
            selectedEnemyMove = pokemon.NONE;
            if(enemyPokemonObject.move4.name == "NONE")
            {
                numMoves = 3;
                if(enemyPokemonObject.move3.name == "NONE")
                {
                    numMoves = 2;
                    if(enemyPokemonObject.move2.name == "NONE")
                    {
                        numMoves = 1;
                        if(enemyPokemonObject.move1.name == "NONE")
                        {
                        }
                    }
                }
            }
            switch (rng.Next(0, numMoves) + 1)
            {
                case 1:
                    selectedEnemyMove = enemyPokemonObject.move1;
                    break;
                case 2:
                    selectedEnemyMove = enemyPokemonObject.move2;
                    break;
                case 3:
                    selectedEnemyMove = enemyPokemonObject.move3;
                    break;
                case 4:
                    selectedEnemyMove = enemyPokemonObject.move4;
                    break;
                default:
                    //What other move slots are there???
                    break;
            }

            //TEST CODE
            type = -1;
            int atkDefResult = 0;
            double damage = 0;
            switch (selectedEnemyMove.type)
            {
                case "Normal":
                    type = 0;
                    break;
                case "Flying":
                    type = 1;
                    break;
                case "Fighting":
                    type = 2;
                    break;
            }

            switch (selectedEnemyMove.effect)
            {
                case 1:
                    atkDefResult = enemyPokemonObject.atk - playerPokemonObject.def;
                    if (atkDefResult <= 1) atkDefResult = 1;//Landed hits do at least 1 damage
                    damage = (enemyPokemonObject.attackAffinity[type] / playerPokemonObject.defenseAffinity[type]) * atkDefResult * selectedEnemyMove.magnitude;
                    break;
            }

            applyAfinity(type, 1);
            netDamage = damage;
            playerPokemonObject.HP -= (int)netDamage;
            playerHPWidth = (int)(142 * ((double)playerPokemonObject.HP / (double)playerPokemonObject.MaxHP));
            Console.WriteLine("Enemy Damage: " + netDamage);
            //END
            reverseAnimation = false;
            enemyAnimationComplete = false;
            previousBattleState = battleState;
            animationTime = DateTime.Now.Ticks;
            battleState = BattleState.displayingResult;
            previousGameTime = DateTime.Now.Ticks;
            didDisplayEffectiveness = false;
            battleMenuDepth = BattleMenuDepth.waiting;
            previousGameTime = DateTime.Now.Ticks;

        }

        //Determine if the player hit
        public bool didHit()
        {
            switch((rng.Next(0, 100) - enemyPokemonObject.evade + playerPokemonObject.evade) > 5)
            {
                case true:
                    return true;
            }
            return false;
        }

        //Determine move result to enemy
        public void playerMoveResult()
        {
            type = -1;
            int atkDefResult = 0;
            double damage = 0;
            switch (moveToUse.type)
            {
                case "Normal":
                    type = 0;
                    break;
                case "Flying":
                    type = 1;
                    break;
                case "Fighting":
                    type = 2;
                    break;
            }

            switch (moveToUse.effect) {
                case 1:
                    atkDefResult = playerPokemonObject.atk - enemyPokemonObject.def;
                    if (atkDefResult <= 1) atkDefResult = 1;//Landed hits do at least 1 damage
                    damage = (playerPokemonObject.attackAffinity[type] / enemyPokemonObject.defenseAffinity[type]) * atkDefResult * moveToUse.magnitude;
                    break;
            }

            applyAfinity(type, 1);
            netDamage = damage;
            enemyPokemonObject.HP -= (int)netDamage;
            enemyHPWidth = (int)(142 * ((double)enemyPokemonObject.HP / (double)enemyPokemonObject.MaxHP));
            Console.WriteLine("Player Damage: " + netDamage);
        }

        //Apply player conditions
        public void applyPlayerConditions()
        {
            //Conditional damages here
        }

        //Apply enemy conditions
        public void applyEnemyConditions()
        {
            //Conditional damages here
        }

        //Apply Afinity
        public void applyAfinity(int type, int afininity)
        {
            //1 = attack
            //2 = defense
            //3 = trainer
            try
            {
                //Apply afininities
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        //Drawing functions below avoid writing new functions not draw or animation related
        public void drawBattle(GameTime gameTime)
        {
            spriteBatch.Draw(battleBG, battleBGRect, Color.White);
            spriteBatch.Draw(battlePlace, battlePlaceRect, Color.White);
            spriteBatch.Draw(enemyPokemon, enemyPokemonRect, Color.White);
            spriteBatch.Draw(playerPokemon, playerPokemonRect, Color.White);
            spriteBatch.Draw(hpBack, enemyHPBackRect, Color.White);
            spriteBatch.Draw(enemyHP, enemyHPRect, Color.White);
            spriteBatch.Draw(enemyHPPanel, enemyHPPanelRect, Color.White);
            spriteBatch.DrawString(font6, enemyPokemonObject.name, enemyNamePos, Color.White);
            spriteBatch.Draw(hpBack, playerHPBackRect, Color.White);
            spriteBatch.Draw(playerHP, playerHPRect, Color.White);
            spriteBatch.Draw(playerHPPanel, playerHPPanelRect, Color.White);
            spriteBatch.DrawString(font6, playerPokemonObject.name, playerNamePos, Color.White);
            spriteBatch.DrawString(font6, "HP: " + playerPokemonObject.HP + "/" + playerPokemonObject.MaxHP, playerHPNumbers, Color.White);
            //spriteBatch.DrawString(font6, "HP: " + enemyPokemonObject.HP + "/" + enemyPokemonObject.MaxHP, new Vector2(0, 0), Color.White);//Enable to test enemy hp
            spriteBatch.Draw(DialogueTexture, DialogueRectangle, Color.White);//Dialogue

            if (battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                spriteBatch.Draw(battleMenu0, battleMenu0Rect, Color.White);
                spriteBatch.Draw(menuSelector, menuSelectorRect, Color.White);
            }
            if (battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.fight)
            {
                spriteBatch.Draw(battleMenuMoves, battleMenuMovesRect, Color.White);
                if (playerPokemonObject.move1.name != "NONE")
                {
                    spriteBatch.DrawString(font, playerPokemonObject.move1.name, move1pos, Color.Black);
                    spriteBatch.DrawString(font, "PP: " + playerPokemonObject.PP_move1_current + "/" + playerPokemonObject.PP_move1, new Vector2(move1pos.X + 40, move1pos.Y + 15), Color.Black);
                }
                if (playerPokemonObject.move2.name != "NONE")
                {
                    spriteBatch.DrawString(font, playerPokemonObject.move2.name, move2pos, Color.Black);
                    spriteBatch.DrawString(font, "PP: " + playerPokemonObject.PP_move2_current + "/" + playerPokemonObject.PP_move2, new Vector2(move2pos.X + 40, move2pos.Y + 15), Color.Black);
                }
                if (playerPokemonObject.move3.name != "NONE")
                {
                    spriteBatch.DrawString(font, playerPokemonObject.move3.name, move3pos, Color.Black);
                    spriteBatch.DrawString(font, "PP: " + playerPokemonObject.PP_move3_current + "/" + playerPokemonObject.PP_move3, new Vector2(move3pos.X + 40, move3pos.Y + 15), Color.Black);
                }
                if (playerPokemonObject.move4.name != "NONE")
                {
                    spriteBatch.DrawString(font, playerPokemonObject.move4.name, move4pos, Color.Black);
                    spriteBatch.DrawString(font, "PP: " + playerPokemonObject.PP_move4_current + "/" + playerPokemonObject.PP_move4, new Vector2(move4pos.X + 40, move4pos.Y + 15), Color.Black);
                }
                spriteBatch.Draw(menuSelectorMoves, menuSelectorMovesRect, Color.White);
            }
            if (battleState == BattleState.begin || battleState == BattleState.runFail || battleState == BattleState.runSucceed) spriteBatch.DrawString(font, encounterText, new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
            if (battleState == BattleState.displayingResult && previousBattleState == BattleState.playerTurn) drawMoveResult(playerPokemonObject.name + " uses " + moveToUse.name + " on enemy " + enemyPokemonObject.name + "!");
            if (battleState == BattleState.displayingResult && previousBattleState == BattleState.enemyTurn) drawMoveResult("Enemy " + enemyPokemonObject.name + " used " + selectedEnemyMove.name + " on " + playerPokemonObject.name + "!");
            if (battleState == BattleState.animatingPlayer) animatePlayer(moveToUse.name, gameTime);
            if (battleState == BattleState.animatingEnemy) animateEnemy(selectedEnemyMove.name, gameTime);
        }

        //Draw main game
        public void drawGame()
        {
            spriteBatch.Draw(CurrentMapBase, CurrentMapBaseRect, Color.White);//Layer 0
            playerLower.Draw(spriteBatch);//Player Lower Half

            for (int x = 0; x < npcs.Count; x++)
            {
                npcs.ElementAt(x).upper.Draw(spriteBatch);
            }

            spriteBatch.Draw(CurrentMapOver, CurrentMapBaseRect, Color.White);//Layer 3

            for (int x = 0; x < npcs.Count; x++)
            {
                npcs.ElementAt(x).lower.Draw(spriteBatch);
            }

            playerUpper.Draw(spriteBatch);//Player Upper Half
            spriteBatch.Draw(CurrentMapTop, CurrentMapBaseRect, Color.White);//Layer 5
            if (gameState == GameState.dialogue) drawDialogue();
        }
       
        //Draw dialogue
        public void drawDialogue()
        {
            spriteBatch.Draw(DialogueTexture, DialogueRectangle, Color.White);//Dialogue
            if (currentPage - 5 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 5], new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
            if (currentPage - 4 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 4], new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
            if (currentPage - 3 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 3], new Vector2(dialogueX + 15, dialogueY + 41), Color.Black);
            if (currentPage - 2 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 2], new Vector2(dialogueX + 15, dialogueY + 57), Color.Black);
            if (currentPage - 1 < dialogueContent.Count) spriteBatch.DrawString(font, dialogueContent[currentPage - 1], new Vector2(dialogueX + 15, dialogueY + 73), Color.Black);
        }

        //Draw battle results
        public void drawMoveResult(string result)
        {
            spriteBatch.DrawString(font, result, new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);

                if(previousBattleState == BattleState.playerTurn)
                {
                    if(playerPokemonObject.attackAffinity[type] / enemyPokemonObject.defenseAffinity[type] >= 1.5)
                    {
                        spriteBatch.DrawString(font, "It was super effective!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
                        if(!didDisplayEffectiveness)previousGameTime = DateTime.Now.Ticks;
                    }
                    else if(playerPokemonObject.attackAffinity[type] / enemyPokemonObject.defenseAffinity[type] <= 0.5)
                    {
                        spriteBatch.DrawString(font, "It was not very effective!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                didDisplayEffectiveness = true;
                }
                if(previousBattleState == BattleState.enemyTurn)
                {
                    if (enemyPokemonObject.attackAffinity[type] / playerPokemonObject.defenseAffinity[type] >= 1.5)
                    {
                        spriteBatch.DrawString(font, "It was super effective!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                    else if (enemyPokemonObject.attackAffinity[type] / playerPokemonObject.defenseAffinity[type] <= 0.5)
                    {
                        spriteBatch.DrawString(font, "It was not very effective!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                    didDisplayEffectiveness = true;
                }
                if(previousBattleState == BattleState.playerTurn && didDisplayEffectiveness && DateTime.Now.Ticks > previousGameTime + KeyboardDelay * BATTLESPEED)
                {
                    battleState = BattleState.animatingPlayer;
                }
                else if(didDisplayEffectiveness && DateTime.Now.Ticks > previousGameTime + KeyboardDelay * BATTLESPEED)
                {
                    battleState = BattleState.animatingEnemy;
                    battleMenuDepth = BattleMenuDepth.initial;
                }
            
        }

        //Animation functions below avoid writing new functions not draw or animation related
        //player pokemon animations
        public void animatePlayer(string move, GameTime gameTime)
        {
            
            switch (move)
            {
                case "Tackle":
                    if (animationCycle < 10 && DateTime.Now.Ticks > animationTime + animationDelay && !reverseAnimation)
                    {
                        playerPokemonRect.X += 2;
                        playerPokemonRect.Y -= 2;
                        animationCycle++;
                        animationTime = DateTime.Now.Ticks;
                    }
                    else if (animationCycle > 0 && DateTime.Now.Ticks > animationTime + animationDelay && reverseAnimation)
                    {
                        if (enemyPokemon.Name == "sprites/pokemon/front" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix) enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front_inv/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                        else enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                        playerPokemonRect.X -= 2;
                        playerPokemonRect.Y += 2;
                        animationCycle--;
                        animationTime = DateTime.Now.Ticks;
                    }
                    if (animationCycle == 10 && !reverseAnimation)
                    {
                        reverseAnimation = true;
                    }
                    else if (animationCycle == 0 && reverseAnimation)
                    {
                        playerAnimationComplete = true;
                        battleState = BattleState.enemyTurn;
                    }
                    break;
                default:
                    battleAnimation(moveToUse.path, true);
                    break;
            }
            if (enemyHPWidth > 71) enemyHP = Content.Load<Texture2D>("battle/components/hp_full");
            else if (enemyHPWidth <= 71 && enemyHPWidth > 21) enemyHP = Content.Load<Texture2D>("battle/components/hp_half");
            else if (enemyHPWidth <= 21) enemyHP = Content.Load<Texture2D>("battle/components/hp_critical");
            enemyHPRect.Width = enemyHPWidth;
        }
        //enemy pokemon animations
        public void animateEnemy(string move, GameTime gameTime)
        {

            switch (move)
            {
                case "Tackle":
                    if (animationCycle < 10 && DateTime.Now.Ticks > animationTime + animationDelay && !reverseAnimation)
                    {
                        enemyPokemonRect.X -= 2;
                        enemyPokemonRect.Y += 2;
                        animationCycle++;
                        animationTime = DateTime.Now.Ticks;
                    }
                    else if (animationCycle > 0 && DateTime.Now.Ticks > animationTime + animationDelay && reverseAnimation)
                    {
                        if (playerPokemon.Name == "sprites/pokemon/back" + shinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix) playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back_inv/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                        else playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + shinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                        enemyPokemonRect.X += 2;
                        enemyPokemonRect.Y -= 2;
                        animationCycle--;
                        animationTime = DateTime.Now.Ticks;
                    }
                    if (animationCycle == 10 && !reverseAnimation)
                    {
                        reverseAnimation = true;
                    }
                    else if (animationCycle == 0 && reverseAnimation)
                    {
                        enemyAnimationComplete = true;
                        battleState = BattleState.playerTurn;
                    }
                    break;
                default:
                    battleAnimation(selectedEnemyMove.path, false);
                    break;
            }
            if (playerHPWidth > 71) playerHP = Content.Load<Texture2D>("battle/components/hp_full");
            else if (playerHPWidth <= 71 && playerHPWidth > 21) playerHP = Content.Load<Texture2D>("battle/components/hp_half");
            else if (playerHPWidth <= 21) playerHP = Content.Load<Texture2D>("battle/components/hp_critical");
            playerHPRect.Width = playerHPWidth;
        }

        //animation frames
        public void battleAnimation(string move, bool onEnemy)
        {
            if (!battleAnimationStarted)
            {
                battleAnimationTexture = Content.Load<Texture2D>("battle/animations/" + move);
                xframes = battleAnimationTexture.Width / 192;
                yframes = battleAnimationTexture.Height / 192;
                battleAnimationStarted = true;
            }

            if (DateTime.Now.Ticks > previousGameTime + animationDelay * ANIMATIONFRAMETIME)
            {
                battleAnimationX++;
                if (battleAnimationX > xframes - 1 && !battleAnimationFinisher)
                {
                    battleAnimationY++;
                    battleAnimationX = 0;

                }
                if (battleAnimationY > yframes - 1 && !battleAnimationFinisher)
                {
                    battleAnimationFinisher = true;
                    battleAnimationX = 0;
                    battleAnimationY = 0;
                }
                if (battleAnimationFinisher && finishFrames > 0)
                {
                    if (onEnemy)
                    {
                        if (enemyPokemon.Name == "sprites/pokemon/front" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix) enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front_inv/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                        else enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                    }
                    else
                    {
                        if (playerPokemon.Name == "sprites/pokemon/back" + shinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix) playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back_inv/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                        else playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + shinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                    }
                    finishFrames--;
                }
                if(finishFrames == 0 && battleAnimationFinisher)
                {
                    if (onEnemy)
                    {
                        enemyAnimationComplete = true;
                        battleAnimationStarted = false;
                        battleState = BattleState.enemyTurn;
                    }
                    else
                    {
                        playerAnimationComplete = true;
                        battleAnimationStarted = false;
                        battleState = BattleState.playerTurn;
                    }
                    battleAnimationFinisher = false;
                    battleAnimationX = 0;
                    battleAnimationY = 0;
                    finishFrames = 6;
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            animationFrameSourceRect.X = 192 * battleAnimationX;
            animationFrameSourceRect.Y = 192 * battleAnimationY;
            if(battleState != BattleState.enemyTurn && !playerAnimationComplete && !battleAnimationFinisher && onEnemy)spriteBatch.Draw(Content.Load<Texture2D>("battle/animations/" + move), enemyBattleAnimationRect, animationFrameSourceRect, Color.White);
            else if(battleState != BattleState.playerTurn && !enemyAnimationComplete && !battleAnimationFinisher && !onEnemy) spriteBatch.Draw(Content.Load<Texture2D>("battle/animations/" + move), playerBattleAnimationRect, animationFrameSourceRect, Color.White);
        }
    }
}
