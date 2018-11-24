using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliManti.Core.DataAccess
{

    //Mysql veya başka bir veritabanı ile çalışırsak diye yapılmıştır.
    //MySql içinde bir klasör oluşturup. Repository.cs ve RepositoryBase.cs classlarını bunun içine aldığımızda. Sadece bir namespace ismi değiştirerek mysql ile çalışmış oluruz.
    public interface IDataAccess<T>
    {
        List<T> List();

        IQueryable<T> ListQueryable();

        List<T> List(Expression<Func<T, bool>> where);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();

        T Find(Expression<Func<T, bool>> where);
    }
}
