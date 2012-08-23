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

namespace SimpleReminder
{
    static class Constants
    {
        public const double ONCE_STACKPANEL_HEIGHT = 70;
        public const double INTERVAL_STACKPANEL_HEIGHT = 150;
    }

    public partial class NewEntryPage : PhoneApplicationPage
    {
        private Entry newEntry;
        // we ignore the first check event
        private bool radioButtonClicked = false;

        public Entry NewEntry
        {
            get
            {
                return newEntry;
            }
        }

        public NewEntryPage()
        {
            InitializeComponent();

            descriptionTextBox.Text = "";
        }

        #region User actions

        private void SaveApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CancelApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        #endregion

        private void onceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true && radioButtonClicked)
            {
                onceDatePicker.IsEnabled = true;
                CreateFadeInOutAnimation(onceStackPanel, 0.0, 1.0, Constants.ONCE_STACKPANEL_HEIGHT).Begin();
                intervalStartDatePicker.IsEnabled = false;
                intervalEndDatePicker.IsEnabled = false;
                CreateFadeInOutAnimation(intervalStackPanel, 1.0, 0.0, Constants.INTERVAL_STACKPANEL_HEIGHT).Begin();
            }
        }

        private void intervalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonClicked = true;
            if ((sender as RadioButton).IsChecked == true)
            {
                intervalStartDatePicker.IsEnabled = true;
                intervalEndDatePicker.IsEnabled = true;
                CreateFadeInOutAnimation(intervalStackPanel, 0.0, 1.0, Constants.INTERVAL_STACKPANEL_HEIGHT).Begin();
                onceDatePicker.IsEnabled = false;
                CreateFadeInOutAnimation(onceStackPanel, 1.0, 0.0, Constants.ONCE_STACKPANEL_HEIGHT).Begin();
            }
        }

        #region Animations

        /**
         * If from is less than two, we fade in the target and increase it's height from 0 to targetRealHeight
         * If from is greater that two, we fade out the target and decrease it's height from targetRealHeight to 0
         */
        private Storyboard CreateFadeInOutAnimation(UIElement target, double from, double to, double targetRealHeight)
        {
            // fade animation
            Storyboard sb = new Storyboard();
            DoubleAnimation fadeInOutAnimation = new DoubleAnimation();
            fadeInOutAnimation.From = from;
            fadeInOutAnimation.To = to;
            fadeInOutAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            Storyboard.SetTarget(fadeInOutAnimation, target);
            Storyboard.SetTargetProperty(fadeInOutAnimation, new PropertyPath("Opacity"));

            sb.Children.Add(fadeInOutAnimation);

            // height animation
            double heightFrom = 0;
            double heightTo = 0;
            // fade in animation
            if (from < to)
            {
                heightFrom = 0;
                heightTo = targetRealHeight;    
            }
            // fade out animation
            else
            {
                heightFrom = targetRealHeight;
                heightTo = 0;
            }
            DoubleAnimation modifyHeightAnimation = new DoubleAnimation();
            modifyHeightAnimation.From = heightFrom;
            modifyHeightAnimation.To = heightTo;
            modifyHeightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            Storyboard.SetTarget(modifyHeightAnimation, target);
            Storyboard.SetTargetProperty(modifyHeightAnimation, new PropertyPath("Height"));

            sb.Children.Add(modifyHeightAnimation);

            return sb;
        }

        #endregion
    }
}