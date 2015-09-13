using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace SongAdventureAndroid
{
    public class InputManager
    {
        TouchCollection currentTouchCollection, prevTouchCollection, prevPrevTouchCollection;
        public bool InputDisabled = false;
		private bool _fResetInputState = false;

        public Vector2 TouchPosition
        {
            get
            {
                if (currentTouchCollection.Count > 0)
                    return new Vector2(currentTouchCollection[0].Position.X, currentTouchCollection[0].Position.Y);

                return new Vector2(-1, -1);
            }
        }
        
        public Vector2 PrevTouchPosition
        {
            get
            {
                if (prevTouchCollection.Count > 0)
                    return new Vector2(prevTouchCollection[0].Position.X, prevTouchCollection[0].Position.Y);

                return new Vector2(-1, -1);
            }
        }

		public Vector2 PrevPrevTouchPosition
		{
			get {
				if (prevPrevTouchCollection.Count > 0)
					return new Vector2 (prevPrevTouchCollection [0].Position.X, prevPrevTouchCollection [0].Position.Y);

				return new Vector2 (-1, -1);
			}
		}

        public Vector2 TransformedTouchPosition
        {
            get
            {
                return Vector2.Transform(TouchPosition, Matrix.Invert(Camera2D.Instance.CameraMatrix));
            }
        }

        public Vector2 PrevTransformedTouchPosition
        {
            get
            {
                return Vector2.Transform(PrevTouchPosition, Matrix.Invert(Camera2D.Instance.CameraMatrix));
            }
        }

		public Vector2 PrevPrevTransformedTouchPosition
		{
			get
			{
				return Vector2.Transform(PrevPrevTouchPosition, Matrix.Invert(Camera2D.Instance.CameraMatrix));
			}
		}

        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();

                return instance;
            }
        }

        public void Update()
        {
			prevPrevTouchCollection = prevTouchCollection;
            prevTouchCollection = currentTouchCollection;
			if (!ScreenManager.Instance.IsTransitioning) {
				if (!_fResetInputState) {
					currentTouchCollection = TouchPanel.GetState ();
				} else {
					if (TouchPanel.GetState ().Count == 0) {
						_fResetInputState = false;
					}
				}
			}
        }

        public void ResetInputState()
        {
            currentTouchCollection = new TouchCollection();
            prevTouchCollection = new TouchCollection();
			prevPrevTouchCollection = new TouchCollection ();
			_fResetInputState = true;
        }

        public bool TouchPanelPressed()
        {
            return (currentTouchCollection.Count > 0 && prevTouchCollection.Count == 0);
        }

        public bool TouchPanelReleased()
        {
            return (currentTouchCollection.Count == 0 && prevTouchCollection.Count > 0);
        }

        public bool TouchPanelDown()
        {
            return currentTouchCollection.Count > 0;
        }
    }
}
