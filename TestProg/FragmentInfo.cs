using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProg
{
    public class FragmentInfo
    {
        public int PictureNumb { get; set; }
        public Point Coords { get; set; }

        public FragmentInfo()
        {
            PictureNumb= 0;    
            Coords = new Point();
        }
        public FragmentInfo(int pictureNumb, Point coords)
        {
            PictureNumb = pictureNumb;
            Coords = coords;
        }

        public override string ToString()
        {

            return $"{PictureNumb};{Coords.X},{Coords.Y}";
        }

    }
}
