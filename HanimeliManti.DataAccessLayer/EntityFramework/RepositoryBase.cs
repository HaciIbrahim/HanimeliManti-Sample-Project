using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanimeliManti.DataAccessLayer;

namespace HanimeliManti.DataAccessLayer.EntityFramework
{
    //Singleon Pattern
    //Static metotlar new'lenmeden çağrılabilir.
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _lockSync = new object();
        protected RepositoryBase()
        {
           CreateContext();
        }
        private static void CreateContext()
        {
            if (context == null)
            {
                // Multi-thread Uygulamalar için..
                lock (_lockSync)
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                }
            }

        }

    }
}
