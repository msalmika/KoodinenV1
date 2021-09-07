using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Kurssi
    {
        public Kurssi()
        {
            KurssiSuoritus = new HashSet<KurssiSuoritu>();
            Oppituntis = new HashSet<Oppitunti>();
        }

        public int KurssiId { get; set; }
        public string Nimi { get; set; }
        public string Kuvaus { get; set; }
        public int? KayttajaId { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual ICollection<KurssiSuoritu> KurssiSuoritus { get; set; }
        public virtual ICollection<Oppitunti> Oppituntis { get; set; }
    }
}
