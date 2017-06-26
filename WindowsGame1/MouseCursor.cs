using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MouseCursor : Microsoft.Xna.Framework.GameComponent
    {
        //[Fields]
        private MouseState m_State;
        private static Texture2D m_Texture;
        private Rectangle m_Canvas;
        private Int32 m_ScrollShift = 3;
        private Int32 m_ScrollSpeed = 5;
        private Int32 m_Dx, m_Dy;
        
        //[Properties]
        public Int32 X
        {
            get {return m_State.X;}//-m_Canvas.Width;}
            set { m_Canvas.X = value;}// +m_Canvas.Width; }
        }
        public Int32 Y
        {
            get { return m_State.Y;}// -m_Canvas.Height; }
            set { m_Canvas.Y = value;}//+ m_Canvas.Height; }
        }
        public Boolean LeftButtonPressed
        {
            get
            {
                if (m_State.LeftButton == ButtonState.Pressed) return true;
                return false;
            }
        }

        public Boolean RightButtonPressed
        {
            get
            {
                if (m_State.RightButton == ButtonState.Pressed) return true;
                return false;
            }
        }

        //[Constructors]
        public MouseCursor(Game game)
            : base(game)
        {
            m_Canvas = new Rectangle(0,0,32,32);
            m_State = Mouse.GetState();
            // TODO: Construct any child components here
        }

        //[Methods]
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            
            base.Initialize();
        }

        public static void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>("Cursor");
            
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            m_State = Mouse.GetState();
            if (m_State.MiddleButton == ButtonState.Pressed)
            {
                m_Dx = m_Canvas.X - m_State.X;
                m_Dy = m_Canvas.Y - m_State.Y;
            }
            else { m_Dx = 0; m_Dy = 0; }
            m_Canvas.X = m_State.X;
            m_Canvas.Y = m_State.Y;
            base.Update(gameTime);
        }

        public void Scroll(ref Rectangle Camera)
        {
            if (this.X < m_ScrollShift) Camera.X -= m_ScrollSpeed;
            if (this.Y < m_ScrollShift) Camera.Y -= m_ScrollSpeed;
            if (this.X > Camera.Width - m_ScrollShift) Camera.X += m_ScrollSpeed;
            if (this.Y > Camera.Height- m_ScrollShift) Camera.Y += m_ScrollSpeed;
            if (m_State.MiddleButton == ButtonState.Pressed)
            {
                Camera.X += m_Dx;
                Camera.Y += m_Dy;
            }
        }

        public void Draw(SpriteBatch sprites)
        {
            if(m_Texture!=null)sprites.Draw(m_Texture, m_Canvas, Color.White);
        }
    }
}
