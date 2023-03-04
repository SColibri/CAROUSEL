using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AMControls.Custom
{
    public class TreeView_custom_AM : TreeView
    {
        public static readonly DependencyProperty IDProperty =
        DependencyProperty.Register("ID",
                                    typeof(int),
                                    typeof(TreeView_custom_AM),
                                    new PropertyMetadata(default(int), OnIDPropertyChanged));

        private static void OnIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // do nothing
        }

        public static readonly DependencyProperty Selectionproperty =
        DependencyProperty.Register("SelectionTree",
                                    typeof(List<int>),
                                    typeof(TreeView_custom_AM),
                                    new PropertyMetadata(default(List<int>), OnSelectionPropertyChanged));

        private static void OnSelectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // do nothing
        }

        private int _id = -1;
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set
            {
                _id = value;
                SetValue(IDProperty, value);
            }
        }

        private List<int> _selectionTree = new();
        public List<int> SelectionTree
        {
            get
            {
                return (List<int>)GetValue(Selectionproperty);
            }
            set
            {
                _selectionTree = value;
                SetValue(Selectionproperty, value);

                int maxLevel = _selectionTree.Count;
                if (maxLevel == 0) return;

                int LevelIndex = 0;
                foreach (var item in Items)
                {
                    if (item is not TreeViewItem_custom_AM) continue;
                    TreeViewItem_custom_AM? treeTemp = item as TreeViewItem_custom_AM;
                    if (treeTemp == null) continue;
                    if (treeTemp.ID != _selectionTree[LevelIndex]) continue;
                    treeTemp.IsExpanded = true;

                    Reccursive_Find(LevelIndex++, maxLevel, treeTemp);

                    break;
                }
            }
        }

        private void Reccursive_Find(int LevelIndex, int MaxLevel, TreeViewItem_custom_AM tree)
        {
            if (MaxLevel == LevelIndex) return;
            foreach (var item in tree.Items)
            {
                if (item is not TreeViewItem_custom_AM) continue;
                TreeViewItem_custom_AM? treeTemp = item as TreeViewItem_custom_AM;
                if (treeTemp == null) continue;
                if (treeTemp.ID != _selectionTree[LevelIndex]) continue;
                treeTemp.IsExpanded = true;

                Reccursive_Find(LevelIndex++, MaxLevel, treeTemp);
                return;
            }
        }


    }
}
