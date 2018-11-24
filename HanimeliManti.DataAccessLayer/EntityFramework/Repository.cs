﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HanimeliManti.Common;
using HanimeliManti.Core.DataAccess;
using HanimeliManti.DataAccessLayer;
using HanimeliManti.Entities;

namespace HanimeliManti.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        //Protected'lar miras alma ile kullanılabilir.
        //Static metotların içerisinde static  değişkenler kullanılmalıdır.
        private DbSet<T> _objectSet;

        public Repository()
        {

            _objectSet = context.Set<T>();
        }


        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername();
            }

            return Save();

        }

        public int Update(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsername(); ;
            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);

            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}