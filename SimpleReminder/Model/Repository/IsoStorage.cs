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
using System.IO.IsolatedStorage;
using System.IO;

namespace SimpleReminder.Model.Repository
{

    public class IsoStorage : IStorage
    {
        IsolatedStorageFile isoStore;
        string fileName;

        StreamReader reader = null;
        StreamWriter writer = null;

        public IsoStorage()
        {
            fileName = "Entries.txt";
        }

        private void InitIsoStoreObject()
        {
            // Get an isolated store for this assembly and put it into an
            // IsolatedStoreFile object.
            isoStore = IsolatedStorageFile.GetUserStoreForApplication();
        }

        #region Data reading

        public List<Entry> GetEntries()
        {
            List<Entry> entries = new List<Entry>();

            InitIsoStoreObject();
            InitStreamReader();

            // read data
            /* every entry will occupy 3 lines inside the file as following:
             *
             * First line: description
             * Second line: 7 digit string in base two; ex: 0100100 (this means that we should be reminded
             * 'marti' and 'vineri'
             * Third line: DateTime objects separated by spaces
             */
            string line;
            while (!reader.EndOfStream)
            {
                // add the description
                Entry newEntry = new Entry();
                line = reader.ReadLine();
                newEntry.Description = line;

                // add the days
                line = reader.ReadLine();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '1')
                    {
                        newEntry.addDay(i);
                    }
                }

                // add the hours
                line = reader.ReadLine();
                char[] separators = { ' ' };
                string [] hours = line.Split(separators);
                foreach (string hour in hours)
                {
                    DateTime newHour;
                    if (DateTime.TryParse(hour, out newHour))
                    {
                        newEntry.addHour(newHour);
                    }
                }

                // add the entry
                entries.Add(newEntry);
            }

            reader.Close();

            return entries;
        }

        private void InitStreamReader()
        {
            // Assign the reader to the store and the file.
            reader = new StreamReader(new IsolatedStorageFileStream(
                fileName, FileMode.Open, isoStore));
        }

        #endregion

        #region Data writing

        public void WriteEntries(List<Entry> entries)
        {
            // we delete the file in order to create it again and fill it up with the new data
            isoStore.DeleteFile(fileName);

            InitIsoStoreObject();
            InitStreamWriter();

            // write data
            /* every entry will occupy 3 lines inside the file as following:
             *
             * First line: description
             * Second line: 7 digit string in base two; ex: 0100100 (this means that we should be reminded
             * 'marti' and 'vineri'
             * Third line: DateTime objects separated by spaces
             */
            foreach (Entry e in entries)
            {
                writer.WriteLine(e.Description);
                writer.WriteLine(e.getDays());
                writer.WriteLine(e.getHours());
            }

            writer.Close();
        }

        private void InitStreamWriter()
        {
            // Assign the writer to the store and the file.
            writer = new StreamWriter(new IsolatedStorageFileStream(
                fileName, FileMode.OpenOrCreate, isoStore));
        }

        #endregion
    }
}
