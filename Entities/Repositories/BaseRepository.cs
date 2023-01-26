﻿using Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Entities.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected ConicErpContext _context;

    public BaseRepository(ConicErpContext context)
    {
        _context = context;
    }

    public BaseRepository()
    {
    }

    public IEnumerable<T> Get()
    {
        return _context.Set<T>().ToList();
    }

    public IEnumerable<T> GetActive(Expression<Func<T, bool>> where)
    {
        return _context.Set<T>().Where(where);
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var incluse in includes)
                query = query.Include(incluse);

        return query.SingleOrDefault(criteria);
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.Where(criteria).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
    {
        return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Edit(T entity)
    {
        _context.Update(entity);
        return entity;
    }

}