using Nokia.Graphics.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramInOneHour
{
    public class ImageFilter
    {
        public string Name { get; set; }
        public IFilter Filter { get; set; }

        public ImageFilter(string name, IFilter filter)
        {
            this.Name = name;
            this.Filter = filter;
        }
    }
}
