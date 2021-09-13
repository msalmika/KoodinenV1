using System;
using System.Collections.Generic;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class TehtavaEpaonnistunut
    {
        public int EpaonnistunutId { get; set; }
        public string TehtavanNimi { get; set; }
        public int? EpaonnistunutMaara { get; set; }
        public int? TehtavaId { get; set; }

        public virtual Tehtava Tehtava { get; set; }
    }
}
