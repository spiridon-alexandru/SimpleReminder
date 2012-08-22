using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SimpleReminder.Model.ListModel
{
    public class Group<T> : ObservableCollection<T>
    {
        public Group(string name, IEnumerable<T> items, Brush headerColor)
        {
            this.Title = name;
            foreach (T item in items)
            {
                this.Add(item);
            }
            this.HeaderColor = headerColor;
        }

        public override bool Equals(object obj)
        {
            Group<T> that = obj as Group<T>;
            return (that != null) && (this.Title.Equals(that.Title));
        }

        public string Title
        {
            get;
            set;
        }

        public Brush HeaderColor
        {
            get;
            set;
        }
    }

    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            ObservableCollection<T> observableCollection = new ObservableCollection<T>();
            foreach (T item in collection)
            {
                observableCollection.Add(item);
            }

            return observableCollection;
        }
    }
}
