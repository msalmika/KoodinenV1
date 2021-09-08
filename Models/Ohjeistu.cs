using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Ohjeistu
    {
        public int OhjeistusId { get; set; }
        public string TekstiKentta { get; set; }
        public int? OppituntiId { get; set; }

        public virtual Oppitunti Oppitunti { get; set; }
    }
}
