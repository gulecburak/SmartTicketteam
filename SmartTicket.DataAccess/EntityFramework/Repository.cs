using SmartTicket.Commonn;
using SmartTicket.Core.DataAccess;
using SmartTicket.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartTicket.DataAccess.EntityFramework
{
    public class Repository<T> : BaseRepository, IDataAccess<T> where T:class
    {
        private DbSet<T> _objectSet; // objectset tipi tek seferde T tipinde dönüşüm yapıldı 1 kere kullanmış olduk yoksa metodlar içinde her seferinde objectset.Set<T> demek zorunda kalırdık
        public Repository()
        {
            
            _objectSet = context.Set<T>();//T tipindeki class a göre tip set edildi

        }


        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is BaseEntitiy)
            {
                BaseEntitiy o = obj as BaseEntitiy;
                DateTime now = DateTime.Now;

                o.CreateDate = now;
                o.UpdateDate = now;
                o.CreatedBy = App.Common.GetCurrentUsername();

            }
            return Save();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public int Update(T obj)
        {
            if (obj is BaseEntitiy)
            {
                BaseEntitiy o = obj as BaseEntitiy;
                DateTime now = DateTime.Now;

                o.CreateDate = now;
                o.UpdateDate = now;
                o.UpdatedBy = App.Common.GetCurrentUsername();

            }
            return Save();
        }
    }
}
