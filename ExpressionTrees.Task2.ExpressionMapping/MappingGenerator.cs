using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));

            var destProperties = Expression.Parameter(typeof(TDestination)).Type.GetProperties()
                .Select(t => t.Name);

            MemberInitExpression properties = Expression.MemberInit(Expression.New(typeof(TDestination)),
                sourceParam.Type.GetProperties().Where(p => destProperties.Contains(p.Name))
                .Select(p => GetProperty<TSource, TDestination>(p, sourceParam)));

            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    properties,
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        public MemberAssignment GetProperty<TSource, TDestination>(PropertyInfo p, ParameterExpression sourceParam) {
            var destProp = typeof(TDestination).GetProperty(p.Name);
            var sourceProp = Expression.Property(sourceParam, p);

            return Expression.Bind(destProp, sourceProp);
        }
    }
}
