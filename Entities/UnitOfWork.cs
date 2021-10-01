using Entities;
using Entities.Interfaces;
using Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConicErpContext DB;

        public IBaseRepository<Bank> Banks { get; private set; }
        public IBaseRepository<Cash> Cashes { get; private set; }

        public UnitOfWork(ConicErpContext DBcontext)
        {
            DB = DBcontext;

            Banks = new BaseRepository<Bank>(DB);
            Cashes = new BaseRepository<Cash>(DB);
        }

        public int Complete()
        {
            return DB.SaveChanges();
        }

        public void Dispose()
        {
            DB.Dispose();
        }
    }
}