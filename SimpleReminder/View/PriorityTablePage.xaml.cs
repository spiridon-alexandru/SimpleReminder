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

namespace SimpleReminder.View
{
    public enum TablePosition
    {
        FirstQuadrant,
        SecondQuadrant,
        ThirdQuadrant,
        FourthQuadrant
    }

    public partial class PriorityTablePage : PhoneApplicationPage
    {
        TablePosition currentPosition;

        public PriorityTablePage()
        {
            InitializeComponent();

            currentPosition = TablePosition.FirstQuadrant;

            listBox1.Items.Add("item1");
            listBox1.Items.Add("item2");
            listBox1.Items.Add("item3");
            listBox1.Items.Add("item4");
            listBox1.Items.Add("item5");

            listBox2.Items.Add("item1");
            listBox2.Items.Add("item2");
            listBox2.Items.Add("item3");
            listBox2.Items.Add("item4");
            listBox2.Items.Add("item5");

            listBox3.Items.Add("item1");
            listBox3.Items.Add("item2");
            listBox3.Items.Add("item3");
            listBox3.Items.Add("item4");
            listBox3.Items.Add("item5");

            listBox4.Items.Add("item1");
            listBox4.Items.Add("item2");
            listBox4.Items.Add("item3");
            listBox4.Items.Add("item4");
            listBox4.Items.Add("item5");
        }

        private void OnVerticalFlick(object sender, FlickGestureEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (e.Direction == System.Windows.Controls.Orientation.Vertical)
            {
                da.From = gridTransform.Y;
                // flick up
                if (e.VerticalVelocity < 0 && (currentPosition == TablePosition.FirstQuadrant || currentPosition == TablePosition.SecondQuadrant))
                {
                    da.To = gridTransform.Y - ContentPanel.Height / 2;
                    if (currentPosition == TablePosition.FirstQuadrant)
                    {
                        currentPosition = TablePosition.ThirdQuadrant;
                    }
                    else
                    {
                        currentPosition = TablePosition.FourthQuadrant;
                    }  
                }
                // flick down
                else if (e.VerticalVelocity > 0 && (currentPosition == TablePosition.ThirdQuadrant || currentPosition == TablePosition.FourthQuadrant))
                {
                    da.To = gridTransform.Y + ContentPanel.Height / 2;
                    if (currentPosition == TablePosition.ThirdQuadrant)
                    {
                        currentPosition = TablePosition.FirstQuadrant;
                    }
                    else
                    {
                        currentPosition = TablePosition.SecondQuadrant;
                    } 
                }
                Storyboard.SetTargetProperty(da, new PropertyPath(TranslateTransform.YProperty));
            }
            da.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            Storyboard.SetTarget(da, gridTransform);

            Storyboard sb1 = new Storyboard();
            if (da.To != null)
            {
                sb1.Children.Add(da);
                sb1.Begin();
            }
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (e.Direction == System.Windows.Controls.Orientation.Horizontal)
            {
                da.From = gridTransform.X;
                // flick right
                if (e.HorizontalVelocity < 0 && (currentPosition == TablePosition.FirstQuadrant || currentPosition == TablePosition.ThirdQuadrant))
                {   
                    da.To = gridTransform.X - ContentPanel.Width/2;
                    if (currentPosition == TablePosition.FirstQuadrant)
                    {
                        currentPosition = TablePosition.SecondQuadrant;
                    }
                    else
                    {
                        currentPosition = TablePosition.FourthQuadrant;
                    }   
                }
                // flick left
                else if (e.HorizontalVelocity > 0 && (currentPosition == TablePosition.SecondQuadrant || currentPosition == TablePosition.FourthQuadrant))
                {
                    da.To = gridTransform.X + ContentPanel.Width/2;
                    if (currentPosition == TablePosition.SecondQuadrant)
                    {
                        currentPosition = TablePosition.FirstQuadrant;
                    }
                    else
                    {
                        currentPosition = TablePosition.ThirdQuadrant;
                    }   
                }
                Storyboard.SetTargetProperty(da, new PropertyPath(TranslateTransform.XProperty));
            }
            da.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            Storyboard.SetTarget(da, gridTransform);

            Storyboard sb1 = new Storyboard();
            if (da.To != null)
            {
                sb1.Children.Add(da);
                sb1.Begin();
            }
        }
    }
}