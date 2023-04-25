using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Net.Http;

using System.Windows.Controls;
using Newtonsoft.Json;

namespace Daily_News
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<NewsArticle> Articles { get; set; }
        private const string BingNewsApiKey = "37fa50eba0eb4b42962351b8b0328de8"; // Replace with your Bing News API key
        private const string BingNewsApiUrl = "https://api.bing.microsoft.com/v7.0/news/search";


        public MainWindow()
        {
            InitializeComponent();
            Articles = new ObservableCollection<NewsArticle>();
            ListViewArticles.ItemsSource = Articles;
        }

        private async void ButtonLoadArticles_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchTextBox.Text;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingNewsApiKey);
                string apiUrl = $"{BingNewsApiUrl}?q={query}&count=10"; // Set count to control the number of news articles to fetch

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    BingNewsApiResponse newsApiResponse = JsonConvert.DeserializeObject<BingNewsApiResponse>(responseContent);

                    ListViewArticles.ItemsSource = newsApiResponse.Value;
                }
                else
                {
                    MessageBox.Show("Failed to fetch news articles. Please try again later.");
                }
            }



        }
        private void NewsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NewsArticle selectedArticle = ListViewArticles.SelectedItem as NewsArticle;
            if (selectedArticle != null)
            {
                MessageBox.Show($"Title: {selectedArticle.Name}\n\nDescription: {selectedArticle.Description}\n\nURL: {selectedArticle.Url}\n\nDate Published: {selectedArticle.DatePublished}\n\nProvider: {selectedArticle.Provider?.FirstOrDefault()?.Name}");
            }
        }
    }
}
