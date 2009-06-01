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
#endregion

namespace RolePlaying
{
    public class MainPlayer
    {
        public float HP;
        public float MP;
        public float Gold;
        public float moveSpeed = 3f;
        public Texture2D sprite;
        public PlayerPosition partyLeaderPosition = new PlayerPosition();
        public PlayerPosition PartyLeaderPosition
        {
            get { return partyLeaderPosition; }
            set { partyLeaderPosition = value; }
        }
        public Vector2 autoPartyLeaderMovement = Vector2.Zero;
    }
}
