using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.Internal;
using PIMTool.Core.Models;

namespace PIMTool.Core.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(IList<SearchByInfo> infos, Expression<Func<T, bool>> initExpr)
    {
        if (infos.Count == 0)
        {
            return initExpr;
        }
        
        var param = Expression.Parameter(typeof(T), typeof(T).Name);
        IList<Expression<Func<T, bool>>> exprList = infos
            .Select(info => (Expression<Func<T, bool>>)(p =>
                ReflectionHelper.GetPropertyValueByName(p, info.FieldName).ToString()!.Contains(info.Value.ToString()!.Trim(), StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var firstCombinedExpr = Expression.And(
            Expression.Invoke(initExpr, param),
            Expression.Invoke(exprList.First(), param));
        var firstExpr = Expression.Lambda<Func<T, bool>>(firstCombinedExpr, param);
        if (exprList.Count < 2)
        {
            return firstExpr;
        }
        
        for (var i = 1; i < exprList.Count; i++)
        {
            var combinedExpr = Expression.Or(
                Expression.Invoke(firstExpr, param),
                Expression.Invoke(exprList.ElementAt(i), param));
            firstExpr = Expression.Lambda<Func<T, bool>>(combinedExpr, param);
        }

        return firstExpr;
    }
    
    public static Expression<Func<T, bool>> CombineOrExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), typeof(T).Name);

        var combinedExpr = Expression.Or(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));

        return Expression.Lambda<Func<T, bool>>(combinedExpr, param);
    }
    
    public static Func<T, bool> CombineAndExpressions<T>(IList<SearchByInfo> infos, Expression<Func<T, bool>> initExpr)
    {
        if (infos.Count == 0)
        {
            return initExpr.Compile();
        }
        
        var param = Expression.Parameter(typeof(T), typeof(T).Name);
        IList<Expression<Func<T, bool>>> exprList = infos
            .Select(info => (Expression<Func<T, bool>>)(p =>
                ReflectionHelper.GetPropertyValueByName(p, info.FieldName).ToString()!.Contains(info.Value.ToString()!.Trim(), StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var firstCombinedExpr = Expression.And(
            Expression.Invoke(initExpr, param),
            Expression.Invoke(exprList.First(), param));
        var firstExpr = Expression.Lambda<Func<T, bool>>(firstCombinedExpr, param);
        if (exprList.Count < 2)
        {
            return firstExpr.Compile();
        }
        
        for (var i = 1; i < exprList.Count; i++)
        {
            var combinedExpr = Expression.And(
                Expression.Invoke(firstExpr, param),
                Expression.Invoke(exprList.ElementAt(i), param));
            firstExpr = Expression.Lambda<Func<T, bool>>(combinedExpr, param);
        }

        return firstExpr.Compile();
    }
    
    public static Expression<Func<T, bool>> CombineAndExpressions<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), typeof(T).Name);

        var combinedExpr = Expression.And(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param));

        return Expression.Lambda<Func<T, bool>>(combinedExpr, param);
    }
}