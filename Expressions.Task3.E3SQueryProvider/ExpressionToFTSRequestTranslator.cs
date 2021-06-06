using Expressions.Task3.E3SQueryProvider.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        public FtsQueryRequest TranslateToObj(Expression exp)
        {
            var result = new FtsQueryRequest();
            var statements = new List<Statement>();


            Visit(exp);
            var queries = _resultStringBuilder.ToString().Split("$");

            foreach(var query in queries)
            {
                statements.Add(new Statement { Query = query });
            }

            result.Statements = statements;
            return result;
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            } else if (node.Method.Name == "Contains") 
            {
                var constant = node.Arguments[0] as ConstantExpression;
                var expression = node.Object as MemberExpression;
                Visit(expression);
                _resultStringBuilder.Append("(*");
                Visit(constant);
                _resultStringBuilder.Append("*)");
                return node;
            }
            else if (node.Method.Name == "StartsWith")
            {
                var constant = node.Arguments[0] as ConstantExpression;
                var expression = node.Object as MemberExpression;
                Visit(expression);
                _resultStringBuilder.Append("(");
                Visit(constant);
                _resultStringBuilder.Append("*)");
                return node;
            }
            else if (node.Method.Name == "EndsWith")
            {
                var constant = node.Arguments[0] as ConstantExpression;
                var expression = node.Object as MemberExpression;
                Visit(expression);
                _resultStringBuilder.Append("(*");
                Visit(constant);
                _resultStringBuilder.Append(")");
                return node;
            }
            else if (node.Method.Name == "Equals")
            {
                var constant = node.Arguments[0] as ConstantExpression;
                var expression = node.Object as MemberExpression;
                Visit(expression);
                _resultStringBuilder.Append("(");
                Visit(constant);
                _resultStringBuilder.Append(")");
                return node;
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    Expression expression = null;
                    Expression constant = null;
                    if (node.Left.NodeType == ExpressionType.MemberAccess && node.Right.NodeType == ExpressionType.Constant)
                    {
                        expression = node.Left;
                        constant = node.Right;
                    } else if (node.Right.NodeType == ExpressionType.MemberAccess && node.Left.NodeType == ExpressionType.Constant) {
                        expression = node.Right;
                        constant = node.Left;
                    } else
                    {
                        throw new NotSupportedException($"Left operand should be property or field: {node.NodeType}");
                    }

                    Visit(expression);
                    _resultStringBuilder.Append("(");
                    Visit(constant);
                    _resultStringBuilder.Append(")");
                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append("$");
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion
    }
}
