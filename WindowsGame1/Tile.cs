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
    public class Tile:ISelectable //Game Tile. Holds links of neighboor tiles, link to up and bottom object and type of tile.
    {
        //[Fields]

        private Tile[] m_SideTile; //Collection of neighbor tiles;
        private static Int32 m_Size = 32; //Radius of the tile;
        private Int32 m_X, m_Y, m_Z;      //Cube coordinates of the tile on the map;
        private Int32 m_LandID;       //ID of the tile type;
        private Plant m_Plant;     //link to object on the ground;
        private Root m_Root;    //link to object below ground;
        private static Map m_Map;
        private static SpriteBatch m_Sprites;
        private static Rectangle m_Camera;
        private static Texture2D[] m_Textures;

        //[Properties]
        public String Name
        {
            get
            {
                return "Луга";
            }
        }
        public String Description
        {
            get
            {
                return
                    "Покрытая травой местность. \nИмеет среднюю влажность, \nумеренную температуру и \nдовольно плодородную почву";
            }
        }
        
        public static Rectangle Camera
        {
            get { return m_Camera;}
            set { m_Camera = value; }
        }
        public static SpriteBatch SpriteBatch
        {
            get { return m_Sprites; }
            set { m_Sprites = value; }
        }
        public static Int32 Size
        {
            get {return m_Size;}
            set { m_Size = value; }
        }
        public Root Root
        {
            get{return m_Root;}
        }
        public Plant Plant
        {
            get { return m_Plant; }
        }
        public Int32 X
        { get { return m_X; } }
        public Int32 Y
        { get { return m_Y; } }
        public Int32 Z
        { get { return m_Z; } }

        //[Constructors]

        public Tile(Int32 X, Int32 Y, Int32 ID)
        {
            SetCoord(X, Y);
            m_LandID = ID;
            m_Root = null;
            m_Plant = null;
            m_SideTile = new Tile[6];

            //for (Int32 i = 0; i < 6; i++) m_SideTile[i]=null;
        }

        //[Methods]
        private bool _LinkTogether(Tile tile, Root root)
        {
            if (tile == null || root == null) return false;
            tile.m_Root = root;
            root.Tile = tile;
            return true;
        }

        private bool _LinkTogether(Tile tile, Plant plant)
        {
            if (tile == null || plant == null) return false;
            tile.m_Plant = plant;
            plant.Tile = tile;
            return true;
        }

        private bool _LinkTogether(Plant plant, Root root)
        {
            if (plant == null || root == null) return false;
            plant.Root = root;
            root.Plant = plant;
            return true;
        }
        public static void LoadContent(ContentManager content)
        {
            m_Textures = new Texture2D[1];
            m_Textures[0] = content.Load<Texture2D>("GrassDoubled");
        }
        public static void SetMap(Map map)
        {
            m_Map = map;
        }
        public Rectangle GetCanvas()
        {
            Int32 h = m_Size*2;
            Int32 w = (Int32)Math.Ceiling(Math.Sqrt(3) / 2 * h)*2;
            //Int32 screenX = (Int32)Math.Round(w * m_X + 0.5 * w * m_Y) - Camera.X;
            //Int32 screenY = (Int32)Math.Round(0.75 * h * m_Y) - Camera.Y;

            Int32 screenX = (Int32)Math.Round(Math.Sqrt(3) * (m_X + 0.5 * m_Y) * m_Size)*2 - Camera.X -w / 2;
            Int32 screenY = (Int32)Math.Round(1.5 * m_Y * m_Size)-Camera.Y - h/2;
            Rectangle canvas = new Rectangle(screenX, screenY, (Int32)w, (Int32)h);
            return canvas;
        }
        public void Draw() 
        {
            Tile.SpriteBatch.Draw(m_Textures[m_LandID], GetCanvas(), Color.White);
        }
        public void DrawIcon(SpriteBatch Sprites, Rectangle Canvas)
        {
            Sprites.Draw(m_Textures[0], Canvas, Color.White);
        }
        public void SetCoord(Int32 x,Int32 y)
        {
            m_X = x;
            m_Y = y;
            m_Z = -x - y;
        }
        public void GetCoord(out Int32 x, out Int32 y)
        {
            x = m_X;
            y = m_Y;
        }
        public void GetCoord(out Int32 x, out Int32 y, out Int32 z)
        {
            GetCoord(out x, out y);
            z = m_Z;
        }
        public void Link(Byte way, Tile tile)
        {
            m_SideTile[way] = tile;
            //tile.m_SideTile[(way+3)%6] = this;
        }
        public static void RefreshRoutes()
        {
            m_Map.RefreshRoutes();
        }
        public void ReplacePlant(Plant plant)
        {
            if(!_LinkTogether(this, plant)) return;
            _LinkTogether(plant, m_Root);
            m_Map.RefreshRoutes();
        }
        public void ReplaceRoot(Root root)
        {
            if(!_LinkTogether(this, root)) return;
            _LinkTogether(m_Plant, root);
            for (Byte i = 0; i < 6; i++)
            {
                if (m_SideTile[i] != null) m_Root.LinkTogether(m_SideTile[i].m_Root, i);
            }
            m_Map.RefreshRoutes();
        }
        public Boolean PlacePlant(Plant plant)
        {
            if (m_Plant != null) return false;
            ReplacePlant(plant);
            return true;
        }
        public Boolean PlaceRoot(Root root)
        {
            if (m_Root != null) return false;
            ReplaceRoot(root);
            return true;
        }



    }  
}