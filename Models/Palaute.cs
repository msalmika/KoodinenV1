using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Palaute
    {
        public int PalauteId { get; set; }
        public string Teksti { get; set; }
        public string Lahettaja { get; set; }
    }
}
