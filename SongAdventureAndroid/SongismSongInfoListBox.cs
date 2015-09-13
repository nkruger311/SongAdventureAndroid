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
	class SongismSongInfoListBox
	{
		
		private List<SongismSongInfoItem> _loSongInfoItem;

		Rectangle _listBoxDisplayArea;
		Vector2 _velocity;
		int _moveSpeed = 50;
		bool _scrollStarted = false;

		[XmlIgnore]
		public bool IsInitializing = true;

		private float _dEndingPositionY = float.MaxValue;

		public SongismSongInfoListBox(string songInfo)
		{
			_loSongInfoItem = new List<SongismSongInfoItem> ();
			//Image imgSongInfo;
			SongismSongInfoItem oSongInfo;

			foreach (String songInfoLine in songInfo.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries)) {
				//imgSongInfo = new Image ();
				oSongInfo = new SongismSongInfoItem(songInfoLine);
				_loSongInfoItem.Add (oSongInfo);
			}

			_listBoxDisplayArea = new Rectangle(54, 128, 512, 480);
			_velocity = Vector2.Zero;

		}

		public void LoadContent()
		{
			IsInitializing = true;

			Vector2 prevItemPosition = new Vector2(64, 128);
			int prevItemHeight = 0;


			for (int i = 0; i < _loSongInfoItem.Count; i++)
			{
				_loSongInfoItem [i].Position = new Vector2 (prevItemPosition.X, prevItemPosition.Y + prevItemHeight + 10);
				_loSongInfoItem [i].SongInfoImage.Position = _loSongInfoItem [i].Position;
				_loSongInfoItem [i].StartingPositionY = _loSongInfoItem [i].SongInfoImage.Position.Y;
				_loSongInfoItem [i].LoadContent ();

				prevItemPosition = _loSongInfoItem [i].Position;
				prevItemHeight = _loSongInfoItem [i].SongInfoImage.SourceRect.Height;
			}

			IsInitializing = false;
		}

		public void UnloadContent()
		{
			for (int i = 0; i < _loSongInfoItem.Count; i++)
			{
				_loSongInfoItem [i].UnloadContent ();
			}
		}

		public void Update(GameTime gameTime)
		{
			if (!IsInitializing) {
				Scroll (gameTime);

				foreach (SongismSongInfoItem songInfoItem in _loSongInfoItem) {
					songInfoItem.Update (gameTime);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!IsInitializing) {
				foreach (SongismSongInfoItem songInfoItem in _loSongInfoItem) {
					if (_listBoxDisplayArea.Contains(songInfoItem.SongInfoImage.Position)) {
						songInfoItem.Draw (spriteBatch);

						if (_loSongInfoItem.IndexOf(songInfoItem) == _loSongInfoItem.Count - 1) {
							if (_dEndingPositionY != float.MinValue) {
								if (_dEndingPositionY == float.MaxValue) {
									_dEndingPositionY = songInfoItem.Position.Y;
								}
							}
						}
					}
				}
			}
		}

		void Scroll(GameTime gameTime)
		{
			GetScrollVelocity (gameTime);

			foreach (SongismSongInfoItem songInfoItem in _loSongInfoItem) {
				if (_dEndingPositionY != float.MinValue) {
					if (_dEndingPositionY < float.MaxValue) {
						songInfoItem.EndingPositionY = songInfoItem.Position.Y;
					}
				}

				if (songInfoItem.Position.Y + _velocity.Y > songInfoItem.StartingPositionY)
					songInfoItem.Position.Y = songInfoItem.StartingPositionY;
				else if (songInfoItem.Position.Y + _velocity.Y < songInfoItem.EndingPositionY)
					songInfoItem.Position.Y = songInfoItem.EndingPositionY;
				else
					songInfoItem.Position.Y += _velocity.Y;
			
				songInfoItem.Update (gameTime);
			}

			if (_dEndingPositionY < float.MaxValue) {
				_dEndingPositionY = float.MinValue;
			}
		}

		private void GetScrollVelocity(GameTime gameTime)
		{
			if (InputManager.Instance.TouchPanelDown() && _listBoxDisplayArea.Contains(InputManager.Instance.TransformedTouchPosition))
			{
				if (InputManager.Instance.PrevTouchPosition != new Vector2(-1, -1))
				{
					_scrollStarted = true;
					float scrollDelta = InputManager.Instance.TransformedTouchPosition.Y - InputManager.Instance.PrevTransformedTouchPosition.Y;
					if (InputManager.Instance.TransformedTouchPosition.Y > InputManager.Instance.PrevTransformedTouchPosition.Y)
					{
						/* Scrolling up */
						_velocity.Y = _moveSpeed * scrollDelta * (float)gameTime.ElapsedGameTime.TotalSeconds;
					}
					else if (InputManager.Instance.TransformedTouchPosition.Y < InputManager.Instance.PrevTransformedTouchPosition.Y)
					{
						/* Scrolling down */
						_velocity.Y = _moveSpeed * scrollDelta * (float)gameTime.ElapsedGameTime.TotalSeconds;
					}
				}
			}
			else if (_scrollStarted)
			{
				/* Apply some friction to slow down the scrolling */
				if (_velocity.Y > 0)
					_velocity.Y -= 0.1f;
				else if (_velocity.Y < 0)
					_velocity.Y += 0.1f;

				if (_velocity.Y >= -0.1f && _velocity.Y <= 0.1f) {
					_scrollStarted = false;
					_velocity.Y = 0.0f;
				}
			}
		}
	}
}