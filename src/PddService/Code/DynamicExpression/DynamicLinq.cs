using PddService.Code.DynamicExpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace PddService.Code.DynamicExpression
{

    public static class DynamicLinq
    {
        /// <summary>  
        /// 创建lambda中的参数,即c=>c.xxx==xx 中的c  
        /// </summary>  
        public static ParameterExpression CreateLambdaParam<T>(string name)
        {
            return Expression.Parameter(typeof(T), name);
        }
        /// <summary>
        /// 创建lambda中的参数,即c=>c.xxx==xx 中的c  
        /// </summary>  
        public static ParameterExpression CreateLambdaParam(string name, Type type)
        {
            return Expression.Parameter(type, name);
        }
        /// <summary>  
        /// 创建linq表达示的body部分,即c=>c.xxx==xx 中的c.xxx==xx  
        /// </summary>  
        public static Expression GenerateBody<T>(this ParameterExpression param, Rule rule)
        {
            var type = typeof(T);
            return param.GenerateBody(rule, type);
        }
        /// <summary>
        /// 创建linq表达示的body部分,即c=>c.xxx==xx 中的c.xxx==xx  
        /// </summary>  
        public static Expression GenerateBody(this ParameterExpression param, Rule rule, Type type)
        {
            PropertyInfo property = type.GetProperty(rule.Field);
            //组装左边  
            Expression left = Expression.Property(param, property);
            //组装右边
            object value = null;
            value = property.PropertyType.IsEnum ? Enum.Parse(property.PropertyType, rule.Value) : Convert.ChangeType(rule.Value, property.PropertyType);
            Expression right = Expression.Constant(value);
            /*
            //todo: 下面根据需要，扩展自己的类型  
            if (property.PropertyType == typeof(int))
            {
                right = Expression.Constant(int.Parse(rule.Value));
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                right = Expression.Constant(DateTime.Parse(rule.Value));
            }
            else if (property.PropertyType == typeof(string))
            {
                right = Expression.Constant((rule.Value));
            }
            else if (property.PropertyType == typeof(decimal))
            {
                right = Expression.Constant(decimal.Parse(rule.Value));
            }
            else if (property.PropertyType == typeof(Guid))
            {
                right = Expression.Constant(Guid.Parse(rule.Value));
            }
            else if (property.PropertyType == typeof(bool))
            {
                right = Expression.Constant(rule.Value.Equals("1"));
            }
            else
            {
                throw new Exception("暂不能解析该Key的类型");
            }*/
            //todo: 下面根据需要扩展自己的比较  
            Expression filter = null;
            switch (rule.Op)
            {
                case "<=":
                    filter = Expression.LessThanOrEqual(left, right);
                    break;
                case "<":
                    filter = Expression.LessThan(left, right);
                    break;
                case ">":
                    filter = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    filter = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "!=":
                case "<>":
                    filter = Expression.NotEqual(left, right);
                    break;
                case "like":
                    filter = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        Expression.Constant(rule.Value));
                    break;
                case "startwith":
                    filter = Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        Expression.Constant(rule.Value));
                    break;
                default:
                    filter = Expression.Equal(left, right);
                    break;
            }
            return filter;
        }
        /// <summary>  
        /// 创建linq表达示的body部分,即c=>c.xxx==xx 中的c.xxx==xx  
        /// </summary>  
        public static IOrderedQueryable<TSource> Sort<TSource>(this IQueryable<TSource> sources, Sort sort)
        {
            Type type = typeof(TSource);
            ParameterExpression param = CreateLambdaParam<TSource>("x");
            PropertyInfo pi = type.GetProperty(sort.Field);
            Expression propertyExpr = Expression.Property(param, pi);
            var delegateType = typeof(Func<,>).MakeGenericType(type, pi.PropertyType);
            var lambda = GenerateLambda(param, propertyExpr, delegateType);
            if (sort.Asc)
            {
                return sources.SortAsc(lambda, type, pi.PropertyType);
            }
            else
            {
                return sources.SortDesc(lambda, type, pi.PropertyType);
            }
        }
        /// <summary>
        /// 创建完整的lambda,即c=>c.xxx==xx
        /// </summary>
        public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body)
        {
            return Expression.Lambda(body, param);
        }
        /// <summary>
        /// 创建完整的lambda,即c=>c.xxx==xx
        /// </summary>
        public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body,
            Type delegateType)
        {
            return Expression.Lambda(delegateType, body, param);
        }
        /// <summary>
        /// 创建完整的lambda，为了兼容EF中的where语句
        /// </summary>
        public static Expression<Func<T, bool>> GenerateTypeLambda<T>(this ParameterExpression param, Expression body)
        {
            return (Expression<Func<T, bool>>)(param.GenerateLambda(body));
        }
        public static Expression AndAlso(this Expression expression, Expression expressionRight)
        {
            return Expression.AndAlso(expression, expressionRight);
        }
        public static Expression Or(this Expression expression, Expression expressionRight)
        {
            return Expression.Or(expression, expressionRight);
        }
        public static Expression And(this Expression expression, Expression expressionRight)
        {
            return Expression.And(expression, expressionRight);
        }
    }
}
