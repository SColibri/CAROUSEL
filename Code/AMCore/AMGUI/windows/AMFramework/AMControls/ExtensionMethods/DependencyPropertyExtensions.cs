using Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AMControls.ExtensionMethods
{
    public static class DependencyPropertyExtensions
    {
        /// <summary>
        /// Find parent element
        /// </summary>
        public static T? TryFindParent<T>(this DependencyObject dependecyObject) where T: DependencyObject
        {
            // output
            T? result;

            // First Parent
            DependencyObject parent = VisualTreeHelper.GetParent(dependecyObject);

            // Try find, returns null if not found
            while(parent != null && parent is not T) 
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            result = parent as T;

            return result;
        }

        /// <summary>
        /// Find child of type in visual tree
        /// </summary>
        public static T? TryFindChildOfType<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            // Check if the parent matches the specified type
            if (parent is T result) return result;

            // Traverse the visual tree and check each child element
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                result = TryFindChildOfType<T>(child);
                if (result != null) return result;
            }

            // If no matching child element is found, return null
            return null;
        }

        /// <summary>
        /// Find all elements of type 
        /// </summary>
        public static IEnumerable<T> FindChildrenOfType<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            // Check if the parent matches the specified type
            T? result = parent as T;
            if (result != null) yield return result;

            // Traverse the visual tree and add each matching child element to the list
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                foreach (T childOfType in FindChildrenOfType<T>(child))
                {
                    yield return childOfType;
                }
            }
        }

    }
}
