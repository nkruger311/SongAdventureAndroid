using System;
using System.IO;
using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SongAdventureAndroid
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		public MainActivity MainActivity{ get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            CheckForRequiredFiles();

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

            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            //ScreenManager.Instance.GameWindow = this.Window;
			ScreenManager.Instance.GameWindow = (AndroidGameWindow)this.Window;
            ScreenManager.Instance.Dimensions = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            ScreenManager.Instance.LoadContent(Content);

            Camera2D.Instance.Initialize();
        }

        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				if (AskBeforeExit ()) {
					try {
						Exit();
					} catch (Exception ex) {
						string message = ex.Message;
					}
				}

			} else {

				ScreenManager.Instance.Update (gameTime);
				/* Update the camera after updating the screen so the camera "follows" the player */
				Camera2D.Instance.Update (gameTime);

				base.Update (gameTime);
			}
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            /*
                Using SpriteSortMode.Deferred below.
                SpriteSortMode.BackToFont and SpriteSortMode.FrontToBack was causing issues when drawing sprites
            */
			try
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera2D.Instance.Transformation);
			}
			catch (Exception ex) {
				string sTest = ex.Message;
			}

			try
			{
				ScreenManager.Instance.Draw(spriteBatch);
			}
			catch(Exception ex) {
				string sTest = ex.Message;
			}

			spriteBatch.End ();

            base.Draw(gameTime);
        }

        public bool AskBeforeExit()
        {
            // TODO: Add a dialog asking the user if they would like to exit 

            return true;
        }

        private void CheckForRequiredFiles()
        {
			CheckForGameProgress ();
            CheckForScreenManager();
            CheckForPlayer();
            CheckForMaps();

			//CheckForMap1Songisms();
			//CheckForDowntownLosAngelesSongisms();
			CheckForDowntownLosAngelesSongismsDemo ();
			CheckForDowntownLosAngelesMapEntrancesDemo ();
			CheckForDowntownLosAngeles2MapEntrances ();
			CheckForDowntownLosAngeles2Songisms ();

			CheckForFrolicRoomSongismsDemo ();
			CheckForFrolicRoomMapEntrancesDemo ();
			CheckForFrolicRoomNPCs ();

            //CheckForCurrentSongism();
            CheckForSongismScreen();
            CheckForSplashScreen();
            //CheckForInventory();
            //CheckForSongBook();
			CheckForInventoryItems();
            CheckForTitleMenu();
            CheckForGuessingItems();
			CheckForDowntownLosAngelesNPCs ();
        }

        private void CheckForScreenManager()
        {
            if (!File.Exists(Path.Combine(Globals.LoadDirectory, "ScreenManager.xml")))
            {
                if (!Directory.Exists(Globals.LoadDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadDirectory, "ScreenManager.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    writer.WriteLine("<ScreenManager>");
                    writer.WriteLine("  <Image>");
                    writer.WriteLine("    <Path>ScreenManager/FadeImage</Path>");
                    writer.WriteLine("    <Effects>FadeEffect</Effects>");
                    writer.WriteLine("    <Scale>");
                    writer.WriteLine("      <X>640</X>");
                    writer.WriteLine("      <Y>480</Y>");
                    writer.WriteLine("    </Scale>");
                    writer.WriteLine("  </Image>");
                    writer.WriteLine("</ScreenManager>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForPlayer()
        {
            if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml")))
            {
                File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"));
            }

            if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplayDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplayDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<Player xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    writer.WriteLine("  <Image>");
                    writer.WriteLine("    <Alpha>1</Alpha>");
                    writer.WriteLine("    <Text />");
                    writer.WriteLine("    <FontName>Fonts/GameFont</FontName>");
                    writer.WriteLine("    <Path>Gameplay/player</Path>");
                    writer.WriteLine("    <Position>");
                    writer.WriteLine("      <X>687</X>");
                    writer.WriteLine("      <Y>303</Y>");
                    writer.WriteLine("    </Position>");
                    writer.WriteLine("    <Scale>");
                    writer.WriteLine("      <X>1</X>");
                    writer.WriteLine("      <Y>1</Y>");
                    writer.WriteLine("    </Scale>");
                    writer.WriteLine("    <SourceRect>");
					writer.WriteLine("      <X>128</X>");
					writer.WriteLine("      <Y>128</Y>");
					writer.WriteLine("      <Width>128</Width>");
					writer.WriteLine("      <Height>128</Height>");
                    writer.WriteLine("      <Location>");
                    writer.WriteLine("        <X>64</X>");
                    writer.WriteLine("        <Y>64</Y>");
                    writer.WriteLine("      </Location>");
                    writer.WriteLine("    </SourceRect>");
                    writer.WriteLine("    <IsActive>false</IsActive>");
                    writer.WriteLine("    <Effects>SpriteSheetEffect</Effects>");
                    writer.WriteLine("    <FadeEffect>");
                    writer.WriteLine("      <IsActive>false</IsActive>");
                    writer.WriteLine("      <FadeSpeed>1</FadeSpeed>");
                    writer.WriteLine("      <Increase>true</Increase>");
                    writer.WriteLine("    </FadeEffect>");
                    writer.WriteLine("    <SpriteSheetEffect>");
                    writer.WriteLine("      <IsActive>true</IsActive>");
                    writer.WriteLine("      <FrameCounter>16</FrameCounter>");
                    writer.WriteLine("      <SwitchFrame>100</SwitchFrame>");
                    writer.WriteLine("      <CurrentFrame>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>1</Y>");
                    writer.WriteLine("      </CurrentFrame>");
                    writer.WriteLine("      <AmountOfFrames>");
                    writer.WriteLine("        <X>3</X>");
                    writer.WriteLine("        <Y>4</Y>");
                    writer.WriteLine("      </AmountOfFrames>");
                    writer.WriteLine("      <DefaultFrame>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("      </DefaultFrame>");
                    writer.WriteLine("    </SpriteSheetEffect>");
                    writer.WriteLine("  </Image>");
                    writer.WriteLine("  <Velocity>");
                    writer.WriteLine("    <X>0</X>");
                    writer.WriteLine("    <Y>0</Y>");
                    writer.WriteLine("  </Velocity>");
                    writer.WriteLine("  <MoveSpeed>100</MoveSpeed>");
                    writer.WriteLine("  <Position>");
                    writer.WriteLine("    <X>687</X>");
                    writer.WriteLine("    <Y>303</Y>");
                    writer.WriteLine("  </Position>");
                    writer.WriteLine("  <ResponseDelay>99</ResponseDelay>");
                    writer.WriteLine("</Player>");
                    
                    writer.Flush();
                }

                fs.Dispose();
            }
        }

		private void CheckForGameProgress()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "game_progress.xml"))) {
				File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "game_progress.xml"));
			}

			if (!File.Exists (Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"))) {
				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayDirectory, "game_progress.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter (fs)) {
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<GameProgress>");

					writer.WriteLine ("  <CurrentMapName>downtown_los_angeles</CurrentMapName>");
					//writer.WriteLine ("  <CurrentMapName>frolic_room</CurrentMapName>");
					writer.WriteLine ("  <PlayerPosition>");
					writer.WriteLine ("    <X>128</X>");
					writer.WriteLine ("    <Y>128</Y>");
					writer.WriteLine ("  </PlayerPosition>");

					writer.WriteLine ("</GameProgress>");
					writer.Flush ();
				}
			}
		}

        private void CheckForMaps()
        {
			CheckForLosAngelesMap ();
			CheckForFrolicRoomMap ();
        }

		private void CheckForLosAngelesMap()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml")))
			{
				//CreateLosAngelesMap ();
				CreateLosAngelesMapDemo();
			}

			if (File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "los_angeles_2.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapsDirectory, "los_angeles_2.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "los_angeles_2.xml")))
			{
				CreateLosAngelesMap2();
			}
		}

		private void CheckForFrolicRoomMap()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml")))
			{
				//CreateLosAngelesMap ();
				//CreateFrolicMapDemo();
				CreateFrolicMapDemo2();
			}
		}

		private void CreateLosAngelesMapDemo()
		{
			if (!Directory.Exists(Globals.LoadGameplayMapsDirectory))
			{
				Directory.CreateDirectory(Globals.LoadGameplayMapsDirectory);
			}

			FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine ("<Map>");
				writer.WriteLine ("  <Layer>");
				writer.WriteLine ("    <Image>");
				writer.WriteLine ("      <Path>Gameplay/TileSheets/downtown_los_angeles_tilesheet</Path>");
				writer.WriteLine ("    </Image>");
				writer.WriteLine ("    <SolidTiles>[3:0][19:1][20:0][17:1][18:1][16:1][33:0][41:0][46:0][53:0][57:0][25:0]</SolidTiles>");
				writer.WriteLine ("    <OverlayTiles></OverlayTiles>");
				writer.WriteLine ("    <EntranceTiles></EntranceTiles>");
				writer.WriteLine ("    <TileMap>");
				writer.WriteLine ("      <Row>[0:0][1:0][2:0][3:0][3:0][3:0][3:0][3:0][3:0][58:0][59:0][60:0][3:0][3:0][3:0][3:0][3:0][61:0][62:0][63:0]</Row>");
				writer.WriteLine ("      <Row>[4:0][5:0][6:0][7:0][8:0][9:0][10:0][11:0][0:1][1:1][2:1][3:1][3:0][3:0][3:0][3:0][3:0][4:1][5:1][6:1]</Row>");
				writer.WriteLine ("      <Row>[12:0][13:0][3:0][14:0][15:0][16:0][17:0][18:0][7:1][8:1][9:1][10:1][3:0][3:0][3:0][3:0][3:0][11:1][12:1][13:1]</Row>");
				writer.WriteLine ("      <Row>[3:0][3:0][3:0][3:0][3:0][3:0][3:0][19:0][14:1][15:1][16:1][10:1][3:0][3:0][3:0][3:0][3:0][3:0][3:0][3:0]</Row>");
				writer.WriteLine ("      <Row>[3:0][3:0][3:0][3:0][3:0][3:0][3:0][20:0][17:1][18:1][16:1][19:1][3:0][3:0][3:0][3:0][3:0][3:0][3:0][3:0]</Row>");
				writer.WriteLine ("      <Row>[21:0][22:0][23:0][24:0][25:0][26:0][27:0][28:0][20:1][21:1][22:1][23:1][24:1][25:1][26:1][27:1][26:1][26:1][28:1][29:1]</Row>");
				writer.WriteLine ("      <Row>[29:0][30:0][31:0][32:0][33:0][34:0][35:0][36:0][30:1][31:1][32:1][33:1][34:1][35:1][37:0][36:1][37:0][37:0][37:1][38:1]</Row>");
				writer.WriteLine ("      <Row>[37:0][38:0][39:0][40:0][41:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0]</Row>");
				writer.WriteLine ("      <Row>[42:0][43:0][44:0][45:0][46:0][47:0][48:0][49:0][39:1][40:1][41:1][42:1][43:1][44:1][37:0][37:0][37:0][37:0][45:1][46:1]</Row>");
				writer.WriteLine ("      <Row>[37:0][50:0][51:0][52:0][53:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0]</Row>");
				writer.WriteLine ("      <Row>[37:0][54:0][55:0][56:0][57:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0]</Row>");
				writer.WriteLine ("      <Row>[37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0][37:0]</Row>");
				writer.WriteLine ("    </TileMap>");
				writer.WriteLine ("    <TileDimensions>");
				writer.WriteLine ("      <X>64</X>");
				writer.WriteLine ("      <Y>64</Y>");
				writer.WriteLine ("    </TileDimensions>");
				writer.WriteLine ("  </Layer>");
				writer.WriteLine ("  <PlayerStartingPosition>");
				writer.WriteLine ("    <X>687</X>");
				writer.WriteLine ("    <Y>431</Y>");
				writer.WriteLine ("  </PlayerStartingPosition>");
				writer.WriteLine ("</Map>");


				//writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				//writer.WriteLine("<Map>");
				//
				///* breaking the image up into tiles allows us to only draw the parts of the image that we need at any given time */
				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/TileSheets/los_angeles_1</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[0:0][1:0][2:0][3:0][4:0][5:0][6:0][7:0][8:0][9:0][10:0][11:0][12:0][13:0][14:0][15:0][16:0][17:0][18:0][19:0]</Row>");
				//writer.WriteLine("      <Row>[0:1][1:1][2:1][3:1][4:1][5:1][6:1][7:1][8:1][9:1][10:1][11:1][12:1][13:1][14:1][15:1][16:1][17:1][18:1][19:1]</Row>");
				//writer.WriteLine("      <Row>[0:2][1:2][2:2][3:2][4:2][5:2][6:2][7:2][8:2][9:2][10:2][11:2][12:2][13:2][14:2][15:2][16:2][17:2][18:2][19:2]</Row>");
				//writer.WriteLine("      <Row>[0:3][1:3][2:3][3:3][4:3][5:3][6:3][7:3][8:3][9:3][10:3][11:3][12:3][13:3][14:3][15:3][16:3][17:3][18:3][19:3]</Row>");
				//writer.WriteLine("      <Row>[0:4][1:4][2:4][3:4][4:4][5:4][6:4][7:4][8:4][9:4][10:4][11:4][12:4][13:4][14:4][15:4][16:4][17:4][18:4][19:4]</Row>");
				//writer.WriteLine("      <Row>[0:5][1:5][2:5][3:5][4:5][5:5][6:5][7:5][8:5][9:5][10:5][11:5][12:5][13:5][14:5][15:5][16:5][17:5][18:5][19:5]</Row>");
				//writer.WriteLine("      <Row>[0:6][1:6][2:6][3:6][4:6][5:6][6:6][7:6][8:6][9:6][10:6][11:6][12:6][13:6][14:6][15:6][16:6][17:6][18:6][19:6]</Row>");
				//writer.WriteLine("      <Row>[0:7][1:7][2:7][3:7][4:7][5:7][6:7][7:7][8:7][9:7][10:7][11:7][12:7][13:7][14:7][15:7][16:7][17:7][18:7][19:7]</Row>");
				//writer.WriteLine("      <Row>[0:8][1:8][2:8][3:8][4:8][5:8][6:8][7:8][8:8][9:8][10:8][11:8][12:8][13:8][14:8][15:8][16:8][17:8][18:8][19:8]</Row>");
				//writer.WriteLine("      <Row>[0:9][1:9][2:9][3:9][4:9][5:9][6:9][7:9][8:9][9:9][10:9][11:9][12:9][13:9][14:9][15:9][16:9][17:9][18:9][19:9]</Row>");
				//writer.WriteLine("      <Row>[0:10][1:10][2:10][3:10][4:10][5:10][6:10][7:10][8:10][9:10][10:10][11:10][12:10][13:10][14:10][15:10][16:10][17:10][18:10][19:10]</Row>");
				//writer.WriteLine("      <Row>[0:11][1:11][2:11][3:11][4:11][5:11][6:11][7:11][8:11][9:11][10:11][11:11][12:11][13:11][14:11][15:11][16:11][17:11][18:11][19:11]</Row>");
				//writer.WriteLine("      <Row>[0:12][1:12][2:12][3:12][4:12][5:12][6:12][7:12][8:12][9:12][10:12][11:12][12:12][13:12][14:12][15:12][16:12][17:12][18:12][19:12]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>64</X>");
				//writer.WriteLine("      <Y>64</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");
				//writer.WriteLine ("  <PlayerStartingPosition>");
				//writer.WriteLine ("    <X>687</X>");
				//writer.WriteLine ("    <Y>431</Y>");
				//writer.WriteLine ("  </PlayerStartingPosition>");
				//writer.WriteLine("");
				//writer.WriteLine("</Map>");

				writer.Flush();
			}
		}

		private void CreateLosAngelesMap2()
		{
			if (!Directory.Exists(Globals.LoadGameplayMapsDirectory))
			{
				Directory.CreateDirectory(Globals.LoadGameplayMapsDirectory);
			}

			FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapsDirectory, "los_angeles_2.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine ("<Map>");
				writer.WriteLine ("  <Layer>");
				writer.WriteLine ("    <Image>");
				writer.WriteLine ("      <Path>Gameplay/TileSheets/los_angeles_2_TileSheet</Path>");
				writer.WriteLine ("    </Image>");
				writer.WriteLine ("    <SolidTiles>[4:0]</SolidTiles>");
				writer.WriteLine ("    <OverlayTiles></OverlayTiles>");
				writer.WriteLine ("    <TileMap>");
				writer.WriteLine ("      <Row>[0:0][1:0][2:0][3:0][4:0][4:0][4:0][5:0][23:0][24:0][25:0][4:0][4:0][4:0][4:0][26:0][27:0][4:0][4:0][4:0][0:0][1:0][2:0][3:0][4:0][4:0][4:0][5:0][23:0][24:0][25:0][4:0][4:0][4:0][4:0][26:0][27:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[4:0][4:0][6:0][7:0][4:0][4:0][8:0][9:0][28:0][29:0][30:0][31:0][4:0][4:0][32:0][33:0][34:0][4:0][4:0][4:0][4:0][4:0][6:0][7:0][4:0][4:0][8:0][9:0][28:0][29:0][30:0][31:0][4:0][4:0][32:0][33:0][34:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][35:0][36:0][37:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][35:0][36:0][37:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][38:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][38:0][4:0][4:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[10:0][10:0][10:0][10:0][10:0][11:0][12:0][13:0][13:0][13:0][13:0][39:0][40:0][41:0][13:0][13:0][13:0][13:0][42:0][10:0][10:0][10:0][10:0][10:0][10:0][11:0][12:0][13:0][13:0][13:0][13:0][39:0][40:0][41:0][13:0][13:0][13:0][13:0][42:0][10:0]</Row>");
				writer.WriteLine ("      <Row>[14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][43:0][44:0][44:0][44:0][44:0][45:0][46:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][14:0][43:0][44:0][44:0][44:0][44:0][45:0][46:0]</Row>");
				writer.WriteLine ("      <Row>[15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0]</Row>");
				writer.WriteLine ("      <Row>[16:0][17:0][18:0][19:0][15:0][20:0][21:0][22:0][47:0][48:0][49:0][50:0][51:0][52:0][53:0][54:0][55:0][56:0][57:0][15:0][16:0][17:0][18:0][19:0][15:0][20:0][21:0][22:0][47:0][48:0][49:0][50:0][51:0][52:0][53:0][54:0][55:0][15:0][58:0][59:0]</Row>");
				writer.WriteLine ("      <Row>[15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0]</Row>");
				writer.WriteLine ("      <Row>[15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0]</Row>");
				writer.WriteLine ("      <Row>[15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0][15:0]</Row>");
				writer.WriteLine ("    </TileMap>");
				writer.WriteLine ("    <TileDimensions>");
				writer.WriteLine ("      <X>64</X>");
				writer.WriteLine ("      <Y>64</Y>");
				writer.WriteLine ("    </TileDimensions>");
				writer.WriteLine ("  </Layer>");
				writer.WriteLine ("  <PlayerStartingPosition>");
				writer.WriteLine ("    <X>250</X>");
				writer.WriteLine ("    <Y>450</Y>");
				writer.WriteLine ("  </PlayerStartingPosition>");
				writer.WriteLine ("</Map>");

				writer.Flush();
			}
		}

		private void CreateFrolicMapDemo()
		{
			if (!Directory.Exists(Globals.LoadGameplayMapsDirectory))
			{
				Directory.CreateDirectory(Globals.LoadGameplayMapsDirectory);
			}

			FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine("<Map>");

				/* breaking the image up into tiles allows us to only draw the parts of the image that we need at any given time */
				writer.WriteLine("  <Layer>");
				writer.WriteLine("    <Image>");
				writer.WriteLine("      <Path>Gameplay/TileSheets/frolic_room</Path>");
				writer.WriteLine("    </Image>");
				writer.WriteLine("    <TileMap>");

				writer.WriteLine("      <Row>[0:0][1:0][2:0][3:0][4:0][5:0][6:0][7:0][8:0][9:0][10:0][11:0][12:0][13:0][14:0][15:0][16:0][17:0][18:0][19:0]</Row>");
				writer.WriteLine("      <Row>[0:1][1:1][2:1][3:1][4:1][5:1][6:1][7:1][8:1][9:1][10:1][11:1][12:1][13:1][14:1][15:1][16:1][17:1][18:1][19:1]</Row>");
				writer.WriteLine("      <Row>[0:2][1:2][2:2][3:2][4:2][5:2][6:2][7:2][8:2][9:2][10:2][11:2][12:2][13:2][14:2][15:2][16:2][17:2][18:2][19:2]</Row>");
				writer.WriteLine("      <Row>[0:3][1:3][2:3][3:3][4:3][5:3][6:3][7:3][8:3][9:3][10:3][11:3][12:3][13:3][14:3][15:3][16:3][17:3][18:3][19:3]</Row>");
				writer.WriteLine("      <Row>[0:4][1:4][2:4][3:4][4:4][5:4][6:4][7:4][8:4][9:4][10:4][11:4][12:4][13:4][14:4][15:4][16:4][17:4][18:4][19:4]</Row>");
				writer.WriteLine("      <Row>[0:5][1:5][2:5][3:5][4:5][5:5][6:5][7:5][8:5][9:5][10:5][11:5][12:5][13:5][14:5][15:5][16:5][17:5][18:5][19:5]</Row>");
				writer.WriteLine("      <Row>[0:6][1:6][2:6][3:6][4:6][5:6][6:6][7:6][8:6][9:6][10:6][11:6][12:6][13:6][14:6][15:6][16:6][17:6][18:6][19:6]</Row>");
				writer.WriteLine("      <Row>[0:7][1:7][2:7][3:7][4:7][5:7][6:7][7:7][8:7][9:7][10:7][11:7][12:7][13:7][14:7][15:7][16:7][17:7][18:7][19:7]</Row>");
				writer.WriteLine("      <Row>[0:8][1:8][2:8][3:8][4:8][5:8][6:8][7:8][8:8][9:8][10:8][11:8][12:8][13:8][14:8][15:8][16:8][17:8][18:8][19:8]</Row>");
				writer.WriteLine("      <Row>[0:9][1:9][2:9][3:9][4:9][5:9][6:9][7:9][8:9][9:9][10:9][11:9][12:9][13:9][14:9][15:9][16:9][17:9][18:9][19:9]</Row>");
				writer.WriteLine("      <Row>[0:10][1:10][2:10][3:10][4:10][5:10][6:10][7:10][8:10][9:10][10:10][11:10][12:10][13:10][14:10][15:10][16:10][17:10][18:10][19:10]</Row>");
				writer.WriteLine("      <Row>[0:11][1:11][2:11][3:11][4:11][5:11][6:11][7:11][8:11][9:11][10:11][11:11][12:11][13:11][14:11][15:11][16:11][17:11][18:11][19:11]</Row>");
				writer.WriteLine("      <Row>[0:12][1:12][2:12][3:12][4:12][5:12][6:12][7:12][8:12][9:12][10:12][11:12][12:12][13:12][14:12][15:12][16:12][17:12][18:12][19:12]</Row>");

				/*
				writer.WriteLine("      [0:0][3:0][2:0][3:0][4:0][5:0][6:0][7:0][8:0][9:0][30:0][31:0][32:0][33:0][34:0][35:0][36:0][37:0][38:0][39:0][20:0][21:0][22:0][23:0][24:0][25:0][26:0][27:0][28:0][29:0][30:0][31:0][32:0][33:0][34:0][35:0][36:0][37:0][38:0][39:0]");
				writer.WriteLine("      [0:1][3:1][2:1][3:1][4:1][5:1][6:1][7:1][8:1][9:1][30:1][31:1][32:1][33:1][34:1][35:1][36:1][37:1][38:1][39:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1][27:1][28:1][29:1][30:1][31:1][32:1][33:1][34:1][35:1][36:1][37:1][38:1][39:1]");
				writer.WriteLine("      [0:2][3:2][2:2][3:2][4:2][5:2][6:2][7:2][8:2][9:2][30:2][31:2][32:2][33:2][34:2][35:2][36:2][37:2][38:2][39:2][20:2][21:2][22:2][23:2][24:2][25:2][26:2][27:2][28:2][29:2][30:2][31:2][32:2][33:2][34:2][35:2][36:2][37:2][38:2][39:2]");
				writer.WriteLine("      [0:3][3:3][2:3][3:3][4:3][5:3][6:3][7:3][8:3][9:3][30:3][31:3][32:3][33:3][34:3][35:3][36:3][37:3][38:3][39:3][20:3][21:3][22:3][23:3][24:3][25:3][26:3][27:3][28:3][29:3][30:3][31:3][32:3][33:3][34:3][35:3][36:3][37:3][38:3][39:3]");
				writer.WriteLine("      [0:4][3:4][2:4][3:4][4:4][5:4][6:4][7:4][8:4][9:4][30:4][31:4][32:4][33:4][34:4][35:4][36:4][37:4][38:4][39:4][20:4][21:4][22:4][23:4][24:4][25:4][26:4][27:4][28:4][29:4][30:4][31:4][32:4][33:4][34:4][35:4][36:4][37:4][38:4][39:4]");
				writer.WriteLine("      [0:5][3:5][2:5][3:5][4:5][5:5][6:5][7:5][8:5][9:5][30:5][31:5][32:5][33:5][34:5][35:5][36:5][37:5][38:5][39:5][20:5][21:5][22:5][23:5][24:5][25:5][26:5][27:5][28:5][29:5][30:5][31:5][32:5][33:5][34:5][35:5][36:5][37:5][38:5][39:5]");
				writer.WriteLine("      [0:6][3:6][2:6][3:6][4:6][5:6][6:6][7:6][8:6][9:6][30:6][31:6][32:6][33:6][34:6][35:6][36:6][37:6][38:6][39:6][20:6][21:6][22:6][23:6][24:6][25:6][26:6][27:6][28:6][29:6][30:6][31:6][32:6][33:6][34:6][35:6][36:6][37:6][38:6][39:6]");
				writer.WriteLine("      [0:7][3:7][2:7][3:7][4:7][5:7][6:7][7:7][8:7][9:7][30:7][31:7][32:7][33:7][34:7][35:7][36:7][37:7][38:7][39:7][20:7][21:7][22:7][23:7][24:7][25:7][26:7][27:7][28:7][29:7][30:7][31:7][32:7][33:7][34:7][35:7][36:7][37:7][38:7][39:7]");
				writer.WriteLine("      [0:8][3:8][2:8][3:8][4:8][5:8][6:8][7:8][8:8][9:8][30:8][31:8][32:8][33:8][34:8][35:8][36:8][37:8][38:8][39:8][20:8][21:8][22:8][23:8][24:8][25:8][26:8][27:8][28:8][29:8][30:8][31:8][32:8][33:8][34:8][35:8][36:8][37:8][38:8][39:8]");
				writer.WriteLine("      [0:9][3:9][2:9][3:9][4:9][5:9][6:9][7:9][8:9][9:9][30:9][31:9][32:9][33:9][34:9][35:9][36:9][37:9][38:9][39:9][20:9][21:9][22:9][23:9][24:9][25:9][26:9][27:9][28:9][29:9][30:9][31:9][32:9][33:9][34:9][35:9][36:9][37:9][38:9][39:9]");
				writer.WriteLine("      [0:10][3:10][2:10][3:10][4:10][5:10][6:10][7:10][8:10][9:10][30:10][31:10][32:10][33:10][34:10][35:10][36:10][37:10][38:10][39:10][20:10][21:10][22:10][23:10][24:10][25:10][26:10][27:10][28:10][29:10][30:10][31:10][32:10][33:10][34:10][35:10][36:10][37:10][38:10][39:10]");
				writer.WriteLine("      [0:11][3:11][2:11][3:11][4:11][5:11][6:11][7:11][8:11][9:11][30:11][31:11][32:11][33:11][34:11][35:11][36:11][37:11][38:11][39:11][20:11][21:11][22:11][23:11][24:11][25:11][26:11][27:11][28:11][29:11][30:11][31:11][32:11][33:11][34:11][35:11][36:11][37:11][38:11][39:11]");
				writer.WriteLine("      [0:12][3:12][2:12][3:12][4:12][5:12][6:12][7:12][8:12][9:12][30:12][31:12][32:12][33:12][34:12][35:12][36:12][37:12][38:12][39:12][20:12][21:12][22:12][23:12][24:12][25:12][26:12][27:12][28:12][29:12][30:12][31:12][32:12][33:12][34:12][35:12][36:12][37:12][38:12][39:12]");
				*/
				writer.WriteLine("    </TileMap>");
				writer.WriteLine("    <TileDimensions>");
				writer.WriteLine("      <X>128</X>");
				writer.WriteLine("      <Y>64</Y>");
				writer.WriteLine("    </TileDimensions>");
				writer.WriteLine("  </Layer>");
				writer.WriteLine ("  <PlayerStartingPosition>");
				writer.WriteLine ("    <X>250</X>");
				writer.WriteLine ("    <Y>450</Y>");
				writer.WriteLine ("  </PlayerStartingPosition>");
				writer.WriteLine("");
				writer.WriteLine("</Map>");

				writer.Flush();
			}
		}

		private void CreateFrolicMapDemo2()
		{
			if (!Directory.Exists(Globals.LoadGameplayMapsDirectory))
			{
				Directory.CreateDirectory(Globals.LoadGameplayMapsDirectory);
			}

			FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (StreamWriter writer = new StreamWriter (fs)) {
				//writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				//writer.WriteLine ("<Map>");
				//writer.WriteLine ("  <Layer>");
				//writer.WriteLine ("    <Image>");
				//writer.WriteLine ("      <Path>Gameplay/TileSheets/frolic_room_TileSheet</Path>");
				//writer.WriteLine ("    </Image>");
				//writer.WriteLine ("    <SolidTiles>[29:0][4:0][30:0][36:0][37:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][23:1][22:1][24:1][25:1][26:1][10:1][11:1][12:1][13:1][14:1]</SolidTiles>");
				//writer.WriteLine ("    <OverlayTiles></OverlayTiles>");
				//writer.WriteLine ("    <EntranceTiles>[32:1]</EntranceTiles>");
				//writer.WriteLine ("    <TileMap>");
				//writer.WriteLine ("      <Row>[0:0][1:0][2:0][3:0][4:0][5:0][6:0][7:0][4:0][41:0][42:0][43:0][4:0][4:0][4:0][44:0][45:0][4:0][4:0][4:0][46:0][47:0][48:0][49:0][4:0][5:0][6:0][7:0][4:0][41:0][42:0][43:0][4:0][4:0][4:0][44:0][45:0][4:0][4:0][4:0]</Row>");
				//writer.WriteLine ("      <Row>[8:0][9:0][10:0][11:0][4:0][12:0][13:0][14:0][4:0][50:0][51:0][52:0][4:0][4:0][4:0][53:0][54:0][4:0][4:0][4:0][55:0][56:0][57:0][58:0][4:0][12:0][13:0][14:0][4:0][50:0][51:0][52:0][4:0][4:0][4:0][53:0][54:0][4:0][4:0][4:0]</Row>");
				//writer.WriteLine ("      <Row>[15:0][16:0][17:0][18:0][19:0][20:0][21:0][22:0][19:0][59:0][60:0][61:0][19:0][19:0][19:0][62:0][63:0][19:0][19:0][0:1][1:1][2:1][3:1][4:1][19:0][20:0][21:0][22:0][19:0][59:0][60:0][61:0][19:0][19:0][19:0][62:0][63:0][19:0][19:0][0:1]</Row>");
				//writer.WriteLine ("      <Row>[23:0][24:0][25:0][26:0][27:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][5:1][6:1][7:1][8:1][9:1][27:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][5:1]</Row>");
				//writer.WriteLine ("      <Row>[29:0][4:0][4:0][4:0][30:0][31:0][32:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][10:1][11:1][12:1][13:1][14:1][30:0][31:0][32:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][10:1]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][36:0][37:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1][35:0][35:0][35:0][35:0][36:0][37:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][40:0][35:0][35:0][27:1][35:0][28:1][35:0][35:0][29:1][35:0][30:1][35:0][31:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][40:0][35:0][35:0][27:1][35:0][28:1][35:0][35:0][29:1][35:0][30:1][35:0][31:1][35:0][35:0]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				//writer.WriteLine ("      <Row>[34:0][32:1][32:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				//writer.WriteLine ("    </TileMap>");
				//writer.WriteLine ("    <TileDimensions>");
				//writer.WriteLine ("      <X>64</X>");
				//writer.WriteLine ("      <Y>64</Y>");
				//writer.WriteLine ("    </TileDimensions>");
				//writer.WriteLine ("  </Layer>");
				//writer.WriteLine ("  <PlayerStartingPosition>");
				//writer.WriteLine ("    <X>250</X>");
				//writer.WriteLine ("    <Y>450</Y>");
				//writer.WriteLine ("  </PlayerStartingPosition>");
				//writer.WriteLine ("</Map>");

				writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine ("<Map>");
				writer.WriteLine ("  <Layer>");
				writer.WriteLine ("    <Image>");
				writer.WriteLine ("      <Path>Gameplay/TileSheets/frolic_room_TileSheet</Path>");
				writer.WriteLine ("    </Image>");
				writer.WriteLine ("    <SolidTiles>[29:0][4:0][30:0][37:0][36:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1][10:1][11:1][12:1][13:1][14:1][5:1][0:1][19:0][28:0][33:0][45:0][44:0][53:0][54:0][63:0][62:0][43:0][42:0][41:0][50:0][51:0][52:0][61:0][60:0][59:0][7:0][14:0][22:0][32:0][21:0][13:0][6:0][0:0][8:0][15:0][23:0][24:0][16:0][9:0][1:0][2:0][10:0][17:0][25:0][26:0][18:0][11:0][3:0][27:0][20:0][12:0][5:0][46:0][47:0][48:0][49:0][58:0][57:0][56:0][55:0][1:1][2:1][3:1][4:1][9:1][8:1][7:1][6:1]</SolidTiles>");
				writer.WriteLine ("    <OverlayTiles></OverlayTiles>");
				writer.WriteLine ("    <TileMap>");
				writer.WriteLine ("      <Row>[0:0][1:0][2:0][3:0][4:0][5:0][6:0][7:0][4:0][41:0][42:0][43:0][4:0][4:0][4:0][44:0][45:0][4:0][4:0][4:0][46:0][47:0][48:0][49:0][4:0][5:0][6:0][7:0][4:0][41:0][42:0][43:0][4:0][4:0][4:0][44:0][45:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[8:0][9:0][10:0][11:0][4:0][12:0][13:0][14:0][4:0][50:0][51:0][52:0][4:0][4:0][4:0][53:0][54:0][4:0][4:0][4:0][55:0][56:0][57:0][58:0][4:0][12:0][13:0][14:0][4:0][50:0][51:0][52:0][4:0][4:0][4:0][53:0][54:0][4:0][4:0][4:0]</Row>");
				writer.WriteLine ("      <Row>[15:0][16:0][17:0][18:0][19:0][20:0][21:0][22:0][19:0][59:0][60:0][61:0][19:0][19:0][19:0][62:0][63:0][19:0][19:0][0:1][1:1][2:1][3:1][4:1][19:0][20:0][21:0][22:0][19:0][59:0][60:0][61:0][19:0][19:0][19:0][62:0][63:0][19:0][19:0][0:1]</Row>");
				writer.WriteLine ("      <Row>[23:0][24:0][25:0][26:0][27:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][5:1][6:1][7:1][8:1][9:1][27:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][28:0][5:1]</Row>");
				writer.WriteLine ("      <Row>[29:0][4:0][4:0][4:0][30:0][31:0][32:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][10:1][11:1][12:1][13:1][14:1][30:0][31:0][32:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][33:0][10:1]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][36:0][37:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1][35:0][35:0][35:0][35:0][36:0][37:0][38:0][39:0][15:1][16:1][17:1][18:1][19:1][20:1][21:1][22:1][23:1][24:1][25:1][26:1]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][40:0][35:0][35:0][27:1][35:0][28:1][35:0][35:0][29:1][35:0][30:1][35:0][31:1][35:0][35:0][35:0][35:0][35:0][35:0][35:0][40:0][35:0][35:0][27:1][35:0][28:1][35:0][35:0][29:1][35:0][30:1][35:0][31:1][35:0][35:0]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				writer.WriteLine ("      <Row>[34:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0][35:0]</Row>");
				writer.WriteLine ("    </TileMap>");
				writer.WriteLine ("    <TileDimensions>");
				writer.WriteLine ("      <X>64</X>");
				writer.WriteLine ("      <Y>64</Y>");
				writer.WriteLine ("    </TileDimensions>");
				writer.WriteLine ("  </Layer>");
				writer.WriteLine ("  <PlayerStartingPosition>");
				writer.WriteLine ("    <X>250</X>");
				writer.WriteLine ("    <Y>450</Y>");
				writer.WriteLine ("  </PlayerStartingPosition>");
				writer.WriteLine ("</Map>");

				writer.Flush();
			}
		}

		private void CreateLosAngelesMap()
		{
			if (!Directory.Exists(Globals.LoadGameplayMapsDirectory))
			{
				Directory.CreateDirectory(Globals.LoadGameplayMapsDirectory);
			}

			FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

			using (StreamWriter writer = new StreamWriter(fs))
			{
				//writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				//writer.WriteLine("<Map>");
				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/Maps/map1</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[0:0]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>1920</X>");
				//writer.WriteLine("      <Y>720</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");
				//writer.WriteLine("");


				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/TileSheets/terrain_tiles</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[0:0][0:0][0:0][0:0][0:0][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][3:3][2:2][1:3][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][3:3][2:2][1:3][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][3:3][2:2][1:3][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][3:3][2:2][4:2][0:3][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][3:3][2:2][2:2][2:2][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][6:3][2:3][2:3][2:3][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("      <Row>[2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4][2:4]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>64</X>");
				//writer.WriteLine("      <Y>64</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");
				//writer.WriteLine("");


				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/TileSheets/clock</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <SolidTiles>[1:1][1:2]</SolidTiles>");
				//writer.WriteLine("    <OverlayTiles>[1:0]</OverlayTiles>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][1:0][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][1:1][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][1:2][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("      <Row>[x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x][x:x]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>64</X>");
				//writer.WriteLine("      <Y>64</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");



				/* breaking the image up into tiles allows us to only draw the parts of the image that we need at any given time */
				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/TileSheets/downtown_los_angeles</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[0:0][1:0][2:0][3:0][4:0][5:0][6:0][7:0][8:0][9:0][10:0][11:0][12:0][13:0][14:0][15:0][16:0][17:0][18:0][19:0][20:0][21:0]</Row>");
				//writer.WriteLine("      <Row>[0:1][1:1][2:1][3:1][4:1][5:1][6:1][7:1][8:1][9:1][10:1][11:1][12:1][13:1][14:1][15:1][16:1][17:1][18:1][19:1][20:1][21:1]</Row>");
				//writer.WriteLine("      <Row>[0:2][1:2][2:2][3:2][4:2][5:2][6:2][7:2][8:2][9:2][10:2][11:2][12:2][13:2][14:2][15:2][16:2][17:2][18:2][19:2][20:2][21:2]</Row>");
				//writer.WriteLine("      <Row>[0:3][1:3][2:3][3:3][4:3][5:3][6:3][7:3][8:3][9:3][10:3][11:3][12:3][13:3][14:3][15:3][16:3][17:3][18:3][19:3][20:3][21:3]</Row>");
				//writer.WriteLine("      <Row>[0:4][1:4][2:4][3:4][4:4][5:4][6:4][7:4][8:4][9:4][10:4][11:4][12:4][13:4][14:4][15:4][16:4][17:4][18:4][19:4][20:4][21:4]</Row>");
				//writer.WriteLine("      <Row>[0:5][1:5][2:5][3:5][4:5][5:5][6:5][7:5][8:5][9:5][10:5][11:5][12:5][13:5][14:5][15:5][16:5][17:5][18:5][19:5][20:5][21:5]</Row>");
				//writer.WriteLine("      <Row>[0:6][1:6][2:6][3:6][4:6][5:6][6:6][7:6][8:6][9:6][10:6][11:6][12:6][13:6][14:6][15:6][16:6][17:6][18:6][19:6][20:6][21:6]</Row>");
				//writer.WriteLine("      <Row>[0:7][1:7][2:7][3:7][4:7][5:7][6:7][7:7][8:7][9:7][10:7][11:7][12:7][13:7][14:7][15:7][16:7][17:7][18:7][19:7][20:7][21:7]</Row>");
				//writer.WriteLine("      <Row>[0:8][1:8][2:8][3:8][4:8][5:8][6:8][7:8][8:8][9:8][10:8][11:8][12:8][13:8][14:8][15:8][16:8][17:8][18:8][19:8][20:8][21:8]</Row>");
				//writer.WriteLine("      <Row>[0:9][1:9][2:9][3:9][4:9][5:9][6:9][7:9][8:9][9:9][10:9][11:9][12:9][13:9][14:9][15:9][16:9][17:9][18:9][19:9][20:9][21:9]</Row>");
				//writer.WriteLine("      <Row>[0:10][1:10][2:10][3:10][4:10][5:10][6:10][7:10][8:10][9:10][10:10][11:10][12:10][13:10][14:10][15:10][16:10][17:10][18:10][19:10][20:10][21:10]</Row>");
				//writer.WriteLine("      <Row>[0:11][1:11][2:11][3:11][4:11][5:11][6:11][7:11][8:11][9:11][10:11][11:11][12:11][13:11][14:11][15:11][16:11][17:11][18:11][19:11][20:11][21:11]</Row>");
				//writer.WriteLine("      <Row>[0:12][1:12][2:12][3:12][4:12][5:12][6:12][7:12][8:12][9:12][10:12][11:12][12:12][13:12][14:12][15:12][16:12][17:12][18:12][19:12][20:12][21:12]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>64</X>");
				//writer.WriteLine("      <Y>64</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");
				//writer.WriteLine("");
				//writer.WriteLine("</Map>");




				//writer.WriteLine("  <Layer>");
				//writer.WriteLine("    <Image>");
				//writer.WriteLine("      <Path>Gameplay/TileSheets/downtown_los_angeles_tilesheet</Path>");
				//writer.WriteLine("    </Image>");
				//writer.WriteLine("    <SolidTiles>[0:0][1:0][2:0][3:0][0:2]</SolidTiles>");
				//writer.WriteLine("    <TileMap>");
				//writer.WriteLine("      <Row>[0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:1][2:1][0:1][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0]</Row>");
				//writer.WriteLine("      <Row>[0:1][0:1][0:1][0:1][0:2][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1]</Row>");
				//writer.WriteLine("      <Row>[0:1][1:1][0:1][1:1][0:2][1:1][0:1][1:1][0:1][1:1][0:1][0:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1]</Row>");
				//writer.WriteLine("      <Row>[0:1][0:1][0:1][0:1][0:2][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("      <Row>[0:7][0:7][0:7][0:7][0:2][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				//writer.WriteLine("    </TileMap>");
				//writer.WriteLine("    <TileDimensions>");
				//writer.WriteLine("      <X>128</X>");
				//writer.WriteLine("      <Y>128</Y>");
				//writer.WriteLine("    </TileDimensions>");
				//writer.WriteLine("  </Layer>");
				//writer.WriteLine("");
				//writer.WriteLine("</Map>");


				writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine ("<Map>");
				writer.WriteLine ("  <Layer>");
				writer.WriteLine ("    <Image>");
				writer.WriteLine ("      <Path>Gameplay/TileSheets/downtown_los_angeles_tilesheet</Path>");
				writer.WriteLine ("    </Image>");
				writer.WriteLine ("    <SolidTiles>[0:0][1:0][2:0][3:0][0:2]</SolidTiles>");
				writer.WriteLine ("    <OverlayTiles></OverlayTiles>");
				writer.WriteLine ("    <TileMap>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:2][0:2][0:2][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:1][2:1][0:1][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:1][0:1][0:1][0:1][0:2][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:1][1:1][0:1][1:1][0:2][1:1][0:1][1:1][0:1][1:1][0:1][0:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:1][0:1][0:1][0:1][0:2][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][2:0][0:1][2:1][0:1][3:0][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:0][0:0][0:0][3:0][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][2:1][0:1][0:0][0:0][0:0][0:0][0:0][0:0][0:1][2:1][0:1][1:0][1:0][1:0][1:0][1:0][1:0][1:0][1:0][1:0][1:0][1:0][0:1][2:1][0:1][0:0][0:0][0:0][0:2][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:2][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][0:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][1:1][0:1][0:1][0:1][1:1][0:1][1:1][0:2][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:1][0:2][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][2:1][0:1][3:0][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:2][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][0:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[2:0][0:1][2:1][0:1][3:0][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:2][0:2][0:2][0:2][0:2][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("      <Row>[0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7][0:7]</Row>");
				writer.WriteLine ("    </TileMap>");
				writer.WriteLine ("    <TileDimensions>");
				writer.WriteLine ("      <X>96</X>");
				writer.WriteLine ("      <Y>96</Y>");
				writer.WriteLine ("    </TileDimensions>");
				writer.WriteLine ("  </Layer>");
				writer.WriteLine ("  <PlayerStartingPosition>");
				writer.WriteLine ("    <X>687</X>");
				writer.WriteLine ("    <Y>431</Y>");
				writer.WriteLine ("  </PlayerStartingPosition>");
				writer.WriteLine ("</Map>");

				writer.Flush();
			}

			fs.Dispose();
		}

		private void CheckForDowntownLosAngelesNPCs()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists (Path.Combine (Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml"))) {
				if (!Directory.Exists (Globals.LoadGameplayNPCDirectory)) {
					Directory.CreateDirectory (Globals.LoadGameplayNPCDirectory);
				}

				FileStream fs = new FileStream (Path.Combine (Globals.LoadGameplayNPCDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter (fs)) {
					writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine ("<ArrayOfNPC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					writer.WriteLine ("</ArrayOfNPC>");

					writer.Flush ();
				}
			}
		}

		private void CheckForFrolicRoomNPCs()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayNPCDirectory, "frolic_room.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayNPCDirectory, "frolic_room.xml"));
			}

			if (!File.Exists (Path.Combine (Globals.LoadGameplayNPCDirectory, "frolic_room.xml"))) {
				if (!Directory.Exists (Globals.LoadGameplayNPCDirectory)) {
					Directory.CreateDirectory (Globals.LoadGameplayNPCDirectory);
				}

				FileStream fs = new FileStream (Path.Combine (Globals.LoadGameplayNPCDirectory, "frolic_room.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter (fs)) {
					writer.WriteLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine ("<ArrayOfNPC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					writer.WriteLine ("  <NPC>");
					writer.WriteLine ("    <Name>Bartender</Name>");

					writer.WriteLine ("    <Image>");
					writer.WriteLine ("      <Alpha>1</Alpha>");
					writer.WriteLine ("      <Text />");
					writer.WriteLine ("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine ("      <Path>Gameplay/NPCs/bartender</Path>");

					// needs to be the position on the map
					writer.WriteLine ("      <Position>");
					writer.WriteLine ("        <X>1536</X>");
					writer.WriteLine ("        <Y>256</Y>");
					writer.WriteLine ("      </Position>");
					writer.WriteLine ("      <Scale>");
					writer.WriteLine ("        <X>1</X>");
					writer.WriteLine ("        <Y>1</Y>");
					writer.WriteLine ("      </Scale>");


					writer.WriteLine ("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine ("        <X>1536</X>");
					writer.WriteLine ("        <Y>256</Y>");
					writer.WriteLine ("        <Width>128</Width>");
					writer.WriteLine ("        <Height>256</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine ("        <Location>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </Location>");
					writer.WriteLine ("      </SourceRect>");
					writer.WriteLine ("      <IsActive>true</IsActive>");
					writer.WriteLine ("      <Effects />");
					writer.WriteLine ("      <FadeEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine ("        <Increase>true</Increase>");
					writer.WriteLine ("      </FadeEffect>");
					writer.WriteLine ("      <SpriteSheetEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine ("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine ("        <CurrentFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </CurrentFrame>");
					writer.WriteLine ("        <AmountOfFrames>");
					writer.WriteLine ("          <X>3</X>");
					writer.WriteLine ("          <Y>4</Y>");
					writer.WriteLine ("        </AmountOfFrames>");
					writer.WriteLine ("        <DefaultFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </DefaultFrame>");
					writer.WriteLine ("      </SpriteSheetEffect>");
					writer.WriteLine ("    </Image>");


					writer.WriteLine ("    <InteractionImage>");
					writer.WriteLine ("      <Alpha>1</Alpha>");
					writer.WriteLine ("      <Text />");
					writer.WriteLine ("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine ("      <Path>Gameplay/NPCs/bartender_question</Path>");

					// needs to be the position on the map
					writer.WriteLine ("      <Position>");
					writer.WriteLine ("        <X>1568</X>");
					writer.WriteLine ("        <Y>960</Y>");
					writer.WriteLine ("      </Position>");
					writer.WriteLine ("      <Scale>");
					writer.WriteLine ("        <X>1</X>");
					writer.WriteLine ("        <Y>1</Y>");
					writer.WriteLine ("      </Scale>");


					writer.WriteLine ("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine ("        <X>1568</X>");
					writer.WriteLine ("        <Y>960</Y>");
					writer.WriteLine ("        <Width>640</Width>");
					writer.WriteLine ("        <Height>256</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine ("        <Location>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </Location>");
					writer.WriteLine ("      </SourceRect>");
					writer.WriteLine ("      <IsActive>true</IsActive>");
					writer.WriteLine ("      <Effects />");
					writer.WriteLine ("      <FadeEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine ("        <Increase>true</Increase>");
					writer.WriteLine ("      </FadeEffect>");
					writer.WriteLine ("      <SpriteSheetEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine ("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine ("        <CurrentFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </CurrentFrame>");
					writer.WriteLine ("        <AmountOfFrames>");
					writer.WriteLine ("          <X>3</X>");
					writer.WriteLine ("          <Y>4</Y>");
					writer.WriteLine ("        </AmountOfFrames>");
					writer.WriteLine ("        <DefaultFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </DefaultFrame>");
					writer.WriteLine ("      </SpriteSheetEffect>");
					writer.WriteLine ("    </InteractionImage>");



					writer.WriteLine ("    <MissionCompleteInteractionImage>");
					writer.WriteLine ("      <Alpha>1</Alpha>");
					writer.WriteLine ("      <Text />");
					writer.WriteLine ("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine ("      <Path>Gameplay/NPCs/bartender_mission_complete</Path>");

					// needs to be the position on the map
					writer.WriteLine ("      <Position>");
					writer.WriteLine ("        <X>1568</X>");
					writer.WriteLine ("        <Y>960</Y>");
					writer.WriteLine ("      </Position>");
					writer.WriteLine ("      <Scale>");
					writer.WriteLine ("        <X>1</X>");
					writer.WriteLine ("        <Y>1</Y>");
					writer.WriteLine ("      </Scale>");


					writer.WriteLine ("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine ("        <X>1568</X>");
					writer.WriteLine ("        <Y>960</Y>");
					writer.WriteLine ("        <Width>640</Width>");
					writer.WriteLine ("        <Height>256</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine ("        <Location>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </Location>");
					writer.WriteLine ("      </SourceRect>");
					writer.WriteLine ("      <IsActive>true</IsActive>");
					writer.WriteLine ("      <Effects />");
					writer.WriteLine ("      <FadeEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine ("        <Increase>true</Increase>");
					writer.WriteLine ("      </FadeEffect>");
					writer.WriteLine ("      <SpriteSheetEffect>");
					writer.WriteLine ("        <IsActive>false</IsActive>");
					writer.WriteLine ("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine ("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine ("        <CurrentFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </CurrentFrame>");
					writer.WriteLine ("        <AmountOfFrames>");
					writer.WriteLine ("          <X>3</X>");
					writer.WriteLine ("          <Y>4</Y>");
					writer.WriteLine ("        </AmountOfFrames>");
					writer.WriteLine ("        <DefaultFrame>");
					writer.WriteLine ("          <X>0</X>");
					writer.WriteLine ("          <Y>0</Y>");
					writer.WriteLine ("        </DefaultFrame>");
					writer.WriteLine ("      </SpriteSheetEffect>");
					writer.WriteLine ("    </MissionCompleteInteractionImage>");


					writer.WriteLine ("    <CurrentlyInteracting>false</CurrentlyInteracting>");
					// needs to be the position on the map
					writer.WriteLine ("    <MapPosition>");
					writer.WriteLine ("      <X>1536</X>");
					writer.WriteLine ("      <Y>256</Y>");
					writer.WriteLine ("    </MapPosition>");
					writer.WriteLine ("    <BoundingBoxDimensions>");
					writer.WriteLine ("      <X>128</X>");
					writer.WriteLine ("      <Y>256</Y>");
					writer.WriteLine ("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine ("    <BoundingBoxPosition>");
					writer.WriteLine ("      <X>1536</X>");
					writer.WriteLine ("      <Y>256</Y>");
					writer.WriteLine ("    </BoundingBoxPosition>");

					writer.WriteLine ("    <InteractionBoundingBoxDimensions>");
					writer.WriteLine ("      <X>640</X>");
					writer.WriteLine ("      <Y>256</Y>");
					writer.WriteLine ("    </InteractionBoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine ("    <InteractionBoundingBoxPosition>");
					writer.WriteLine ("      <X>1568</X>");
					writer.WriteLine ("      <Y>960</Y>");
					writer.WriteLine ("    </InteractionBoundingBoxPosition>");

					writer.WriteLine ("    <State>Solid</State>");
					writer.WriteLine ("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasMission>true</HasMission>");
					writer.WriteLine ("    <MissionCompleted>false</MissionCompleted>");
					writer.WriteLine ("    <MissionSongismName>Firewater (Slo-mo)</MissionSongismName>");
					writer.WriteLine ("    <MissionReward>");
					writer.WriteLine ("      <Name>RV Keys</Name>");
					writer.WriteLine ("    </MissionReward>");
					writer.WriteLine ("    <IsActive>true</IsActive>");
					writer.WriteLine ("  </NPC>");

					writer.WriteLine ("</ArrayOfNPC>");

					writer.Flush ();
				}
			}
		}

		private void CheckForDowntownLosAngelesSongismsDemo()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfSongism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");


					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Frolic Room</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/frolic_room_dark</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>448</X>");
					writer.WriteLine("        <Y>64</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>448</X>");
					writer.WriteLine("        <Y>64</Y>");
					writer.WriteLine("        <Width>256</Width>");
					writer.WriteLine("        <Height>128</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>448</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>256</X>");
					writer.WriteLine("      <Y>128</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>448</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
					writer.WriteLine("    <SongInfo>Frolic Room is a bar;located on Hollywood Blvd in LA;Both Frolic Room and;Hollywood Blvd are mentioned;in the song Guns (Are;For Pussies) on the;blue album;more stuff to take up;some space on the;screen.......;311's got the boom ya'll!</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");


					writer.WriteLine("</ArrayOfSongism>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForDowntownLosAngelesMapEntrancesDemo()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "downtown_los_angeles.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplayMapEntrancesDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplayMapEntrancesDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfMapEntrance xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					// Entrance to Frolic Room
					writer.WriteLine("  <MapEntrance>");
					writer.WriteLine("    <Name>Frolic Room</Name>");
					writer.WriteLine ("    <HasImage>true</HasImage>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Maps/frolic_room_map_entrance</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>448</X>");
					writer.WriteLine("        <Y>192</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>448</X>");
					writer.WriteLine("        <Y>192</Y>");
					writer.WriteLine("        <Width>256</Width>");
					writer.WriteLine("        <Height>128</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");

					writer.WriteLine ("    <InteractionType>Click</InteractionType>");
					writer.WriteLine("    <Locked>false</Locked>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>448</X>");
					writer.WriteLine("      <Y>192</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>256</X>");
					writer.WriteLine("      <Y>128</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>448</X>");
					writer.WriteLine("      <Y>192</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <NextMapName>frolic_room</NextMapName>");
					writer.WriteLine("  </MapEntrance>");




					// Entrance to another part of the street
					writer.WriteLine("  <MapEntrance>");
					writer.WriteLine("    <Name>Downtown Los Angeles Part 2</Name>");
					writer.WriteLine ("    <HasImage>false</HasImage>");
					writer.WriteLine("    <Image />");

					writer.WriteLine ("    <InteractionType>PlayerEnter</InteractionType>");
					writer.WriteLine("    <Locked>false</Locked>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>832</X>");
					writer.WriteLine("      <Y>320</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>256</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>832</X>");
					writer.WriteLine("      <Y>320</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <NextMapName>los_angeles_2</NextMapName>");
					writer.WriteLine("  </MapEntrance>");

					writer.WriteLine("</ArrayOfMapEntrance>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForDowntownLosAngeles2MapEntrances()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "los_angeles_2.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "los_angeles_2.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "los_angeles_2.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplayMapEntrancesDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplayMapEntrancesDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "los_angeles_2.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfMapEntrance xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					// Entrance to Downtown Los Angeles
					writer.WriteLine("  <MapEntrance>");
					writer.WriteLine("    <Name>Downtown Los Angeles</Name>");
					writer.WriteLine ("    <HasImage>false</HasImage>");
					writer.WriteLine("    <Image />");

					writer.WriteLine ("    <InteractionType>PlayerEnter</InteractionType>");
					writer.WriteLine("    <Locked>false</Locked>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>0</X>");
					writer.WriteLine("      <Y>0</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>200</X>");
					writer.WriteLine("      <Y>800</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>0</X>");
					writer.WriteLine("      <Y>0</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <NextMapName>downtown_los_angeles</NextMapName>");
					writer.WriteLine("  </MapEntrance>");

					writer.WriteLine("</ArrayOfMapEntrance>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForDowntownLosAngeles2Songisms()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "los_angeles_2.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "los_angeles_2.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "los_angeles_2.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "los_angeles_2.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfSongism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Firewater (Slo-mo)</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/firewater</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>2048</X>");
					writer.WriteLine("        <Y>328</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>2048</X>");
					writer.WriteLine("        <Y>328</Y>");
					writer.WriteLine("        <Width>64</Width>");
					writer.WriteLine("        <Height>64</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>2048</X>");
					writer.WriteLine("      <Y>328</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>64</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>2048</X>");
					writer.WriteLine("      <Y>328</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Enlarged to Show Detail</AlbumName>");
					writer.WriteLine("    <SongInfo>Dex is looking at something;out the window.;It might be a chipmunk</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");





					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Same Mistake Twice</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/same_mistake_twice_locked</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>800</X>");
					writer.WriteLine("        <Y>140</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>800</X>");
					writer.WriteLine("        <Y>140</Y>");
					writer.WriteLine("        <Width>320</Width>");
					writer.WriteLine("        <Height>180</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>800</X>");
					writer.WriteLine("      <Y>140</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>320</X>");
					writer.WriteLine("      <Y>180</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>800</X>");
					writer.WriteLine("      <Y>140</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
					writer.WriteLine("    <SongInfo>Check it</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>true</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism>Amber</PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>true</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem>Beer Goggles</PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");

					writer.WriteLine("</ArrayOfSongism>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForFrolicRoomSongismsDemo()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "frolic_room.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "frolic_room.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "frolic_room.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "frolic_room.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfSongism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Amber</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/amber</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>512</X>");
					writer.WriteLine("        <Y>128</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>512</X>");
					writer.WriteLine("        <Y>128</Y>");
					writer.WriteLine("        <Width>64</Width>");
					writer.WriteLine("        <Height>64</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>512</X>");
					writer.WriteLine("      <Y>128</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>64</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>512</X>");
					writer.WriteLine("      <Y>128</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
					writer.WriteLine("    <SongInfo>Whoa! Amber is the color of my ale...</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward>");
					writer.WriteLine ("      <Name>Beer Goggles</Name>");
					writer.WriteLine ("    </InventoryReward>");
					writer.WriteLine("  </Songism>");





					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Freeze Time</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/freeze_time</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>800</X>");
					writer.WriteLine("        <Y>32</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");

					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>800</X>");
					writer.WriteLine("        <Y>32</Y>");
					writer.WriteLine("        <Width>108</Width>");
					writer.WriteLine("        <Height>136</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");

					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>800</X>");
					writer.WriteLine("      <Y>32</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>108</X>");
					writer.WriteLine("      <Y>136</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>800</X>");
					writer.WriteLine("      <Y>32</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
					writer.WriteLine("    <SongInfo>Golden Butter Jam</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");

					writer.WriteLine("</ArrayOfSongism>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForFrolicRoomMapEntrancesDemo()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "frolic_room.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "frolic_room.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "frolic_room.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplayMapEntrancesDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplayMapEntrancesDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayMapEntrancesDirectory, "frolic_room.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfMapEntrance xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

					// Entrance to Downtown Los Angeles
					writer.WriteLine("  <MapEntrance>");
					writer.WriteLine("    <Name>Downtown Los Angeles</Name>");
					writer.WriteLine ("    <HasImage>false</HasImage>");
					writer.WriteLine("    <Image />");

					writer.WriteLine ("    <InteractionType>PlayerEnter</InteractionType>");
					writer.WriteLine("    <Locked>false</Locked>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>0</X>");
					writer.WriteLine("      <Y>0</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>200</X>");
					writer.WriteLine("      <Y>800</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>0</X>");
					writer.WriteLine("      <Y>0</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <NextMapName>downtown_los_angeles</NextMapName>");
					writer.WriteLine("  </MapEntrance>");

					writer.WriteLine("</ArrayOfMapEntrance>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CheckForDowntownLosAngelesSongisms()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfSongism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");


					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Frolic Room</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/frolic_room_dark</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>1632</X>");
					writer.WriteLine("        <Y>960</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");


					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>1632</X>");
					writer.WriteLine("        <Y>960</Y>");
					writer.WriteLine("        <Width>210</Width>");
					writer.WriteLine("        <Height>230</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");
					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>1632</X>");
					writer.WriteLine("      <Y>960</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>210</X>");
					writer.WriteLine("      <Y>230</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>1632</X>");
					writer.WriteLine("      <Y>960</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
					writer.WriteLine("    <SongInfo>Frolic Room is a bar;located on Hollywood Blvd in LA;Both Frolic Room and;Hollywood Blvd are mentioned;in the song Guns (Are;For Pussies) on the;blue album;more stuff to take up;some space on the;screen.......;311's got the boom ya'll!</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>true</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism>Firewater (Slo-mo)</PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>true</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem>RV Keys</PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");






					writer.WriteLine("  <Songism>");
					writer.WriteLine("    <Name>Firewater (Slo-mo)</Name>");
					writer.WriteLine("    <Image>");
					writer.WriteLine("      <Alpha>1</Alpha>");
					writer.WriteLine("      <Text />");
					writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
					writer.WriteLine("      <Path>Gameplay/Songisms/firewater</Path>");

					// needs to be the position on the map
					writer.WriteLine("      <Position>");
					writer.WriteLine("        <X>2304</X>");
					writer.WriteLine("        <Y>864</Y>");
					writer.WriteLine("      </Position>");
					writer.WriteLine("      <Scale>");
					writer.WriteLine("        <X>1</X>");
					writer.WriteLine("        <Y>1</Y>");
					writer.WriteLine("      </Scale>");


					writer.WriteLine("      <SourceRect>");
					// needs to be the position on the map
					writer.WriteLine("        <X>2304</X>");
					writer.WriteLine("        <Y>864</Y>");
					writer.WriteLine("        <Width>64</Width>");
					writer.WriteLine("        <Height>64</Height>");
					// 0, 0 or the image will shift
					writer.WriteLine("        <Location>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </Location>");
					writer.WriteLine("      </SourceRect>");
					writer.WriteLine("      <IsActive>true</IsActive>");
					writer.WriteLine("      <Effects />");
					writer.WriteLine("      <FadeEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
					writer.WriteLine("        <Increase>true</Increase>");
					writer.WriteLine("      </FadeEffect>");
					writer.WriteLine("      <SpriteSheetEffect>");
					writer.WriteLine("        <IsActive>false</IsActive>");
					writer.WriteLine("        <FrameCounter>96</FrameCounter>");
					writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
					writer.WriteLine("        <CurrentFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </CurrentFrame>");
					writer.WriteLine("        <AmountOfFrames>");
					writer.WriteLine("          <X>3</X>");
					writer.WriteLine("          <Y>4</Y>");
					writer.WriteLine("        </AmountOfFrames>");
					writer.WriteLine("        <DefaultFrame>");
					writer.WriteLine("          <X>0</X>");
					writer.WriteLine("          <Y>0</Y>");
					writer.WriteLine("        </DefaultFrame>");
					writer.WriteLine("      </SpriteSheetEffect>");
					writer.WriteLine("    </Image>");
					writer.WriteLine("    <Discovered>false</Discovered>");
					// needs to be the position on the map
					writer.WriteLine("    <MapPosition>");
					writer.WriteLine("      <X>2304</X>");
					writer.WriteLine("      <Y>864</Y>");
					writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
					writer.WriteLine("      <X>64</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("    </BoundingBoxDimensions>");
					// needs to be the position on the map
					writer.WriteLine("    <BoundingBoxPosition>");
					writer.WriteLine("      <X>2304</X>");
					writer.WriteLine("      <Y>864</Y>");
					writer.WriteLine("    </BoundingBoxPosition>");
					writer.WriteLine("    <State>Solid</State>");
					writer.WriteLine("    <AlbumName>Enlarged To Show Detail</AlbumName>");
					writer.WriteLine("    <SongInfo>Firewater is a pretty;bad-ass song!</SongInfo>");
					writer.WriteLine("    <LevelNumber>1</LevelNumber>");
					writer.WriteLine ("    <HasPrerequisiteSongism>false</HasPrerequisiteSongism>");
					writer.WriteLine ("    <PrerequisiteSongism></PrerequisiteSongism>");
					writer.WriteLine ("    <HasPrerequisiteInventoryItem>false</HasPrerequisiteInventoryItem>");
					writer.WriteLine ("    <PrerequisiteInventoryItem></PrerequisiteInventoryItem>");
					writer.WriteLine ("    <InventoryReward />");
					writer.WriteLine("  </Songism>");



					/*
					writer.WriteLine("  <Songism>");
                    writer.WriteLine("    <Name>Time Bomb</Name>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Alpha>1</Alpha>");
                    writer.WriteLine("      <Text />");
                    writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
                    writer.WriteLine("      <Path>Gameplay/Songisms/timebomb</Path>");
                    writer.WriteLine("      <Position>");
                    writer.WriteLine("        <X>128</X>");
                    writer.WriteLine("        <Y>128</Y>");
                    writer.WriteLine("      </Position>");
                    writer.WriteLine("      <Scale>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>1</Y>");
                    writer.WriteLine("      </Scale>");
                    writer.WriteLine("      <SourceRect>");
                    writer.WriteLine("        <X>0</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("        <Width>128</Width>");
                    writer.WriteLine("        <Height>128</Height>");
                    writer.WriteLine("        <Location>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </Location>");
                    writer.WriteLine("      </SourceRect>");
                    writer.WriteLine("      <IsActive>true</IsActive>");
                    writer.WriteLine("      <Effects />");
                    writer.WriteLine("      <FadeEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
                    writer.WriteLine("        <Increase>true</Increase>");
                    writer.WriteLine("      </FadeEffect>");
                    writer.WriteLine("      <SpriteSheetEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FrameCounter>96</FrameCounter>");
                    writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
                    writer.WriteLine("        <CurrentFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </CurrentFrame>");
                    writer.WriteLine("        <AmountOfFrames>");
                    writer.WriteLine("          <X>3</X>");
                    writer.WriteLine("          <Y>4</Y>");
                    writer.WriteLine("        </AmountOfFrames>");
                    writer.WriteLine("        <DefaultFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </DefaultFrame>");
                    writer.WriteLine("      </SpriteSheetEffect>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <MapPosition>");
                    writer.WriteLine("      <X>128</X>");
                    writer.WriteLine("      <Y>128</Y>");
                    writer.WriteLine("    </MapPosition>");
                    writer.WriteLine("    <BoundingBoxDimensions>");
                    writer.WriteLine("      <X>128</X>");
                    writer.WriteLine("      <Y>128</Y>");
                    writer.WriteLine("    </BoundingBoxDimensions>");
                    writer.WriteLine("    <BoundingBoxPosition>");
                    writer.WriteLine("      <X>128</X>");
                    writer.WriteLine("      <Y>128</Y>");
                    writer.WriteLine("    </BoundingBoxPosition>");
                    writer.WriteLine("    <State>Solid</State>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <SongInfo>Time Bomb is the first track on Uplifter</SongInfo>");
					writer.WriteLine("    <LevelNumber>Time Bomb is the first track on Uplifter</LevelNumber>");
                    writer.WriteLine("  </Songism>");
                    

					writer.WriteLine("  <Songism>");
                    writer.WriteLine("    <Name>Down</Name>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Alpha>1</Alpha>");
                    writer.WriteLine("      <Text />");
                    writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
                    writer.WriteLine("      <Path>Gameplay/Songisms/down</Path>");
                    writer.WriteLine("      <Position>");
                    writer.WriteLine("        <X>256</X>");
                    writer.WriteLine("        <Y>256</Y>");
                    writer.WriteLine("      </Position>");
                    writer.WriteLine("      <Scale>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>1</Y>");
                    writer.WriteLine("      </Scale>");
                    writer.WriteLine("      <SourceRect>");
                    writer.WriteLine("        <X>0</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("        <Width>64</Width>");
                    writer.WriteLine("        <Height>64</Height>");
                    writer.WriteLine("        <Location>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </Location>");
                    writer.WriteLine("      </SourceRect>");
                    writer.WriteLine("      <IsActive>true</IsActive>");
                    writer.WriteLine("      <Effects />");
                    writer.WriteLine("      <FadeEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
                    writer.WriteLine("        <Increase>true</Increase>");
                    writer.WriteLine("      </FadeEffect>");
                    writer.WriteLine("      <SpriteSheetEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FrameCounter>96</FrameCounter>");
                    writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
                    writer.WriteLine("        <CurrentFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </CurrentFrame>");
                    writer.WriteLine("        <AmountOfFrames>");
                    writer.WriteLine("          <X>3</X>");
                    writer.WriteLine("          <Y>4</Y>");
                    writer.WriteLine("        </AmountOfFrames>");
                    writer.WriteLine("        <DefaultFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </DefaultFrame>");
                    writer.WriteLine("      </SpriteSheetEffect>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <MapPosition>");
                    writer.WriteLine("      <X>256</X>");
                    writer.WriteLine("      <Y>256</Y>");
                    writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
                    writer.WriteLine("      <X>64</X>");
                    writer.WriteLine("      <Y>64</Y>");
                    writer.WriteLine("    </BoundingBoxDimensions>");
                    writer.WriteLine("    <BoundingBoxPosition>");
                    writer.WriteLine("      <X>256</X>");
                    writer.WriteLine("      <Y>256</Y>");
                    writer.WriteLine("    </BoundingBoxPosition>");
                    writer.WriteLine("    <State>Solid</State>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <SongInfo>Down is an awesome song</SongInfo>");
					writer.WriteLine("    <LevelNumber>Down is an awesome song</LevelNumber>");
                    writer.WriteLine("  </Songism>");
                    


					writer.WriteLine("  <Songism>");
                    writer.WriteLine("    <Name>Color</Name>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Alpha>1</Alpha>");
                    writer.WriteLine("      <Text />");
                    writer.WriteLine("      <FontName>Fonts/GameFont</FontName>");
                    writer.WriteLine("      <Path>Gameplay/Songisms/color</Path>");
                    writer.WriteLine("      <Position>");
                    writer.WriteLine("        <X>32</X>");
                    writer.WriteLine("        <Y>256</Y>");
                    writer.WriteLine("      </Position>");
                    writer.WriteLine("      <Scale>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>1</Y>");
                    writer.WriteLine("      </Scale>");
                    writer.WriteLine("      <SourceRect>");
                    writer.WriteLine("        <X>0</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("        <Width>64</Width>");
                    writer.WriteLine("        <Height>64</Height>");
                    writer.WriteLine("        <Location>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </Location>");
                    writer.WriteLine("      </SourceRect>");
                    writer.WriteLine("      <IsActive>true</IsActive>");
                    writer.WriteLine("      <Effects />");
                    writer.WriteLine("      <FadeEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FadeSpeed>1</FadeSpeed>");
                    writer.WriteLine("        <Increase>true</Increase>");
                    writer.WriteLine("      </FadeEffect>");
                    writer.WriteLine("      <SpriteSheetEffect>");
                    writer.WriteLine("        <IsActive>false</IsActive>");
                    writer.WriteLine("        <FrameCounter>96</FrameCounter>");
                    writer.WriteLine("        <SwitchFrame>100</SwitchFrame>");
                    writer.WriteLine("        <CurrentFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </CurrentFrame>");
                    writer.WriteLine("        <AmountOfFrames>");
                    writer.WriteLine("          <X>3</X>");
                    writer.WriteLine("          <Y>4</Y>");
                    writer.WriteLine("        </AmountOfFrames>");
                    writer.WriteLine("        <DefaultFrame>");
                    writer.WriteLine("          <X>0</X>");
                    writer.WriteLine("          <Y>0</Y>");
                    writer.WriteLine("        </DefaultFrame>");
                    writer.WriteLine("      </SpriteSheetEffect>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <MapPosition>");
                    writer.WriteLine("      <X>32</X>");
                    writer.WriteLine("      <Y>256</Y>");
                    writer.WriteLine("    </MapPosition>");
					writer.WriteLine("    <BoundingBoxDimensions>");
                    writer.WriteLine("      <X>64</X>");
                    writer.WriteLine("      <Y>64</Y>");
                    writer.WriteLine("    </BoundingBoxDimensions>");
                    writer.WriteLine("    <BoundingBoxPosition>");
                    writer.WriteLine("      <X>32</X>");
                    writer.WriteLine("      <Y>256</Y>");
                    writer.WriteLine("    </BoundingBoxPosition>");
                    writer.WriteLine("    <State>Solid</State>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <SongInfo>Color is an awesome song</SongInfo>");
					writer.WriteLine("    <LevelNumber>Color is an awesome song</LevelNumber>");
                    writer.WriteLine("  </Songism>");
                    */



					writer.WriteLine("</ArrayOfSongism>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

        private void CheckForCurrentSongism()
        {
            //if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml")))
            //{
            //    File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"));
            //}

            if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<Songism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    writer.WriteLine("</Songism>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForSongismScreen()
        {
            //if (File.Exists(Path.Combine(Globals.LoadGameplayScreensDirectory, "SongismScreen.xml")))
            //{
            //    File.Delete(Path.Combine(Globals.LoadGameplayScreensDirectory, "SongismScreen.xml"));
            //}

            if (!File.Exists(Path.Combine(Globals.LoadGameplayScreensDirectory, "SongismScreen.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplayScreensDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplayScreensDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayScreensDirectory, "SongismScreen.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<SongismScreen xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    writer.WriteLine("  <XmlPath>Load/SongismScreen.xml</XmlPath>");
                    //writer.WriteLine("  <Background>");
                    //writer.WriteLine("    <Alpha>1</Alpha>");
                    //writer.WriteLine("    <Text />");
                    //writer.WriteLine("    <FontName>Fonts/GameFont</FontName>");
                    //writer.WriteLine("    <Path>Gameplay/UI/dialogbackground</Path>");
                    //writer.WriteLine("    <Position>");
                    //writer.WriteLine("      <X>0</X>");
                    //writer.WriteLine("      <Y>0</Y>");
                    //writer.WriteLine("    </Position>");
                    //writer.WriteLine("    <Scale>");
                    //writer.WriteLine("      <X>1</X>");
                    //writer.WriteLine("      <Y>1</Y>");
                    //writer.WriteLine("    </Scale>");
                    //writer.WriteLine("    <SourceRect>");
                    //writer.WriteLine("      <X>0</X>");
                    //writer.WriteLine("      <Y>0</Y>");
                    //writer.WriteLine("      <Width>640</Width>");
                    //writer.WriteLine("      <Height>480</Height>");
                    //writer.WriteLine("      <Location>");
                    //writer.WriteLine("        <X>0</X>");
                    //writer.WriteLine("        <Y>0</Y>");
                    //writer.WriteLine("      </Location>");
                    //writer.WriteLine("    </SourceRect>");
                    //writer.WriteLine("    <IsActive>true</IsActive>");
                    //writer.WriteLine("    <Effects />");
                    //writer.WriteLine("    <FadeEffect>");
                    //writer.WriteLine("      <IsActive>false</IsActive>");
                    //writer.WriteLine("      <FadeSpeed>1</FadeSpeed>");
                    //writer.WriteLine("      <Increase>true</Increase>");
                    //writer.WriteLine("    </FadeEffect>");
                    //writer.WriteLine("    <SpriteSheetEffect>");
                    //writer.WriteLine("      <IsActive>false</IsActive>");
                    //writer.WriteLine("      <FrameCounter>0</FrameCounter>");
                    //writer.WriteLine("      <SwitchFrame>100</SwitchFrame>");
                    //writer.WriteLine("      <CurrentFrame>");
                    //writer.WriteLine("        <X>0</X>");
                    //writer.WriteLine("        <Y>0</Y>");
                    //writer.WriteLine("      </CurrentFrame>");
                    //writer.WriteLine("      <AmountOfFrames>");
                    //writer.WriteLine("        <X>3</X>");
                    //writer.WriteLine("        <Y>4</Y>");
                    //writer.WriteLine("      </AmountOfFrames>");
                    //writer.WriteLine("      <DefaultFrame>");
                    //writer.WriteLine("        <X>0</X>");
                    //writer.WriteLine("        <Y>0</Y>");
                    //writer.WriteLine("      </DefaultFrame>");
                    //writer.WriteLine("    </SpriteSheetEffect>");
                    //writer.WriteLine("  </Background>");
                    writer.WriteLine("  <CurrentSongismImage>");
                    writer.WriteLine("    <Alpha>1</Alpha>");
                    writer.WriteLine("    <Text />");
                    writer.WriteLine("    <FontName>Fonts/GameFont</FontName>");
                    writer.WriteLine("    <Path>Gameplay/Songisms/timebomb</Path>");
                    writer.WriteLine("    <Position>");
                    writer.WriteLine("      <X>448</X>");
                    writer.WriteLine("      <Y>216</Y>");
                    writer.WriteLine("    </Position>");
                    writer.WriteLine("    <Scale>");
                    writer.WriteLine("      <X>9</X>");
                    writer.WriteLine("      <Y>9</Y>");
                    writer.WriteLine("    </Scale>");
                    writer.WriteLine("    <SourceRect>");
                    writer.WriteLine("      <X>0</X>");
                    writer.WriteLine("      <Y>0</Y>");
                    writer.WriteLine("      <Width>128</Width>");
                    writer.WriteLine("      <Height>128</Height>");
                    writer.WriteLine("      <Location>");
                    writer.WriteLine("        <X>0</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("      </Location>");
                    writer.WriteLine("    </SourceRect>");
                    writer.WriteLine("    <IsActive>false</IsActive>");
                    writer.WriteLine("    <Effects />");
                    writer.WriteLine("    <FadeEffect>");
                    writer.WriteLine("      <IsActive>false</IsActive>");
                    writer.WriteLine("      <FadeSpeed>1</FadeSpeed>");
                    writer.WriteLine("      <Increase>true</Increase>");
                    writer.WriteLine("    </FadeEffect>");
                    writer.WriteLine("    <SpriteSheetEffect>");
                    writer.WriteLine("      <IsActive>false</IsActive>");
                    writer.WriteLine("      <FrameCounter>0</FrameCounter>");
                    writer.WriteLine("      <SwitchFrame>100</SwitchFrame>");
                    writer.WriteLine("      <CurrentFrame>");
                    writer.WriteLine("        <X>1</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("      </CurrentFrame>");
                    writer.WriteLine("      <AmountOfFrames>");
                    writer.WriteLine("        <X>3</X>");
                    writer.WriteLine("        <Y>4</Y>");
                    writer.WriteLine("      </AmountOfFrames>");
                    writer.WriteLine("      <DefaultFrame>");
                    writer.WriteLine("        <X>0</X>");
                    writer.WriteLine("        <Y>0</Y>");
                    writer.WriteLine("      </DefaultFrame>");
                    writer.WriteLine("    </SpriteSheetEffect>");
                    writer.WriteLine("  </CurrentSongismImage>");
                    writer.WriteLine("</SongismScreen>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForSplashScreen()
        {
            if (!File.Exists(Path.Combine(Globals.LoadGameplayScreensDirectory, "SplashScreen.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplayScreensDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplayScreensDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayScreensDirectory, "SplashScreen.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    writer.WriteLine("<SplashScreen>");
                    writer.WriteLine("  <Image>");
                    writer.WriteLine("    <Path>SplashScreen/SplashScreen</Path>");
                    writer.WriteLine("    <IsActive>true</IsActive>");
                    writer.WriteLine("    <Alpha>0.5</Alpha>");
                    writer.WriteLine("  </Image>");
                    writer.WriteLine("</SplashScreen>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForInventory()
        {
            if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "Inventory.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplayDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplayDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayDirectory, "Inventory.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForSongBook()
        {
            //if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml")))
            //{
            //    File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml"));
            //}

            if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplayDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplayDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    writer.WriteLine("<ArrayOfSongism xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
                    writer.WriteLine("</ArrayOfSongism>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

		private void CheckForInventoryItems()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "InventoryItems.xml")))
			{
			    File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "InventoryItems.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "InventoryItems.xml")))
			{
				if (!Directory.Exists(Globals.LoadGameplayDirectory))
				{
					Directory.CreateDirectory(Globals.LoadGameplayDirectory);
				}

				FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplayDirectory, "InventoryItems.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfInventoryItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
					writer.WriteLine("</ArrayOfInventoryItem>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

        private void CheckForTitleMenu()
        {
			if (!File.Exists(Path.Combine(Globals.LoadMenusDirectory, "TitleMenu.xml")))
            {
                if (!Directory.Exists(Globals.LoadMenusDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadMenusDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadMenusDirectory, "TitleMenu.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    writer.WriteLine("<Menu>");
                    writer.WriteLine("  <Axis>Y</Axis>");
                    //writer.WriteLine("  <Effects>FadeEffect</Effects>");
                    writer.WriteLine("  <Item>");
                    writer.WriteLine("    <LinkType>Screen</LinkType>");
                    writer.WriteLine("    <LinkID>GameplayScreen</LinkID>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Text>New Game</Text>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("  </Item>");
                    writer.WriteLine("");
                    writer.WriteLine("  <Item>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Text>Load Game</Text>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("  </Item>");
                    writer.WriteLine("");
                    writer.WriteLine("  <Item>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Text>Options</Text>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("  </Item>");
                    writer.WriteLine("");
                    writer.WriteLine("  <Item>");
                    writer.WriteLine("    <Image>");
                    writer.WriteLine("      <Text>Credits</Text>");
                    writer.WriteLine("    </Image>");
                    writer.WriteLine("  </Item>");
                    writer.WriteLine("</Menu>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }

        private void CheckForGuessingItems()
        {
            if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml")))
            {
                File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml"));
            }

            if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml")))
            {
                if (!Directory.Exists(Globals.LoadGameplaySongismsDirectory))
                {
                    Directory.CreateDirectory(Globals.LoadGameplaySongismsDirectory);
                }

                FileStream fs = new FileStream(Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml"), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

                using (StreamWriter writer = new StreamWriter(fs))
                {
					writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
					writer.WriteLine("<ArrayOfSongismGuessingItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");



					/* Music */
					writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Welcome</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Freak Out</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Visit</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Paradise</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Unity</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Hydroponic</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>My Stoney Baby</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Nix Hex</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Plain</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Feels So Good</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Do You Right</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Fat Chance</SongName>");
                    writer.WriteLine("    <AlbumName>Music</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    

					/* Grassroots */
					writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Homebrew</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Lucky</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Nutsymptom</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>8:16 A.M.</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Omaha Stylee</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Applied Science</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Taiyed</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Silver</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Grassroots</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Salsa</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Lose</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Six</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Offbeat Bare-Ass</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>1,2,3</SongName>");
                    writer.WriteLine("    <AlbumName>Grassroots</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* 311 */
					writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Down</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Random</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Jackolantern's Weather</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>All Mixed Up</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Hive</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Guns (Are For Pussies)</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Misdirected Hostility</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Purpose</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Loco</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Brodels</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Don't Stay Home</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>DLMD</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sweet</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>T &amp; P Combo</SongName>");
                    writer.WriteLine("    <AlbumName>311</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    

					/*
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Who's Got The Herb?</SongName>");
                    writer.WriteLine("    <AlbumName>Hempilation</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
                    writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Outside</SongName>");
                    writer.WriteLine("    <AlbumName>National Lampoon's Senior Trip</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
                    writer.WriteLine("  </SongismGuessingItem>");
					*/

					/* Enlarged To Show Detail */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Tribute</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Let the Cards Fall</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Gap</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Firewater (Slo-mo)</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Transistor */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Transistor</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Prisoner</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Galaxy</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Beautiful Disaster</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Inner Light Spectrum</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Electricity</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>What Was I Thinking</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Jupiter</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Use of Time</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>The Continuous Life</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>No Control</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Running</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Color</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Light Years</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Creature Feature</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Tune In</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Rub a Dub</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Starshines</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Strangers</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Borders</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Stealing Happy Hours</SongName>");
                    writer.WriteLine("    <AlbumName>Transistor</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Omaha Sessions */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Soulsucker (New Mix)</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Today My Love (New Mix)</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Slinky (New Mix)</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Summer of Love</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Damn</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Down South</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Rollin'</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Right Now</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>This Too Shall Pass (New Mix)</SongName>");
                    writer.WriteLine("    <AlbumName>Omaha Sessions</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Soundsystem */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Freeze Time</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Come Original</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Large in the Margin</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Flowing</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Can't Fade Me</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Life's Not a Race</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Strong All Along</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sever</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Eons</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Evolution</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Leaving Babylon</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Mindspin</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Livin' &amp; Rockin'</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Blizza</SongName>");
                    writer.WriteLine("    <AlbumName>Soundsystem</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* From Chaos */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>You Get Worked</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sick Tight</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>You Wouldn't Believe</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Full Ride</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>From Chaos</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>I Told Myself</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Champagne</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Hostile Apostle</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Wake Your Mind Up</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Amber</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Uncalm</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>I'll Be Here Awhile</SongName>");
                    writer.WriteLine("    <AlbumName>From Chaos</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Enlarged To Show Detail 2 */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Dancehall</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Bomb the Town</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Will the World</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>We Do it Like This</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Dreamland</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>I'll Be Here Awhile (Acoustic)</SongName>");
                    writer.WriteLine("    <AlbumName>Enlarged to Show Detail 2</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Evolver */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Creatures (For a While)</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Reconsider Everything</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Crack the Code</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Same Mistake Twice</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Beyond the Gray Sky</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Seems Uncertain</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Still Dreaming</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Give Me a Call</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Don't Dwell</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Other Side of Things</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sometimes Jacks Rule the Realm</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Coda</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>What Do You Do</SongName>");
                    writer.WriteLine("    <AlbumName>Evolver</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					
                    //writer.WriteLine("  <SongismGuessingItem>");
                    //writer.WriteLine("    <SongName>Time Is Precious</SongName>");
                    //writer.WriteLine("    <AlbumName>311.com</AlbumName>");
                    //writer.WriteLine("    <Discovered>false</Discovered>");
                    //writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    //writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
                    //writer.WriteLine("  </SongismGuessingItem>");
					

					/* Greatest Hits */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Love Song</SongName>");
                    writer.WriteLine("    <AlbumName>Greatest Hits '93-'03</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>How Do You Feel?</SongName>");
                    writer.WriteLine("    <AlbumName>Greatest Hits '93-'03</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>First Straw</SongName>");
                    writer.WriteLine("    <AlbumName>Greatest Hits '93-'03</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Don't Tread on Me */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Don't Tread on Me</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Thank Your Lucky Stars</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Frolic Room</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Speak Easy</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Solar Flare</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Waiting</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Long for the Flowers</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Getting Through to Her</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Whiskey &amp; Wine</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>It's Getting OK Now</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>There's Always an Excuse</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Little Brother</SongName>");
                    writer.WriteLine("    <AlbumName>Don't Tread On Me</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Uplifter */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Hey You</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>It's Alright</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Mix It Up</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Golden Sunlight</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>India Ink</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Daisy Cutter</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Too Much Too Fast</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Never Ending Summer</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Two Drops In the Ocean</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Something Out of Nothing</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Jackpot</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>My Heart Sings</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>I Like the Way</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Get Down</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>How Long Has It Been</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sun Come Through</SongName>");
                    writer.WriteLine("    <AlbumName>Uplifter</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Universal Pulse */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Time Bomb</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Wild Nights</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sunset in July</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Trouble</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Count Me In</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Rock On</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Weightless</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>And a Ways to Go</SongName>");
                    writer.WriteLine("    <AlbumName>Universal Pulse</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


					/* Stereolithic */
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Ebb and Flow</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Five of Everything</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Showdown</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Revelation of the Year</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Sand Dollars</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Boom Shaka</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Make it Rough</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>The Great Divide</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Friday Afternoon</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Simple True</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>First Dimension</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Made in the Shade</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Existential Hero</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>The Call</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Tranquility</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
                    writer.WriteLine ("    <LastSongOnAlbum>false</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");
                    writer.WriteLine("  <SongismGuessingItem>");
                    writer.WriteLine("    <SongName>Vape 'N Away</SongName>");
                    writer.WriteLine("    <AlbumName>Stereolithic</AlbumName>");
                    writer.WriteLine("    <Discovered>false</Discovered>");
                    writer.WriteLine("    <DisplayItem>true</DisplayItem>");
					writer.WriteLine ("    <LastSongOnAlbum>true</LastSongOnAlbum>");
					writer.WriteLine("  </SongismGuessingItem>");


                    writer.WriteLine("</ArrayOfSongismGuessingItem>");

                    writer.Flush();
                }

                fs.Dispose();
            }
        }
    }
}
