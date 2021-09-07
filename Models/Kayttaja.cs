using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Kayttaja
    {
        public Kayttaja()
        {
            KurssiSuoritus = new HashSet<KurssiSuoritu>();
            Kurssis = new HashSet<Kurssi>();
            OppituntiSuoritus = new HashSet<OppituntiSuoritu>();
            TehtavaSuoritus = new HashSet<TehtavaSuoritu>();
        }

        public int KayttajaId { get; set; }
        public string Nimi { get; set; }
        public string Email { get; set; }
        public string Salasana { get; set; }

        public virtual ICollection<KurssiSuoritu> KurssiSuoritus { get; set; }
        public virtual ICollection<Kurssi> Kurssis { get; set; }
        public virtual ICollection<OppituntiSuoritu> OppituntiSuoritus { get; set; }
        public virtual ICollection<TehtavaSuoritu> TehtavaSuoritus { get; set; }
    }
}
