using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SongAdventureAndroid
{
    public interface IFocusable
    {
        Vector2 FocusPosition { get; }
    }

    public interface ICamera2D
    {
        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        /// <value>The position.</value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera.
        /// The camera will tween to its destination.
        /// </summary>
        /// <value>The move speed.</value>
        float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        /// <value>The rotation.</value>
        float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale)
        /// </summary>        
        /// <value>The origin.</value>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the scale of the Camera
        /// </summary>
        /// <value>The scale.</value>
        float Scale { get; set; }

        /// <summary>
        /// Gets the screen center (does not account for Scale)
        /// </summary>
        /// <value>The screen center.</value>
        Vector2 ScreenCenter { get; }

        /// <summary>
        /// Gets the transformation that can be applied to 
        /// the SpriteBatch Class.
        /// </summary>
        /// <see cref="SpriteBatch"/>
        /// <value>The transformation.</value>
        Matrix Transformation { get; }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if the target is in view at the specified position; otherwise, <c>false</c>.
        /// </returns>
        bool IsInView(Vector2 position, Texture2D texture);
    }

    public class Camera2D : ICamera2D
    {
		#region Variables

		private Vector2 _vPosition;
        protected float _fViewportHeight;
        protected float _fViewportWidth;

        private static Camera2D _oInstance;

		private bool _bIsCameraMoving;

		private Rectangle _rectPlayerMovableArea;

		private Matrix _mPlayerTransformation;
		private Vector2 _vVelocity = Vector2.Zero;
		private Vector2 _vDestination = Vector2.Zero;

		#endregion // Variables

        #region Properties

        public static Camera2D Instance
        {
            get
            {
				if (_oInstance == null)
					_oInstance = new Camera2D();

				return _oInstance;
            }
        }

        public Vector2 Position
        {
			get { return _vPosition; }
			set { _vPosition = value; }
        }

		public bool IsCameraMoving
		{
			get { return _bIsCameraMoving; }
			set { _bIsCameraMoving = value; }
		}

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transformation { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 PlayerPosition { get; set; }
        public Matrix CameraMatrix { get; set; }

        #endregion

        private Camera2D()
        {

        }

        /// <summary>
        /// Called when the GameComponent needs to be initialized. 
        /// </summary>
        public void Initialize()
        {
			_fViewportWidth = ScreenManager.Instance.GraphicsDevice.Viewport.Width;
			_fViewportHeight = ScreenManager.Instance.GraphicsDevice.Viewport.Height;

			ScreenCenter = new Vector2(_fViewportWidth / 2, _fViewportHeight / 2);
            Scale = 1;
			//MoveSpeed = 1.25f;
			MoveSpeed = 1000;

			_vPosition = ScreenCenter;
			_vDestination = Position;

			_rectPlayerMovableArea = new Rectangle (200, 0, ScreenManager.Instance.GraphicsDevice.Viewport.Width - 400, ScreenManager.Instance.GraphicsDevice.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            Origin = ScreenCenter / Scale;

            CameraMatrix = Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                           Matrix.CreateRotationZ(Rotation) *
                           Matrix.CreateTranslation(new Vector3(Origin, 0)) *
                           Matrix.CreateScale(new Vector3(Scale, Scale, 1));
            
            /* Create the Transform used by any spritebatch process */
            Transformation = Matrix.Identity * 
				Matrix.CreateTranslation(new Vector3(-_vPosition.X, -_vPosition.Y, 0)) *
                             Matrix.CreateRotationZ(Rotation) *
                             Matrix.CreateScale(new Vector3(Scale, Scale, Scale)) *
                             Matrix.CreateTranslation(new Vector3(ScreenManager.Instance.GraphicsDevice.Viewport.Width * 0.5f, ScreenManager.Instance.GraphicsDevice.Viewport.Height * 0.5f, 0));

			_mPlayerTransformation = Matrix.Identity * 
				Matrix.CreateTranslation(new Vector3(-PlayerPosition.X, -PlayerPosition.Y, 0)) *
				Matrix.CreateRotationZ(Rotation) *
				Matrix.CreateScale(new Vector3(Scale, Scale, Scale)) *
				Matrix.CreateTranslation(new Vector3(ScreenManager.Instance.GraphicsDevice.Viewport.Width * 0.5f, ScreenManager.Instance.GraphicsDevice.Viewport.Height * 0.5f, 0));

			//FollowPlayer (gameTime);
			MoveCamera(gameTime);

			//_rectNavigationArea = new Rectangle (0, 100, ScreenManager.Instance.GraphicsDevice.Viewport.Width, ScreenManager.Instance.GraphicsDevice.Viewport.Height - 200);
			//_rectPlayerMovableArea.Location = new Point ((int)(PlayerPosition.X - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) + 200), (int)(Camera2D.Instance.Position.Y - (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2)));
			_rectPlayerMovableArea.Location = new Point ((int)(Camera2D.Instance.Position.X - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2) + 200), (int)(Camera2D.Instance.Position.Y - (ScreenManager.Instance.GraphicsDevice.Viewport.Height / 2)));
			if (!_bIsCameraMoving) {
				if (!_rectPlayerMovableArea.Contains (PlayerPosition)) {
					if (PlayerPosition.X < _rectPlayerMovableArea.X) {
						//Camera2D.Instance.SetCameraDestination (gameTime, -1);
						Camera2D.Instance.SetCameraVelocity (gameTime, -1);
					} else {
						//Camera2D.Instance.SetCameraDestination (gameTime, 1);
						Camera2D.Instance.SetCameraVelocity (gameTime, 1);
					}
				}
			}

        }

		private void MoveCamera(GameTime gameTime)
		{
			if (_bIsCameraMoving) {
				Position += _vVelocity;

				if (_vVelocity.X > 0) {
					//if ((int)_rectPlayerMovableArea.X + 100 >= (int)PlayerPosition.X) {
					if ((int)_rectPlayerMovableArea.X + (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 4) >= (int)PlayerPosition.X) {
						_bIsCameraMoving = false;
					}
				} else {
					//if ((int)_rectPlayerMovableArea.Right - 100 <= (int)PlayerPosition.X) {
					if ((int)_rectPlayerMovableArea.Right - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 4) <= (int)PlayerPosition.X) {
						_bIsCameraMoving = false;
					}
				}
			}
		}

		public void SetCameraVelocity(GameTime gameTime, int velocityModifier)
		{
			_bIsCameraMoving = true;

			_vVelocity.X = (MoveSpeed * velocityModifier) * (float)gameTime.ElapsedGameTime.TotalSeconds;
			//while (Position.X != PlayerPosition.X) {
			//	Position += _Velocity;
			//}
		}

		/// <summary>
		/// Follows the player.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		private void FollowPlayer(GameTime gameTime)
		{
			// Move the Camera to the position that it needs to go
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			_vPosition += (PlayerPosition - Position) * MoveSpeed * delta;
		}

		/// <summary>
		/// Sets the camera destination.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="velocityModifier">Velocity modifier.</param>
		public void SetCameraDestination(GameTime gameTime, int velocityModifier)
		{
			// Move the Camera to the position that it needs to go
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			_vVelocity.X = (MoveSpeed * velocityModifier) * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		/// <summary>
		/// Adjusts the camera position.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void AdjustCameraPosition(GameTime gameTime, int velocityModifier)
		{
			_bIsCameraMoving = true;

			//_Velocity.X = (MoveSpeed * velocityModifier) * (float)gameTime.ElapsedGameTime.TotalSeconds;
			//while (Position.X != PlayerPosition.X) {
			//	Position += _Velocity;
			//}
		}

		int mod(int a, int n)
		{
			int result = a % n;
			if ((a<0 && n>0) || (a>0 && n<0))
				result += n;
			return result;
		}

		int Remainder(int a, int n)
		{
			return a % n;
		}

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            /* If the object is not within the horizontal bounds of the screen */
            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            /* If the object is not within the vertical bounds of the screen */
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            /* In View */
            return true;
        }
    }
}