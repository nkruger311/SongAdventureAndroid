using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class GameplayScreen : GameScreen
    {
        Player player;
        Map map;

		AreaProgressBar progressBar;
		BackpackInventory backpack;
		//private bool _fInitializing = true;
		public override bool IsInitializing {get;set;}

        public override void LoadContent()
        {
			IsInitializing = true;

            base.LoadContent();

            XmlManager<Player> playerLoader = new XmlManager<Player>();
            XmlManager<Map> mapLoader = new XmlManager<Map>();


			XmlManager<GameProgress> xmGameProgessLoader = new XmlManager<GameProgress> ();
			GameProgress gpProgress = xmGameProgessLoader.Load (Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"));
            
			player = playerLoader.Load(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"));
			//map = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"));
			map = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, gpProgress.CurrentMapName + ".xml"));
			map.MapName = gpProgress.CurrentMapName;

			player.Position = map.PlayerStartingPosition;
			player.Image.Position = map.PlayerStartingPosition;

			map.OnMapChanging += Map_OnMapChanging;

			progressBar = new AreaProgressBar ();
			backpack = new BackpackInventory ();

            player.LoadContent();
            map.LoadContent();

			progressBar.LoadContent ();
			backpack.LoadContent ();

            /* Let's focus the camera on the player at game start */
            //Camera2D.Instance.Position = player.Position;
            Camera2D.Instance.PlayerPosition = player.Position;

			IsInitializing = false;
        }

        void Map_OnMapChanging (object sender, EventArgs e)
        {
			map.UnloadContent ();

			XmlManager<GameProgress> xmGameProgessLoader = new XmlManager<GameProgress> ();
			GameProgress gpProgress = xmGameProgessLoader.Load (Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"));

			XmlManager<Map> mapLoader = new XmlManager<Map>();
			map = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, gpProgress.CurrentMapName + ".xml"));
			map.MapName = gpProgress.CurrentMapName;

			player.Position = map.PlayerStartingPosition;
			player.Image.Position = map.PlayerStartingPosition;
			player.StopPlayerMovement ();

			map.LoadContent ();

			map.OnMapChanging += Map_OnMapChanging;

			InputManager.Instance.ResetInputState ();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            
            XmlManager<Player> playerLoader = new XmlManager<Player>();
            playerLoader.Save(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"), player);

            player.UnloadContent();

            map.UnloadContent();

			progressBar.UnloadContent ();

			backpack.UnloadContent ();
        }

        public override void Update(GameTime gameTime)
        {
			if (!this.IsInitializing) {
				base.Update(gameTime);
				if (!ScreenManager.Instance.IsTransitioning) {
					if (!Camera2D.Instance.IsCameraMoving) {
						player.Update (gameTime);
					}
				}
				map.Update(gameTime, ref player);

				progressBar.Update (gameTime);
				backpack.Update (gameTime);
			}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			if (!this.IsInitializing) {
				base.Draw(spriteBatch);
				map.Draw(spriteBatch, "Underlay");

				/* Tutorial Begin */
				/*
				//spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);


				//Vector2 firstSquare = new Vector2(Camera2D.Instance.Position.X / TutorialTile.TileStepX, Camera2D.Instance.Position.Y / TutorialTile.TileStepY);
				Vector2 firstSquare = Vector2.Zero;
				int firstX = (int)firstSquare.X;
				int firstY = (int)firstSquare.Y;

				//Vector2 squareOffset = new Vector2(Camera2D.Instance.Position.X % TutorialTile.TileStepX, Camera2D.Instance.Position.Y % TutorialTile.TileStepY);
				Vector2 squareOffset = new Vector2(0 % TutorialTile.TileStepX, 0 % TutorialTile.TileStepY);
				int offsetX = (int)squareOffset.X;
				int offsetY = (int)squareOffset.Y;

				float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * TutorialTile.TileWidth)) * 10;
				float depthOffset;

				for (int y = 0; y < squaresDown; y++)
				{
					int rowOffset = 0;
					if ((firstY + y) % 2 == 1)
						rowOffset = TutorialTile.OddRowXOffset;
					
					for (int x = 0; x < squaresAcross; x++)
					{
						int mapx = (firstX + x);
						int mapy = (firstY + y);
						depthOffset = 0.7f - ((mapx + (mapy * TutorialTile.TileWidth)) / maxdepth);

						foreach (int tileID in myMap.Rows[mapy].Columns[mapx].BaseTiles)
						{
							spriteBatch.Draw(

								TutorialTile.TileSetTexture,
								new Rectangle(
									(x * TutorialTile.TileStepX) - offsetX + rowOffset + baseOffsetX,
									(y * TutorialTile.TileStepY) - offsetY + baseOffsetY,
									TutorialTile.TileWidth, TutorialTile.TileHeight),
								TutorialTile.GetSourceRectangle(tileID),
								Color.White,
								0.0f,
								Vector2.Zero,
								SpriteEffects.None,
								1.0f);
						}
					}
				}
				*/
				/* Tutorial End */


				player.Draw(spriteBatch);



				/* Tutorial Begin */
				/*
				for (int y = 0; y < squaresDown; y++)
				{
					int rowOffset = 0;
					if ((firstY + y) % 2 == 1)
						rowOffset = TutorialTile.OddRowXOffset;

					for (int x = 0; x < squaresAcross; x++)
					{
						int mapx = (firstX + x);
						int mapy = (firstY + y);
						depthOffset = 0.7f - ((mapx + (mapy * TutorialTile.TileWidth)) / maxdepth);

						int heightRow = 0;

						foreach (int tileID in myMap.Rows[mapy].Columns[mapx].HeightTiles)
						{
							spriteBatch.Draw(
								TutorialTile.TileSetTexture,
								new Rectangle(
									(x * TutorialTile.TileStepX) - offsetX + rowOffset + baseOffsetX,
									(y * TutorialTile.TileStepY) - offsetY + baseOffsetY - (heightRow * TutorialTile.HeightTileOffset),
									TutorialTile.TileWidth, TutorialTile.TileHeight),
								TutorialTile.GetSourceRectangle(tileID),
								Color.White,
								0.0f,
								Vector2.Zero,
								SpriteEffects.None,
								depthOffset - ((float)heightRow * heightRowDepthMod));
							heightRow++;
						}

						foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].TopperTiles)
						{
							spriteBatch.Draw(
								TutorialTile.TileSetTexture,
								new Rectangle(
									(x * TutorialTile.TileStepX) - offsetX + rowOffset + baseOffsetX,
									(y * TutorialTile.TileStepY) - offsetY + baseOffsetY - (heightRow * TutorialTile.HeightTileOffset),
									TutorialTile.TileWidth, TutorialTile.TileHeight),
								TutorialTile.GetSourceRectangle(tileID),
								Color.White,
								0.0f,
								Vector2.Zero,
								SpriteEffects.None,
								depthOffset - ((float)heightRow * heightRowDepthMod));
						}
					}
				}
				*/
				/* Tutorial End */

				//map.Draw(spriteBatch, "Overlay");
				progressBar.Draw (spriteBatch);
				backpack.Draw (spriteBatch);
			}
        }
    }
}
