using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WindowsGame1
{
    public interface IConsumer
    {
        //[Method]
        Int32 GetDemands(Int16 resource);
        void FulfilDemands(Int16 resource, Int32 income);
        void TakeBack(Int16 resource, Int32 income);
        Root Root{get;set;}
        
    }
}
