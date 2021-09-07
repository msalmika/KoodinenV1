using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class KurssiSuoritu
    {
        public int KurssiSuoritusId { get; set; }
        public DateTime? SuoritusPvm { get; set; }
        public int? KayttajaId { get; set; }
        public int? KurssiId { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual Kurssi Kurssi { get; set; }
    }
}
