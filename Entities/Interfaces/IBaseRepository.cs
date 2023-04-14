using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    T GetById(int id);

    IEnumerable<T> Get();
    IEnumerable<T> GetActive(Expression<Func<T, bool>> where);
    T Find(Expression<Func<T, bool>> criteria, string[] includes = null);

    IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);

    T Create(T entity);

    T Edit(T entity);

}