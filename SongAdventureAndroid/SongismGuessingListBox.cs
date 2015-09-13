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
    class SongismGuessingListBox
    {
        [XmlIgnore]
        public List<SongismGuessingItem> SongismGuessingItems;

		//private string _sCurrentAlbumName = "Music";
		private List<string> _lsAlbumNames;
		private short _iCurrentAlbumNameIndex = 0;
		private short _iNumberOfAlbums = 1;
		private short _iNumberOfAlbumsLoaded = 0;

		private float _dGuessingItemDockingPositionX;
		private List<float> _ldDockingPositionsX;
		private float _dSwipeMoveVelocityX;
		private float _dPreviousPositionX;
		private bool _fList1Docked = false;
		private bool _fList2Docked = false;
		private bool _fList3Docked = false;

		[XmlIgnore]
        public SongismGuessingItem SelectedItem;
        Rectangle _listBoxDisplayArea;
        Vector2 _velocity;
        int _moveSpeed = 50;
		short _iSwipeSpeed = 50;
        bool _scrollStarted = false;
		bool _fSwipeStarted = false;
		bool _fSwipeCompleted = true;

		[XmlIgnore]
		public bool IsInitializing = true;

		private float _dEndingPositionY = float.MaxValue;

		private Vector2 prevItemPosition = new Vector2(16, 128);
		private int prevItemHeight = 0;
		private string sPreviousAlbumName = "";
		private short iResetCounter = 1;

		private enum DockingPositionIndex
		{
			Left = 0,
			Center = 1,
			Right = 2
		}

        public SongismGuessingListBox()
        {
			_lsAlbumNames = new List<string> ();
			_lsAlbumNames.Add ("Music");
			_lsAlbumNames.Add ("Grassroots");
			_lsAlbumNames.Add ("311");
			_lsAlbumNames.Add ("Enlarged to Show Detail");
			_lsAlbumNames.Add ("Transistor");
			_lsAlbumNames.Add ("Omaha Sessions");
			_lsAlbumNames.Add ("Soundsystem");
			_lsAlbumNames.Add ("From Chaos");
			_lsAlbumNames.Add("Enlarged to Show Detail 2");
			_lsAlbumNames.Add ("Evolver");
			_lsAlbumNames.Add ("Greatest Hits '93-'03");
			_lsAlbumNames.Add ("Don't Tread On Me");
			_lsAlbumNames.Add ("Uplifter");
			_lsAlbumNames.Add ("Universal Pulse");
			_lsAlbumNames.Add ("Stereolithic");

			_ldDockingPositionsX = new List<float> ();

            SongismGuessingItems = new List<SongismGuessingItem>();

            SelectedItem = null;
            _listBoxDisplayArea = new Rectangle(54, 128, 512, 480);
            _velocity = Vector2.Zero;

        }

        public void LoadContent()
        {
			IsInitializing = true;

            XmlManager<List<SongismGuessingItem>> guessingItemsLoader = new XmlManager<List<SongismGuessingItem>>();
            SongismGuessingItems = guessingItemsLoader.Load(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml"));

            //Vector2 prevItemPosition = new Vector2(256, 128);
			//Vector2 prevItemPosition = new Vector2(16, 128);
            //int prevItemHeight = 0;

			//string sPreviousAlbumName = "";
			//short iResetCounter = 1;

			// Set Docking Position X to the first X value
			_dGuessingItemDockingPositionX = prevItemPosition.X;
			_dPreviousPositionX = prevItemPosition.X;

			_ldDockingPositionsX.Add(prevItemPosition.X - _listBoxDisplayArea.Width);
			_ldDockingPositionsX.Add(prevItemPosition.X);
			_ldDockingPositionsX.Add(64 + _listBoxDisplayArea.Width);

			LoadGuessingItemsContent ();

			IsInitializing = false;
        }

		private void LoadGuessingItemsContent(bool initialLoading = true)
		{
			//for (int i = 0; i < SongismGuessingItems.Count; i++)
			for (int i = _iNumberOfAlbumsLoaded; i <SongismGuessingItems.Count; i++)
			{
				if (!SongismGuessingItems [i].Loaded) {
					if (SongismGuessingItems[i].DisplayItem)
					{
						if (!String.Equals (SongismGuessingItems [i].AlbumName, sPreviousAlbumName) && !String.IsNullOrEmpty (sPreviousAlbumName)) {
							prevItemPosition = new Vector2(64 + _listBoxDisplayArea.Width, 128);
							prevItemHeight = 0;

							iResetCounter++;
							_iNumberOfAlbums++;
							_iNumberOfAlbumsLoaded++;

							if (_iNumberOfAlbumsLoaded >= 3 && initialLoading) {
								_iNumberOfAlbumsLoaded--;
								break;
							}

							initialLoading = true;
						}

						sPreviousAlbumName = SongismGuessingItems [i].AlbumName;

						SongismGuessingItems [i].Position = new Vector2 (prevItemPosition.X, prevItemPosition.Y + prevItemHeight + 10);
						SongismGuessingItems [i].StartingPositionY = SongismGuessingItems [i].Position.Y;

						SongismGuessingItems [i].LoadContent ();

						if (String.Equals (SongismGuessingItems [i].AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
							SongismGuessingItems [i].OnGuessingItemPressed += SongismGuessingListBox_OnGuessingItemPressed;
							SongismGuessingItems [i].IsButtonEnabled = true;
						}

						prevItemPosition = SongismGuessingItems [i].Position;
						//prevItemHeight = SongismGuessingItems [i].SongNameImage.SourceRect.Height;
						prevItemHeight = SongismGuessingItems[i].RadioButton.SourceRect.Height + 10;
					}
				}
			}
		}

        void SongismGuessingListBox_OnGuessingItemPressed(object sender, EventArgs e)
        {
            for (int i = 0; i < SongismGuessingItems.Count; i++)
            {
                if (SongismGuessingItems[i].Equals((SongismGuessingItem)sender))
                {
                    //SongismGuessingItems[i].Checked = !((SongismGuessingItem)sender).Checked;
					SongismGuessingItems[i].Checked = true;
                }
                else
                {
                    SongismGuessingItems[i].Checked = false;
                }
            }

            if (((SongismGuessingItem)sender).Checked)
                SelectedItem = (SongismGuessingItem)sender;
            else
                SelectedItem = null;
        }

        public void UnloadContent()
        {
            XmlManager<List<SongismGuessingItem>> guessingItemsSaver = new XmlManager<List<SongismGuessingItem>>();
            guessingItemsSaver.Save(System.IO.Path.Combine(Globals.LoadGameplaySongismsDirectory, "SongismGuessingItems.xml"), SongismGuessingItems);

            foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems)
            {
				if (songismGuessingItem.Loaded) {
					songismGuessingItem.UnloadContent ();
				} else {
					break;
				}
				/*
				if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
					if (songismGuessingItem.DisplayItem)
						songismGuessingItem.UnloadContent ();
				} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 1])) {
					if (songismGuessingItem.DisplayItem)
						songismGuessingItem.UnloadContent ();
				} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 2])) {
					if (songismGuessingItem.DisplayItem)
						songismGuessingItem.UnloadContent ();
				} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 3])) {
					if (songismGuessingItem.DisplayItem)
						songismGuessingItem.UnloadContent ();
				}
				*/
            }
        }

        public void Update(GameTime gameTime)
        {
			if (!IsInitializing) {

				Swipe (gameTime);

				if (!_fSwipeStarted) {
					Scroll (gameTime);
				}

				foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems) {
					if (songismGuessingItem.Loaded) {
						songismGuessingItem.Update (gameTime);
					} else {
						break;
					}
				}
			}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
			if (!IsInitializing) {
				foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems) {
					//if (songismGuessingItem.IsInitializing) {
					//	break;
					//}

					if (songismGuessingItem.Loaded) {
						if (songismGuessingItem.DisplayItem) {

							if (songismGuessingItem.Position.X > (_ldDockingPositionsX [0] + 10) && songismGuessingItem.Position.X < (_ldDockingPositionsX [2] - 10)) {
								if (_listBoxDisplayArea.Contains (new Point (_listBoxDisplayArea.Left + 10, (int)songismGuessingItem.Position.Y))) {
									songismGuessingItem.Draw (spriteBatch);

									if (songismGuessingItem.LastSongOnAlbum) {
										if (_dEndingPositionY != float.MinValue) {
											if (_dEndingPositionY == float.MaxValue) {
												_dEndingPositionY = songismGuessingItem.Position.Y;
											}
										}
									}
								}
							}
						}
					} else {
						break;
					}
				}
			}
        }

        void Scroll(GameTime gameTime)
        {
			GetScrollVelocity (gameTime);

			foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems) {
				if (songismGuessingItem.Loaded) {
					if (songismGuessingItem.DisplayItem) {
						if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
							if (_dEndingPositionY != float.MinValue) {
								if (_dEndingPositionY < float.MaxValue) {
									songismGuessingItem.EndingPositionY = songismGuessingItem.Position.Y;
								}
							}

							if (songismGuessingItem.Position.Y + _velocity.Y > songismGuessingItem.StartingPositionY)
								songismGuessingItem.Position.Y = songismGuessingItem.StartingPositionY;
							else if (songismGuessingItem.Position.Y + _velocity.Y < songismGuessingItem.EndingPositionY)
								songismGuessingItem.Position.Y = songismGuessingItem.EndingPositionY;
							else
								songismGuessingItem.Position.Y += _velocity.Y;
						}
						songismGuessingItem.Update (gameTime);
					}
				} else {
					break;
				}
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

		void Swipe(GameTime gameTime)
		{
			GetSwipeVelocity (gameTime);

			if (_fSwipeStarted) {
				

				foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems) {
					if (songismGuessingItem.Loaded) {
						if (songismGuessingItem.DisplayItem) {
							if (_velocity.X != 0 && _dSwipeMoveVelocityX != 0) {
								if (_iCurrentAlbumNameIndex == 0) {
									if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [1] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [1] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList1Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList1Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}

										_dPreviousPositionX = songismGuessingItem.Position.X;
									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 1])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [2] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [2] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList2Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList2Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}
										_dPreviousPositionX = songismGuessingItem.Position.X;

									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 2])) {
										if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 2])) {
											if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [2] + (_dSwipeMoveVelocityX * 2))) {
												songismGuessingItem.Position.X = _ldDockingPositionsX [2] - _dSwipeMoveVelocityX;
												//_fSwipeStarted = false;
												_fList3Docked = true;

												_fSwipeCompleted = true;
											}

											if (!_fList3Docked) {
												songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
											}
											_dPreviousPositionX = songismGuessingItem.Position.X;
										}
									}






								} else if (_iCurrentAlbumNameIndex == _lsAlbumNames.Count - 1) {

									if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex - 2])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [0] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [0] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList1Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList1Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}

										_dPreviousPositionX = songismGuessingItem.Position.X;
									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex - 1])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [0] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [0] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList2Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList2Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}
										_dPreviousPositionX = songismGuessingItem.Position.X;

									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
										if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
											if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [1] + (_dSwipeMoveVelocityX * 2))) {
												songismGuessingItem.Position.X = _ldDockingPositionsX [1] - _dSwipeMoveVelocityX;
												//_fSwipeStarted = false;
												_fList3Docked = true;

												_fSwipeCompleted = true;
											}

											if (!_fList3Docked) {
												songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
											}
											_dPreviousPositionX = songismGuessingItem.Position.X;
										}
									}


								} else if (_dSwipeMoveVelocityX < 0) {
									if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex - 1])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [0] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [0] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList1Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList1Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}

										_dPreviousPositionX = songismGuessingItem.Position.X;
									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [1] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [1] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList2Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList2Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}
										_dPreviousPositionX = songismGuessingItem.Position.X;

									} else {
										if (_iCurrentAlbumNameIndex <= _lsAlbumNames.Count - 1) {
											if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 1])) {
												if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) < (_ldDockingPositionsX [2] + (_dSwipeMoveVelocityX * 2))) {
													songismGuessingItem.Position.X = _ldDockingPositionsX [2] - _dSwipeMoveVelocityX;
													//_fSwipeStarted = false;
													_fList3Docked = true;

													_fSwipeCompleted = true;
												}

												if (!_fList3Docked) {
													songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
												}
												_dPreviousPositionX = songismGuessingItem.Position.X;
											}
										}
									}
								} else if (_dSwipeMoveVelocityX > 0) {
									if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex - 1])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [0] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [0] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList1Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList1Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}

										_dPreviousPositionX = songismGuessingItem.Position.X;
									} else if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
										if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [1] + (_dSwipeMoveVelocityX * 2))) {
											songismGuessingItem.Position.X = _ldDockingPositionsX [1] - _dSwipeMoveVelocityX;
											//_fSwipeStarted = false;
											_fList2Docked = true;

											_fSwipeCompleted = true;
										}

										if (!_fList2Docked) {
											songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
										}
										_dPreviousPositionX = songismGuessingItem.Position.X;

									} else {
										if (_iCurrentAlbumNameIndex <= _lsAlbumNames.Count - 1) {
											if (String.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex + 1])) {
												if ((songismGuessingItem.Position.X + _dSwipeMoveVelocityX) > (_ldDockingPositionsX [2] + (_dSwipeMoveVelocityX * 2))) {
													songismGuessingItem.Position.X = _ldDockingPositionsX [2] - _dSwipeMoveVelocityX;
													//_fSwipeStarted = false;
													_fList3Docked = true;

													_fSwipeCompleted = true;
												}

												if (!_fList3Docked) {
													songismGuessingItem.Position.X += _dSwipeMoveVelocityX;
												}
												_dPreviousPositionX = songismGuessingItem.Position.X;
											}
										}
									}
								}


								//if (_dSwipeMoveVelocityX == 0.0f) {
								//	if (songismGuessingItem.Position.X == _dGuessingItemDockingPositionX)
								//		_fSwipeStarted = false;
								//}


								//if (songismGuessingItem.Position.X
							}

							songismGuessingItem.Update (gameTime);
						}
					} else {
						break;
					}
				}

				if (_fList1Docked && _fList2Docked && _fList3Docked) {
					_fList1Docked = false;
					_fList2Docked = false;
					_fList3Docked = false;
					_fSwipeStarted = false;

					_dEndingPositionY = float.MaxValue;

					foreach (SongismGuessingItem songismGuessingItem in SongismGuessingItems) {
						if (songismGuessingItem.Loaded) {
							if (string.Equals (songismGuessingItem.AlbumName, _lsAlbumNames [_iCurrentAlbumNameIndex])) {
								songismGuessingItem.OnGuessingItemPressed += SongismGuessingListBox_OnGuessingItemPressed;
								songismGuessingItem.IsButtonEnabled = true;
							} else if (songismGuessingItem.IsButtonEnabled) {
								songismGuessingItem.OnGuessingItemPressed -= SongismGuessingListBox_OnGuessingItemPressed;
								songismGuessingItem.IsButtonEnabled = false;
							}
						} else {
							break;
						}
					}
				}
			}
		}

		private void GetSwipeVelocity(GameTime gameTime)
		{
			if (_fSwipeCompleted) {
				//if (InputManager.Instance.TouchPanelDown () && _listBoxDisplayArea.Contains (InputManager.Instance.TransformedTouchPosition)) {
				if (InputManager.Instance.TouchPanelReleased () && _listBoxDisplayArea.Contains (InputManager.Instance.PrevTransformedTouchPosition)) {
					//Vector2 delta = InputManager.Instance.TransformedTouchPosition - InputManager.Instance.PrevTransformedTouchPosition;
					Vector2 delta = Vector2.Zero;

					//if (InputManager.Instance.PrevTouchPosition != new Vector2 (-1, -1)) {
					if (InputManager.Instance.PrevPrevTouchPosition != new Vector2 (-1, -1)) {
						//_fSwipeStarted = true;
						//float scrollDelta = InputManager.Instance.TransformedTouchPosition.X - InputManager.Instance.PrevTransformedTouchPosition.X;
						//Vector2 delta = InputManager.Instance.TransformedTouchPosition - InputManager.Instance.PrevTransformedTouchPosition;
						//if (InputManager.Instance.TransformedTouchPosition.X > InputManager.Instance.PrevTransformedTouchPosition.X) {
						if (InputManager.Instance.PrevTransformedTouchPosition.X > InputManager.Instance.PrevPrevTransformedTouchPosition.X) {
							if (_iCurrentAlbumNameIndex == 0) {
								_fSwipeStarted = false;
								_dSwipeMoveVelocityX = 0.0f;
								_velocity.X = 0.0f;
								delta = Vector2.Zero;
							} else {
								if (!_fSwipeStarted) {
									//delta = InputManager.Instance.TransformedTouchPosition - InputManager.Instance.PrevTransformedTouchPosition;
									delta = InputManager.Instance.PrevTransformedTouchPosition - InputManager.Instance.PrevPrevTransformedTouchPosition;

									/* Swiping from left to right, velocity is positive */
									_velocity.X = _iSwipeSpeed * delta.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
									_dSwipeMoveVelocityX = 10.0f;
									//_iCurrentAlbumNameIndex--;
								}
							}
						//} else if (InputManager.Instance.TransformedTouchPosition.X < InputManager.Instance.PrevTransformedTouchPosition.X) {
						} else if (InputManager.Instance.PrevTransformedTouchPosition.X < InputManager.Instance.PrevPrevTransformedTouchPosition.X) {

							if (_iCurrentAlbumNameIndex == _lsAlbumNames.Count - 1) {
								_fSwipeStarted = false;
								_dSwipeMoveVelocityX = 0.0f;
								_velocity.X = 0.0f;
								delta = Vector2.Zero;
							} else {
								if (!_fSwipeStarted) {
									//delta = InputManager.Instance.TransformedTouchPosition - InputManager.Instance.PrevTransformedTouchPosition;
									delta = InputManager.Instance.PrevTransformedTouchPosition - InputManager.Instance.PrevPrevTransformedTouchPosition;

									/* Swiping from right to left, velocity is negative */
									_velocity.X = _iSwipeSpeed * delta.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
									_dSwipeMoveVelocityX = -10.0f;
									//_iCurrentAlbumNameIndex++;
								}
							}
						}
					}

					if (Math.Abs (delta.X) > Math.Abs (delta.Y) && !_fSwipeStarted) {
						if (Math.Abs (delta.X) > 10.0f) {
							_fSwipeStarted = true;
							_fSwipeCompleted = false;

							// Stop scrolling
							_velocity.Y = 0.0f;

							if (_dSwipeMoveVelocityX > 0) {
								_iCurrentAlbumNameIndex--;
							} else {
								_iCurrentAlbumNameIndex++;

								if (_iCurrentAlbumNameIndex > _iNumberOfAlbumsLoaded - 2) {
									LoadGuessingItemsContent (false);
								}
							}

							InputManager.Instance.ResetInputState ();
						}
					}
				}
			}
		}
    }
}