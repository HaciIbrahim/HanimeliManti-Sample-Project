using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliManti.Entities
{
    [Table("HanimeliUsers")]
    public class HanimeliUser : EntityBase
    {
        [DisplayName("İsim"),
         StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyad"),
         StringLength(50)]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-Posta"),
         Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"),
         Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        [StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır."),
        ScaffoldColumn(false)] // images/user12.jpg
        public string ProfileImageFilename { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }
    }
}
