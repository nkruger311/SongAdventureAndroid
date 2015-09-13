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
	public class SongismSongInfoItem
	{
		public event EventHandler OnGuessingItemPressed;

		[XmlIgnore]
		public bool IsInitializing = true;

		[XmlIgnore]
		public Image SongInfoImage;

		[XmlIgnore]
		public Vector2 Position;
		[XmlIgnore]
		public float StartingPositionY;
		[XmlIgnore]
		public float EndingPositionY = float.MinValue;

		[XmlIgnore]
		Rectangle _songNameBoundingBox;

		private string _sSongInfoText;

		public SongismSongInfoItem()
		{

		}

		public SongismSongInfoItem(string songInfoText)
		{
			_sSongInfoText = songInfoText;

			SongInfoImage = new Image();
			SongInfoImage.Text = _sSongInfoText;
			SongInfoImage.FontName = "Fonts/GameFont_Size32";
		}

		public void LoadContent()
		{
			IsInitializing = true;

			SongInfoImage = new Image();
			SongInfoImage.Text = _sSongInfoText;
			SongInfoImage.FontName = "Fonts/GameFont_Size32";

			SongInfoImage.LoadContent();
			SongInfoImage.DeactivateEffect("FadeEffect");
			SongInfoImage.DeactivateEffect("SpriteSheetEffect");
			SongInfoImage.Alpha = 1.0f;

			SongInfoImage.AddText(SongInfoImage.Text);
			//SongNameImage.AddTextThreadSafe(SongNameImage.Text);

			SongInfoImage.Position = Position;

			_songNameBoundingBox = new Rectangle((int)SongInfoImage.Position.X, (int)SongInfoImage.Position.Y, SongInfoImage.SourceRect.Width, SongInfoImage.SourceRect.Height);

			IsInitializing = false;
		}

		public void UnloadContent()
		{
			SongInfoImage.UnloadContent();
		}

		public void Update(GameTime gameTime)
		{
			UpdateSongInfo(gameTime);

			SongInfoImage.Update(gameTime);
		}

		void UpdateSongInfo(GameTime gameTime)
		{
			SongInfoImage.IsActive = true;
			SongInfoImage.Position = Position;

			//SongNameImage.SourceRect.X = (int)SongNameImage.Position.X;
			//SongNameImage.SourceRect.Y = (int)SongNameImage.Position.Y;
			//_songNameBoundingBox.X = (int)SongInfoImage.Position.X;
			//_songNameBoundingBox.Y = (int)SongInfoImage.Position.Y;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			SongInfoImage.Draw(spriteBatch);
		}
	}
}