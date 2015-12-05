using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using StockExchangeSystem.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StockExchangeSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StockInfo : Page
    {
        public StockInfo()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += (sender, args) =>
            {
                Frame.Navigate(typeof(MainPage));
                args.Handled = true;
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var stockArg = e.Parameter as Stock;
            if (stockArg == null) throw new ArgumentNullException(nameof(stockArg));
            this.StockName.Text = stockArg.Name ?? "";
            this.Stock = stockArg;

            string url = "https://cmovstocksystem.herokuapp.com/stock/currentPrice?symbol=" + Stock.Symbol;
            Uri uri = new Uri(url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string answer = await response.Content.ReadAsStringAsync();
                JsonObject json = JsonObject.Parse(answer);
                var price = json.GetNamedNumber("lastTradePriceOnly");
                this.StockName.Text = Stock.Name + " - " + price + " $";
            }


        }

        public Stock Stock { get; set; }

        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
