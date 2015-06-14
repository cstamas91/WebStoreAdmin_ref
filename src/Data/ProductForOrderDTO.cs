using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Data
{
    public class ProductForOrderDTO
    {
        public ProductDTO Product { get; set; }
        public Int32 Quantity { get; set; }
    }
}
