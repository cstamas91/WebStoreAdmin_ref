using System;

namespace ITStore.Data
{
    public class ProductDTO
    {
        public Int32 ID { get; set; }

        public CategoryDTO Category { get; set; }

        public Int32 CategoryID { get; set; }

        public String Manufacturer { get; set; }

        public String Model { get; set; }

        public String Descr { get; set; }

        public Int32 Price { get; set; }

        public Double PriceWithTax { get; set; }

        public Int32 Stock { get; set; }

        public Boolean IsActive { get; set; }
    }
}
