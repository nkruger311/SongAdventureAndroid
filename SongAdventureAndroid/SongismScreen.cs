using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public class SongismScreen : GameScreen
    {
        //public Image Background;
        public Image CurrentSongismImage;
        Songism currentSongism;
        DialogButton addToSongBook;
        DialogButton cancel;
        bool _isLeaving = false;
        Image _songismName;

		private Image loadingImage;

		//private bool _fInitializing = true;
		public override bool IsInitializing {get;set;}

        //SongismGuessingItem _guessingItem;
        SongismGuessingListBox _guessingList;
		SongismSongInfoListBox _oSongInfoList;

		private bool _fDiscovered;

        public override void LoadContent()
        {
			loadingImage = new Image();
			loadingImage.Path = "Gameplay/UI/loadingscreenanimation";
			//Image.SourceRect = new Rectangle(0, 0, 400, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);
			loadingImage.LoadContent();

			loadingImage.SourceRect = new Rectangle(0, 0, 100, 100);
			//Image.Position = new Vector2 ((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - 100,
			//	(ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - 100);

			loadingImage.IsActive = true;
			loadingImage.ActivateEffect ("SpriteSheetEffect");
			loadingImage.SpriteSheetEffect.IsActive = true;

			loadingImage.FadeEffect.IsActive = false;

			loadingImage.SpriteSheetEffect.SwitchFrame = 250;
			loadingImage.SpriteSheetEffect.AmountOfFrames = new Vector2(4, 1);
			loadingImage.SpriteSheetEffect.DefaultFrame = Vector2.Zero;
			loadingImage.SpriteSheetEffect.CurrentFrame = loadingImage.SpriteSheetEffect.DefaultFrame;

			loadingImage.Position = new Vector2 (loadingImage.Position.X - 50, loadingImage.Position.Y - 50);


		

			IsInitializing = true;

            base.LoadContent();

            InputManager.Instance.ResetInputState();

            Camera2D.Instance.PlayerPosition = Camera2D.Instance.ScreenCenter;
            Camera2D.Instance.Position = Camera2D.Instance.ScreenCenter;

            LoadBackground();
            LoadSongism();
            LoadButtons();

            //_guessingItem = new SongismGuessingItem("All Mixed Up");
            //_guessingItem.Position = new Vector2(16, 16);
            //_guessingItem.LoadContent();

			_fDiscovered = currentSongism.Discovered;
			if (_fDiscovered) {
				_oSongInfoList = new SongismSongInfoListBox (currentSongism.SongInfo);
				_oSongInfoList.LoadContent ();
			} else {
            
				_guessingList = new SongismGuessingListBox();
				_guessingList.LoadContent();
			}

			IsInitializing = false;
        }

        void LoadBackground()
        {
            //Background.LoadContent();
            //Background.DeactivateEffect("FadeEffect");
            //Background.DeactivateEffect("SpriteSheetEffect");
            //Background.Alpha = 1.0f;
            //Background.SourceRect = new Rectangle(0, 0, 640, 480);
        }

        void LoadSongism()
        {
            XmlManager<Songism> currentSongismLoader = new XmlManager<Songism>();
            currentSongism = currentSongismLoader.Load(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"));

            CurrentSongismImage = new Image();
            CurrentSongismImage.Path = currentSongism.Image.Path;
			CurrentSongismImage.LoadContent();
            CurrentSongismImage.DeactivateEffect("FadeEffect");
            CurrentSongismImage.DeactivateEffect("SpriteSheetEffect");
            CurrentSongismImage.Alpha = 1.0f;
            CurrentSongismImage.Scale = new Vector2((512 / CurrentSongismImage.SourceRect.Width), (512 / CurrentSongismImage.SourceRect.Height));
            CurrentSongismImage.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) + (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 4) - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 20), // - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 100), 
                (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) - (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 10));

            _songismName = new Image();
            _songismName.FontName = "Fonts/GameFont_Size32";
            _songismName.TextAlignment = Globals.TextAlignment.Center;

            if (currentSongism.Discovered)
                _songismName.Text = currentSongism.Name;
            else
                _songismName.Text = "????";

            _songismName.LoadContent();
            _songismName.DeactivateEffect("FadeEffect");
            _songismName.DeactivateEffect("SpriteSheetEffect");
            _songismName.Alpha = 1.0f;

            if (currentSongism.Discovered)
                _songismName.AddText(currentSongism.Name);
            else
                _songismName.AddText("????");

            _songismName.Position = new Vector2(CurrentSongismImage.Position.X + (CurrentSongismImage.SourceRect.Width / 2) - (_songismName.SourceRect.Width / 2), 16);
        }

        void LoadButtons()
        {
            addToSongBook = new DialogButton(!currentSongism.Discovered);
            cancel = new DialogButton(true);

            Image addImage = new Image();
            addImage.Path = "Gameplay/UI/buttonsheet";
            addImage.Effects = "SpriteSheetEffect";
            addImage.FontName = "Fonts/GameFont_Size32";
            addImage.TextAnimationTravel = new Vector2(0, 9);
            addImage.TextAlignment = Globals.TextAlignment.Center;
            if (currentSongism.Discovered)
                addImage.TextColor = Color.Gray;

            addImage.LoadContent();

            Image cancelImage = new Image();
            cancelImage.Path = "Gameplay/UI/buttonsheet";
            cancelImage.Effects = "SpriteSheetEffect";
            cancelImage.FontName = "Fonts/GameFont_Size32";
            cancelImage.TextAnimationTravel = new Vector2(0, 9);
            cancelImage.TextAlignment = Globals.TextAlignment.Center;

            cancelImage.LoadContent();

            addToSongBook.ButtonName = "Add";
            addToSongBook.Image = addImage;
            //addToSongBook.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (addImage.SourceRect.Width / 2),
            //    (ScreenManager.Instance.GraphicsDevice.Viewport.Height - addImage.SourceRect.Height));
            //addToSongBook.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (addImage.SourceRect.Width),
            //    (ScreenManager.Instance.GraphicsDevice.Viewport.Height - addImage.SourceRect.Height));

            addImage.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
            addImage.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
            addImage.UpdateSourceRectPosition ((int)addImage.Position.X, (int)addImage.Position.Y);

            //addToSongBook.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) - (addImage.SourceRect.Width),
            //    (ScreenManager.Instance.GraphicsDevice.Viewport.Height - addImage.SpriteSheetEffect.FrameHeight - 16));
            addToSongBook.Image.Position = new Vector2(CurrentSongismImage.Position.X - addImage.SpriteSheetEffect.FrameWidth + 16,
                ScreenManager.Instance.GraphicsDevice.Viewport.Height - addImage.SpriteSheetEffect.FrameHeight - 16);

            addImage.AddText("Add");

            cancel.ButtonName = "Cancel";
            cancel.Image = cancelImage;
            //cancel.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width - cancelImage.SourceRect.Width) - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 25),
            //    (ScreenManager.Instance.GraphicsDevice.Viewport.Height - cancelImage.SourceRect.Height));

            cancelImage.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
            cancelImage.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
            cancelImage.UpdateSourceRectPosition ((int)cancelImage.Position.X, (int)cancelImage.Position.Y);

            //cancel.Image.Position = new Vector2((ScreenManager.Instance.GraphicsDevice.Viewport.Width - cancelImage.SourceRect.Width) - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 25),
            //    (ScreenManager.Instance.GraphicsDevice.Viewport.Height - cancelImage.SpriteSheetEffect.FrameHeight - 16));
            cancel.Image.Position = new Vector2(CurrentSongismImage.Position.X + (CurrentSongismImage.SourceRect.Width / 2) + 16,
                ScreenManager.Instance.GraphicsDevice.Viewport.Height - cancelImage.SpriteSheetEffect.FrameHeight - 16);

            cancelImage.AddText("Cancel");

            addToSongBook.OnButtonRelease += addToSongBook_OnButtonRelease;
            cancel.OnButtonRelease += cancel_OnButtonRelease;
        }

        void cancel_OnButtonRelease(object sender, EventArgs e)
        {
            if (!_isLeaving)
            {
                _isLeaving = true;

                Leave();
            }
        }

        void addToSongBook_OnButtonRelease(object sender, EventArgs e)
        {
            //TODO: Compare user's guess to song name, add to song book if correct

            /* For testing only, this doensn't actually work */
            if (!_isLeaving)
            {
                _isLeaving = true;

                //string guess = currentSongism.Name;
                if (_guessingList.SelectedItem != null)
                {
                    if (_guessingList.SelectedItem.SongName.Equals(currentSongism.Name))
                    {
                        currentSongism.Discovered = true;
                        _guessingList.SelectedItem.Discovered = true;
                        Inventory.Instance.SongBook.Add(currentSongism);

						if (currentSongism.InventoryReward.Name != null) {
							Inventory.Instance.Items.Add (currentSongism.InventoryReward);
						}

						Inventory.Instance.Save ();

                        Leave();
                    }
                }

                _isLeaving = false;
            }
        }

        void Leave()
        {
            XmlManager<Songism> currentSongismSaver = new XmlManager<Songism>();
            currentSongismSaver.Save(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "CurrentSongism.xml"), currentSongism);

            ScreenManager.Instance.ChangeScreens("GameplayScreen");
        }

        public override void UnloadContent()
        {
            XmlManager<SongismScreen> songismScreenSaver = new XmlManager<SongismScreen>();
            songismScreenSaver.Save(System.IO.Path.Combine(Globals.LoadGameplayScreensDirectory, "SongismScreen.xml"), this);
            _songismName.UnloadContent();
            addToSongBook.UnloadContent();
            cancel.UnloadContent();
            //Background.UnloadContent();
            CurrentSongismImage.UnloadContent();

			if (_fDiscovered) {
				_oSongInfoList.UnloadContent ();
			} else {
				//_guessingItem.UnloadContent();
				_guessingList.UnloadContent ();
			}

			loadingImage.UnloadContent ();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
			if (_fDiscovered) {
				if (!_oSongInfoList.IsInitializing) {
					base.Update (gameTime);
					//Background.Update(gameTime);
					_songismName.Update (gameTime);
					CurrentSongismImage.Update (gameTime);
					addToSongBook.Update (gameTime);
					cancel.Update (gameTime);

					_oSongInfoList.Update (gameTime);
				} else {
					loadingImage.Update (gameTime);
				}
			} else {
				if (!_guessingList.IsInitializing) {
					base.Update (gameTime);
					//Background.Update(gameTime);
					_songismName.Update (gameTime);
					CurrentSongismImage.Update (gameTime);
					addToSongBook.Update (gameTime);
					cancel.Update (gameTime);

					//_guessingItem.Update(gameTime);
					_guessingList.Update (gameTime);
				} else {
					loadingImage.Update (gameTime);
				}
			}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
			if (_fDiscovered) {
				if (!_oSongInfoList.IsInitializing) {
					base.Draw (spriteBatch);
					//Background.Draw(spriteBatch);
					_songismName.Draw (spriteBatch);
					CurrentSongismImage.Draw (spriteBatch);
					addToSongBook.Draw (spriteBatch);
					cancel.Draw (spriteBatch);

					_oSongInfoList.Draw (spriteBatch);
				}
			} else {
				if (!_guessingList.IsInitializing) {
					base.Draw (spriteBatch);
					//Background.Draw(spriteBatch);
					_songismName.Draw (spriteBatch);
					CurrentSongismImage.Draw (spriteBatch);
					addToSongBook.Draw (spriteBatch);
					cancel.Draw (spriteBatch);

					//_guessingItem.Draw(spriteBatch);
					_guessingList.Draw (spriteBatch);
				} else {
					Color backColor = new Color (89, 144, 25);
					ScreenManager.Instance.GraphicsDevice.Clear(backColor);
					loadingImage.Draw (spriteBatch);
				}
			}
        }
    }
}
