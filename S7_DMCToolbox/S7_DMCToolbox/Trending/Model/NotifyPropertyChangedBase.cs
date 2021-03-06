﻿using System.ComponentModel;

namespace Trending
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;  //Propertry changed is the event

        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
