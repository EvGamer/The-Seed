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
    public class Map
    {
        //[Fields]
        private Int32 m_Radius;
        private Tile[] m_Tiles;
        private List<Route> m_Routes;
        private Tile m_SelectedTile = null;
        private Tile m_HoverOverTile = null;
        private static Texture2D m_TexHoverOver;
        private static Int32[,] _WAY_VECTORS = { { 1, 0, -1 }, { 1, -1, 0 }, { 0, -1, 1 }, { -1, 0, 1 }, { -1, 1, 0 }, { 0, 1, -1 } };
        //[Properties]
        public Tile SelectedTile
        {
            get { return m_SelectedTile; }
        }
        //[Methods]
        private Int32 _GetTileNum(Int32 column, Int32 row)//Returns tile number by the row and column they in
        {
            Int32 Number = column;
            Int32 RowLength = m_Radius - 1;

            Int32 StopRow = Math.Min(m_Radius, row);
            for (Int32 i = 0; i < StopRow; i++)
            {
                RowLength++;
                Number += RowLength;
            }

            StopRow = row - m_Radius;
            if (row > m_Radius)
                for (Int32 i = 0; i < StopRow; i++)
                {
                    RowLength--;
                    Number += RowLength;
                }
            return Number;
        }
        private Boolean _CheckCoord(Int32 x,Int32 y,Int32 z) //making sure if coordinates are out of borders
        {
            if (Math.Abs(x) >= m_Radius) return false;
            if (Math.Abs(y) >= m_Radius) return false;
            if (Math.Abs(z) >= m_Radius) return false;
            return true;
        }
        private Boolean _CheckCoord(Int32 x, Int32 y)
        { return _CheckCoord(x, y, -x - y); }
        private void _RoundCoord(ref Double x, ref Double y)
        {
            Double z = -x - y;
            Double rx = Math.Round(x);
            Double ry = Math.Round(y);
            Double rz = Math.Round(z);
            Double dx = Math.Abs(rx - x);
            Double dy = Math.Abs(ry - y);
            Double dz = Math.Abs(rz - z);
            if ((dx > dy) && (dx > dz))
            {
                x = -ry - rz;
                y = ry;
                return;
            }
            if ((dy > dz))
            {
                x = rx;
                y = -rx - rz;
                return;
            }
            x = rx;
            y = ry;
        }
        private void _LinkTiles(Tile tile)
        {
            Int32 x=0,y=0,z=0;
            tile.GetCoord(out x,out y,out z);
            for (Byte i = 0; i < 6; i++)
            {
                Int32 x1 = x + _WAY_VECTORS[i, 0];
                Int32 y1 = y + _WAY_VECTORS[i, 1];
                //Int32 z1 = z + WAY_VECTORS[i, 2];
                //if (_CheckCoord(x1,y1,z1))
                tile.Link(i, GetTile(x1,y1)); 
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (Tile tile in m_Tiles) if (tile.Plant != null) tile.Plant.Update(gameTime);
        }
        public void Draw(Flora buildOption)
        {
            //Layer 0
            foreach (Tile tile in m_Tiles)
            {
                tile.Draw();
                
                if (tile.Root != null) tile.Root.Draw();
            }
           // for (Int32 i=0; i < m_Routes.Count; i++) m_Routes[i].Draw(i);
                //Layer 1
            if (m_HoverOverTile != null)
                Tile.SpriteBatch.Draw(m_TexHoverOver, m_HoverOverTile.GetCanvas(), Color.White);

            if (m_SelectedTile != null)
                Tile.SpriteBatch.Draw(m_TexHoverOver, m_SelectedTile.GetCanvas(), Color.Red);
            //Layer 2
            foreach (Tile tile in m_Tiles) 
                {
                    if (tile.Plant != null) tile.Plant.Draw();   
                }
            //Layer 3
            Boolean isChosen = (m_HoverOverTile != null) && (buildOption != null) && (m_HoverOverTile.Plant == null);
            if (isChosen)
                buildOption.Draw(m_HoverOverTile);
        }
        public Boolean HighlightTile(MouseCursor cursor, Rectangle camera)
        {
            Double Xabs = (cursor.X +camera.X)/2;
            Double Yabs = cursor.Y +camera.Y;
            Double Xfloat = (Xabs * Math.Sqrt(3) / 3 - Yabs / 3) / Tile.Size;
            Double Yfloat = (2.0/3*Yabs)/Tile.Size;
            _RoundCoord(ref Xfloat, ref Yfloat);

            Int32 x = Convert.ToInt32(Xfloat);
            Int32 y = Convert.ToInt32(Yfloat);
            if (!_CheckCoord(x, y))
            {
                m_HoverOverTile = null;
                return false;
            }
            m_HoverOverTile = GetTile(x, y);
            return true;
        }
        public Boolean SelectTile(MouseCursor cursor)
        {
            if (!cursor.LeftButtonPressed) return false;
            if (m_HoverOverTile==null) return false;
            m_SelectedTile = m_HoverOverTile;
            return true;
        }
        public void RefreshRoutes()
        {
            if(m_Routes!=null) foreach (Route route in m_Routes) route.Clear();
            m_Routes = new List<Route>();
            foreach (Tile tile in m_Tiles) if(tile.Plant!=null)tile.Plant.LookForResource(m_Routes); 
        }
        public Boolean Build(MouseCursor cursor, Flora buildOption)
        {
            if (!cursor.LeftButtonPressed) return false;
            if (m_HoverOverTile == null) return false;
            if (buildOption == null) return false;
            return buildOption.Build(m_HoverOverTile);
        }
        public Map(Int32 radius)//Creating hexagon map with specific Radius
        {
            m_Radius = radius;
            Int32 radNum = m_Radius - 1;
            int Length = _GetTileNum(radNum, 2 * radNum);
            m_Tiles = new Tile[Length+1];
            for (Int32 i = -radNum; i <= radNum; i++)
            {
                Int32 Start;
                Int32 Finish;
                if (i < 0) { Start = -radNum - i; Finish = radNum; }
                else       { Start = -radNum; Finish = radNum - i; }
                for (Int32 j = Start; j <= Finish; j++)
                {
                    Tile newTile = new Tile(j,i,0);
                    SetTile(newTile, j, i);

                }
            }

            foreach (Tile tile in m_Tiles) _LinkTiles(tile);

            
            return;
        }
        public Tile GetTile(Int32 x, Int32 y)//Returns copy of tile by selected coordinates;
        {
            if(!_CheckCoord(x,y)) return null;
            Int32 R = y + (m_Radius - 1);
            Int32 Q = x + Math.Min(R,m_Radius - 1);
            return m_Tiles[_GetTileNum(Q,R)];
        }
        public Boolean SetTile(Tile tile, Int32 x, Int32 y)
        {
            if (!_CheckCoord(x,y,-x-y)) 
                return false;
            Int32 R = y + (m_Radius - 1);
            Int32 Q = x + Math.Min(R, m_Radius - 1);
            Int32 ID = _GetTileNum(Q, R);
            m_Tiles[ID]=tile;
            return true;
        }
        public static void LoadContent(ContentManager content)
        {
            m_TexHoverOver = content.Load<Texture2D>("Overhead");
        }

    }

        
}

