using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanimeliManti.DataAccessLayer.EntityFramework;
using HanimeliManti.Entities;

namespace HanimeliManti.BusinessLayer
{
    public class FoodManager : ManagerBase<Food>
    {
        public List<Food> GetAllFood()
        {
            return List();
        }

        public IQueryable<Food> GetAllFoodQueryable()
        {
            return ListQueryable();
        }
    }
}
