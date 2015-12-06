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
        private double upper;
        private double lower;
        private bool notify;
        private int id;

        public event PropertyChangedEventHandler PropertyChanged;

        public Stock(string symbol)
        {
            this.Symbol = symbol;
        }

        public Stock(string symbol, string name, string upper, string lower,string notify,string id)
        {
            this.Symbol = symbol;
            this.name = name;
            this.upper = Double.Parse(upper);
            this.lower = Double.Parse(lower);
            this.notify= Boolean.Parse(notify);
            this.id    = Int32.Parse(id);
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

        public bool Notify
        {
            get
            {
                return notify;
            }
            set { this.notify = value; }
        }

        public double Upper
        {
            get { return upper; }
            set { upper = value; }
        }

        public double Lower
        {
            get { return lower; }
            set { lower = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
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
