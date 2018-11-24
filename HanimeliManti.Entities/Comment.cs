using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliManti.Entities
{
    [Table("Comments")]
    public class Comment : EntityBase
    {
        [Required, StringLength(1000)]
        public string Text { get; set; }

        public virtual Food Food { get; set; }
        public virtual HanimeliUser Owner { get; set; }
    }
}
