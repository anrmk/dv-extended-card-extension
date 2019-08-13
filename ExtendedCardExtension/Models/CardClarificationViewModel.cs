using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExtendedCardExtension.Models {
    public class CardClarificationViewModel {
        [Required]
        public Guid cardId { get; set; }

        [Required]
        public string content { get; set; }
    }
}