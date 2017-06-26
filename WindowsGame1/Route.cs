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
    public class Route
    {
        //[Types]
        public class Step
        {
            //[Fields]
            public Step StepBack;
            public Byte Dirrection;
            public Root Root;
            //[Constructors]
            public Step(Step stepBack,Root root,Byte dirrection)
            {
                StepBack = stepBack;
                Root = root;
                Dirrection = dirrection;
            }
        };
        //[Fields]
        private Int16 m_ResourceID;
        private IConsumer m_Consumer;
        private Plant m_Supply;
        private List<Step> m_Path;
        private Int32 m_Rate;
        private static Texture2D[] m_TextureList;
        
        //[Constructors]
        public Route()
        {
            m_Path = new List<Step>();
            m_ResourceID = 0;
            m_Consumer = null;
            m_Supply = null;
        }


        //[Properties]
        public Int16 ResourceID
        {
            get {return m_ResourceID;}
        }
        public Int32 Rate
        {
            get { return m_Rate; }
        }
        public Plant Source
        {
            get {return m_Supply; }
        }
        
        
        //[Methods]
        public static void LoadContent(ContentManager content)
        {
            m_TextureList = new Texture2D[6];
            for (Byte i = 0; i < 6; i++) m_TextureList[i] = content.Load<Texture2D>("Route" + i);
        }
        private static bool _AbleToLetThrough(Root root, Int16 resourceID)
        {
            if (root == null) return false;
            return    
                      (root.Capacity != 0) 
                && (  (root.ResourceID == resourceID) 
                ||    (root.ResourceID == 0)  );
        }
        private static bool _Contains(Queue<Step> queue, Root root)
        {
            foreach (Step step in queue) if (root == step.Root) 
                return true;
            return false;
        }
        private static List<Step> _GetPath(Step lastStep,ref Int32 rate)
        {
            Step current = lastStep;
            List<Step> path = new List<Step>();
            while (current != null)
            {
                path.Add(current);
                rate = Math.Min(rate, current.Root.Capacity);
                current = current.StepBack;
            }
            return path;
        }
        public static Route Create(Root start, Int16 resourceID)
        {
            if (start == null) return null;
            IConsumer consumer = start.Consumer;
            Plant supply = null;
            Queue<Step> frontier = new Queue<Step>();
            List<Root> visited = new List<Root>();

            Step current = new Step(null, start, 7);
            visited.Add(start);
            frontier.Enqueue(current);

            Boolean foundSupply = false;
            while (frontier.Count != 0)
            {
                current = frontier.Dequeue();
                
                foundSupply = (current.Root.Plant != null ) 
                    && (current.Root.Plant.IsProducing(resourceID))
                    && (current.Root.Plant!=consumer)
                    && (current.Root.Plant.GetType() != consumer.GetType() );

                if (foundSupply) 
                {
                    supply = current.Root.Plant;
                    break;
                }

                for (Byte i = 0; i<6; i++)
                {
                    Root newRoot = current.Root.GetNeighboor(i);
                    if (!visited.Contains(newRoot) && _AbleToLetThrough(newRoot, resourceID))
                    {
                        visited.Add(newRoot);
                        frontier.Enqueue(new Step(current, newRoot, i));
                    }
                }
            }

            if (!foundSupply) return null;
            Route route = new Route();
            route.m_ResourceID = resourceID;
            route.m_Consumer = consumer;
            route.m_Supply = supply;

            Int32 rate = route.m_Supply.GetIncome(resourceID);
            route.m_Path = _GetPath(current,ref rate);
            rate = Math.Min(rate, route.m_Consumer.GetDemands(resourceID));
            route.m_Rate = rate;
            if (rate == 0) return null;

            route.m_Consumer.FulfilDemands(resourceID, route.Rate);
            route.m_Supply.AddIncome(resourceID, -route.Rate);
            foreach (Step step in route.m_Path) step.Root.AddFlow(route);
            return route;
        }
        public void Clear()
        {
            foreach (Step step in m_Path) step.Root.RemoveFlow(this);
            m_Consumer.TakeBack(m_ResourceID, Rate);
            m_Supply.AddIncome(m_ResourceID, Rate);
        }
        public void Draw(Int32 index)
        {
            Int32 colorPick = index % 7;
            Color[] colors= new Color[7];
            colors[0] = Color.Red;
            colors[1] = Color.Orange;
            colors[2] = Color.Yellow;
            colors[3] = Color.Green;
            colors[4] = Color.Cyan;
            colors[5] = Color.Blue;
            colors[6] = Color.Purple;
            foreach(Step step in m_Path)
                if (step.Dirrection < 6)
                {
                    if(step.StepBack!=null)
                        Tile.SpriteBatch.Draw(m_TextureList[step.Dirrection], step.StepBack.Root.Tile.GetCanvas(), colors[colorPick]);
                    Tile.SpriteBatch.Draw(m_TextureList[(step.Dirrection + 3) % 6], step.Root.Tile.GetCanvas(), colors[colorPick]);
                }
        }
        public void SetRate(Int16 res,Int32 rate)
        {
            m_ResourceID = 0;
            if(m_Rate!=0) m_ResourceID = res;
            m_Rate = rate;
        }
    }
}
