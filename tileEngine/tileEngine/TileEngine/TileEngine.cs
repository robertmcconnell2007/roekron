#region File Description
//-----------------------------------------------------------------------------
// PlayerPosition.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RolePlayingGameData;
#endregion

namespace RolePlaying
{
    /// <summary>
    /// Static class for a tileable map
    /// </summary>
    static class TileEngine
    {
        public static ContentManager ContentManager = null;
        public static Random rand = new Random();
        #region Map


        /// <summary>
        /// The map being used by the tile engine.
        /// </summary>
        private static Map map = null;

        /// <summary>
        /// The map being used by the tile engine.
        /// </summary>
        public static Map Map
        {
            get { return map; }
        }


        /// <summary>
        /// The position of the outside 0,0 corner of the map, in pixels.
        /// </summary>
        private static Vector2 mapOriginPosition;


        /// <summary>
        /// Calculate the screen position of a given map location (in tiles).
        /// </summary>
        /// <param name="mapPosition">A map location, in tiles.</param>
        /// <returns>The current screen position of that location.</returns>
        public static Vector2 GetScreenPosition(Point mapPosition)
        {
            return new Vector2(
                mapOriginPosition.X + mapPosition.X * map.TileSize.X,
                mapOriginPosition.Y + mapPosition.Y * map.TileSize.Y);
        }


        /// <summary>
        /// Set the map in use by the tile engine.
        /// </summary>
        /// <param name="map">The new map for the tile engine.</param>
        /// <param name="portal">The portal the party is entering on, if any.</param>
        public static void SetMap(Map newMap, MapEntry<Portal> portalEntry)
        {
            // check the parameter
            if (newMap == null)
            {
                throw new ArgumentNullException("newMap");
            }

            // assign the new map
            map = newMap;

            // reset the map origin, which will be recalculate on the first update
            mapOriginPosition = Vector2.Zero;

            //this needs to be moved later

            // move the party to its initial position
            if (portalEntry == null)
            {
                // no portal - use the spawn position
                player.PartyLeaderPosition.TilePosition = map.SpawnMapPosition;
                player.partyLeaderPosition.TileOffset = Vector2.Zero;
                player.partyLeaderPosition.Direction = Direction.South;

                npc1.npcPosition.TilePosition = map.SpawnMapPosition;
                npc1.npcPosition.TileOffset = Vector2.Zero;
                npc1.npcPosition.Direction = Direction.South;
            }
            else
            {
                // use the portal provided, which may include automatic movement
                player.partyLeaderPosition.TilePosition = portalEntry.MapPosition;
                player.partyLeaderPosition.TileOffset = Vector2.Zero;
                player.partyLeaderPosition.Direction = portalEntry.Direction;
                player.autoPartyLeaderMovement = Vector2.Multiply(
                    new Vector2(map.TileSize.X, map.TileSize.Y), new Vector2(
                    portalEntry.Content.LandingMapPosition.X -
                        player.partyLeaderPosition.TilePosition.X,
                    portalEntry.Content.LandingMapPosition.Y -
                        player.partyLeaderPosition.TilePosition.Y));

                npc1.npcPosition.TilePosition = portalEntry.MapPosition;
                npc1.npcPosition.TileOffset = Vector2.Zero;
                npc1.npcPosition.Direction = portalEntry.Direction;
            }
        }


        #endregion


        #region Graphics Data


        /// <summary>
        /// The viewport that the tile engine is rendering within.
        /// </summary>
        private static Viewport viewport;

        /// <summary>
        /// The viewport that the tile engine is rendering within.
        /// </summary>
        public static Viewport Viewport
        {
            get { return viewport; }
            set
            {
                viewport = value;
                viewportCenter = new Vector2(
                    viewport.X + viewport.Width / 2f,
                    viewport.Y + viewport.Height / 2f);
            }
        }


        /// <summary>
        /// The center of the current viewport.
        /// </summary>
        private static Vector2 viewportCenter;


        #endregion


        #region Party


        /// <summary>
        /// The speed of the party leader, in units per second.
        /// </summary>
        /// <remarks>
        /// The movementCollisionTolerance constant should be a multiple of this number.
        /// </remarks>
        /// 
        //private const float partyLeaderMovementSpeed = 3f;

        //This is the new built class to handle all player options
        //like movement and money etc.
        public static MainPlayer player = new MainPlayer();
        public static NPC npc1 = new NPC();

        /// <summary>
        /// The current position of the party leader.
        /// </summary>
        //private static PlayerPosition player.partyLeaderPosition = new PlayerPosition();
        //public static PlayerPosition player.partyLeaderPosition
        //{
        //    get { return player.partyLeaderPosition; }
        //    set { player.partyLeaderPosition = value; }
        //}


