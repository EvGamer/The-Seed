using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public struct Demands
    {
        //[Fields]
        private Int16 m_ResourceID;
        private Int32 m_RegenerationInitial;
        private Int32 m_Regeneration;
        private Int32 m_SurvivalInitial;
        private Int32 m_Survival;
        private Int32 m_ProductionInitial;
        private Int32 m_Production;
        //[Constructor]

        public Demands(Int16 ResourceID , Int32 forSurvival=0, Int32 forProduction=0, Int32 forRegen=0)
        { 
            m_ResourceID = ResourceID;
            m_SurvivalInitial = forSurvival;
            m_ProductionInitial = forProduction;
            m_RegenerationInitial = forRegen;
            m_Survival = m_SurvivalInitial;
            m_Regeneration = m_RegenerationInitial;
            m_Production = m_ProductionInitial;
        }

        //[Properties]

        public Int16 Resource
        { 
            get { return m_ResourceID; }
            set { m_ResourceID = value; }
        }
        public Int32 Total
        {
            get
            {return m_Survival + m_Regeneration + m_Production;}
        }
        public Int32 Production
        {
            get {return m_Production;}
            set { m_Production = value; }
        }
        public Int32 Survival
        {
            get {return m_Survival;}
            set { m_Survival = value; }
        }
        public Int32 Regen
        {
            get {return m_Regeneration;}
            set { m_Regeneration = value; }
        }

        //[Methods]

        private Boolean _FulfillSingle(ref Int32 demand,ref Int32 income)
        {
            if (demand > income) 
            {
                demand -= income;
                income = 0; 
                return false;
            }
            income -= demand;
            demand = 0;
            return true;
        }
        public Boolean Fulfil(Byte priority, Int32 income)
        {
            if (!_FulfillSingle(ref m_Survival, ref income)) return false;
            if (priority < 1) return true;
            if (!_FulfillSingle(ref m_Production, ref income)) return false;
            if (priority < 2) return true;
            if (!_FulfillSingle(ref m_Regeneration, ref income)) return false;
            return true;
        }
        private bool _TakeBackSingle(Int32 initialDemand,ref Int32 demand, ref Int32 rate)
        {
            if (rate >= initialDemand)
            {
                rate -= initialDemand;
                demand = initialDemand;
                return true;
            }
            demand += rate;
            rate = 0;
            return false;
        }

        public Boolean TakeBack(Byte priority, Int32 rate)
        {
            if (!_TakeBackSingle(m_SurvivalInitial, ref m_Survival, ref rate)) return false;
            if (priority < 1) return true;
            if (!_TakeBackSingle(m_ProductionInitial, ref m_Production, ref rate)) return false;
            if (priority < 2) return true;
            if (!_TakeBackSingle(m_RegenerationInitial, ref m_Regeneration, ref rate)) return false;
            return true;
        }

        public void Set(Int32 forSurvival, Int32 forProduction=-1, Int32 forRegen=-1)
        {
            if (forSurvival < 0) return;
            m_Survival += (forSurvival - m_SurvivalInitial);
            m_SurvivalInitial = forSurvival;

            if (forProduction < 0) return;
            m_Production += (forProduction - m_ProductionInitial);
            m_ProductionInitial = forProduction;

            if (forRegen < 0) return;
            m_Regeneration+= (forRegen - m_RegenerationInitial);
            m_RegenerationInitial = forRegen;
        }
        public void Restore()
        {
            m_Survival = m_SurvivalInitial;
            m_Regeneration = m_RegenerationInitial;
            m_Production = m_ProductionInitial;
        }
        public void Increase(Int32 forSurvival, Int32 forProduction=0, Int32 forRegen=0)
        {
            m_Survival += forSurvival;
            m_SurvivalInitial += forSurvival;
            m_Production += forProduction;
            m_ProductionInitial += forProduction;
            m_Regeneration += forRegen;
            m_RegenerationInitial += forRegen;
        }
    }
}
