using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
	public class TitleScreen : GameScreen
	{
		//public Image Background;
		//public Image CurrentSongismImage;
		//Songism currentSongism;
		DialogButton _btnNewGame;
		DialogButton _btnLoadGame;
		//bool _isLeaving = false;
		//Image _songismName;

		//SongismGuessingItem _guessingItem;
		//SongismGuessingListBox _guessingList;

		public override void LoadContent()
		{
			base.LoadContent();

			InputManager.Instance.ResetInputState();

			/*
			 	We need to set both the PlayerPosition and Position to the same value.
			*/
			Camera2D.Instance.PlayerPosition = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2),
				(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2));
			Camera2D.Instance.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2),
				(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2));

			//LoadBackground();
			LoadButtons();
		}

		void LoadBackground()
		{
			//Background.LoadContent();
			//Background.DeactivateEffect("FadeEffect");
			//Background.DeactivateEffect("SpriteSheetEffect");
			//Background.Alpha = 1.0f;
			//Background.SourceRect = new Rectangle(0, 0, 640, 480);
		}
			
		void LoadButtons()
		{
			_btnNewGame = new DialogButton(true);
			_btnLoadGame = new DialogButton(CheckForExistingGameFiles());

			Image newGameImage = new Image();
			newGameImage.Path = "Gameplay/UI/buttonsheet";
			newGameImage.Effects = "SpriteSheetEffect";
			newGameImage.FontName = "Fonts/GameFont_Size32";
			newGameImage.TextAnimationTravel = new Vector2(0, 9);
			newGameImage.TextAlignment = Globals.TextAlignment.Center;

			newGameImage.LoadContent();

			Image loadGameImage = new Image();
			loadGameImage.Path = "Gameplay/UI/buttonsheet";
			loadGameImage.Effects = "SpriteSheetEffect";
			loadGameImage.FontName = "Fonts/GameFont_Size32";
			loadGameImage.TextAnimationTravel = new Vector2(0, 9);
			loadGameImage.TextAlignment = Globals.TextAlignment.Center;

			if (!CheckForExistingGameFiles())
				loadGameImage.TextColor = Color.Gray;

			loadGameImage.LoadContent();

			_btnNewGame.ButtonName = "_btnNewGame";
			_btnNewGame.Image = newGameImage;

			newGameImage.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
			newGameImage.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
			newGameImage.SourceRect.X = (int)newGameImage.Position.X;
			newGameImage.SourceRect.Y = (int)newGameImage.Position.Y;


			_btnNewGame.Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (newGameImage.SourceRect.Width),
				(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - newGameImage.SpriteSheetEffect.FrameHeight);

			newGameImage.AddText("New Game");

			_btnLoadGame.ButtonName = "_btnLoadGame";
			_btnLoadGame.Image = loadGameImage;

			loadGameImage.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
			loadGameImage.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
			loadGameImage.SourceRect.X = (int)loadGameImage.Position.X;
			loadGameImage.SourceRect.Y = (int)loadGameImage.Position.Y;

			_btnLoadGame.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width - loadGameImage.SourceRect.Width) - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 25),
				(ScreenManager.Instance.GraphicsDevice.Viewport.Height - loadGameImage.SpriteSheetEffect.FrameHeight - 16));

			_btnLoadGame.Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (newGameImage.SourceRect.Width),
				(_btnNewGame.Image.Position.Y + loadGameImage.SpriteSheetEffect.FrameHeight + 50));

			loadGameImage.AddText("Load Game");

			_btnNewGame.OnButtonRelease += _btnNewGame_OnButtonRelease;
			_btnLoadGame.OnButtonRelease += _btnLoadGame_OnButtonRelease;
		}

		void _btnLoadGame_OnButtonRelease(object sender, EventArgs e)
		{
			Leave();
		}

		void _btnNewGame_OnButtonRelease(object sender, EventArgs e)
		{
			NewGame ();
		}

		void Leave()
		{
			//XmlManager<Songism> currentSongismSaver = new XmlManager<Songism>();
			//currentSongismSaver.Save(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"), currentSongism);

			ScreenManager.Instance.ChangeScreens("GameplayScreen");
		}

		public override void UnloadContent()
		{
			_btnNewGame.UnloadContent();
			_btnLoadGame.UnloadContent();

			base.UnloadContent();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			_btnNewGame.Update(gameTime);
			_btnLoadGame.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			_btnNewGame.Draw(spriteBatch);
			_btnLoadGame.Draw(spriteBatch);
		}

		private bool CheckForExistingGameFiles()
		{
			if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml")))
				return false;

			//if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			//	return false;

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
				return false;

			if (!File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml")))
				return false;

			if (!File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml")))
				return false;

			return true;
		}

		private void NewGame()
		{
			CreateNewGameFiles ();

			Leave ();
		}

		private void CreateNewGameFiles()
		{
			DeleteExistingGameFiles ();

			CreateNewPlayer ();
			//CreateNewMap1Songisms ();
			CreateNewCurrentSongism ();
			CreateNewSongBook ();
		}

		private void DeleteExistingGameFiles()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"));
			}

			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml")))
			{
			    //File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "downtown_los_angeles.xml"));
			}

			if (File.Exists(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml")))
			{
			    File.Delete(Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"));
			}

			if (File.Exists(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml")))
			{
			    File.Delete(Path.Combine(Globals.LoadGameplayDirectory, "SongBook.xml"));
			}
		}

		private void CreateNewPlayer()
		{
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
					//writer.WriteLine("    <Path>Gameplay/player</Path>");
					writer.WriteLine("    <Path>Gameplay/alienplayer</Path>");
					writer.WriteLine("    <Position>");
					writer.WriteLine("      <X>687</X>");
					writer.WriteLine("      <Y>431</Y>");
					writer.WriteLine("    </Position>");
					writer.WriteLine("    <Scale>");
					writer.WriteLine("      <X>1</X>");
					writer.WriteLine("      <Y>1</Y>");
					writer.WriteLine("    </Scale>");
					writer.WriteLine("    <SourceRect>");
					writer.WriteLine("      <X>64</X>");
					writer.WriteLine("      <Y>64</Y>");
					writer.WriteLine("      <Width>64</Width>");
					writer.WriteLine("      <Height>64</Height>");
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
					//writer.WriteLine("        <X>3</X>");
					writer.WriteLine("        <X>9</X>");
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
					//writer.WriteLine("  <MoveSpeed>100</MoveSpeed>");
					writer.WriteLine("  <MoveSpeed>150</MoveSpeed>");
					writer.WriteLine("  <Position>");
					writer.WriteLine("    <X>687</X>");
					writer.WriteLine("    <Y>431</Y>");
					writer.WriteLine("  </Position>");
					writer.WriteLine("  <ResponseDelay>99</ResponseDelay>");
					writer.WriteLine("</Player>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CreateNewMap1Songisms()
		{
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
					writer.WriteLine("  </Songism>");
					writer.WriteLine("</ArrayOfSongism>");

					writer.Flush();
				}

				fs.Dispose();
			}
		}

		private void CreateNewCurrentSongism()
		{
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

		private void CreateNewSongBook()
		{
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
	}
}







/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class TitleScreen : GameScreen
    {
        MenuManager menuManager;

        public TitleScreen()
        {
            menuManager = new MenuManager();

        }

        public override void LoadContent()
        {
            base.LoadContent();

            //Camera2D.Instance.PlayerPosition = Camera2D.Instance.ScreenCenter;
            //Camera2D.Instance.Position = Camera2D.Instance.ScreenCenter;


            //menuManager.LoadContent("Load/Menus/TitleMenu.xml");
            menuManager.LoadContent(System.IO.Path.Combine(Globals.LoadMenusDirectory, "TitleMenu.xml"));

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}


*/