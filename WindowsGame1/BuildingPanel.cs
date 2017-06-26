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
    class BuildingPanel
    {
        //[Fields]
        private Rectangle m_Canvas;
        private static Rectangle m_ButtonCanvas;
        private static Flora[] m_BuildOptions;
        private Int32 m_SelectedID = -1;
        private Int32 m_HoverOverID;
        private static Texture2D m_PanelTexture;
        private static Texture2D m_ButtonHoverTexture;
        private static Texture2D m_ButtonSelectedTexture;
        private static Texture2D m_ButtonTexture;
        private static Int32 m_X,m_Y;

        //[Constructors]
        public BuildingPanel(Int32 x, Int32 y)
        {
            m_Canvas = new Rectangle(x,y,414,60);
            m_X = x; m_Y = y;
        }

        //[Properties]
        public Flora SelectedOption
        {
            get 
            {
                if (m_SelectedID < 0 || m_SelectedID >= m_BuildOptions.Length) return null;
                return m_BuildOptions[m_SelectedID];
            }
        }
        //[Methods]
        public static void LoadContent(ContentManager content)
        {
            m_PanelTexture = content.Load<Texture2D>("BuildingPanel");
            m_ButtonHoverTexture= content.Load<Texture2D>("IconCanvasHighligted");
            m_ButtonSelectedTexture = content.Load<Texture2D>("IconCanvasSelected");
            m_ButtonTexture = content.Load<Texture2D>("IconCanvas");
            m_ButtonCanvas = m_ButtonHoverTexture.Bounds;
            m_ButtonCanvas.Height = m_ButtonCanvas.Height/3*2;
            m_ButtonCanvas.Width = 49;
            
            //Declaring Build options
            m_BuildOptions = new Flora[4];
            m_BuildOptions[0] = new PlantSolar();
            m_BuildOptions[1] = new PlantStorage();
            m_BuildOptions[2] = new RootBasic();
            m_BuildOptions[3] = new RootBig();
            foreach (Flora buildOption in m_BuildOptions) buildOption.LoadContent(content);

           
        }
        public void Update(Rectangle camera)
        {
            m_Canvas.X = m_X+ (camera.Width - m_Canvas.Width)/2;
            m_Canvas.Y = m_Y+ camera.Height - m_Canvas.Height;
        }
        public Boolean Select(MouseCursor cursor)
        {

            Double x = cursor.X - m_Canvas.X - 8;
            Double y = cursor.Y - m_Canvas.Y - 7;

            if (cursor.RightButtonPressed) m_SelectedID = -1;
        

            if ((x > (m_Canvas.Width - 7)) || (y > m_Canvas.Height) || (x < 0) || (y < 0)) 
                { m_HoverOverID = -1; return false; };
            Int32 ID = (Int32)Math.Floor(x / m_ButtonCanvas.Width);
            m_HoverOverID = ID;

            if (cursor.LeftButtonPressed)
            {
                m_SelectedID = m_HoverOverID;
                return true;
            }

            

            return true;
            
        }
        public void Draw(SpriteBatch sprites)
        {
            sprites.Draw(m_PanelTexture, m_Canvas, Color.White);
            Rectangle buttonCanvas = m_ButtonCanvas;
            buttonCanvas.Y = m_Canvas.Y + 7;
            for (Int32 i=0; i < 8; i++)
            {
                buttonCanvas.X = i * m_ButtonCanvas.Width + m_Canvas.X + 9;
                if (i == m_SelectedID) sprites.Draw(m_ButtonSelectedTexture, buttonCanvas, Color.White);
                else if (i == m_HoverOverID) sprites.Draw(m_ButtonHoverTexture, buttonCanvas, Color.White);
                else sprites.Draw(m_ButtonTexture, buttonCanvas, Color.White);
                if (i < m_BuildOptions.Length)
                {
                    m_BuildOptions[i].DrawIcon(sprites,buttonCanvas);

                }
            }
            
        }
    }
}
