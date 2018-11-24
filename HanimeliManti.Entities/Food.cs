using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliManti.Entities
{
    [Table("Foods")]
    public class Food : EntityBase
    {
        [DisplayName("Yemek Adı"),Required, StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Yemek Açıklama"),Required, StringLength(3000)]
        public string Description { get; set; }

        [DisplayName("Yemek Beğeni Sayısı"),Required]
        public int LikeCount { get; set; }

        [DisplayName("Yemek Fiyatı"),Required]
        public float Price { get; set; }

        public virtual HanimeliUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Food()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
