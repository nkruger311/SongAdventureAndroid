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
        private Player _oPlayer;
		private Map _oMap;

		private AreaProgressBar _oProgressBar;
		private BackpackInventory _oBackpack;
		public override bool IsInitializing { get; set; }

        public override void LoadContent()
        {
			IsInitializing = true;

            base.LoadContent();

            XmlManager<Player> playerLoader = new XmlManager<Player>();
            XmlManager<Map> mapLoader = new XmlManager<Map>();


			XmlManager<GameProgress> xmGameProgessLoader = new XmlManager<GameProgress> ();
			GameProgress gpProgress = xmGameProgessLoader.Load (Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"));
            
			_oPlayer = playerLoader.Load(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"));
			//map = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, "downtown_los_angeles.xml"));
			_oMap = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, gpProgress.CurrentMapName + ".xml"));
			_oMap.MapName = gpProgress.CurrentMapName;

			_oPlayer.Position = _oMap.PlayerStartingPosition;
			_oPlayer.Image.Position = _oMap.PlayerStartingPosition;

			_oMap.OnMapChanging += Map_OnMapChanging;

			_oProgressBar = new AreaProgressBar ();
			_oBackpack = new BackpackInventory ();

			_oPlayer.LoadContent();
			_oMap.LoadContent();

			_oProgressBar.LoadContent ();
			_oBackpack.LoadContent ();

            /* Let's focus the camera on the player at game start */
            //Camera2D.Instance.Position = player.Position;
			Camera2D.Instance.PlayerPosition = _oPlayer.Position;

			IsInitializing = false;
        }

        void Map_OnMapChanging (object sender, EventArgs e)
        {
			_oMap.UnloadContent ();

			XmlManager<GameProgress> xmGameProgessLoader = new XmlManager<GameProgress> ();
			GameProgress gpProgress = xmGameProgessLoader.Load (Path.Combine (Globals.LoadGameplayDirectory, "game_progress.xml"));

			XmlManager<Map> mapLoader = new XmlManager<Map>();
			_oMap = mapLoader.Load(Path.Combine(Globals.LoadGameplayMapsDirectory, gpProgress.CurrentMapName + ".xml"));
			_oMap.MapName = gpProgress.CurrentMapName;

			_oPlayer.Position = _oMap.PlayerStartingPosition;
			_oPlayer.Image.Position = _oMap.PlayerStartingPosition;
			_oPlayer.StopPlayerMovement ();

			_oMap.LoadContent ();

			_oMap.OnMapChanging += Map_OnMapChanging;

			InputManager.Instance.ResetInputState ();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            
            XmlManager<Player> playerLoader = new XmlManager<Player>();
			playerLoader.Save(Path.Combine(Globals.LoadGameplayDirectory, "Player.xml"), _oPlayer);

			_oPlayer.UnloadContent();

			_oMap.UnloadContent();

			_oProgressBar.UnloadContent ();

			_oBackpack.UnloadContent ();
        }

        public override void Update(GameTime gameTime)
        {
			if (!this.IsInitializing) {
				base.Update(gameTime);
				if (!ScreenManager.Instance.IsTransitioning) {
					if (!Camera2D.Instance.IsCameraMoving) {
						_oPlayer.Update (gameTime);
					}
				}
				_oMap.Update(gameTime, ref _oPlayer);

				_oProgressBar.Update (gameTime);
				_oBackpack.Update (gameTime);
			}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			if (!this.IsInitializing) {
				base.Draw(spriteBatch);
				_oMap.Draw(spriteBatch, "Underlay");

				_oPlayer.Draw(spriteBatch);

				//map.Draw(spriteBatch, "Overlay");
				_oProgressBar.Draw (spriteBatch);
				_oBackpack.Draw (spriteBatch);
			}
        }
    }
}
