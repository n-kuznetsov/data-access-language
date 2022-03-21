﻿using DataAccessLanguage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLanguage
{
    public class OrderByDescPart : IExpressionPart
    {
        public ExpressionType Type => ExpressionType.Function;

        private IExpression expression;

        public OrderByDescPart(IExpressionFactory expressionFactory, string parameter)
        {
            expression = expressionFactory.Create(parameter);
        }

        public object GetValue(object dataObject) =>
            dataObject switch
            {
                IEnumerable<object> e => e.OrderByDescending(x => expression.GetValue(x)),
                _ => null
            };

        public bool SetValue(object dataObject, object value) =>
            throw new NotImplementedException();

        public async Task<object> GetValueAsync(object dataObject) =>
            dataObject switch
            {
                IEnumerable<object> e => 
                (await e.SelectAsync(async x => new { OrderValue = await expression.GetValueAsync(x), Data = x }))
                    .OrderByDescending(x => x.OrderValue).Select(x => x.Data).ToList(),
                _ => null
            };

        public Task<bool> SetValueAsync(object dataObject, object value) =>
            Task.FromResult(SetValue(dataObject, value));
    }
}