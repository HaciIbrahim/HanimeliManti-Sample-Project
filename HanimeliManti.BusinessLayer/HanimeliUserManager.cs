using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanimeliManti.DataAccessLayer.EntityFramework;
using HanimeliManti.Entities;
using HanimeliManti.Entities.Messages;
using HanimeliManti.Entities.ValueObjects;

namespace HanimeliManti.BusinessLayer
{
    public class HanimeliUserManager : ManagerBase<HanimeliUser>
    {
        //private Repository<HanimeliUser> repo_user = new Repository<HanimeliUser>();

        //Bu kontrolleri bu katmanda yapmamızın sebebi, yarın öbürgün projenin bir mobil UI'ı tasarlanılırsa kullanılsın diye.
        public BusinessLayerResult<HanimeliUser> RegisterUser(RegisterViewModel data)
        {
            HanimeliUser user = Find(x => x.Username == data.Username && x.Email == data.Email);
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta Adresi Kayıtlı");
                }
            }
            else
            {
                int dbResult = base.Insert(new HanimeliUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    ProfileImageFilename = "user_boy.png",
                    Password = data.Password,
                    IsAdmin = false

                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);

                }
            }

            return res;
        }

        public BusinessLayerResult<HanimeliUser> GetUserById(int id)
        {
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı!");
            }

            return res;
        }

        public BusinessLayerResult<HanimeliUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);


            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı yada Şifre hatalı!");
            }

            return res;
        }

        public BusinessLayerResult<HanimeliUser> UpdateProfile(HanimeliUser data)
        {
            HanimeliUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUptated, "Profil Güncellenemedi");
            }

            return res;

        }

        public BusinessLayerResult<HanimeliUser> RemoveUserById(int id)
        {
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();
            HanimeliUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı");
            }

            return res;
        }

        //Metot hiding. Hataları alabilmek için, Insert metodunun geri dönüş tipini değiştirmek gerekiyordu.
        //Override yaparak metodun tipini değiştiremezdim. O yüzden Metot hiding tercih ettim.
        public new BusinessLayerResult<HanimeliUser> Insert(HanimeliUser data)
        {
            HanimeliUser user = Find(x => x.Username == data.Username && x.Email == data.Email);
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();

            res.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta Adresi Kayıtlı");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_boy.png";


                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }

            }

            return res;
        }

        public new BusinessLayerResult<HanimeliUser> Update(HanimeliUser data)
        {
            HanimeliUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<HanimeliUser> res = new BusinessLayerResult<HanimeliUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-Posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsAdmin = data.IsAdmin;


            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUptated, "Kullanıcı Güncellenemedi.");
            }

            return res;

        }
    }
}