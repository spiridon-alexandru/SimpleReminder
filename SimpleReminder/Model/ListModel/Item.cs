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
using System.ComponentModel;

namespace SimpleReminder.Model.ListModel
{
    public class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string name;
        private Brush backgroundColor = new SolidColorBrush(Colors.Black);
        private FontFamily font = new System.Windows.Media.FontFamily("Courier New");

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string GroupBy
        {
            get;
            set;
        }

        public Brush BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public FontFamily ItemFont
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
                OnPropertyChanged("ItemFont");
            }
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
