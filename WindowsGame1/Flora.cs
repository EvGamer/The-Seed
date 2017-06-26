using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    public abstract class Flora:ISelectable
    {
        //[Fields]

        protected String m_Name;
        protected String m_Description;
        protected Tile m_Tile;
        protected Int32 m_Health;
        protected Int32 m_HealthMax;
        protected Int32 m_Cost;
        protected Texture2D m_Texture;
        
        //[Properties]

        public Int32 X
        { get { if (m_Tile == null)return 0; return m_Tile.X; } }
        public Int32 Y
        { get { if (m_Tile == null)return 0; return m_Tile.Y; } }
        public Int32 Z
        { get { if (m_Tile == null)return 0; return m_Tile.Z; } }
        public String Name
        {
            get { return m_Name; }
        }
        public virtual String Description
        {
            get { return m_Description; }
        }
        public Tile Tile
        {
            get { return m_Tile; }
            set { m_Tile = value; }
        }
        public Int32 Cost
        {
            get{return m_Cost;}
        }

        //[Methods]

        public abstract void Draw();
        public abstract void Draw(Tile tile);
        public abstract Boolean Update(GameTime gameTime);
        public abstract void Initialize();
        public abstract Boolean Build(Tile tile);
        public abstract void LoadContent(ContentManager content);
        public void DrawIcon(SpriteBatch Sprites, Rectangle Canvas)
        {
            Sprites.Draw(m_Texture, Canvas, Color.White);
        }
        
    }
}
