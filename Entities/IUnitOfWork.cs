using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Bank> Banks { get; }
        IBaseRepository<Cash> Cashes { get; }

        int Complete();
    }
}