using StockExchangeSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Networking.PushNotifications;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Net.Http;
using System.Net;
using Windows.Data.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace StockExchangeSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Stock> stocks;
        private PushNotificationChannel channel;
        private int userID;


        public MainPage()
        {
            this.InitializeComponent();

            // Initialize Stocks
            stocks = new ObservableCollection<Stock>();
            stocksList.ItemsSource = stocks;

            // Initialize Acccount
            loginOrRegister();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void loginOrRegister()
        {
            try
            {
                this.channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

                var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("uri", this.channel.Uri)
                    };

                string url = "https://cmovstocksystem.herokuapp.com/login";
                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(uri, new FormUrlEncodedContent(values));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string answer = await response.Content.ReadAsStringAsync();

                    JsonObject json = JsonObject.Parse(answer);
                    this.userID = (int) json.GetNamedNumber("id");
                    JsonArray arr = json.GetNamedArray("stocks");
                    
                    // If there is any stock already associated, show it on the interface
                    if (arr != null)
                    {
                        foreach (var item in arr)
                        {
                            JsonObject stock = JsonObject.Parse(item.Stringify());
                            string symbol = stock.GetNamedString("symbol");
                            string name = stock.GetNamedString("name");
                            string upper = stock.GetNamedNumber("upper").ToString();
                            string lower = stock.GetNamedNumber("lower").ToString();
                            stocks.Add(new Stock(symbol, name, upper,lower));
                        }
                    }
                    
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Something went wrong with the request

                }
            }
            catch (Exception e)
            {
                // Oops error somewhere
            }

        }

        private async void addStockToUser(string symbol, int id)
        {
            string url = "https://cmovstocksystem.herokuapp.com/stock/add?user=" + id + "&symbol=" + symbol + "&lower=0&upper=300";
            Uri uri = new Uri(url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string answer = await response.Content.ReadAsStringAsync();
                JsonObject json = JsonObject.Parse(answer);
                if(json.GetNamedString("name") != null){
                    string name = json.GetNamedString("name");
                    string upper = json.GetNamedNumber("upper").ToString();
                    string lower = json.GetNamedNumber("lower").ToString();
                    stocks.Add(new Stock(symbol, name, upper, lower));
                }
                
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Request went wrong
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            addStockToUser(tbox1.Text.ToString(), userID);
            tbox1.Text = "";

        }

        private void btn_more_info_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
