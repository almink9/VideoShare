﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VideoShare.Core.Entities;
using VideoShare.Core.IRepo;
using VideoShare.DataAccess.Data;

namespace VideoShare.DataAccess.Repo
{
  public class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity
  {
    private readonly Context _context;
    internal DbSet<T> _contextSet;

    public BaseRepo(Context context)
    {
      _context = context;
      _contextSet = _context.Set<T>();
    }

    public void Add(T entity)
    {
      _contextSet.Add(entity);
    }
    public void Update(T source, T destination)
    {
      _contextSet.Entry(source).CurrentValues.SetValues(destination);
    }
    public void Remove(T entity)
    {
      _contextSet.Remove(entity);
    }
    public void RemoveRange(IEnumerable<T> entities)
    {
      _contextSet.RemoveRange(entities);
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria)
    {
      IQueryable<T> query = _contextSet;
      query = query.Where(criteria);
      return await query.AnyAsync();
    }
    public async Task<T> GetByIdAsync(int id, string includeProperties = null)
    {
      IQueryable<T> query = _contextSet;
      if (!string.IsNullOrEmpty(includeProperties))
      {
        query = GetQueryWithIncludedProperties(query, includeProperties);
      }

      return await query.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> criteria, string includeProperties = null)
    {
      IQueryable<T> query = _contextSet;
      if (!string.IsNullOrEmpty(includeProperties))
      {
        query = GetQueryWithIncludedProperties(query, includeProperties);
      }

      return await query.Where(criteria).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> criteria = null, string includeProperties = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> orderedBy = null)
    {
      IQueryable<T> query = _contextSet;
      if (criteria != null)
      {
        query = query.Where(criteria);
      }

      if (!string.IsNullOrEmpty(includeProperties))
      {
        query = GetQueryWithIncludedProperties(query, includeProperties);
      }

      if (orderedBy != null)
      {
        return await orderedBy(query).ToListAsync();
      }

      return await query.ToListAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
    {
      IQueryable<T> query = _contextSet;
      if (criteria != null)
      {
        query = query.Where(criteria);
      }

      return await query?.CountAsync();
    }

    #region Static Methods
    public static IQueryable<T> GetQueryWithIncludedProperties(IQueryable<T> query, string includeProperties)
    {
      var props = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      foreach(var prop in props)
      {
        query = query.Include(prop);
      }

      return query;
    }
    #endregion
  }
}
