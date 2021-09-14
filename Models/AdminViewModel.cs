using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Models
{
    public class AdminViewModel
    {
        public List<Tehtava> Tehtävät { get; set; }
        public Ohjeistu Ohje { get; set; }
    }
}
