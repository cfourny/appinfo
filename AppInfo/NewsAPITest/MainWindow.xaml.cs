using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Daily_News;
using System.Numerics;

namespace NewsAPITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<NewsArticle> Articles { get; set; }
        private const string BingNewsApiKey = "37fa50eba0eb4b42962351b8b0328de8";
        private const string BingNewsApiUrl = "https://api.bing.microsoft.com/v7.0/news/search";
        public MainWindow()
        {
            InitializeComponent();
            Articles = new ObservableCollection<NewsArticle>();
        }
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        //Lancement de la recherche lors de l'appui sur le boutton
        private async void getnews(object sender, RoutedEventArgs e)
        {
            GridNews.Children.Clear();
            GridNews.RowDefinitions.Clear();
            Confirm.Text = "Lancement...";
            string query = KeywordBox.Text;
            var index = (ComboBoxItem)CountryFilter.SelectedValue;
            var Country = (string)index.Content;
            index = (ComboBoxItem)CategoryFilter.SelectedValue;
            var Category = (string)index.Content;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingNewsApiKey);
                string apiUrl = $"{BingNewsApiUrl}?q={query}&cc={Country}&category={Category}&count=25";
                Confirm.Text = "Recherche...";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    Confirm.Text = "Affichage...";
                    string responseContent = await response.Content.ReadAsStringAsync();
                    displaynews(responseContent);
                }
                else
                {
                    Confirm.Text = "Erreur!";
                }
            }

        }

        //Fonction de recherche et d'affichage des articles
        private void displaynews(String a)
        {
            BingNewsApiResponse newsApiResponse = JsonConvert.DeserializeObject<BingNewsApiResponse>(a);
            //var responsearticles = newsApiResponse.Daily_News;

            foreach (var article in newsApiResponse.Value)
            {
                //Création de la ligne
                RowDefinition DynRow = new RowDefinition();
                DynRow.Height = new GridLength(200);
                GridNews.RowDefinitions.Add(DynRow);

                //Création du titre, de l'image et du lien de l'article
                Image thmbnl = new Image();
                thmbnl.SetValue(Grid.RowProperty, 2);
                thmbnl.SetValue(Grid.ColumnProperty, 0);
                BitmapImage Blnk = new BitmapImage();
                Blnk.BeginInit();
                Blnk.UriSource = new Uri("blank.png", UriKind.Relative);
                Blnk.EndInit();
                thmbnl.Source = Blnk;
                if (article.Image != null)
                {
                    //thmbnl.Source = new BitmapImage(new Uri("blank.png", UriKind.Relative));
                    thmbnl.Source = new BitmapImage(new Uri(article.Image.Thumbnail.ContentUrl));
                }
                /*
                else
                {
                    //thmbnl.Source = new BitmapImage(new Uri(@"NewsAPITest\blank.png", UriKind.Relative));
                };
                */
                //thmbnl.Width = 200;
                thmbnl.Height = 200;
                GridNews.Children.Add(thmbnl);

                StackPanel StckPnl = new StackPanel();
                StckPnl.SetValue(Grid.RowProperty, 2);
                StckPnl.SetValue(Grid.ColumnProperty, 1);
                StckPnl.Orientation = Orientation.Vertical;

                TextBlock txtb = new TextBlock();
                txtb.VerticalAlignment = VerticalAlignment.Top;
                txtb.TextWrapping = TextWrapping.Wrap;
                txtb.FontSize = 20;
                txtb.Text = article.Name;
                StckPnl.Children.Add(txtb);

                TextBlock tlink = new TextBlock();
                Hyperlink hlink = new Hyperlink()
                {
                    NavigateUri = new Uri(article.Url)
                };
                tlink.Inlines.Clear();
                hlink.Inlines.Add(article.Url);
                hlink.RequestNavigate += Hyperlink_RequestNavigate;
                tlink.Inlines.Add(hlink);
                StckPnl.Children.Add(tlink);

                TextBlock tsrc = new TextBlock();
                tsrc.Text = "DailyNews";
                tsrc.Text = article.Provider.First().Name;
                tsrc.VerticalAlignment = VerticalAlignment.Bottom;
                StckPnl.Children.Add(tsrc);
                GridNews.Children.Add(StckPnl);
                Grid.SetRow(StckPnl, GridNews.RowDefinitions.Count - 1);

            }
            Confirm.Text = "Terminé !";

        }

        private void Hlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
