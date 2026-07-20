using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Ecommerce.Helper.Imaging
{
    public class CropBox
    {
        //attributi per memorizzare il punto di partenza del quadrato del crop
        public int X { get; set; }
        public int Y { get; set; }

        // dimensione per memorizzare le dimenzoini del crop
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
