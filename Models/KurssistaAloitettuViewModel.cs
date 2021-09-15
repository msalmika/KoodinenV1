using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Models
{
    public class KurssistaAloitettuViewModel
    {
        public Kurssi Kurssi { get; set; }
        public List<Oppitunti> Oppitunnit { get; set; }
        public List<Tehtava> Tehtävät { get; set; }
        public List<int?> Suoritetut { get; set; }
    }
}
