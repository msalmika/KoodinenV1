using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoodinenV1.Models
{
    public class SalasananVaihtoViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Nykyinen salasana")]
        public string VanhaSalasana { get; set; }

        [StringLength(100, ErrorMessage = "Salasanan on oltava vähintään {2} merkkiä pitkä,\nja sen tulee sisältää ainakin yksi pieni ja suuri\nkirjain sekä numero.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Uusi salasana")]
        public string UusiSalasana { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Vahvista uusi salasana")]
        [Compare("UusiSalasana", ErrorMessage = "Salasanat eivät täsmää.")]
        public string VahvistusSalasana { get; set; }

        public int Id { get; set; }
    }
}

