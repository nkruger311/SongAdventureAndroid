using System;
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
	[XmlType("NPC")]
	public class NPC
	{
		public event EventHandler OnClick;
		public event EventHandler OnIsActiveChanged;

		public string Name;
		public Image Image;
		public Image InteractionImage;
		public Image MissionCompleteInteractionImage;
		public bool CurrentlyInteracting;
		public Vector2 MapPosition;
		[XmlIgnore]
		public Rectangle BoundingBox;
		public Vector2 BoundingBoxDimensions;
		public Vector2 BoundingBoxPosition;

		[XmlIgnore]
		public Rectangle InteractionBoundingBox;
		public Vector2 InteractionBoundingBoxDimensions;
		public Vector2 InteractionBoundingBoxPosition;

		public Tile.TileState State = Tile.TileState.Solid;
		public string LevelNumber;
		public bool HasMission;
		public bool MissionCompleted;
		public string MissionSongismName;
		public InventoryItem MissionReward;
		public bool IsActive;


		public NPC()
		{

		}

		public void LoadContent()
		{
			Image.LoadContent();

			InteractionBoundingBoxPosition = new Vector2 (0, (int)(ScreenManager.Instance.GraphicsDevice.Viewport.Height - InteractionBoundingBoxDimensions.Y));

			InteractionImage.Position = InteractionBoundingBoxPosition;
			MissionCompleteInteractionImage.Position = InteractionBoundingBoxPosition;

			InteractionImage.LoadContent ();
			MissionCompleteInteractionImage.LoadContent ();
			BoundingBox = new Rectangle((int)BoundingBoxPosition.X, (int)BoundingBoxPosition.Y, (int)BoundingBoxDimensions.X, (int)BoundingBoxDimensions.Y);

			InteractionBoundingBox = new Rectangle((int)InteractionBoundingBoxPosition.X, (int)InteractionBoundingBoxPosition.Y, (int)InteractionBoundingBoxDimensions.X, (int)InteractionBoundingBoxDimensions.Y);
		}

		public void UnloadContent()
		{
			Image.UnloadContent();
			InteractionImage.UnloadContent ();
			MissionCompleteInteractionImage.UnloadContent ();
		}

		public void Update(GameTime gameTime, ref Player player)
		{
			if (InputManager.Instance.TouchPanelPressed () && BoundingBox.Contains (InputManager.Instance.TransformedTouchPosition)) {
				if (Inventory.Instance.PrerequisiteSongismHasBeenDiscovered (this.MissionSongismName)) {
					MissionCompleted = true;


				} else {
					InteractionBoundingBoxPosition.X = (int)(Camera2D.Instance.Position.X - (ScreenManager.Instance.GraphicsDevice.Viewport.Width / 2)) + 256;

					InteractionImage.Position.X = InteractionBoundingBoxPosition.X;
					MissionCompleteInteractionImage.Position.X = InteractionBoundingBoxPosition.X;
					InteractionBoundingBox.X = (int)InteractionBoundingBoxPosition.X;
				}

				OnClick (this, null);
			}
			else if (InputManager.Instance.TouchPanelPressed () && InteractionBoundingBox.Contains (InputManager.Instance.TransformedTouchPosition) && CurrentlyInteracting) {
				OnClick (this, null);

				if (MissionCompleted) {
					if (MissionReward != null) {
						Inventory.Instance.Items.Add (MissionReward);
					}

					SetNpcToInactive ();
				}
			}

			if (CurrentlyInteracting) {
				if (MissionCompleted) {
					MissionCompleteInteractionImage.Update (gameTime);
				} else {
					InteractionImage.Update (gameTime);
				}
			} else {
				Image.Update (gameTime);

				if ((State & Tile.TileState.Solid) == Tile.TileState.Solid) {
					Rectangle playerRect = new Rectangle ((int)player.Image.Position.X, (int)(player.Image.Position.Y + (player.Image.SourceRect.Height / 2)), player.Image.SourceRect.Width, (int)(player.Image.SourceRect.Height / 2));

					if (playerRect.Intersects(BoundingBox))
					{
						Rectangle rectIntersection = Rectangle.Intersect (playerRect, BoundingBox);

						if (rectIntersection.Height > rectIntersection.Width) {
							if (player.Velocity.X < 0) {
								player.Image.Position.X = BoundingBox.Right;
								player.Velocity.X = 0;
							}

							if (player.Velocity.X > 0) {
								player.Image.Position.X = BoundingBox.Left - player.Image.SourceRect.Width;
								player.Velocity.X = 0;
							}
						}

						if (rectIntersection.Width > rectIntersection.Height) {
							if (player.Velocity.Y < 0) {
								//player.Image.Position.Y = BoundingBox.Bottom;
								player.Image.Position.Y = BoundingBox.Bottom - (player.Image.SourceRect.Height / 2);
								player.Velocity.Y = 0;
							}

							if (player.Velocity.Y > 0) {
								player.Image.Position.Y = BoundingBox.Top - player.Image.SourceRect.Height;
								player.Velocity.Y = 0;
							}
						}

						//if (player.Velocity.X < 0)
						//player.Image.Position.X = BoundingBox.Right;
						//else if (player.Velocity.X > 0)
						//player.Image.Position.X = BoundingBox.Left - player.Image.SourceRect.Width;
						//else if (player.Velocity.Y < 0)
						//player.Image.Position.Y = BoundingBox.Bottom;
						//else
						//player.Image.Position.Y = BoundingBox.Top - player.Image.SourceRect.Height;

						//player.Velocity = Vector2.Zero;
					}
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (CurrentlyInteracting) {
				if (MissionCompleted) {
					MissionCompleteInteractionImage.Draw (spriteBatch);
				} else {
					InteractionImage.Draw (spriteBatch);
				}
			} 
			//else {
			//	if (Camera2D.Instance.IsInView (Image.Position, Image.Texture))
			//		Image.Draw (spriteBatch);
			//}

			if (Camera2D.Instance.IsInView (Image.Position, Image.Texture))
				Image.Draw (spriteBatch);
		}

		public void SetNpcToInactive()
		{
			this.IsActive = false;
			OnIsActiveChanged (this, null);
		}
	}
}
