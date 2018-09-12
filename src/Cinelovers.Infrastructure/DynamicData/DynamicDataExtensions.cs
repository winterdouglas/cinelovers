using DynamicData;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinelovers.Infrastructure.DynamicData
{
    public static class DynamicDataExtensions
    {
        public static void Upsert<TObject, TKey>(this ISourceCache<TObject, TKey> source, IEnumerable<TObject> items)
        {
            var keyComparer = new KeyComparer<TObject, TKey>();
            Func<TObject, TObject, bool> areEqual = EqualityComparer<TObject>.Default.Equals;

            source.Edit(innerCache =>
            {
                var originalItems = innerCache.KeyValues.AsArray();
                var newItems = innerCache.GetKeyValues(items).AsArray();

                var adds = newItems.Except(originalItems, keyComparer).ToArray();
                var intersect = newItems
                    .Select(kvp => new { Original = innerCache.Lookup(kvp.Key), NewItem = kvp })
                    .Where(x => x.Original.HasValue && !areEqual(x.Original.Value, x.NewItem.Value))
                    .Select(x => new KeyValuePair<TKey, TObject>(x.NewItem.Key, x.NewItem.Value))
                    .ToArray();

                innerCache.AddOrUpdate(adds.Union(intersect));
            });
        }
    }
}
