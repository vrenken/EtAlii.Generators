namespace EtAlii.Generators
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

        private Correlation(string key, bool thrownIfNotExists)
        {
            _bookmark = Items;
            if (thrownIfNotExists && !Items.ContainsKey(key))
            {
                throw new InvalidOperationException($"{key} is not being correlated - can't remove key from current scope");
            }
            _relatedDisposables.Push(LogContext.Suspend());
            Items = Items.Remove(key);
            foreach(var pair in Items)
            {
                _relatedDisposables.Push(LogContext.PushProperty(pair.Key, pair.Value));
            }
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

        public static Correlation Begin(CorrelationType type, bool thrownIfExists = true)
        {
            return Begin(type, ShortId.GetId(), thrownIfExists);
        }

        public static Correlation Suspend()
        {
            return new Correlation();
        }

        public static Correlation Suspend(CorrelationType type, bool thrownIfNotExists = false)
        {
            return new Correlation(type.ToString(), thrownIfNotExists);
        }

        public static Correlation Begin(CorrelationType type, string value, bool thrownIfExists = true)
        {
            value ??= ShortId.GetId();
            return new Correlation(type.ToString(), value, thrownIfExists);
        }

        public static IImmutableDictionary<string, string> GetKeyValuePairs()
        {
            return Items.ToImmutableDictionary();
        }
    }
}
