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
    class InfoPanel
    {
        //[Fields]
        private static Texture2D m_Texture;
        private static Texture2D m_TexIconFrame;
        private static SpriteFont m_NameFont;
        private static SpriteFont m_InfoFont;
        private static Rectangle m_Canvas;
        private static Double m_Scale = 1.5;
        //[Methods]
        private static Int32 _screen(Int32 length)
        {
            return (Int32)Math.Round(length / m_Scale);
        }

        private static void _DisplayInfo(Int32 texY, ISelectable subject)
        {
            if (subject==null) return; 
            //Имя
            Vector2 textPlace = new Vector2(m_Canvas.X + _screen(102), m_Canvas.Y + _screen(texY));
            Tile.SpriteBatch.DrawString(m_NameFont, subject.Name, textPlace, Color.White);
            //Описание
            textPlace.Y += 14;
            Tile.SpriteBatch.DrawString(m_InfoFont, subject.Description, textPlace, Color.White);
            //Иконка
            Rectangle IconCanvas = new Rectangle(m_Canvas.X + _screen(28), m_Canvas.Y + _screen(texY+15), _screen(64), _screen(64));
            
            Tile.SpriteBatch.Draw(m_TexIconFrame, IconCanvas, Color.White);
            IconCanvas.X +=_screen(7);           IconCanvas.Y +=_screen(7);
            IconCanvas.Width -= _screen(14);     IconCanvas.Height-= _screen(14);
            subject.DrawIcon(Tile.SpriteBatch, IconCanvas);
        }

        public static void LoadContent(ContentManager Content)
        {
            m_Texture = Content.Load<Texture2D>("InfoPanel");
            m_NameFont = Content.Load<SpriteFont>("FontHeader");
            m_InfoFont = Content.Load<SpriteFont>("FontDescription");
            m_TexIconFrame = Content.Load<Texture2D>("IconFrame");
        }
        
        public static void Draw(Tile tile)
        {
            if (tile == null) return;
            m_Canvas = m_Texture.Bounds;
            m_Canvas.Height = _screen(m_Canvas.Height);
            m_Canvas.Width = _screen(m_Canvas.Width);
            m_Canvas.Y = Tile.Camera.Height - m_Canvas.Height;
            m_Canvas.X = Tile.Camera.Width - m_Canvas.Width;
            Tile.SpriteBatch.Draw(m_Texture, m_Canvas, Color.White);

            //tile Info
            
            _DisplayInfo(20,tile);
            _DisplayInfo(130,tile.Plant);
            _DisplayInfo(240,tile.Root);

        }
    }
}
