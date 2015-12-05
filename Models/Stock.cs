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
            getStockName(symbol);
        }

        public Stock(string symbol, string name, string upper, string lower)
        {
            this.Symbol = symbol;
            this.name = name;
            this.upper = upper;
            this.lower = lower;
        }

        private async void getStockName(string symbol)
        {

            string url = "https://cmovstocksystem.herokuapp.com/stock/add?user=1&symbol="+symbol+"&lower=0&upper=300";
            Uri uri = new Uri(url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string answer = await response.Content.ReadAsStringAsync();
                JsonObject json = JsonObject.Parse(answer);
                this.Name = json.GetNamedString("name");
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {

            }
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
