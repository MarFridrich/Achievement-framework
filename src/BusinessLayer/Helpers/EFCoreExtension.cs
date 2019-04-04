using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Helpers
{
    public static class EfCoreExtension
    {
        public static IQueryable<TOut> Set<TOut>(this DbContext context, Type T) where TOut : class
        {

            var method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);

            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable<TOut>;
        }
        
        public static IQueryable Set(this DbContext context, Type T)
        {

            var method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance);
            method = method.MakeGenericMethod(T);

            return method.Invoke(context, null) as IQueryable;
        }
        
        
    }
}