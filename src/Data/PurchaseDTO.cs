using System;
using System.Collections.Generic;

namespace ITStore.Data
{
    public class PurchaseDTO
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Addr { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public Boolean Completion { get; set; }
        public Nullable<System.DateTime> Datum { get; set; }

        //public Dictionary<Int32,Int32> Products { get; set; }


    }
}
