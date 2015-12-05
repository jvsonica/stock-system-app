using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using Windows.Data.Json;

namespace StockExchangeSystem.Models
{
    public class Stock : INotifyPropertyChanged
    {
        private String symbol;
        private String name;
        private String upper;
        private String lower;

        public event PropertyChangedEventHandler PropertyChanged;

        public Stock(string symbol)
        {
            this.Symbol = symbol;
        }

        public Stock(string symbol, string name, string upper, string lower)
        {
            this.Symbol = symbol;
            this.name = name;
            this.upper = upper;
            this.lower = lower;
        }

        public string Symbol
        {
            get
            {
                return this.symbol;
            }
            set
            {
                this.symbol = value;
                NotifyPropertyChanged("Symbol");
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                NotifyPropertyChanged("Name");
            }
        }


        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
