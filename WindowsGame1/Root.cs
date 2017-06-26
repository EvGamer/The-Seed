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
    abstract public class Root:Flora //Parrent class for objects underground
    {
        //[Fields]
        protected static SpriteFont m_InfoFont;
        protected Int32 m_Capacity;
        protected Int32 m_MaxCapacity;
        protected Plant m_Plant;
        protected Root[] m_Joints;
        protected Int16 m_Resource;
        protected List<Route> m_Flows;
        //[Property]
        public override string Description
        {
            get { return "Пропускная способность: " + m_Capacity; }
        }
        public Plant Plant
        {
            get {return m_Plant;}
            set {m_Plant = value;}
        }
        public Int32 Capacity
        {
            get { return m_Capacity; }
        }
        public Int16 ResourceID
        {
            get { return m_Resource; }
        }
        public IConsumer Consumer
        {
            get
            {
                if (m_Plant == null) return null;
                return m_Plant;
            }
        }
        public static void SetFont(SpriteFont spriteFont)
        {
            m_InfoFont = spriteFont;
        }
        //[Method]
        public void AddFlow(Route flow)
        {
            m_Capacity -= flow.Rate;
            m_Flows.Add(flow);
        }
        public void RemoveFlow(Route flow)
        {
            m_Capacity += flow.Rate;
            m_Flows.Remove(flow);
        }
        public override Boolean Update(GameTime gameTime) 
        { return false;/*ToDo*/}
        public override void Draw() 
        {Draw( m_Tile);
            Int32 x = m_Tile.GetCanvas().X + 32;
            Int32 y = m_Tile.GetCanvas().Y;
            Vector2 TextPos = new Vector2(x, y);
            String cap = Convert.ToString(Capacity);
            Tile.SpriteBatch.DrawString(m_InfoFont, cap, TextPos, Color.Yellow);
        }
        public override void Draw( Tile tile) 
        {
            Tile.SpriteBatch.Draw(m_Texture, tile.GetCanvas(), Color.White);
        }
        public Root GetNeighboor(Byte rootEnd)
        {
            return m_Joints[rootEnd];
        }
        public bool LinkTogether(Root root2, Byte rootEnd)
        {
            if (root2 == null) return false;
            this.m_Joints[rootEnd] = root2;
            root2.m_Joints[(rootEnd + 3) % 6] = this;
            return true;
        }

    }

    public class RootBasic : Root
    {
        //[Fields]
        
        //[Constructors]
        public RootBasic()
        {
            Initialize();
        }
        public RootBasic(RootBasic root)
        {
            Initialize();
            m_Texture = root.m_Texture;
        }
        //[Methods]
        
        public override void Initialize() 
        {
            m_Name = "Обычный корень";
            m_Description = "Немутировавший корень растения. \nСпособен провести через себя \nпоток силой в 20 единиц";
            m_Joints = new Root[6];
            m_Flows = new List<Route>();
            for(Byte i=0;i<6;i++) m_Joints[i] = null;
            m_Capacity = 20;
            m_Resource = 0;
        }
        public override bool Build(Tile tile)
        {
            Root root = new RootBasic(this);
            return tile.PlaceRoot(root);
        }
        public override void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>("Root");

        }
    }

    public class RootBig : Root
    {
        //[Fields]

        //[Constructors]
        public RootBig()
        {
            Initialize();
        }
        public RootBig(RootBig root)
        {
            Initialize();
            m_Texture = root.m_Texture;
        }
        //[Methods]

        public override void Initialize()
        {
            m_Name = "Широкий корень";
            m_Description = "Крупный немутировавший корень растения. \nСпособен провести через себя \nпоток силой в 60 единиц";
            m_Joints = new Root[6];
            m_Flows = new List<Route>();
            for (Byte i = 0; i < 6; i++) m_Joints[i] = null;
            m_Capacity = 60;
            m_Resource = 0;
        }
        public override bool Build(Tile tile)
        {
            Root root = new RootBig(this);
            return tile.PlaceRoot(root);
        }
        public override void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>("RootBig");

        }
    }
}
