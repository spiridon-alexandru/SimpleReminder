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
using System.Collections.Generic;

namespace SimpleReminder.Model
{
    public enum EntryPriority
    {
        ImportantUrgent,
        ImportantNotUrgent,
        NotImportantUrgent,
        NotImportantNotUrgent
    }

    public enum RoWeekDays
    {
        Luni,
        Marti,
        Miercuri,
        Joi,
        Vineri,
        Sambata,
        Duminica
    }

    public class Entry
    {
        private int [] days;
        private List<DateTime> alertHours;
        private EntryPriority priority;

        #region Constructors

        public Entry()
        {
            days = new int[7];
            alertHours = new List<DateTime>();
            priority = EntryPriority.ImportantUrgent;
        }

        public Entry(string description)
        {
            this.Description = description;
            days = new int[7];
            for (int i = 0; i < 7; i++)
            {
                days[i] = 0;
            }
            alertHours = new List<DateTime>();
        }

        #endregion

        #region Properties and helper methods

        public string Description
        {
            get;
            set;
        }

        public void addDay(string day)
        {
            string lowerDay = day.ToLower();
            switch (day)
            {
                case "luni":
                    days[0] = 1;
                    break;
                case "marti":
                    days[1] = 1;
                    break;
                case "miercuri":
                    days[2] = 1;
                    break;
                case "joi":
                    days[3] = 1;
                    break;
                case "vineri":
                    days[4] = 1;
                    break;
                case "sambata":
                    days[5] = 1;
                    break;
                case "duminica":
                    days[6] = 1;
                    break;
                default:
                    break;
            }
        }

        public void addDay(int dayIndex)
        {
            days[dayIndex] = 1;
        }

        public string getDays()
        {
            string s = "";
            for (int i = 0; i < 7; i++)
            {
                if (days[i] == 1)
                {
                    s += '1';
                }
                else
                {
                    s += '0';
                }
            }

            return s;
        }

        public void addHour(DateTime hour)
        {
            if (!alertHours.Contains(hour))
            {
                alertHours.Add(hour);
            }
        }

        public string getHours()
        {
            string h = "";
            foreach (DateTime dt in alertHours)
            {
                h += dt.ToString();
                h += " ";
            }
            // remove the last space
            h.Remove(h.Length - 1, 1);

            return h;
        }

        #endregion
    }
}
