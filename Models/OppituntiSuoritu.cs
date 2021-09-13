using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class OppituntiSuoritu
    {
        public int OppituntiSuoritusId { get; set; }
        public DateTime? SuoritusPvm { get; set; }
        public int? KayttajaId { get; set; }
        public int? OppituntiId { get; set; }
        public bool? Kesken { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual Oppitunti Oppitunti { get; set; }
    }
}
