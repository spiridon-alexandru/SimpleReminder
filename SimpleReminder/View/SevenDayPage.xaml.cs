using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using SimpleReminder.Model;
using SimpleReminder.Model.ListModel;
using SimpleReminder.Model.Repository;
using System.Windows.Navigation;

namespace SimpleReminder.View
{
    public partial class SevenDayPage : PhoneApplicationPage
    {
        // long list selector object and the list of groups
        LongListSelector daysListSelector;
        IList<Group<Item>> itemList;

        // the storage object
        IStorage storage;

        // Constructor
        public SevenDayPage()
        {
            InitializeComponent();

            InitList();
            InitStorage();
        }

        #region Initialization

        private void InitList()
        {
            // create the list selector and add the templates
            daysListSelector = new LongListSelector();
            daysListSelector.ItemTemplate = Application.Current.Resources["jumpListItemTemplate"] as DataTemplate;
            daysListSelector.GroupItemTemplate = Application.Current.Resources["groupItemTemplate"] as DataTemplate;
            daysListSelector.GroupHeaderTemplate = Application.Current.Resources["groupHeaderTemplate"] as DataTemplate;
            daysListSelector.GroupItemsPanel = Application.Current.Resources["groupHeaderItemsTemplate"] as ItemsPanelTemplate;

            // when the user clicks on an item, we need to see more editable details about that particular selection
            daysListSelector.SelectionChanged += new SelectionChangedEventHandler(listSelector_SelectionChanged);

            // add the list to the main layout
            ContentPanel.Children.Add(daysListSelector);
        }

        private void InitStorage()
        {
            storage = new IsoStorage();
        }

        #endregion

        #region Overriden methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PopulateList();
        }

        #endregion

        #region User events

        /**
         * ListViewItem click event - don't know what it does.
         */
        void listSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (daysListSelector.SelectedItem != null)
            {
                (daysListSelector.SelectedItem as Item).Name = "test";
            }
        }

        /**
         * New entry button press - navitates to the new entry page.
         */
        private void NewEntryBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("View/NewEntryPage.xaml", UriKind.Relative));
        }

        #endregion

        #region Private methods

        /**
         * Populates the current day entries
         */
        private void PopulateList()
        {
            // populate the list with dummy data - for testing only
            List<Item> source = new List<Item>();

            DateTime day = DateTime.Now;
            int i = 0;

            // we need to add the current and the next 6 days to the week
            while (i < 6)
            {
                switch (day.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Luni.ToString() });
                        break;
                    case DayOfWeek.Tuesday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Marti.ToString() });
                        break;
                    case DayOfWeek.Wednesday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Miercuri.ToString() });
                        break;
                    case DayOfWeek.Thursday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Joi.ToString() });
                        break;
                    case DayOfWeek.Friday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Vineri.ToString() });
                        break;
                    case DayOfWeek.Saturday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Sambata.ToString() });
                        break;
                    case DayOfWeek.Sunday:
                        source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Duminica.ToString() });
                        break;
                }

                // iterate
                day = day.AddDays(1);
                i++;
            }

            var groupBy = from jumpdemo in source
                          group jumpdemo by jumpdemo.GroupBy into c
                          select new Group<Item>(c.Key, c, new SolidColorBrush(Colors.White));
            itemList = groupBy.ToObservableCollection();
            this.daysListSelector.ItemsSource = itemList;
        }

        #endregion
    }
}