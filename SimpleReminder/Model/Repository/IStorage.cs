using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleReminder.Model.Repository
{
    interface IStorage
    {
        List<Entry> GetEntries();
        void WriteEntries(List<Entry> entries);
    }
}
