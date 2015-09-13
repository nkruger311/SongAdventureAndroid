using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    [XmlType("SongismGuessingItem")]
    public class SongismGuessingItem
    {
        public event EventHandler OnGuessingItemPressed;

        public string SongName;
        public string AlbumName;
        public bool Discovered;
        public bool DisplayItem;
		public bool LastSongOnAlbum;

		[XmlIgnore]
		public bool IsButtonEnabled { get; set; }
		[XmlIgnore]
		public bool IsInitializing = true;
		[XmlIgnore]
		public bool Loaded = false;

        [XmlIgnore]
        public Image RadioButton;

		[XmlIgnore]
		SpriteFont font;

        [XmlIgnore]
        public Vector2 Position;
        [XmlIgnore]
        public float StartingPositionY;
		[XmlIgnore]
		public float EndingPositionY = float.MinValue;
		[XmlIgnore]
		public float DockingPositionX;
		[XmlIgnore]
		public List<float> DockingPositionsX;
        [XmlIgnore]
        public bool Checked;
        
        [XmlIgnore]
        Rectangle _radioButtonBoundingBox;
        [XmlIgnore]
        Rectangle _songNameBoundingBox;

        public SongismGuessingItem()
        {
            SongName = string.Empty;
            AlbumName = string.Empty;
            Discovered = false;
            DisplayItem = true;
        }

		public SongismGuessingItem(string songName)
        {
            Position = Vector2.Zero;
            Checked = false;

            RadioButton = new Image();
            RadioButton.Path = "Gameplay/UI/radiobuttonsheet";
            RadioButton.SourceRect = new Rectangle(0, 0, 36, 36);

            RadioButton.Effects = "SpriteSheetEffect";
            RadioButton.DeactivateEffect("FadeEffect");

            SongName = songName;
        }

        public void LoadContent()
        {
			IsInitializing = true;

            RadioButton = new Image();
            RadioButton.Path = "Gameplay/UI/radiobuttonsheet";
            RadioButton.SourceRect = new Rectangle(0, 0, 36, 36);

            RadioButton.Effects = "SpriteSheetEffect";

            RadioButton.LoadContent();
			RadioButton.DeactivateEffect("FadeEffect");
            RadioButton.Alpha = 1.0f;
            RadioButton.SpriteSheetEffect.AmountOfFrames = new Vector2(1, 2);
            RadioButton.SpriteSheetEffect.CurrentFrame = new Vector2(0, 0);
            RadioButton.Position = Position;
            RadioButton.SourceRect.X = (int)RadioButton.Position.X;
            RadioButton.SourceRect.Y = (int)RadioButton.Position.Y;

			font = ScreenManager.Instance.Content.Load<SpriteFont> ("Fonts/GameFont_Size32");

            _radioButtonBoundingBox = new Rectangle(RadioButton.SourceRect.X, RadioButton.SourceRect.Y, RadioButton.SourceRect.Width, RadioButton.SourceRect.Height);
            _songNameBoundingBox = new Rectangle((int)RadioButton.Position.X + RadioButton.SpriteSheetEffect.FrameWidth + 10, (int)Position.Y - 10, (int)font.MeasureString(SongName).X, (int)font.MeasureString(SongName).Y);

			Loaded = true;
			IsInitializing = false;
        }

        public void UnloadContent()
        {
            RadioButton.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            UpdateRadioButton(gameTime);
            UpdateSongName(gameTime);

            RadioButton.Update(gameTime);
        }

        void UpdateSongName(GameTime gameTime)
        {
            _songNameBoundingBox.X = (int)RadioButton.Position.X + RadioButton.SpriteSheetEffect.FrameWidth + 10;
			_songNameBoundingBox.Y = (int)Position.Y - 10;

			if (IsButtonEnabled) {
				if (InputManager.Instance.TouchPanelPressed () && _songNameBoundingBox.Contains (InputManager.Instance.TransformedTouchPosition)) {
					if (!InputManager.Instance.InputDisabled) {
						OnGuessingItemPressed (this, null);
					}
				}
			}
        }

        void UpdateRadioButton(GameTime gameTime)
        {
            RadioButton.IsActive = true;
            RadioButton.Position = Position;

            _radioButtonBoundingBox.X = (int)RadioButton.Position.X;
			_radioButtonBoundingBox.Y = (int)RadioButton.Position.Y;

			if (IsButtonEnabled) {
				if (InputManager.Instance.TouchPanelPressed () && _radioButtonBoundingBox.Contains (InputManager.Instance.TransformedTouchPosition)) {
					if (!InputManager.Instance.InputDisabled) {
						OnGuessingItemPressed (this, null);
					}
				}
			}

            if (Checked)
                RadioButton.SpriteSheetEffect.CurrentFrame.Y = 1;
            else
                RadioButton.SpriteSheetEffect.CurrentFrame.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            RadioButton.Draw(spriteBatch);
           
			ScreenManager.Instance.SpriteBatch.DrawString(font, SongName, new Vector2(_songNameBoundingBox.X, _songNameBoundingBox.Y), Color.White);
        }
    }
}