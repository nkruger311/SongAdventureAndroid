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
			CheckForScreenManager();
            
			CheckForSongismScreen();
            CheckForSplashScreen();
            CheckForTitleMenu();
            CheckForGuessingItems();
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
