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
    public class NPC
    {
        public float moveSpeed = 1f;


        public AnimatedTexture SpriteTexture;
        public float Rotation = 0;
        public float Scale = 1.5f;
        public float Depth = 0.5f;


        public PlayerPosition npcPosition = new PlayerPosition();
        public PlayerPosition NPCPosition
        {
            get { return NPCPosition; }
            set { NPCPosition = value; }
        }
    }
}
