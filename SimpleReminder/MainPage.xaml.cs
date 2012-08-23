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

using SimpleReminder.Model.ListModel;
using SimpleReminder.Model;
using SimpleReminder.Model.Repository;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;

namespace SimpleReminder
{
    public partial class MainPage : PhoneApplicationPage
    {
        // a periodic task that updates the live title when new reminders appear
        PeriodicTask periodicTask;
        string periodicTaskName = "PeriodicLiveTitleUpdateAgent";

        // long list selector object and the list of groups
        LongListSelector dayListSelector;
        IList<Group<Item>> itemList;
        
        // the list of entryes
        List<Entry> entries;

        // the storage object
        IStorage storage;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            InitList();
            InitStorage();

            // creates the application title
            CreateApplicationTile();
            // Creates a periodic task that updates the live title when the application
            // is in foreground mode
            StartPeriodicAgent();
        }

        #region Initialization

        private void InitList()
        {
            // create the list selector and add the templates
            dayListSelector = new LongListSelector();
            dayListSelector.ItemTemplate = Application.Current.Resources["jumpListItemTemplate"] as DataTemplate;
            dayListSelector.GroupItemTemplate = Application.Current.Resources["groupItemTemplate"] as DataTemplate;
            dayListSelector.GroupHeaderTemplate = Application.Current.Resources["groupHeaderTemplate"] as DataTemplate;
            dayListSelector.GroupItemsPanel = Application.Current.Resources["groupHeaderItemsTemplate"] as ItemsPanelTemplate;

            // when the user clicks on an item, we need to see more editable details about that particular selection
            dayListSelector.SelectionChanged += new SelectionChangedEventHandler(listSelector_SelectionChanged);

            // add the list to the main layout
            ContentPanel.Children.Add(dayListSelector);
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
            if (dayListSelector.SelectedItem != null)
            {
                (dayListSelector.SelectedItem as Item).Name = "test";
            }
        }

        /**
         * New entry button press - navitates to the new entry page.
         */
        private void NewEntryBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/NewEntryPage.xaml", UriKind.Relative));
        }

        private void EntryListBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/SevenDayPage.xaml", UriKind.Relative));
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

            DateTime today = DateTime.Now;

            switch (today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Luni.ToString() });
                    break;
                case DayOfWeek.Tuesday:
                    source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Marti.ToString() });
                    break;
                case DayOfWeek.Wednesday:
                    source.Add(new Item() { Name = "Dummy 1", GroupBy = RoWeekDays.Miercuri.ToString() });
                    source.Add(new Item() { Name = "Dummy 2", GroupBy = RoWeekDays.Miercuri.ToString() });
                    source.Add(new Item() { Name = "Dummy 3", GroupBy = RoWeekDays.Miercuri.ToString() });
                    source.Add(new Item() { Name = "Dummy 4", GroupBy = RoWeekDays.Miercuri.ToString() });
                    source.Add(new Item() { Name = "Dummy 5", GroupBy = RoWeekDays.Miercuri.ToString() });
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

            var groupBy = from jumpdemo in source
                          group jumpdemo by jumpdemo.GroupBy into c
                          select new Group<Item>(c.Key, c, new SolidColorBrush(Colors.White));
            itemList = groupBy.ToObservableCollection();
            this.dayListSelector.ItemsSource = itemList;
        }

        /**
         * Creates the live application title that will contain the reminder notifications
         */
        public void CreateApplicationTile()
        {
            var appTile = ShellTile.ActiveTiles.First();

            if (appTile != null)
            {
                // all the back properties are set to empty strings -> this hides the back of the title
                var standardTile = new StandardTileData
                {
                    Title = "Remainders",
                    BackgroundImage = new Uri("Images/SecondaryTileFrontIcon.jpg", UriKind.Relative),  
                    Count = 13,
                    BackTitle = "",
                    BackBackgroundImage = new Uri("", UriKind.Relative),
                    BackContent = ""
                };

                appTile.Update(standardTile);
            }
        }  

        /**
         * Starts a periodic task managed by the system that updates the live title when new
         * reminders appear.
         */
        private void StartPeriodicAgent()
        {
            // Obtain a reference to the period task, if one exists
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                RemoveAgent(periodicTaskName);
            }

            periodicTask = new PeriodicTask(periodicTaskName);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            periodicTask.Description = "This updates the SimpleReminder live title.";

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);

                ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(2));
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                }

                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    MessageBox.Show("The maximum number of ScheduledActions of this type have already been added.");
                }
            }
            catch (SchedulerServiceException)
            {
                // No user action required.
            }
        }

        /**
         * Removes the periodic task.
         */
        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}