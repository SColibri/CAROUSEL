using Assimp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AMControls.ExtensionMethods
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Removes a range of items in an observable collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="count"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RemoveLast<T>(this ObservableCollection<T> collection, int count)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (count <= 0)
            {
                return;
            }

            int startIndex = collection.Count - count;
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            for (int i = startIndex; i < collection.Count; i++)
            {
                collection.RemoveAt(startIndex);
            }
        }
    }
}