        /// <summary>
        /// The automatic movement remaining for the party leader.
        /// </summary>
        /// <remarks>
        /// This is typically used for automatic movement when spawning on a map.
        /// </remarks>
        //private static Vector2 autoPartyLeaderMovement = Vector2.Zero;


        /// <summary>
        /// Updates the automatic movement of the party.
        /// </summary>
        /// <returns>The automatic movement for this update.</returns>
        private static Vector2 UpdatePartyLeaderAutoMovement(GameTime gameTime)
        {
            // check for any remaining auto-movement
            if (player.autoPartyLeaderMovement == Vector2.Zero)
            {
                return Vector2.Zero;
            }

            // get the remaining-movement direction
            Vector2 autoMovementDirection = Vector2.Normalize(player.autoPartyLeaderMovement);

            // calculate the potential movement vector
            Vector2 movement = Vector2.Multiply(autoMovementDirection,
                player.moveSpeed);

            // limit the potential movement vector by the remaining auto-movement
            movement.X = Math.Sign(movement.X) * MathHelper.Min(Math.Abs(movement.X),
                Math.Abs(player.autoPartyLeaderMovement.X));
            movement.Y = Math.Sign(movement.Y) * MathHelper.Min(Math.Abs(movement.Y),
                Math.Abs(player.autoPartyLeaderMovement.Y));

            // remove the movement from the total remaining auto-movement
            player.autoPartyLeaderMovement -= movement;

            return movement;
        }


