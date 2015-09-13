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
			newGameImage.UpdateSourceRectPosition ((int)newGameImage.Position.X, (int)newGameImage.Position.Y);

			_btnNewGame.Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (newGameImage.SourceRect.Width),
				(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - newGameImage.SpriteSheetEffect.FrameHeight);

			newGameImage.AddText("New Game");

			_btnLoadGame.ButtonName = "_btnLoadGame";
			_btnLoadGame.Image = loadGameImage;

			loadGameImage.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
			loadGameImage.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
			loadGameImage.UpdateSourceRectPosition ((int)loadGameImage.Position.X, (int)loadGameImage.Position.Y);

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

			CreateNewGameProgress ();
			CreateNewMaps ();
			CheckForDowntownLosAngelesSongismsDemo ();
			CheckForDowntownLosAngelesMapEntrancesDemo ();
			CheckForDowntownLosAngeles2MapEntrances ();
			CheckForDowntownLosAngeles2Songisms ();

			CheckForFrolicRoomSongismsDemo ();
			CheckForFrolicRoomMapEntrancesDemo ();
			CheckForFrolicRoomNPCs ();
			CheckForDowntownLosAngelesNPCs ();




			CreateNewPlayer ();
			CreateNewCurrentSongism ();
			CreateNewSongBook ();
			CreateNewInventory ();
			CreateNewInventoryItems ();
		}

		private void CreateNewGameProgress()
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

		private void CreateNewMaps()
		{
			CreateNewLosAngelesMap ();
			CreateNewFrolicRoomMap ();
		}

		private void CreateNewLosAngelesMap()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml")))
			{
				CreateLosAngelesMap();
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

		private void CreateLosAngelesMap()
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

		private void CreateNewFrolicRoomMap()
		{
			if (File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml")))
			{
				File.Delete(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml"));
			}

			if (!File.Exists(Path.Combine(Globals.LoadGameplayMapsDirectory, "frolic_room.xml")))
			{
				CreateFrolicRoomMap ();
			}
		}

		private void CreateFrolicRoomMap()
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

		private void CreateNewInventory()
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

		private void CreateNewInventoryItems()
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
	}
}