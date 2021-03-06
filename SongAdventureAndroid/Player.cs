﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SongAdventureAndroid
{
    public class Player
    {
		private Vector2 _vDestination;
		private Vector2 _vVelocity;

		public Image Image {get;set;}
		//public Vector2 Velocity { get { return _vVelocity; } set { _vVelocity = value; } }
		public Vector2 Velocity
		{
			get { return _vVelocity; }
			set { _vVelocity = value; }
		}
		public float MoveSpeed { get; set; }
        [XmlIgnore]
		public Vector2 Destination { get { return _vDestination; } set { _vDestination = value; } }
		public Vector2 Position { get; set; }
        Vector2 prevPosition;
        bool moveStarted;
        Vector2 FocusPosition { get { return Position; } }
		public float ResponseDelay { get; set; }
        int _responseDelayTimer;
        bool _isRepsonseDelayStarted = false;
        bool _isMoving = false;

		private Rectangle _rectNavigationArea;
		//private Rectangle _rectCameraStillArea;

        public Player()
        {
            Velocity = Vector2.Zero;
            Position = Vector2.Zero;
            prevPosition = Vector2.Zero;
            moveStarted = false;
			_rectNavigationArea = new Rectangle (0, 100, ScreenManager.Instance.GraphicsDevice.Viewport.Width, ScreenManager.Instance.GraphicsDevice.Viewport.Height - 200);
			//_rectCameraStillArea = new Rectangle (200, 0, ScreenManager.Instance.GraphicsDevice.Viewport.Width - 400, ScreenManager.Instance.GraphicsDevice.Viewport.Height);
        }

        public void LoadContent()
        {
            Image.LoadContent();
            /* Turn the fade effect off */
            Image.FadeEffect.IsActive = false;
            /* Make the player face the camera */
            Image.SpriteSheetEffect.DefaultFrame = Vector2.Zero;
            Image.SpriteSheetEffect.CurrentFrame = Image.SpriteSheetEffect.DefaultFrame;
            /* Set velocity to zero */
            Velocity = Vector2.Zero;
            /* Set destination equal to middle of sprite */
			_vDestination.X = (int)Image.Position.X + (Image.SourceRect.Width / 2);
			_vDestination.Y = (int)Image.Position.Y + (Image.SourceRect.Height / 2);
        }

        public void UnloadContent()
        {
            Inventory.Instance.Unload();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.IsActive = true;
            /* Set position to Image.Position */
            Position = Image.Position;

            Move(gameTime);
            Image.Update(gameTime);
            Image.Position += Velocity;


			//_rectNavigationArea.Location += new Point ((int)Velocity.X, (int)Velocity.Y);

			_rectNavigationArea.Location = new Point ((int)(Camera2D.Instance.Position.X - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2)), (int)(Camera2D.Instance.Position.Y - (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2) + 100));

            
            Camera2D.Instance.PlayerPosition = Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }

        private void Move(GameTime gameTime)
        {
			if (ScreenManager.Instance.GameplayScreenActive && !ScreenManager.Instance.IsInteractingWithNpc) {
				SetPlayerDestination (gameTime);                
				SetPlayerVelocity (gameTime);

				/* We're not walking so let's stop the animation */
				if (Velocity.X == 0 && Velocity.Y == 0) {
					Image.IsActive = false;
					_isMoving = false;
				}

				prevPosition = Image.Position;
			} else if (ScreenManager.Instance.IsInteractingWithNpc) {
				Velocity = Vector2.Zero;

				/* We're not walking so let's stop the animation */
				Image.IsActive = false;
				_isMoving = false;
			}
        }

        void SetPlayerDestination(GameTime gameTime)
        {
            /* Checking for a mouse click, setting a destination */
			if (InputManager.Instance.TouchPanelPressed ()) {
				_isRepsonseDelayStarted = true;
				moveStarted = true;

				if (_rectNavigationArea.Contains (InputManager.Instance.TransformedTouchPosition)) 
				{
					if (InputManager.Instance.TransformedTouchPosition.Y > (Image.Position.Y + (Image.SourceRect.Height / 2)))
						_vDestination.Y = InputManager.Instance.TransformedTouchPosition.Y;
					else if (InputManager.Instance.TransformedTouchPosition.Y < (Image.Position.Y + (Image.SourceRect.Height / 2)))
						_vDestination.Y = InputManager.Instance.TransformedTouchPosition.Y;
					else
						_vDestination.Y = Image.Position.Y + (Image.SourceRect.Height / 2);

					if (InputManager.Instance.TransformedTouchPosition.X > (Image.Position.X + (Image.SourceRect.Width / 2)))
						_vDestination.X = InputManager.Instance.TransformedTouchPosition.X;
					else if (InputManager.Instance.TransformedTouchPosition.X < (Image.Position.X + (Image.SourceRect.Width / 2)))
						_vDestination.X = InputManager.Instance.TransformedTouchPosition.X;
					else
						_vDestination.X = Image.Position.X + (Image.SourceRect.Width / 2);
				}
			}
        }

        void SetPlayerVelocity(GameTime gameTime)
        {
            if (_isRepsonseDelayStarted)
            {
                _responseDelayTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_responseDelayTimer >= ResponseDelay)
                {
                    _responseDelayTimer = 0;
                    _isMoving = true;
                    _isRepsonseDelayStarted = false;
                }
            }

            if (_isMoving)
            {
                /* Moving to Destination.Y */
                if (Destination.Y > ((int)Image.Position.Y + (Image.SourceRect.Height / 2)) + 1)
                {
					_vVelocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                }
                else if (Destination.Y < ((int)Image.Position.Y + (Image.SourceRect.Height / 2)) - 1)
                {
					_vVelocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 3;
                }
                else
					_vVelocity.Y = 0;

                /* Moving to Destination.X */
                if ((int)Destination.X > ((int)Image.Position.X + (Image.SourceRect.Width / 2)) + 1)
                {
					_vVelocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                }
                else if ((int)Destination.X < ((int)Image.Position.X + (Image.SourceRect.Width / 2)) - 1)
                {
					_vVelocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                }
                else
					_vVelocity.X = 0;

                if (!moveStarted)
                {
                    if (Velocity.Y > 0 && (int)prevPosition.Y == (int)Image.Position.Y)
						_vVelocity.Y = 0;
                    else if (Velocity.Y < 0 && (int)prevPosition.Y == (int)Image.Position.Y)
						_vVelocity.Y = 0;

                    if (Velocity.X > 0 && (int)prevPosition.X == (int)Image.Position.X)
						_vVelocity.X = 0;
                    else if (Velocity.X < 0 && (int)prevPosition.X == (int)Image.Position.X)
						_vVelocity.X = 0;
                }

                moveStarted = false;
            }
        }     

		public void StopPlayerMovement()
		{
			/* Set velocity to zero */
			Velocity = Vector2.Zero;

			/* Set destination equal to middle of sprite */
			_vDestination.X = (int)Image.Position.X + (Image.SourceRect.Width / 2);
			_vDestination.Y = (int)Image.Position.Y + (Image.SourceRect.Height / 2);

			/* We're not walking so let's stop the animation */
			Image.IsActive = false;
			_isMoving = false;
		}

		public void UpdateImagePositionX(int x)
		{
			UpdateImagePosition (x, (int)Image.Position.Y);
		}

		public void UpdateImagePositionY(int y)
		{
			UpdateImagePosition ((int)Image.Position.X, y);
		}

		public void UpdateImagePosition(int x, int y)
		{
			Image.Position = new Vector2 (x, y);
		}
    }
}