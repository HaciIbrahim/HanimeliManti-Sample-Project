using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HanimeliManti.Entities;

namespace HanimeliManti.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            HanimeliUser admin = new HanimeliUser()
            {
                Name = "İbrahim",
                Surname = "Tutumlu",
                Email = "haciibrahimtutumlu@gmail.com",
                Username = "ibrahim",
                IsAdmin = true,
                Password = "123456",
                ProfileImageFilename = "user_boy.png",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ibrahim"
            };

            HanimeliUser standartUser = new HanimeliUser()
            {
                Name = "Hacı",
                Surname = "Tutumlu",
                Email = "ibrahim.tutumlu.5@gmail.com",
                Username = "haci",
                IsAdmin = false,
                Password = "123456",
                ProfileImageFilename = "user_boy.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "ibrahim"
            };

            context.HanimeliUsers.Add(admin);
            context.HanimeliUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                HanimeliUser user = new HanimeliUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Username = $"user{i}",
                    IsAdmin = false,
                    Password = "123",
                    ProfileImageFilename = "user_boy.png",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.HanimeliUsers.Add(user);
            }

            context.SaveChanges();

            //Adding fake food..
            for (int i = 0; i < 10; i++)
            {
                Food food = new Food()
                {
                    Name = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = "ibrahim",
                    LikeCount = FakeData.NumberData.GetNumber(1,9),
                    Price = FakeData.NumberData.GetNumber(10,50),
                    Owner = admin
                };

                context.Foods.Add(food);

                for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                {
                    Comment comment = new Comment()
                    {
                        Text = FakeData.TextData.GetSentence(),
                        Owner = standartUser,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = "haci",
                    };

                    food.Comments.Add(comment);
                }

                List<HanimeliUser> userList = context.HanimeliUsers.ToList();

                for (int k = 0; k <food.LikeCount; k++)
                {
                    Liked liked = new Liked()
                    {
                        LikedUser = userList[k],
   
                    };

                    food.Likes.Add(liked);
                }
            }

            context.SaveChanges();
        }
    }
}
