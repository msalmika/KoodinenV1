using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class Vihje
    {
        public int VihjeId { get; set; }
        public string Vihje1 { get; set; }
        public string Vihje2 { get; set; }
        public string Vihje3 { get; set; }
        public int? TehtavaId { get; set; }

        public virtual Tehtava Tehtava { get; set; }
    }
}
