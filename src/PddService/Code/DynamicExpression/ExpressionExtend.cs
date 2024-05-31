using PddService.Code.DynamicExpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression
{

    public static class ExpressionExtend
    {
        
         public static IQueryable<TSource> Query<TSource>(this IQueryable<TSource> source, PaginationInfo pagination)
        {
            if (pagination == null)
            {
                pagination = new PaginationInfo();
            }
            pagination.PageIndex = pagination.PageIndex < 1 ? 0 : pagination.PageIndex - 1;
            pagination.PageSize = pagination.PageSize < 1 ? 10 : pagination.PageSize;
            if (pagination.Sort == null)
            {
                pagination.Sort = new Sort();
                pagination.Sort.Field = "Id";
                pagination.Sort.Asc = false;
            }
            #region 生成查询表达式
            var param = DynamicLinq.CreateLambdaParam<TSource>("c");
            Expression body = Expression.Constant(true); //初始默认一个true  
            if (pagination.Condition != null || pagination.Groups != null)
            {
                if (pagination.Condition != null)
                {
                    foreach (var rule in pagination.Condition.Rules)
                    {
                        body = body.AndAlso(param.GenerateBody<TSource>(rule));
                    }
                }
                else
                {
                    if (pagination.Groups.Any())
                    {
                        foreach (var condition in pagination.Groups)
                        {
                            Expression nodeBody = Expression.Constant(true); //初始默认一个true  
                            foreach (var rule in condition.Rules)
                            {
                                nodeBody = nodeBody.AndAlso(param.GenerateBody<TSource>(rule));
                            }
                            switch (condition.Op)
                            {
                                case Operator.And:
                                    {
                                        body = body.AndAlso(nodeBody);
                                    }
                                    break;
                                case Operator.Or:
                                    {
                                        body = body.Or(nodeBody);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion 生成查询表达式
            var lambda = param.GenerateTypeLambda<TSource>(body); //最终组成lambda
            return source.Where(lambda).Sort(pagination.Sort);
        }
        internal static IOrderedQueryable<TSource> SortDesc<TSource>(this IQueryable<TSource> source,
            LambdaExpression lambda,
            Type type, Type propertyType)
        {
            Expression expr = Expression.Call(typeof(Queryable), "OrderByDescending",
                new Type[] { type, propertyType },
                Expression.Constant(source), lambda);
            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expr);
        }
        internal static IOrderedQueryable<TSource> SortAsc<TSource>(this IQueryable<TSource> source,
            LambdaExpression lambda,
            Type type, Type propertyType)
        {
            Expression expr = Expression.Call(typeof(Queryable), "OrderBy",
                new Type[] { type, propertyType },
                Expression.Constant(source), lambda);
            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(expr);
        }
    }
}
