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
        public const double DATE_PICKER_HEIGHT = 70;
    }

    public partial class NewEntryPage : PhoneApplicationPage
    {
        private Entry newEntry;

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
       //     onceRadioButton.IsChecked = true;
            intervalStartDatePicker.Opacity = 0.0;
            intervalStartDatePicker.Height = 0.0;
            intervalEndDatePicker.Opacity = 0.0;
            intervalEndDatePicker.Height = 0.0;
            onceDatePicker.Opacity = 0.0;
            onceDatePicker.Height = 0.0;
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
            if ((sender as RadioButton).IsChecked == true)
            {
                CreateFadeInOutAnimation(onceDatePicker, 0.0, 1.0).Begin();
                CreateFadeInOutAnimation(intervalStartDatePicker, 1.0, 0.0).Begin();
                CreateFadeInOutAnimation(intervalEndDatePicker, 1.0, 0.0).Begin();
            }
        }

        private void intervalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)
            {
                CreateFadeInOutAnimation(intervalStartDatePicker, 0.0, 1.0).Begin();
                CreateFadeInOutAnimation(intervalEndDatePicker, 0.0, 1.0).Begin();
                CreateFadeInOutAnimation(onceDatePicker, 1.0, 0.0).Begin();
            }
        }

        #region Animations

        private Storyboard CreateFadeInOutAnimation(UIElement target, double from, double to)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation fadeInOutAnimation = new DoubleAnimation();
            fadeInOutAnimation.From = from;
            fadeInOutAnimation.To = to;
            fadeInOutAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            Storyboard.SetTarget(fadeInOutAnimation, target);
            Storyboard.SetTargetProperty(fadeInOutAnimation, new PropertyPath("Opacity"));

            sb.Children.Add(fadeInOutAnimation);

            // fade in animation
            if (from < to)
            {
                DoubleAnimation modifyHeightAnimation = new DoubleAnimation();
                modifyHeightAnimation.From = 0;
                modifyHeightAnimation.To = Constants.DATE_PICKER_HEIGHT;
                modifyHeightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                Storyboard.SetTarget(modifyHeightAnimation, target);
                Storyboard.SetTargetProperty(modifyHeightAnimation, new PropertyPath("Height"));

                sb.Children.Add(modifyHeightAnimation);

                // we need to move all the children bellow the target downwards
                int targetIndex = (mainScrollViewer.Content as Grid).Children.IndexOf(target);
                for (int i = targetIndex + 1; i < (mainScrollViewer.Content as Grid).Children.Count; i++)
                {
                    FrameworkElement child = (mainScrollViewer.Content as Grid).Children[i] as FrameworkElement;
                    TranslateTransform trans = new TranslateTransform();
                    child.RenderTransform = trans;

                    DoubleAnimation moveDownAnimation = new DoubleAnimation();
                    moveDownAnimation.From = 0;
                    moveDownAnimation.To = Constants.DATE_PICKER_HEIGHT;
                    moveDownAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                    Storyboard.SetTarget(moveDownAnimation, child);
                    Storyboard.SetTargetProperty(moveDownAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                    sb.Children.Add(moveDownAnimation);
                }
            }
            // fade out animation
            else
            {
                DoubleAnimation modifyHeightAnimation = new DoubleAnimation();
                modifyHeightAnimation.From = Constants.DATE_PICKER_HEIGHT;
                modifyHeightAnimation.To = 0;
                modifyHeightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                Storyboard.SetTarget(modifyHeightAnimation, target);
                Storyboard.SetTargetProperty(modifyHeightAnimation, new PropertyPath("Height"));

                sb.Children.Add(modifyHeightAnimation);

                // we need to move all the children bellow the target upwards
                int targetIndex = (mainScrollViewer.Content as Grid).Children.IndexOf(target);
                for (int i = targetIndex + 1; i < (mainScrollViewer.Content as Grid).Children.Count; i++)
                {
                    FrameworkElement child = (mainScrollViewer.Content as Grid).Children[i] as FrameworkElement;
                    TranslateTransform trans = new TranslateTransform();
                    child.RenderTransform = trans;

                    DoubleAnimation moveUpAnimation = new DoubleAnimation();
                    moveUpAnimation.From = 0;
                    moveUpAnimation.To = -Constants.DATE_PICKER_HEIGHT;
                    moveUpAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

                    Storyboard.SetTarget(moveUpAnimation, child);
                    Storyboard.SetTargetProperty(moveUpAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

                    sb.Children.Add(moveUpAnimation);
                }
            }

            return sb;
        }

        #endregion
    }
}