        /// <summary> 
        /// Update the user-controlled movement of the party. 
        /// </summary> 
        /// <returns>The controlled movement for this update.</returns> 
        private static Vector2 UpdateUserMovement(GameTime gameTime)
        {
            Vector2 desiredMovement = Vector2.Zero;
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();
            KeyboardState oldKeyboardState = keyboardState;

#if !XBOX
            if(keyboardState.IsKeyDown(Keys.Up))
            {
                if (CanCharMoveUp())
                {
                    desiredMovement.Y -= player.moveSpeed;
                }
            }
            if(keyboardState.IsKeyDown(Keys.Down))
            {
                if (CanCharMoveDown())
                {
                    desiredMovement.Y += player.moveSpeed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (CanCharMoveLeft())
                {
                    desiredMovement.X -= player.moveSpeed;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if (CanCharMoveRight())
                {
                    desiredMovement.X += player.moveSpeed;
                }
            }
#endif
            // accumulate the desired direction from user input 
            if (gamePadState.ThumbSticks.Left.Y > 0f)
            {
                if (CanCharMoveUp())
                {
                    desiredMovement.Y -= player.moveSpeed;
                }
            }
            if (gamePadState.ThumbSticks.Left.Y < 0f)
            {
                if (CanCharMoveDown())
                {
                    desiredMovement.Y += player.moveSpeed;
                }
            }
            if (gamePadState.ThumbSticks.Left.X < 0f)
            {
                if (CanCharMoveLeft())
                {
                    desiredMovement.X -= player.moveSpeed;
                }
            }
            if (gamePadState.ThumbSticks.Left.X > 0f)
            {
                if (CanCharMoveRight())
                {
                    desiredMovement.X += player.moveSpeed;
                }
            }
            if (desiredMovement == Vector2.Zero)
            {
                return Vector2.Zero;
            }
            return desiredMovement;
        }


        #endregion


        #region Collision


        /// <summary>
        /// The number of pixels that characters should be allowed to move into 
        /// blocking tiles.
        /// </summary>
        /// <remarks>
        /// The partyMovementSpeed constant should cleanly divide this number.
        /// </remarks>
        const int movementCollisionTolerance = 12;


        /// <summary>
        /// Returns true if the player can move up from their current position.
        /// </summary>
        private static bool CanCharMoveUp()
        {
            // if they're not within the tolerance of the next tile, then this is moot
            if (player.partyLeaderPosition.TileOffset.Y > -movementCollisionTolerance)
            {
                return true;
            }

            // if the player is at the outside left and right edges, 
            // then check the diagonal tiles
            if (player.partyLeaderPosition.TileOffset.X < -movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X - 1,
                    player.partyLeaderPosition.TilePosition.Y - 1)))
                {
                    return false;
                }
            }
            else if (player.partyLeaderPosition.TileOffset.X > movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X + 1,
                    player.partyLeaderPosition.TilePosition.Y - 1)))
                {
                    return false;
                }
            }

            // check the tile above the current one
            return !map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X,
                    player.partyLeaderPosition.TilePosition.Y - 1));
        }


        /// <summary>
        /// Returns true if the player can move down from their current position.
        /// </summary>
        private static bool CanCharMoveDown()
        {
            // if they're not within the tolerance of the next tile, then this is moot
            if (player.partyLeaderPosition.TileOffset.Y < movementCollisionTolerance)
            {
                return true;
            }

            // if the player is at the outside left and right edges, 
            // then check the diagonal tiles
            if (player.partyLeaderPosition.TileOffset.X < -movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X - 1,
                    player.partyLeaderPosition.TilePosition.Y + 1)))
                {
                    return false;
                }
            }
            else if (player.partyLeaderPosition.TileOffset.X > movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X + 1,
                    player.partyLeaderPosition.TilePosition.Y + 1)))
                {
                    return false;
                }
            }

            // check the tile below the current one
            return !map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X,
                    player.partyLeaderPosition.TilePosition.Y + 1));
        }


        /// <summary>
        /// Returns true if the player can move left from their current position.
        /// </summary>
        private static bool CanCharMoveLeft()
        {
            // if they're not within the tolerance of the next tile, then this is moot
            if (player.partyLeaderPosition.TileOffset.X > -movementCollisionTolerance)
            {
                return true;
            }

            // if the player is at the outside left and right edges, 
            // then check the diagonal tiles
            if (player.partyLeaderPosition.TileOffset.Y < -movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X - 1,
                    player.partyLeaderPosition.TilePosition.Y - 1)))
                {
                    return false;
                }
            }
            else if (player.partyLeaderPosition.TileOffset.Y > movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X - 1,
                    player.partyLeaderPosition.TilePosition.Y + 1)))
                {
                    return false;
                }
            }

            // check the tile to the left of the current one
            return !map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X - 1,
                    player.partyLeaderPosition.TilePosition.Y));
        }


        /// <summary>
        /// Returns true if the player can move right from their current position.
        /// </summary>
        private static bool CanCharMoveRight()
        {
            // if they're not within the tolerance of the next tile, then this is moot
            if (player.partyLeaderPosition.TileOffset.X < movementCollisionTolerance)
            {
                return true;
            }

            // if the player is at the outside left and right edges, 
            // then check the diagonal tiles
            if (player.partyLeaderPosition.TileOffset.Y < -movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X + 1,
                    player.partyLeaderPosition.TilePosition.Y - 1)))
                {
                    return false;
                }
            }
            else if (player.partyLeaderPosition.TileOffset.Y > movementCollisionTolerance)
            {
                if (map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X + 1,
                    player.partyLeaderPosition.TilePosition.Y + 1)))
                {
                    return false;
                }
            }

            // check the tile to the right of the current one
            return !map.IsBlocked(new Point(
                    player.partyLeaderPosition.TilePosition.X + 1,
                    player.partyLeaderPosition.TilePosition.Y));
        }


        #endregion


        #region Updating


        /// <summary>
        /// Update the tile engine.
        /// </summary>
        public static void Update(GameTime gameTime)
        {
            // check for auto-movement
            Vector2 autoMovement = UpdatePartyLeaderAutoMovement(gameTime);

            // if there is no auto-movement, handle user controls
            Vector2 userMovement = Vector2.Zero;
            if (autoMovement == Vector2.Zero)
            {
                userMovement = UpdateUserMovement(gameTime);
                // calculate the desired position
                if (userMovement != Vector2.Zero)
                {
                    Point desiredTilePosition = player.partyLeaderPosition.TilePosition;
                    Vector2 desiredTileOffset = player.partyLeaderPosition.TileOffset;
                    PlayerPosition.CalculateMovement(
                        Vector2.Multiply(userMovement, 15f),
                        ref desiredTilePosition, ref desiredTileOffset);
                    // check for collisions or encounters in the new tile
                    if ((player.partyLeaderPosition.TilePosition != desiredTilePosition) &&
                        !MoveIntoTile(desiredTilePosition))
                    {
                        userMovement = Vector2.Zero;
                    }
                }
            }

            if (rand.Next(1, 100) > 90)
            {

            }

            // move the party
            Point oldPartyLeaderTilePosition = player.partyLeaderPosition.TilePosition;
            player.partyLeaderPosition.Move(autoMovement + userMovement);

            // if the tile position has changed, check for random combat
            ////if ((autoMovement == Vector2.Zero) &&
            ////    (player.partyLeaderPosition.TilePosition != oldPartyLeaderTilePosition))
            ////{
            ////    Session.CheckForRandomCombat(Map.RandomCombat);
            ////}

            // adjust the map origin so that the party is at the center of the viewport
            ////////mapOriginPosition += viewportCenter - (player.partyLeaderPosition.ScreenPosition +
            ////////    Session.Party.Players[0].MapSprite.SourceOffset);
            mapOriginPosition += viewportCenter - player.partyLeaderPosition.ScreenPosition;

            // make sure the boundaries of the map are never inside the viewport
            mapOriginPosition.X = MathHelper.Min(mapOriginPosition.X, viewport.X);
            mapOriginPosition.Y = MathHelper.Min(mapOriginPosition.Y, viewport.Y);
            mapOriginPosition.X += MathHelper.Max(
                (viewport.X + viewport.Width) -
                (mapOriginPosition.X + map.MapDimensions.X * map.TileSize.X), 0f);
            ////////////mapOriginPosition.Y += MathHelper.Max(
            ////////////    (viewport.Y + viewport.Height - Hud.HudHeight) -
            ////////////    (mapOriginPosition.Y + map.MapDimensions.Y * map.TileSize.Y), 0f);
            mapOriginPosition.Y += MathHelper.Max((viewport.Y + viewport.Height)
                -(mapOriginPosition.Y + map.MapDimensions.Y * map.TileSize.Y), 0f);
        }

        
        /// <summary>
        /// Performs any actions associated with moving into a new tile.
        /// </summary>
        /// <returns>True if the character can move into the tile.</returns>
        private static bool MoveIntoTile(Point mapPosition)
        {
           // if the tile is blocked, then this is simple
            if (map.IsBlocked(mapPosition))
            {
                return false;
            }
            // search for portals in the new tile 
            MapEntry<Portal> portalEntry = map.PortalEntries.Find(delegate(MapEntry<Portal> entry) { return (entry.MapPosition == mapPosition); });
            // if there is a portal, then move through it 
            if ((portalEntry != null) && (portalEntry.Content != null))
            {
                // make sure the content name is valid 
                string mapContentName = portalEntry.Content.DestinationMapContentName;
                if (!mapContentName.StartsWith(@"Maps\"))
                {
                    mapContentName = System.IO.Path.Combine(@"Maps", mapContentName);
                }
                // load the new map 
                Map newMap = ContentManager.Load<Map>(mapContentName);
                SetMap(newMap,
                    newMap.FindPortal(portalEntry.Content.DestinationMapPortalName));
                return false;
            }
            // nothing stops the party from moving into the tile
            return true;
        }


        #endregion


        #region Drawing


        /// <summary>
        /// Draw the visible tiles in the given map layers.
        /// </summary>
        public static void DrawLayers(SpriteBatch spriteBatch, bool drawBase,
            bool drawFringe, bool drawObject)
        {
            // check the parameters
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }
            if (!drawBase && !drawFringe && !drawObject)
            {
                return;
            }

            Rectangle destinationRectangle =
                new Rectangle(0, 0, map.TileSize.X, map.TileSize.Y);

            for (int y = 0; y < map.MapDimensions.Y; y++)
            {
                for (int x = 0; x < map.MapDimensions.X; x++)
                {
                    destinationRectangle.X =
                        (int)mapOriginPosition.X + x * map.TileSize.X;
                    destinationRectangle.Y =
                        (int)mapOriginPosition.Y + y * map.TileSize.Y;

                    // If the tile is inside the screen
                    if (CheckVisibility(destinationRectangle))
                    {
                        Point mapPosition = new Point(x, y);
                        if (drawBase)
                        {
                            Rectangle sourceRectangle =
                                map.GetBaseLayerSourceRectangle(mapPosition);
                            if (sourceRectangle != Rectangle.Empty)
                            {
                                spriteBatch.Draw(map.Texture, destinationRectangle,
                                    sourceRectangle, Color.White);
                            }
                        }
                        if (drawFringe)
                        {
                            Rectangle sourceRectangle =
                                map.GetFringeLayerSourceRectangle(mapPosition);
                            if (sourceRectangle != Rectangle.Empty)
                            {
                                spriteBatch.Draw(map.Texture, destinationRectangle,
                                    sourceRectangle, Color.White);
                            }
                        }
                        if (drawObject)
                        {
                            Rectangle sourceRectangle =
                                map.GetObjectLayerSourceRectangle(mapPosition);
                            if (sourceRectangle != Rectangle.Empty)
                            {
                                spriteBatch.Draw(map.Texture, destinationRectangle,
                                    sourceRectangle, Color.White);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Returns true if the given rectangle is within the viewport.
        /// </summary>
        public static bool CheckVisibility(Rectangle screenRectangle)
        {
            return ((screenRectangle.X > viewport.X - screenRectangle.Width) &&
                (screenRectangle.Y > viewport.Y - screenRectangle.Height) &&
                (screenRectangle.X < viewport.X + viewport.Width) &&
                (screenRectangle.Y < viewport.Y + viewport.Height));
        }


        #endregion
    }
}
