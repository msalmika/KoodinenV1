using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Tehtava
    {
        public Tehtava()
        {
            TehtavaSuoritus = new HashSet<TehtavaSuoritu>();
        }

        public int TehtavaId { get; set; }
        public string Nimi { get; set; }
        public string Kuvaus { get; set; }
        public string Vihje { get; set; }
        public string TehtavaUrl { get; set; }
        public int? OppituntiId { get; set; }

        public virtual Oppitunti Oppitunti { get; set; }
        public virtual ICollection<TehtavaSuoritu> TehtavaSuoritus { get; set; }
    }
}
