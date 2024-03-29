﻿namespace EtAlii.Generators
{

    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Threading;
    using Serilog.Context;

    public class Correlation : IDisposable
    {
        private static IImmutableDictionary<string, string> Items { get => _items.Value ?? (_items.Value = ImmutableDictionary<string, string>.Empty); set => _items.Value = value; }
        // ReSharper disable once InconsistentNaming
        private static readonly AsyncLocal<IImmutableDictionary<string, string>> _items = new();

        private readonly IImmutableDictionary<string, string> _bookmark;

        private readonly Stack<IDisposable> _relatedDisposables = new();

        private Correlation(string key, string value, bool thrownIfExists)
        {
            _bookmark = Items;
            if (thrownIfExists && Items.ContainsKey(key))
            {
                throw new InvalidOperationException($"{key} is already being correlated with value {Items[key]}");
            }
            _relatedDisposables.Push(LogContext.PushProperty(key, value));
            Items = Items.SetItem(key, value);
        }

        //Used for suspension
        private Correlation()
        {
            _bookmark = Items;
            _relatedDisposables.Push(LogContext.Suspend());
            Items = Items.Clear();
        }

        public void Dispose()
        {
            foreach (var disposable in _relatedDisposables)
            {
                disposable.Dispose();
            }
            //Reset Items back to bookmark
            Items = _bookmark;
        }

        public static Correlation Begin(string correlationKey, bool thrownIfExists = true)
        {
            return Begin(correlationKey, null, thrownIfExists);
        }

        public static Correlation Begin(string correlationKey, string value, bool thrownIfExists = true)
        {
            value ??= ShortId.GetId();
            return new Correlation(correlationKey, value, thrownIfExists);
        }


        public static Correlation Suspend()
        {
            return new Correlation();
        }

        public static IImmutableDictionary<string, string> GetKeyValuePairs()
        {
            return Items.ToImmutableDictionary();
        }
    }
}
