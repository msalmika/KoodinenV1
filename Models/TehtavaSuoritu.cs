using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class TehtavaSuoritu
    {
        public int TehtavaSuoritusId { get; set; }
        public DateTime? SuoritusPvm { get; set; }
        public int? KayttajaId { get; set; }
        public int? TehtavaId { get; set; }

        public virtual Kayttaja Kayttaja { get; set; }
        public virtual Tehtava Tehtava { get; set; }
    }
}
