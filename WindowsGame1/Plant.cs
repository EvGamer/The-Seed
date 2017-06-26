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
    abstract public class Plant:Flora,IConsumer //Parent class of object on land;
    {
        
        //[Fields]
        protected Root m_Root;
        protected Int16[] m_ResourceID;
        protected Int32[] m_Income;
        protected Int32[] m_Storage;
        protected Int32[] m_StorageMax;
        protected Demands[] m_Demands;
        protected Int32 m_Timer = 0;
        
        //[Properties]

        
        public Root Root
        {
            get { return m_Root; }
            set{m_Root = value;}
        }

        //[Methods]

        public virtual Int32 GetDemands(Int16 resource)
        {
            for (Int32 i=0; i < m_Demands.Length; i++)
            {
                if (m_Demands[i].Resource == resource) return m_Demands[i].Total;
            }
                return 0;
        }
        public virtual Int32 GetIncome(Int16 resource)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) return m_Income[i];
            }
            return 0;
        }
        public void SetIncome(Int16 resource, Int32 income)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) m_Income[i] = income;
            }
        }
        public virtual Int32 GetStorage(Int16 resource)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) return m_Storage[i];
            }
            return 0;
        }
        public void SetStorage(Int16 resource, Int32 storage)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) m_Storage[i] = storage;
            }
        }
        public void AddIncome(Int16 resource, Int32 income)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) m_Income[i] += income;
            }
        }
        public virtual Boolean IsProducing(Int16 ResourceID)
        {

            for (Int32 i = 0; i < m_Income.Length; i++) 
                if (m_Income[i] > 0 && m_ResourceID[i] == ResourceID) return true;
            return false;

        }
        public void FulfilDemands(Int16 resource,Int32 income)
        {
            for (Int32 i = 0; i < m_Demands.Length; i++)
            {
                if (m_Demands[i].Resource == resource) m_Demands[i].Fulfil(3,income);
            }
        }
        public void TakeBack(Int16 resource, Int32 income)
        {
            for (Int32 i = 0; i < m_Demands.Length; i++)
            {
                if (m_Demands[i].Resource == resource) m_Demands[i].TakeBack(3,income);
            }
        }

        public override sealed void Draw()
        {
            Draw(m_Tile);
        }
        public override sealed void Draw(Tile tile)
        {
            Rectangle canvas = tile.GetCanvas();
            canvas.Y -= canvas.Height;
            canvas.Height *= 2;
            Tile.SpriteBatch.Draw(m_Texture, canvas, Color.White);
        }
        public override Boolean Update(GameTime gameTime)
        {
            m_Timer += gameTime.ElapsedGameTime.Milliseconds;
            if (m_Timer < 500) return false;
            m_Timer = 0;

            if (m_Health < m_HealthMax)
            {
                m_Demands[0].Regen = 5;
                m_Health += 5;
            }
            else m_Demands[0].Regen = 0;
            return true;

        }
        public virtual void LookForResource(List<Route> RouteList)
        {
            foreach (Demands demand in m_Demands)
            {
                while(GetDemands(demand.Resource)>0)
                {
                    Route NewRoute = Route.Create(m_Root, demand.Resource);
                    if (NewRoute == null) break;
                    RouteList.Add(NewRoute);
                }
            }
        }
       
    }
    

    public class PlantMain : Plant
    {
        //[Fields]
        //[Constructors]
        public PlantMain()
        {
            Initialize();
        }

        public PlantMain(PlantMain plant)
        {
            m_Texture = plant.m_Texture;
            Initialize();
        }

        //[Methods]
        public override Boolean Build(Tile tile)
        {
            PlantMain plant = new PlantMain(this);
            return tile.PlacePlant(plant);
        }
        public override void Initialize()
        {
            Int32 resCount = 1;
            m_Name = "Головное дерево";
            m_Description = "Главный узел вашей сети. \nОсуществляет контроль над всеми \nдействиями растения. Производит \nнебольшое количество сахара";
            m_HealthMax = 3000;
            m_Health = m_HealthMax;

            m_StorageMax = new Int32[resCount];
            m_StorageMax[0] = 200;
            m_Storage = new Int32[resCount];
            m_Storage[0] = 0;


            m_Income = new Int32[resCount];
            m_ResourceID = new Int16[resCount];
            m_Income[0] = 10;
            m_ResourceID [0] = 1;
            m_Demands = new Demands[resCount];
            m_Demands[0] = new Demands(0);
        }
        public override void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>("MotherTree");
        }
        public override string Description
        {
            get
            {
                return "Производит " + GetIncome(1) + " сахара";
            }
        }
    }


    public class PlantSolar : Plant
    {
        //[Fields]
        Seed m_Seed;
        //[Constructors]
        public PlantSolar()
        {
            Initialize();
            m_Seed = new Seed();
        }

        public PlantSolar(PlantSolar plant)
        {
            m_Texture = plant.m_Texture;
            Initialize();
        }

        //[Methods]
        public override Boolean Build(Tile tile)
        {
            Seed seed = new Seed(m_Seed);
            seed.SetPlant(new PlantSolar(this));
            return tile.PlacePlant(seed);
        }
        public override void Initialize()
        {
            Int16 resCount = 1;
            m_Name = "Солнечный папоротник";
            m_Description = "Производит небольшое количество сахара, находясь на солнце";
            m_HealthMax = 200;
            m_Health = m_HealthMax;
            m_Cost = 300;

            m_Income = new Int32[resCount];
            m_ResourceID = new Int16[resCount];
            m_Income[0] = 5;
            m_ResourceID[0] = 1;

            m_StorageMax = new Int32[resCount];
            m_StorageMax[0] = 200;
            m_Storage= new Int32[resCount];
            m_Storage[0] = 0;

            m_Demands = new Demands[resCount];
            m_Demands[0] = new Demands(0);
        }
        public override void LoadContent(ContentManager content)
        {
            m_Seed.LoadContent(content);
            m_Texture = content.Load<Texture2D>("SugarPlant");
        }
        public override string Description
        {
            get
            {
                return "Производит " + GetIncome(1) + " сахара";
            }
        }
    }

    public class PlantStorage : Plant
    {
        //[Fields]
        Seed m_Seed;
        //[Constructors]
        public PlantStorage()
        {
            Initialize();
            m_Seed = new Seed();
        }

        public PlantStorage(PlantStorage plant)
        {
            m_Texture = plant.m_Texture;
            Initialize();
        }

        //[Methods]
        public override bool Build(Tile tile)
        {
            Seed seed = new Seed(m_Seed);
            seed.SetPlant(new PlantStorage(this));
            return tile.PlacePlant(seed);
        }
        public override void Initialize()
        {
            Int16 resCount = 1;
            m_Name = "Запасовый пузырь";
            m_Description = "Позволяет хранить ресурсы";
            m_HealthMax = 200;
            m_Health = m_HealthMax;
            m_Cost = 400;

            m_Income = new Int32[resCount];
            m_ResourceID = new Int16[resCount];
            m_Income[0] = 60;
            m_ResourceID[0] = 1;

            m_StorageMax = new Int32[resCount];
            m_StorageMax[0] = 2000;
            m_Storage = new Int32[resCount];
            m_Storage[0] = 0;

            m_Demands = new Demands[resCount];
            m_Demands[0] = new Demands(1,0,60,0);
        }
        public override void LoadContent(ContentManager content)
        {
            m_Seed.LoadContent(content);
            m_Texture = content.Load<Texture2D>("Storage");
        }
        //public override void LookForResource(List<Route> RouteList) { }
        public override Int32 GetIncome(Int16 resource)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                if (m_ResourceID[i] == resource) return Math.Min(m_Income[i],m_Storage[i]);
            }
            return 0;
        }
        public override Int32 GetDemands(Int16 resource)
        {
            for (Int32 i = 0; i < m_Demands.Length; i++)
            {
                if (m_Demands[i].Resource == resource) return Math.Min(m_Demands[i].Total, m_StorageMax[i] - m_Storage[i]);
            }
            return 0;
        }
        public override Boolean Update(GameTime gameTime)
        {
            if (!base.Update(gameTime)) return false;
            for (Int32 i = 0; i < m_Income.Length; i++)
            {
                var rID = m_ResourceID[i];
                Int32 lastIncome = GetIncome(rID);
                Int32 lastDemand = GetDemands(rID);;

                m_Storage[i] +=m_Income[i] - m_Demands[i].Total;
                m_Storage[i] = Math.Max(0,m_Storage[i]);
                m_Storage[i] = Math.Min(m_StorageMax[i], m_Storage[i]);

                if (lastIncome != GetIncome(rID) || lastDemand != GetDemands(rID)) 
                    Tile.RefreshRoutes();
            }
            return true;
        }
        public override bool IsProducing(short ResourceID)
        {
            for (Int32 i = 0; i < m_Income.Length; i++)
                if (m_Storage[i] > 0 && m_ResourceID[i] == ResourceID) return true;
            return false;
        }
        public override string Description
        {
            get
            {
                return "Хранит " + m_Storage[0] + " сахара\nПрибывает" + (m_Income[0] - m_Demands[0].Total);
            }
        }
    }

    public class Seed : Plant
    {
        private Plant m_Plant;
        
        public Seed()
        {
            Initialize();
        }

        public Seed(Plant plant)
        {
            Initialize();
            SetPlant(plant);
        }

        public Seed(Seed seed)
        {
            Initialize();
            m_Texture = seed.m_Texture;
            m_Plant = seed.m_Plant;
            m_StorageMax = seed.m_StorageMax;
        }
        public void SetPlant(Plant plant)
        {
            m_StorageMax[0] = plant.Cost;
            m_Plant=plant;
        }
        public override bool Build(Tile tile)
        {
            tile.ReplacePlant(m_Plant);
            return true;
        }
        public override void Initialize()
        {
            Int32 resCount = 1;
            m_Name = "Семя";
            m_Description = "Подведите к нему ресурсы с \nпомощью корней, и из него \nвырастет полноценное дерево";
            m_HealthMax = 100;
            m_Health = m_HealthMax;
            m_Income = new Int32[resCount];
            m_Income[0] = 5;
            m_ResourceID = new Int16[resCount];
            m_ResourceID[0] = 1;
            m_StorageMax = new Int32[resCount];
            m_StorageMax[0] = 100;
            m_Storage= new Int32[resCount];
            m_Storage[0] = 0;
            m_Demands = new Demands[resCount];
            m_Demands[0] = new Demands(1,0,5);
        }
        
        public override Boolean IsProducing(Int16 ResourceID)
        {
            { return false; }
        }
        public override void LoadContent(ContentManager content)
        {
            m_Texture = content.Load<Texture2D>("Seed");
        }
        public override Boolean Update(GameTime gameTime)
        {
            if(!base.Update(gameTime)) return false;
            if (m_Storage[0]>m_StorageMax[0]) Build(m_Tile);

            for (Int32 i = 0; i < m_Income.Length; i++)
                if ((m_Storage[i] <= m_StorageMax[i])
                    && (m_Storage[i] >= 0))
                    m_Storage[i] += m_Income[i] - m_Demands[i].Production;
            return true;
        }
        public override string Description
        {
            get
            {
                String str = "";
                str +="Нехватает ";
                str +=GetDemands(1);
                str +=" сахара";
                str +="\nОсталось доставить ";
                Int32 rest = m_StorageMax[0] - m_Storage[0];
                str += rest;
                str += " сахара";
                
                return str;
            }
        }
    }
}