#region USINGS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using RolePlayingGameData;
using AnimatedSprite;
#endregion

namespace RolePlaying
{
    public class MainPlayer
    {
        public float HP;
        public float MP;
        public float Gold;
        public float moveSpeed = 3f;


        public AnimatedTexture SpriteTexture;
        public float Rotation = 0;
        public float Scale = 2.0f;
        public float Depth = 0.5f;


        public PlayerPosition partyLeaderPosition = new PlayerPosition();
        public PlayerPosition PartyLeaderPosition
        {
            get { return partyLeaderPosition; }
            set { partyLeaderPosition = value; }
        }
        public Vector2 autoPartyLeaderMovement = Vector2.Zero;
    }
}
