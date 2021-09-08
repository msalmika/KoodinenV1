using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Oppitunti
    {
        public Oppitunti()
        {
            Ohjeistus = new HashSet<Ohjeistu>();
            OppituntiSuoritus = new HashSet<OppituntiSuoritu>();
            Tehtavas = new HashSet<Tehtava>();
        }

        public int OppituntiId { get; set; }
        public string Nimi { get; set; }
        public string Kuvaus { get; set; }
        public int? KurssiId { get; set; }

        public virtual Kurssi Kurssi { get; set; }
        public virtual ICollection<Ohjeistu> Ohjeistus { get; set; }
        public virtual ICollection<OppituntiSuoritu> OppituntiSuoritus { get; set; }
        public virtual ICollection<Tehtava> Tehtavas { get; set; }
    }
}
