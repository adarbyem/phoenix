using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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

        //Title Globals
        Song titleBGM;

        //Menu Globals
        Texture2D menuSelector;
        Rectangle menuSelectorRect;
        Texture2D menuSelectorMoves;
        Rectangle menuSelectorMovesRect;

        //Battle Globals
        LevelUp levelup;
        Combat combat;
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
        Texture2D enemyBattleStatus;
        Rectangle enemyBattleStatusRect;
        Texture2D playerBattleStatus;
        Rectangle playerBattleStatusRect;
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
        Texture2D enemyPokemonGender;
        Texture2D playerPokemonGender;
        Rectangle enemyPokemonGenderRect;
        Rectangle playerPokemonGenderRect;
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
        string battleConditionEffectString;
        bool paralyzeSkip = false;
        string failedSkill = "";
        bool cureCondition = false;
        int netAffinityGain = 0;
        bool didLevelUp = false;
        int[] enemyStatMod = new int[7];
        int[] playerStatMod = new int[7];
        string additionalStatString = "";
        int tempPlayerPokemonHP = 0;

        //Pokemon Globals
        Pokemon pokemon;
        string shinyString;
        string femaleString;
        string playerShinyString = "";
        string playerFemaleString = "";

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
        string leveledStat = "";

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
            playerConditions,
            enemyConditions,
            enemyTurn,
            runSucceed,
            runFail,
            displayingResult,
            animatingPlayer,
            animatingEnemy,
            enemyFaint,
            playerFaint
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
            enemyBattleStatusRect = new Rectangle(357, 76, 44, 16);
            playerBattleStatusRect = new Rectangle(610, 232, 44, 16);
            enemyPokemonGenderRect = new Rectangle(204, 91, 15, 18);
            playerPokemonGenderRect = new Rectangle(460, 246, 15, 18);
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
            titleBGM = Content.Load<Song>("audio/bgm/title");
            levelup = new LevelUp();
            combat = new Combat();
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
            playerBox.Add(pokemon.generatePokemon("7"));//INITIAL PLAYER POKEMON
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
            MediaPlayer.Play(titleBGM);

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
                playerPokemonObject.attackAffinity[0] = 10000;
                Console.WriteLine("Player: " + playerPokemonObject.atk + " " + playerPokemonObject.satk + " " + playerPokemonObject.def + " " + playerPokemonObject.sdef + " " + playerPokemonObject.evade + " " + playerPokemonObject.spd + " " + playerPokemonObject.MaxHP + " ");
                Console.WriteLine("Enemy: " + enemyPokemonObject.atk + " " + enemyPokemonObject.satk + " " + enemyPokemonObject.def + " " + enemyPokemonObject.sdef + " " + enemyPokemonObject.evade + " " + enemyPokemonObject.spd + " " + enemyPokemonObject.MaxHP + " ");
            }
            /*
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                enemyPokemonGenderRect.Y--;
                Console.WriteLine(enemyPokemonGenderRect.X + " " + enemyPokemonGenderRect.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                enemyPokemonGenderRect.Y++;
                Console.WriteLine(enemyPokemonGenderRect.X + " " + enemyPokemonGenderRect.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                enemyPokemonGenderRect.X--;
                Console.WriteLine(enemyPokemonGenderRect.X + " " + enemyPokemonGenderRect.Y);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                enemyPokemonGenderRect.X++;
                Console.WriteLine(enemyPokemonGenderRect.X + " " + enemyPokemonGenderRect.Y);
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
                netAffinityGain = 0;
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
            if ((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S)) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move3" && selectedMove != "move4" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
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
            if ((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W)) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move1" && selectedMove != "move2" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
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
            if ((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A)) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move1" && selectedMove != "move3" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
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
            if ((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D)) && battleMenuDepth == BattleMenuDepth.fight && selectedMove != "move2" && selectedMove != "move4" && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
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
            if (currentKeyboardState.IsKeyDown(Keys.E) && battleMenuDepth == BattleMenuDepth.fight && battleState != BattleState.enemyFaint && DateTime.Now.Ticks > previousGameTime + KeyboardDelay)
            {
                battleMenuDepth = BattleMenuDepth.waiting;
                processMoveSelection(selectedMove, gameTime);
                previousGameTime = DateTime.Now.Ticks;
            }
            if (battleState == BattleState.enemyTurn)
            {
                battleMenuDepth = BattleMenuDepth.waiting;
                processEnemyMoves();
            }
            //Movement keys for initial battle menu
            if ((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S)) && battleMenuSelection != "item" && battleMenuSelection != "run" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.Y += 41;
                if (battleMenuSelection == "fight") battleMenuSelection = "item";
                else battleMenuSelection = "run";
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W)) && battleMenuSelection != "fight" && battleMenuSelection != "pokemon" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.Y -= 41;
                if (battleMenuSelection == "item") battleMenuSelection = "fight";
                else battleMenuSelection = "pokemon";
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A)) && battleMenuSelection != "fight" && battleMenuSelection != "item" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.X -= 105;
                if (battleMenuSelection == "pokemon") battleMenuSelection = "fight";
                else battleMenuSelection = "item";
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D)) && battleMenuSelection != "pokemon" && battleMenuSelection != "run" && battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial)
            {
                menuSelectorRect.X += 105;
                if (battleMenuSelection == "fight") battleMenuSelection = "pokemon";
                else battleMenuSelection = "run";
            }
            if(battleState == BattleState.enemyFaint && currentKeyboardState.IsKeyDown(Keys.E) && DateTime.Now.Ticks > previousGameTime + KeyboardDelay && enemyPokemonRect.Height <= 0)
            {
                //Trigger logic for trainer battles here


                //End the battle below
                savePokemon();
                netAffinityGain = 0;
                gameState = GameState.ready;
                battleMenuDepth = BattleMenuDepth.initial;
                battleState = BattleState.begin;
                battleMenuSelection = "fight";
                menuSelectorRect.X = 110;
                menuSelectorRect.Y = 274;
                enemyPokemonRect.Height = 192;
                enemyPokemonRect.Y = 45;
                didLevelUp = false;
                previousGameTime = DateTime.Now.Ticks;
            }
            if (battleState == BattleState.playerFaint && currentKeyboardState.IsKeyDown(Keys.E) && DateTime.Now.Ticks > previousGameTime + KeyboardDelay && playerPokemonRect.Height <= 0)
            {
                //Trigger logic for selecting next pokemon here

                //Test code to fully heal pokemon until the pokemon switch code is operational
                playerPokemonObject.fullHeal();

                //End the battle
                savePokemon();
                netAffinityGain = 0;
                gameState = GameState.ready;
                battleMenuDepth = BattleMenuDepth.initial;
                battleState = BattleState.begin;
                battleMenuSelection = "fight";
                menuSelectorRect.X = 110;
                menuSelectorRect.Y = 274;
                playerPokemonRect.Height = 288;
                playerPokemonRect.Y = 140;
                didLevelUp = false;
                previousGameTime = DateTime.Now.Ticks;
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
                MediaPlayer.Stop();
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
                            enemyPokemonObject = pokemon.generatePokemon(encounterID.ToString());
                            playerPokemonObject = playerBox.ElementAt(currentPokemon);
                            //Determine if player's pokemon is shiny
                            shinyString = "";
                            femaleString = "";
                            playerFemaleString = "";
                            playerShinyString = "";
                            if (playerPokemonObject.isShiny) playerShinyString = "_shiny";
                            if (enemyPokemonObject.isShiny) shinyString = "_shiny";
                            if (playerPokemonObject.isFemale && playerPokemonObject.hasFemaleSprite) playerFemaleString = "_female";
                            if (enemyPokemonObject.isFemale && enemyPokemonObject.hasFemaleSprite) femaleString = "_female";
                            playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerFemaleString + "" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                            enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + femaleString + "" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                            mapPaths = maps.getMap(mapEnvironmentType, false);
                            battleBG = Content.Load<Texture2D>("battle/backgrounds/" + mapPaths[0]);
                            battlePlace = Content.Load<Texture2D>("battle/backgrounds/" + mapPaths[1]);
                            gameState = GameState.battle;
                            //Reset stat mods
                            playerStatMod = new int[7];
                            enemyStatMod = new int[7];
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

            if(previousBattleState != BattleState.playerConditions)
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
            }

            if (playerPokemonObject.status != "normal" && previousBattleState != BattleState.playerConditions)
            {
                previousBattleState = battleState;
                applyPlayerConditions();
                setConditionResult();
            }
            else if ((playerPokemonObject.status != "normal" && !paralyzeSkip) || playerPokemonObject.status == "normal")
            {
                if (!didHit(true))
                {
                    failedSkill = moveToUse.name;
                    moveToUse = pokemon.blank;
                    type = 0;
                }
                else
                {
                    playerMoveResult();
                }
                battleState = BattleState.playerTurn;
                reverseAnimation = false;
                playerAnimationComplete = false;
                animationTime = DateTime.Now.Ticks;
                previousGameTime = DateTime.Now.Ticks;
                didDisplayEffectiveness = false;
                previousBattleState = battleState;
                battleState = BattleState.displayingResult;
            }
        }

        //Process enemy move commands here
        public void processEnemyMoves()
        {
            double damage = 0;
            double mod = 0;

            additionalStatString = "";

            if (previousBattleState != BattleState.enemyConditions)
            {
                int numMoves = 4;
                selectedEnemyMove = pokemon.NONE;
                if (enemyPokemonObject.move4.name == "NONE")
                {
                    numMoves = 3;
                    if (enemyPokemonObject.move3.name == "NONE")
                    {
                        numMoves = 2;
                        if (enemyPokemonObject.move2.name == "NONE")
                        {
                            numMoves = 1;
                            if (enemyPokemonObject.move1.name == "NONE")
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
                type = -1;

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
                    case "Grass":
                        type = 3;
                        break;
                    case "Poison":
                        type = 4;
                        break;
                    case "Fire":
                        type = 5;
                        break;
                    case "Electric":
                        type = 6;
                        break;
                    case "Ice":
                        type = 7;
                        break;
                    case "Bug":
                        type = 8;
                        break;
                    case "Water":
                        type = 9;
                        break;
                    case "Ground":
                        type = 10;
                        break;
                    case "Ghost":
                        type = 11;
                        break;
                    case "Rock":
                        type = 12;
                        break;
                    case "Dark":
                        type = 13;
                        break;
                    case "Psychic":
                        type = 14;
                        break;
                    case "Fairy":
                        type = 15;
                        break;
                    case "Steel":
                        type = 16;
                        break;
                    case "Dragon":
                        type = 17;
                        break;
                }
                /*
            "Normal": 0
            "Flying": 1
            "Fighting": 2
            "Grass": 3
            "Poison": 4
            "Fire": 5
            "Electric": 6
            "Ice": 7
            "Bug": 8
            "Water": 9
            "Ground": 10
            "Ghost": 11
            "Rock": 12
            "Dark": 13
            "Psychic": 14
            "Fairy: 15
            "Steel": 16
            "Dragon: 17
           */
            }

            if (enemyPokemonObject.status != "normal" && previousBattleState != BattleState.enemyConditions)
            {
                previousBattleState = battleState;
                applyEnemyConditions();
                setConditionResult();
            }

            else if ((enemyPokemonObject.status != "normal" && !paralyzeSkip) || enemyPokemonObject.status == "normal")
            {
                if (!didHit(false))
                {
                    failedSkill = selectedEnemyMove.name;
                    selectedEnemyMove = pokemon.blank;
                    type = 0;
                }
                else
                {
                    //mods
                    //0 = Def
                    //1 = SDef
                    //2 = Atk
                    //3 = SAtk
                    //4 = Spd
                    //5 = Evade
                    //6 = Acc
                    switch (selectedEnemyMove.effect)
                    {
                        case 1:
                            damage = combat.getAtkMult(enemyPokemonObject.atk + enemyStatMod[2], playerPokemonObject.def + playerStatMod[0], enemyPokemonObject.attackAffinity[type], playerPokemonObject.defenseAffinity[type], selectedEnemyMove.magnitude);
                            break;
                        case 2:
                            damage = combat.getAtkMult(enemyPokemonObject.satk + enemyStatMod[3], playerPokemonObject.sdef + playerStatMod[1], enemyPokemonObject.attackAffinity[type], playerPokemonObject.defenseAffinity[type], selectedEnemyMove.magnitude);
                            break;
                        case 3:
                            mod = (int)Math.Ceiling(combat.getAtkMult(enemyPokemonObject.atk + enemyStatMod[3], playerPokemonObject.def + playerStatMod[1], enemyPokemonObject.attackAffinity[type], playerPokemonObject.defenseAffinity[type], selectedEnemyMove.magnitude));
                            playerStatMod[0] -= (int)mod;
                            if(mod > 20) additionalStatString = playerPokemonObject.name + "'s defense harshly fell!";
                            else additionalStatString = playerPokemonObject.name + "'s defense fell!";
                            break;
                        case 8:
                            mod = (int)Math.Ceiling(combat.getAtkMult(enemyPokemonObject.atk + enemyStatMod[3], playerPokemonObject.def + playerStatMod[1], enemyPokemonObject.attackAffinity[type], playerPokemonObject.defenseAffinity[type], selectedEnemyMove.magnitude));
                            playerStatMod[2] -= (int)mod;
                            if (mod > 20) additionalStatString = playerPokemonObject.name + "'s attack harshly fell!";
                            else additionalStatString = playerPokemonObject.name + "'s attack fell!";
                            break;
                        case 11:
                            if (playerPokemonObject.status == "normal") playerPokemonObject.status = "sleep";
                            break;
                        case 13:
                            if (playerPokemonObject.status == "normal") playerPokemonObject.status = "paralyze";
                            break;
                        case 15:
                            if (playerPokemonObject.status == "normal") playerPokemonObject.status = "poison";
                            break;
                        case 19:
                            mod = selectedEnemyMove.magnitude;
                            playerStatMod[6] -= (int)mod;
                            if (playerStatMod[6] < -90) additionalStatString = "But it had no additional effect...";
                            else additionalStatString = playerPokemonObject.name + "'s accuracy fell!";
                            break;
                    }

                    applyAfinity(type);
                    netDamage = damage;
                    Console.WriteLine("Enemy Damage Gross: " + damage);
                    if ((int)netDamage < 1) netDamage++;
                    if (selectedEnemyMove.effect != 1 && selectedEnemyMove.effect != 2) netDamage = 0;
                    if (selectedEnemyMove.magnitude == 0) netDamage = 0;
                    tempPlayerPokemonHP = playerPokemonObject.HP - (int)netDamage;
                    if (tempPlayerPokemonHP < 0) tempPlayerPokemonHP = 0;
                    if (tempPlayerPokemonHP > playerPokemonObject.MaxHP) tempPlayerPokemonHP = playerPokemonObject.MaxHP;
                    Console.WriteLine("Enemy Damage Net: " + netDamage);
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
            }
            else if(paralyzeSkip)
            {
                Console.WriteLine("Setting Skip to False");
                paralyzeSkip = false;
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
        }

        //Determine if the player hit
        public bool didHit(bool player)
        {
            additionalStatString = "";
            if (player)
            {
                if (playerStatMod[6] <= -90) playerStatMod[6] = -90;
                switch ((rng.Next(0, 100 + playerStatMod[6]) - (enemyPokemonObject.evade + enemyStatMod[5]) + (playerPokemonObject.evade + playerStatMod[5])) > 5)
                {
                    case true:
                        return true;
                }
            }
            else
            {
                if (enemyStatMod[6] <= -90) enemyStatMod[6] = -90;
                switch ((rng.Next(0, 100 + enemyStatMod[6]) - (playerPokemonObject.evade + playerStatMod[5]) + (playerPokemonObject.evade + enemyStatMod[5])) > 5)
                {
                    case true:
                        return true;
                }
            }

            return false;
        }

        //Determine move result of player selection
        public void playerMoveResult()
        {
            type = -1;
            double damage = 0;
            double mod;

            additionalStatString = "";

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
                case "Grass":
                    type = 3;
                    break;
                case "Poison":
                    type = 4;
                    break;
                case "Fire":
                    type = 5;
                    break;
                case "Electric":
                    type = 6;
                    break;
                case "Ice":
                    type = 7;
                    break;
                case "Bug":
                    type = 8;
                    break;
                case "Water":
                    type = 9;
                    break;
                case "Ground":
                    type = 10;
                    break;
                case "Ghost":
                    type = 11;
                    break;
                case "Rock":
                    type = 12;
                    break;
                case "Dark":
                    type = 13;
                    break;
                case "Psychic":
                    type = 14;
                    break;
                case "Fairy":
                    type = 15;
                    break;
                case "Steel":
                    type = 16;
                    break;
                case "Dragon":
                    type = 17;
                    break;
            }
            //mods
            //0 = Def
            //1 = SDef
            //2 = Atk
            //3 = SAtk
            //4 = Spd
            //5 = Evade
            //6 = Acc
            switch (moveToUse.effect) {
                case 1:
                    damage = combat.getAtkMult(playerPokemonObject.atk + playerStatMod[2], enemyPokemonObject.def + enemyStatMod[0], playerPokemonObject.attackAffinity[type], enemyPokemonObject.defenseAffinity[type], moveToUse.magnitude);
                    break;
                case 2:
                    damage = combat.getAtkMult(playerPokemonObject.satk + playerStatMod[3], enemyPokemonObject.sdef + enemyStatMod[1], playerPokemonObject.attackAffinity[type], enemyPokemonObject.defenseAffinity[type], moveToUse.magnitude);
                    break;
                case 3:
                    mod = (int)Math.Ceiling(combat.getAtkMult(playerPokemonObject.atk + playerStatMod[3], enemyPokemonObject.def + enemyStatMod[1], playerPokemonObject.attackAffinity[type], enemyPokemonObject.defenseAffinity[type], moveToUse.magnitude));
                    enemyStatMod[0] -= (int)mod;
                    if (mod > 20) additionalStatString = "Enemy " + enemyPokemonObject.name + "'s defense harshly fell!";
                    else additionalStatString = "Enemy " + enemyPokemonObject.name + "'s defense fell!";
                    break;
                case 8:
                    mod = (int)Math.Ceiling(combat.getAtkMult(playerPokemonObject.atk + playerStatMod[3], enemyPokemonObject.def + enemyStatMod[1], playerPokemonObject.attackAffinity[type], enemyPokemonObject.defenseAffinity[type], moveToUse.magnitude));
                    enemyStatMod[2] -= (int)mod;
                    if (mod > 20) additionalStatString = "Enemy " + enemyPokemonObject.name + "'s attack harshly fell!";
                    else additionalStatString = "Enemy " + enemyPokemonObject.name + "'s attack fell!";
                    break;
                case 11:
                    if (enemyPokemonObject.status == "normal") enemyPokemonObject.status = "sleep";
                    break;
                case 13:
                    if (enemyPokemonObject.status == "normal") enemyPokemonObject.status = "paralyze";
                    break;
                case 15:
                    if (enemyPokemonObject.status == "normal") enemyPokemonObject.status = "poison";
                    break;
                case 19:
                    enemyStatMod[6] -= moveToUse.magnitude;
                    if (enemyStatMod[6] < -90) additionalStatString = "But it had no additional effect...";
                    else additionalStatString = "Enemy " + enemyPokemonObject.name + "'s accuracy fell!";
                    break;
            }

            applyAfinity(type);
            netDamage = damage;
            Console.WriteLine("Player Damage Gross: " + damage);
            if ((int)netDamage < 1) netDamage++;
            if (moveToUse.effect != 1 && moveToUse.effect != 2) netDamage = 0;
            if (moveToUse.magnitude == 0) netDamage = 0;
            enemyPokemonObject.HP -= (int)netDamage;
            if (enemyPokemonObject.HP < 0) enemyPokemonObject.HP = 0;
            if (enemyPokemonObject.HP > enemyPokemonObject.MaxHP) enemyPokemonObject.HP = enemyPokemonObject.MaxHP;
            Console.WriteLine("Player Damage Net: " + netDamage + " Enemy HP: " + enemyPokemonObject.HP);

        }

        //Apply player conditions
        public void applyPlayerConditions()
        {
            int conditionRNG = rng.Next(0, 100);//Default 0, 100
            //Conditional damages here
            if(playerPokemonObject.status != "normal")
            {
                previousGameTime = DateTime.Now.Ticks;
                int damage;
                switch (playerPokemonObject.status)
                {
                    case "poison":
                        battleConditionEffectString = playerPokemonObject.name + " has suffered poison damage!";
                        damage = (int)((double)playerPokemonObject.MaxHP * 0.05) + 1;
                        playerPokemonObject.HP -= damage;
                        type = 4;
                        break;
                    case "paralyze":
                        if (conditionRNG <= 50)
                        {
                            battleConditionEffectString = playerPokemonObject.name + " is paralyzed and cannot move!";
                            paralyzeSkip = true;
                        }
                        else
                        {
                            battleConditionEffectString = "";
                            paralyzeSkip = false;
                        }
                        break;
                    case "sleep":
                        if (conditionRNG > 30)
                        {
                            battleConditionEffectString = playerPokemonObject.name + " is fast asleep...";
                            paralyzeSkip = true;
                        }
                        else
                        {
                            battleConditionEffectString = " has waken up!";
                            paralyzeSkip = false;
                        }
                        break;
                    case "confuse":
                        if (conditionRNG > 50)
                        {
                            battleConditionEffectString = playerPokemonObject.name + " hurt iself in it's confusion!";
                            damage = (int)((double)playerPokemonObject.MaxHP * 0.1) + 1;
                            playerPokemonObject.HP -= damage;
                            type = 1;
                            paralyzeSkip = true;
                        }
                        else
                        {
                            if (rng.Next(0, 100) >= 50)
                            {
                                battleConditionEffectString = " has snapped out of it's confusion!";
                                cureCondition = true;
                            }
                            paralyzeSkip = false;
                        }
                        break;
                    case "frozen":
                        battleConditionEffectString = playerPokemonObject.name + " is frozen solid!";
                        paralyzeSkip = true;
                        break;
                    case "burn":
                        if (conditionRNG <= 50)
                        {
                            battleConditionEffectString = playerPokemonObject.name + " is no longer on fire.";
                            cureCondition = true;
                        }
                        else
                        {
                            battleConditionEffectString = playerPokemonObject.name + " has suffered burning damage!";
                            damage = (int)((double)playerPokemonObject.MaxHP * 0.1) + 1;
                            playerPokemonObject.HP -= damage;
                            type = 5;
                        }
                        break;
                }
            }
        }

        //Apply enemy conditions
        public void applyEnemyConditions()
        {
            //Conditional damages here

            int conditionRNG = rng.Next(0, 100);//Default 0, 100
            //Conditional damages here
            if (enemyPokemonObject.status != "normal")
            {
                previousGameTime = DateTime.Now.Ticks;
                int damage;
                switch (enemyPokemonObject.status)
                {
                    case "poison":
                        battleConditionEffectString = enemyPokemonObject.name + " has suffered poison damage!";
                        damage = (int)((double)enemyPokemonObject.MaxHP * 0.05) + 1;
                        enemyPokemonObject.HP -= damage;
                        type = 4;
                        break;
                    case "paralyze":
                        if (conditionRNG <= 50)
                        {
                            battleConditionEffectString = enemyPokemonObject.name + " is paralyzed and cannot move!";
                            paralyzeSkip = true;
                        }
                        else
                        {
                            battleConditionEffectString = "";
                            paralyzeSkip = false;
                        }
                        break;
                    case "sleep":
                        if (conditionRNG > 30)
                        {
                            battleConditionEffectString = enemyPokemonObject.name + " is fast asleep...";
                            paralyzeSkip = true;
                        }
                        else
                        {
                            battleConditionEffectString = " has waken up!";
                            paralyzeSkip = false;
                        }
                        break;
                    case "confuse":
                        if (conditionRNG > 50)
                        {
                            battleConditionEffectString = enemyPokemonObject.name + " hurt iself in it's confusion!";
                            damage = (int)((double)enemyPokemonObject.MaxHP * 0.1) + 1;
                            enemyPokemonObject.HP -= damage;
                            type = 1;
                            paralyzeSkip = true;
                            cureCondition = false;
                        }
                        else
                        {
                            cureCondition = false;
                            if (rng.Next(0, 100) >= 50)
                            {
                                battleConditionEffectString = " has snapped out of it's confusion!";
                                cureCondition = true;
                            }
                            paralyzeSkip = false;
                        }
                        break;
                    case "frozen":
                        battleConditionEffectString = enemyPokemonObject.name + " is frozen solid!";
                        paralyzeSkip = true;
                        break;
                    case "burn":
                        if (conditionRNG <= 50)
                        {
                            Console.WriteLine(conditionRNG);
                            battleConditionEffectString = enemyPokemonObject.name + " is no longer on fire.";
                            cureCondition = true;
                        }
                        else
                        {
                            battleConditionEffectString = enemyPokemonObject.name + " has suffered burning damage!";
                            damage = (int)((double)enemyPokemonObject.MaxHP * 0.1) + 1;
                            enemyPokemonObject.HP -= damage;
                            cureCondition = false;
                            type = 5;
                        }
                        break;
                }
            }

        }

        //Apply Afinity
        public void applyAfinity(int type)
        {
            //TYPE
            //1 = attackPKMN
            //2 = defensePKMN
            //3 = trainer
            if (battleState == BattleState.playerTurn)
            {
                Console.WriteLine("Attack affinity points applied! " + type + " " + playerPokemonObject.attackAffinity[type]);
                netAffinityGain++;
                playerPokemonObject.totalAffinity++;
                playerPokemonObject.attackAffinity[type]++;
                enemyPokemonObject.defenseAffinity[type]++;
                //TRAINER AFFINITY[TYPE]++
            }
            else
            {
                Console.WriteLine("Defense affinity points applied!" + type + " " + playerPokemonObject.defenseAffinity[type]);
                netAffinityGain++;
                playerPokemonObject.totalAffinity++;
                playerPokemonObject.defenseAffinity[type]++;
                enemyPokemonObject.attackAffinity[type]++;
            }
            
        }

        //Set Condition Result
        public void setConditionResult()
        {
            if(previousBattleState == BattleState.playerTurn)
            {
                battleState = BattleState.playerConditions;
                previousBattleState = BattleState.playerConditions;
            }
            else if (previousBattleState == BattleState.enemyTurn)
            {
                battleState = BattleState.enemyConditions;
                previousBattleState = BattleState.enemyConditions;
            }
        }

        //Check for learned moves
        public void doMoveCheck()
        {
            Pokemon.moves moveToLearn = new Pokemon.moves();
            int sum = 0;
            foreach(int x in playerPokemonObject.attackAffinity)
            {
                sum += x;
            }
            foreach(int x in playerPokemonObject.defenseAffinity)
            {
                sum += x;
            }
            moveToLearn = levelup.moveCheck(sum - playerPokemonObject.startAffinity, playerPokemonObject.pokedex, playerPokemonObject.currentMoveIndex);
            
            if (moveToLearn.name != "blank" && playerPokemonObject.move3.name == "NONE")
            {
                playerPokemonObject.move3 = moveToLearn;
                playerPokemonObject.PP_move3 = moveToLearn.PP;
                playerPokemonObject.PP_move3_current = playerPokemonObject.PP_move3;
                playerPokemonObject.currentMoveIndex++;
                Console.WriteLine(playerPokemonObject.name + " learned " + moveToLearn.name);
            }
            else if (moveToLearn.name != "blank" && playerPokemonObject.move4.name == "NONE")
            {
                playerPokemonObject.move4 = moveToLearn;
                playerPokemonObject.PP_move4 = moveToLearn.PP;
                playerPokemonObject.PP_move4_current = playerPokemonObject.PP_move4;
                playerPokemonObject.currentMoveIndex++;
                Console.WriteLine(playerPokemonObject.name + " learned " + moveToLearn.name);
            }

        }

        //Level up a stat
        public void doLevelStat(string stat)
        {
            switch (stat)
            {
                case "ATK":
                    playerPokemonObject.atk++;
                    break;
                case "DEF":
                    playerPokemonObject.def++;
                    break;
                case "S.ATK":
                    playerPokemonObject.satk++;
                    break;
                case "S.DEF":
                    playerPokemonObject.sdef++;
                    break;
                case "EVD":
                    playerPokemonObject.evade++;
                    break;
                case "SPD":
                    playerPokemonObject.spd++;
                    break;
                case "HP":
                    playerPokemonObject.MaxHP++;
                    playerPokemonObject.HP++;
                    break;
            }
        }

        //Drawing functions below avoid writing new functions not draw or animation related
        public void drawBattle(GameTime gameTime)
        {
            spriteBatch.Draw(battleBG, battleBGRect, Color.White);
            spriteBatch.Draw(battlePlace, battlePlaceRect, Color.White);
            spriteBatch.Draw(enemyPokemon, enemyPokemonRect, Color.White);
            spriteBatch.Draw(playerPokemon, playerPokemonRect, Color.White);
            if (battleState == BattleState.begin)
            {
                tempPlayerPokemonHP = playerPokemonObject.HP;
                refreshHP(true);
            }
            else refreshHP(false);
            drawConditions();
            drawGender();
            //spriteBatch.DrawString(font6, "HP: " + enemyPokemonObject.HP + "/" + enemyPokemonObject.MaxHP, new Vector2(0, 0), Color.White);//Enable to test enemy hp
            spriteBatch.Draw(DialogueTexture, DialogueRectangle, Color.White);//Dialogue
            if (battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.initial) drawBattleMenu();
            
            if (battleState == BattleState.begin || battleState == BattleState.runFail || battleState == BattleState.runSucceed) spriteBatch.DrawString(font, encounterText, new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
            if ((battleState == BattleState.animatingPlayer || battleState == BattleState.displayingResult) && previousBattleState == BattleState.playerTurn)
            {
                if(moveToUse.name != "blank")drawMoveResult(playerPokemonObject.name + " uses " + moveToUse.name + " on enemy " + enemyPokemonObject.name + "!");
                else drawMoveResult(playerPokemonObject.name + " uses " + failedSkill + " but missed!");
            }
            if ((battleState == BattleState.animatingEnemy || battleState == BattleState.displayingResult) && previousBattleState == BattleState.enemyTurn)
            {
                if(selectedEnemyMove.name != "blank")drawMoveResult("Enemy " + enemyPokemonObject.name + " used " + selectedEnemyMove.name + "!");
                else drawMoveResult("Enemy " + enemyPokemonObject.name + " uses " + failedSkill + " but missed!");
            }
            if (battleState == BattleState.animatingPlayer) animatePlayer(moveToUse.name, gameTime);
            if (battleState == BattleState.animatingEnemy) animateEnemy(selectedEnemyMove.name, gameTime);
            if (battleState == BattleState.playerConditions || previousBattleState == BattleState.playerConditions) drawMoveResult(battleConditionEffectString);
            if (battleState == BattleState.enemyConditions || previousBattleState == BattleState.enemyConditions) drawMoveResult(battleConditionEffectString);
            if (battleState == BattleState.playerTurn && battleMenuDepth == BattleMenuDepth.fight) drawMoves();
            if (battleState == BattleState.enemyFaint) drawEnemyFaint();
            if (battleState == BattleState.playerFaint) drawPlayerFaint();
        }

        //Draw Gender
        public void drawGender()
        {
            if (enemyPokemonObject.isFemale) enemyPokemonGender = Content.Load<Texture2D>("battle/components/female");
            else enemyPokemonGender = Content.Load<Texture2D>("battle/components/male");
            if (playerPokemonObject.isFemale) playerPokemonGender = Content.Load<Texture2D>("battle/components/female");
            else playerPokemonGender = Content.Load<Texture2D>("battle/components/male");
            spriteBatch.Draw(enemyPokemonGender, enemyPokemonGenderRect, Color.White);
            spriteBatch.Draw(playerPokemonGender, playerPokemonGenderRect, Color.White);
        }

        //Draw enemy faint
        public void drawEnemyFaint()
        {
            spriteBatch.DrawString(font, "Enemy " + enemyPokemonObject.name + " has fainted!", new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
            spriteBatch.DrawString(font, playerPokemonObject.name + " has gained " + netAffinityGain + " affinity points!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
            if(didLevelUp) spriteBatch.DrawString(font, playerPokemonObject.name + " has gained 1 point of " + leveledStat + "!", new Vector2(dialogueX + 15, dialogueY + 41), Color.Black);
            if (playerPokemonObject.totalAffinity >= 20)
            {
                leveledStat = levelup.levelUp();
                didLevelUp = true;
                playerPokemonObject.totalAffinity -= 20;
                doLevelStat(leveledStat);
                doMoveCheck();
            }
            if (DateTime.Now.Ticks >= previousGameTime + KeyboardDelay && enemyPokemonRect.Height > 0)
            {
                enemyPokemonRect.Height -= 5;
                enemyPokemonRect.Y += 4;
            }
        }

        //Draw player faint
        public void drawPlayerFaint()
        {
            spriteBatch.DrawString(font, playerPokemonObject.name + " has fainted!", new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
            spriteBatch.DrawString(font, playerPokemonObject.name + " has gained " + netAffinityGain + " affinity points!", new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
            if(didLevelUp) spriteBatch.DrawString(font, playerPokemonObject.name + " has gained 1 point of " + leveledStat + "!", new Vector2(dialogueX + 15, dialogueY + 41), Color.Black);
            if (playerPokemonObject.totalAffinity >= 20)
            {
                leveledStat = levelup.levelUp();
                didLevelUp = true;
                playerPokemonObject.totalAffinity -= 20;
                doLevelStat(leveledStat);
                doMoveCheck();
            }
            if (DateTime.Now.Ticks >= previousGameTime + KeyboardDelay && playerPokemonRect.Height > 0)
            {
                playerPokemonRect.Height -= 5;
                playerPokemonRect.Y += 4;
            }
        }

        //Refresh hp bars
        public void refreshHP(bool recalculate)
        {
            if (recalculate)
            {
                playerPokemonObject.HP = tempPlayerPokemonHP;
                enemyHPWidth = (int)(142 * ((double)enemyPokemonObject.HP / (double)enemyPokemonObject.MaxHP));
                playerHPWidth = (int)(142 * ((double)playerPokemonObject.HP / (double)playerPokemonObject.MaxHP));
                if (playerHPWidth > 71) playerHP = Content.Load<Texture2D>("battle/components/hp_full");
                else if (playerHPWidth <= 71 && playerHPWidth > 21) playerHP = Content.Load<Texture2D>("battle/components/hp_half");
                else if (playerHPWidth <= 21) playerHP = Content.Load<Texture2D>("battle/components/hp_critical");
                playerHPRect.Width = playerHPWidth;
                if (enemyHPWidth > 71) enemyHP = Content.Load<Texture2D>("battle/components/hp_full");
                else if (enemyHPWidth <= 71 && enemyHPWidth > 21) enemyHP = Content.Load<Texture2D>("battle/components/hp_half");
                else if (enemyHPWidth <= 21) enemyHP = Content.Load<Texture2D>("battle/components/hp_critical");
                enemyHPRect.Width = enemyHPWidth;
                if (enemyPokemonObject.HP <= 0 && battleState != BattleState.enemyFaint && recalculate)
                {
                    previousBattleState = BattleState.enemyFaint;
                    battleState = BattleState.enemyFaint;
                }
                if (playerPokemonObject.HP <= 0 && battleState != BattleState.playerFaint && recalculate)
                {
                    previousBattleState = BattleState.playerFaint;
                    battleState = BattleState.playerFaint;
                }
            }
            spriteBatch.Draw(hpBack, enemyHPBackRect, Color.White);
            spriteBatch.Draw(enemyHP, enemyHPRect, Color.White);
            spriteBatch.Draw(enemyHPPanel, enemyHPPanelRect, Color.White);
            spriteBatch.DrawString(font6, enemyPokemonObject.name, enemyNamePos, Color.White);
            spriteBatch.Draw(hpBack, playerHPBackRect, Color.White);
            spriteBatch.Draw(playerHP, playerHPRect, Color.White);
            spriteBatch.Draw(playerHPPanel, playerHPPanelRect, Color.White);
            spriteBatch.DrawString(font6, playerPokemonObject.name, playerNamePos, Color.White);
            spriteBatch.DrawString(font6, "HP: " + playerPokemonObject.HP + "/" + playerPokemonObject.MaxHP, playerHPNumbers, Color.White);
        }

        //Draw battle menu
        public void drawBattleMenu()
        {
            spriteBatch.Draw(battleMenu0, battleMenu0Rect, Color.White);
            spriteBatch.Draw(menuSelector, menuSelectorRect, Color.White);
        }

        //Draw moves
        public void drawMoves()
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
            int offset = 0;
            if (battleState != BattleState.playerConditions && battleState != BattleState.enemyConditions)
            {
                spriteBatch.DrawString(font, result, new Vector2(dialogueX + 15, dialogueY + 9), Color.Black);
                if(additionalStatString != "")
                {
                    offset = 16;
                    spriteBatch.DrawString(font, additionalStatString, new Vector2(dialogueX + 15, dialogueY + 25), Color.Black);
                }
            }

            if (previousBattleState == BattleState.playerTurn && battleState != BattleState.playerConditions)
                {
                    if((double)playerPokemonObject.attackAffinity[type] / (double)enemyPokemonObject.defenseAffinity[type] >= 1.5)
                    {
                        spriteBatch.DrawString(font, "It was super effective!", new Vector2(dialogueX + 15, dialogueY + 25 + offset), Color.Black);
                        if(!didDisplayEffectiveness)previousGameTime = DateTime.Now.Ticks;
                    }
                    else if((double)playerPokemonObject.attackAffinity[type] / (double)enemyPokemonObject.defenseAffinity[type] <= 0.5)
                    {
                        spriteBatch.DrawString(font, "It was not very effective!", new Vector2(dialogueX + 15, dialogueY + 25 + offset), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                didDisplayEffectiveness = true;
                }
                if(previousBattleState == BattleState.enemyTurn && battleState != BattleState.enemyConditions)
                {
                    if ((double)enemyPokemonObject.attackAffinity[type] / (double)playerPokemonObject.defenseAffinity[type] >= 1.5)
                    {
                        spriteBatch.DrawString(font, "It was super effective!", new Vector2(dialogueX + 15, dialogueY + 25 + offset), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                    else if ((double)enemyPokemonObject.attackAffinity[type] / (double)playerPokemonObject.defenseAffinity[type] <= 0.5)
                    {
                        spriteBatch.DrawString(font, "It was not very effective!", new Vector2(dialogueX + 15, dialogueY + 25 + offset), Color.Black);
                        if (!didDisplayEffectiveness) previousGameTime = DateTime.Now.Ticks;
                    }
                    didDisplayEffectiveness = true;
                }
                if (previousBattleState == BattleState.playerTurn && didDisplayEffectiveness && DateTime.Now.Ticks > previousGameTime + KeyboardDelay * BATTLESPEED)
                {
                    battleState = BattleState.animatingPlayer;
                }
                else if (previousBattleState == BattleState.enemyTurn && didDisplayEffectiveness && DateTime.Now.Ticks > previousGameTime + KeyboardDelay * BATTLESPEED)
                {
                    battleState = BattleState.animatingEnemy;
                }

                //PLAYER CONDITIONS DUPLICATE FOR ENEMY 
                else if (battleState == BattleState.playerConditions)
                {
                    battleAnimationStarted = false;
                    if (playerPokemonObject.status == "poison")
                    {
                        selectedEnemyMove = pokemon.poisonDamage;
                    }
                    else if (playerPokemonObject.status == "paralyze")
                    {
                        if (paralyzeSkip) selectedEnemyMove = pokemon.paralyzeDamage;
                        else selectedEnemyMove = pokemon.blank;
                    }
                    else if (playerPokemonObject.status == "sleep")
                    {
                        if (!paralyzeSkip)
                        {
                            playerPokemonObject.status = "normal";
                            selectedEnemyMove = pokemon.blank;
                        }
                        else selectedEnemyMove = pokemon.sleepDamage;
                    }
                    else if (playerPokemonObject.status == "confuse")
                    {
                        if (!paralyzeSkip)
                        {
                            selectedEnemyMove = pokemon.blank;
                            if (cureCondition) playerPokemonObject.status = "normal";
                        }
                        else selectedEnemyMove = pokemon.confusionDamage;
                    }
                    else if (playerPokemonObject.status == "frozen")
                    {
                        selectedEnemyMove = pokemon.frozenDamage;
                    }
                    else if (playerPokemonObject.status == "burn")
                    {
                        selectedEnemyMove = pokemon.burnDamage;
                        if (cureCondition)
                        {
                            playerPokemonObject.status = "normal";
                            selectedEnemyMove = pokemon.blank;
                        
                        }
                    }
                battleState = BattleState.animatingEnemy;
                result = "";
                }

            //ENEMY CONDITIONS
            else if (battleState == BattleState.enemyConditions)
            {
                battleAnimationStarted = false;
                if (enemyPokemonObject.status == "poison")
                {
                    moveToUse = pokemon.poisonDamage;
                }
                else if (enemyPokemonObject.status == "paralyze")
                {
                    if (paralyzeSkip) moveToUse = pokemon.paralyzeDamage;
                    else moveToUse = pokemon.blank;
                }
                else if (enemyPokemonObject.status == "sleep")
                {
                    if (!paralyzeSkip)
                    {
                        enemyPokemonObject.status = "normal";
                        moveToUse = pokemon.blank;
                    }
                    else moveToUse = pokemon.sleepDamage;
                }
                else if (enemyPokemonObject.status == "confuse")
                {
                    if (!paralyzeSkip)
                    {
                        moveToUse = pokemon.blank;
                        if (cureCondition) enemyPokemonObject.status = "normal";
                    }
                    else moveToUse = pokemon.confusionDamage;
                }
                else if (enemyPokemonObject.status == "frozen")
                {
                    moveToUse = pokemon.frozenDamage;
                }
                else if (enemyPokemonObject.status == "burn")
                {
                    moveToUse = pokemon.burnDamage;
                    if (cureCondition)
                    {
                        enemyPokemonObject.status = "normal";
                        moveToUse = pokemon.blank;
                    }
                }
                battleState = BattleState.animatingPlayer;
                result = "";
            }
        }

        //Draw conditions
        public void drawConditions()
        {
            enemyBattleStatus = Content.Load<Texture2D>("conditions/" + enemyPokemonObject.status);
            playerBattleStatus = Content.Load<Texture2D>("conditions/" + playerPokemonObject.status);
            spriteBatch.Draw(enemyBattleStatus, enemyBattleStatusRect, Color.White);
            spriteBatch.Draw(playerBattleStatus, playerBattleStatusRect, Color.White);
        }

        //Animation functions below avoid writing new functions not draw or animation related
        //player pokemon animations
        public void animatePlayer(string move, GameTime gameTime)
        {
            switch (move)
            {
                case "Tackle":
                    if (animationCycle < 10 && DateTime.Now.Ticks > animationTime + animationDelay * (BATTLESPEED / 2) && !reverseAnimation)
                    {
                        playerPokemonRect.X += 2;
                        playerPokemonRect.Y -= 2;
                        animationCycle++;
                        animationTime = DateTime.Now.Ticks;
                    }
                    else if (animationCycle > 0 && DateTime.Now.Ticks > animationTime + animationDelay * (BATTLESPEED / 2) && reverseAnimation)
                    {
                        if (enemyPokemon.Name == "sprites/pokemon/front" + femaleString + "" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix) enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + femaleString + "_inv/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                        else enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + femaleString + "" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
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
                        refreshHP(true);
                    }
                    
                    break;
                default:
                    battleAnimation(moveToUse.path, true);
                    break;
            }

        }
        //enemy pokemon animations
        public void animateEnemy(string move, GameTime gameTime)
        {
            switch (move)
            {
                case "Tackle":
                    if (animationCycle < 10 && DateTime.Now.Ticks > animationTime + animationDelay * (BATTLESPEED / 2) && !reverseAnimation)
                    {
                        enemyPokemonRect.X -= 2;
                        enemyPokemonRect.Y += 2;
                        animationCycle++;
                        animationTime = DateTime.Now.Ticks;
                    }
                    else if (animationCycle > 0 && DateTime.Now.Ticks > animationTime + animationDelay * (BATTLESPEED / 2) && reverseAnimation)
                    {
                        if (playerPokemon.Name == "sprites/pokemon/back" + playerFemaleString + "" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix) playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerFemaleString + "" + "_inv/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                        else playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerFemaleString + "" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
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
                        battleMenuDepth = BattleMenuDepth.initial;
                        refreshHP(true);
                    }
                    
                    break;
                default:
                    battleAnimation(selectedEnemyMove.path, false);
                    break;
            }

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
                playerAnimationComplete = false;
                enemyAnimationComplete = false;
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
                    if (onEnemy && move != "blank" && move != "sleep" && move != "frozen" && move != "paralyze")
                    {
                        if (enemyPokemon.Name == "sprites/pokemon/front" + femaleString + "" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix) enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + femaleString + "_inv/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                        else enemyPokemon = Content.Load<Texture2D>("sprites/pokemon/front" + femaleString + "" + shinyString + "/" + enemyPokemonObject.pokedex + "" + enemyPokemonObject.pokedexSuffix);
                    }
                    else if (move != "blank" && move != "sleep" && move != "frozen" && move != "paralyze")
                    {
                        if (playerPokemon.Name == "sprites/pokemon/back" + playerFemaleString + "" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix) playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerFemaleString + "" + "_inv/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                        else playerPokemon = Content.Load<Texture2D>("sprites/pokemon/back" + playerFemaleString + "" + playerShinyString + "/" + playerPokemonObject.pokedex + "" + playerPokemonObject.pokedexSuffix);
                    }
                    else finishFrames = 0;
                    finishFrames--;
                }
                if (finishFrames <= 0 && battleAnimationFinisher)
                {
                    if (onEnemy)
                    {
                        Console.WriteLine("On Enemy");
                        enemyAnimationComplete = true;
                        battleAnimationStarted = false;
                        battleState = BattleState.enemyTurn;
                        if (previousBattleState == BattleState.enemyConditions)
                        {
                            if (!paralyzeSkip)
                            {
                                battleState = BattleState.enemyTurn;
                                processEnemyMoves();
                            }
                            else
                            {
                                //paralyzeSkip = false;
                                battleState = BattleState.playerTurn;
                            }
                        }
                    }
                    else
                    {
                        playerAnimationComplete = true;
                        battleAnimationStarted = false;
                        battleState = BattleState.playerTurn;
                        if (previousBattleState == BattleState.playerConditions)
                        {
                            if (!paralyzeSkip)
                            {
                                battleState = BattleState.playerTurn;
                                processMoveSelection(moveToUse.name, null);
                            }
                            else
                            {
                                paralyzeSkip = false;
                                battleState = BattleState.enemyTurn;
                            }
                        }
                    }
                    if (battleState == BattleState.playerTurn) battleMenuDepth = BattleMenuDepth.initial;
                    Console.WriteLine("Animation Complete");
                    battleAnimationFinisher = false;
                    battleAnimationX = 0;
                    battleAnimationY = 0;
                    finishFrames = 6;
                    refreshHP(true);
                }
                previousGameTime = DateTime.Now.Ticks;
            }
            animationFrameSourceRect.X = 192 * battleAnimationX;
            animationFrameSourceRect.Y = 192 * battleAnimationY;

            if (battleState != BattleState.enemyTurn && !playerAnimationComplete && !battleAnimationFinisher && onEnemy) spriteBatch.Draw(Content.Load<Texture2D>("battle/animations/" + move), enemyBattleAnimationRect, animationFrameSourceRect, Color.White);
            else if (battleState != BattleState.playerTurn && !enemyAnimationComplete && !battleAnimationFinisher && !onEnemy) spriteBatch.Draw(Content.Load<Texture2D>("battle/animations/" + move), playerBattleAnimationRect, animationFrameSourceRect, Color.White);
        }
    }
}
