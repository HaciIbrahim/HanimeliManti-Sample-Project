using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliManti.Entities
{
    public class EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required,ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        [Required, ScaffoldColumn(false)]
        public DateTime ModifiedOn { get; set; }

        [Required, StringLength(50), ScaffoldColumn(false)]
        public string ModifiedUsername { get; set; }
    }
}
