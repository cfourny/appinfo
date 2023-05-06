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
using MySql.Data.MySqlClient;
using System.Collections;

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
        private async void ArchiveClick(object sender, RoutedEventArgs e)
        {
            Archive archive = new Archive();
            archive.Show();
            Hide();
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

            foreach (var article in newsApiResponse.Value)
            {
                //Création de la ligne
                RowDefinition DynRow = new RowDefinition();
                DynRow.Height = new GridLength(200);
                GridNews.RowDefinitions.Add(DynRow);

                //Création du titre, de l'image et du lien de l'article
                Border BorderLeft = new Border();
                BorderLeft.BorderBrush = Brushes.Black;
                BorderLeft.BorderThickness = new Thickness(0, 0, 0, 1);
                Image thmbnl = new Image();
                thmbnl.SetValue(Grid.RowProperty, 2);
                thmbnl.SetValue(Grid.ColumnProperty, 0);
                thmbnl.Height = 200;
                if (article.Image != null)
                {
                    thmbnl.Source = new BitmapImage(new Uri(article.Image.Thumbnail.ContentUrl));
                }
                else
                {
                    thmbnl.Source = new BitmapImage(new Uri("https://hearhear.org/wp-content/uploads/2019/09/no-image-icon-300x300.png"));
                };

                BorderLeft.Child = thmbnl;
                GridNews.Children.Add(BorderLeft);
                Grid.SetRow(BorderLeft, GridNews.RowDefinitions.Count - 1);

                StackPanel StckPnl = new StackPanel();
                StckPnl.SetValue(Grid.RowProperty, 2);
                StckPnl.SetValue(Grid.ColumnProperty, 1);
                StckPnl.Orientation = Orientation.Vertical;
                StckPnl.VerticalAlignment = VerticalAlignment.Top;

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

                StackPanel StckPnlBttm = new StackPanel();
                StckPnlBttm.SetValue(Grid.RowProperty, 2);
                StckPnlBttm.SetValue(Grid.ColumnProperty, 1);
                StckPnlBttm.Orientation = Orientation.Horizontal;
                StckPnlBttm.VerticalAlignment = VerticalAlignment.Bottom;
                StckPnlBttm.HorizontalAlignment = HorizontalAlignment.Center;
                Border BorderRight = new Border();
                BorderRight.SetValue(Grid.RowProperty, 2);
                BorderRight.SetValue(Grid.ColumnProperty, 1);
                BorderRight.BorderBrush = Brushes.Black;
                BorderRight.BorderThickness = new Thickness(0, 0, 0, 1);

                TextBlock tsrc = new TextBlock();
                tsrc.Text = article.Provider.First().Name;
                tsrc.VerticalAlignment = VerticalAlignment.Bottom;
                StckPnlBttm.Children.Add(tsrc);

                //Ajout du boutton d'archivage
                Button BtnArchv = new Button();
                BtnArchv.Width = 100;
                BtnArchv.Height = 25;
                BtnArchv.Click += new RoutedEventHandler(Archivate);
                BtnArchv.Content = "Archiver";

                TextBlock telem = new TextBlock();
                telem.Width = 0;
                telem.Height = 0;
                telem.Text = article.Url;
                StckPnlBttm.Children.Add(telem);

                StckPnlBttm.Children.Add(BtnArchv);
                BorderRight.Child = StckPnlBttm;

                GridNews.Children.Add(StckPnl);
                GridNews.Children.Add(BorderRight);
                Grid.SetRow(StckPnl, GridNews.RowDefinitions.Count - 1);
                Grid.SetRow(BorderRight, GridNews.RowDefinitions.Count - 1);
            }
            Confirm.Text = "Terminé !";

        }
        //Fonction de récupération des éléments constituant l'article
        //dans la ligne correspondante à l'article désigné
        public static string[] RecoverArticle(Object a)
        {
            Button b = (Button)a;
            StackPanel StckPnlBttm = (StackPanel)b.Parent;
            TextBlock tsrc = (TextBlock)VisualTreeHelper.GetChild(StckPnlBttm, 0);
            var source = tsrc.Text;
            TextBlock telem = (TextBlock)VisualTreeHelper.GetChild(StckPnlBttm, 1);
            var url = telem.Text;

            Border BorderRight = (Border)StckPnlBttm.Parent;
            Grid GridNews = (Grid)BorderRight.Parent;
            int row = Grid.GetRow(BorderRight);
            var RowElements = GridNews.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == 0);
            Border BorderLeft = (Border)GridNews.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == 0);
            Image thmbnl = (Image)VisualTreeHelper.GetChild(BorderLeft, 0);
            var urlPicture = thmbnl.Source;

            StackPanel StckPnl = (StackPanel)GridNews.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == 1);
            TextBlock txtb = (TextBlock)VisualTreeHelper.GetChild(StckPnl, 0);
            var title = txtb.Text;
            string[] ArticleElements = { title, source, url.ToString(), urlPicture.ToString() };
            return ArticleElements;
        }

        //Fonction d'insertion des éléments de l'article dans la base de donnée
        private void InsertIntoDB(string[] a)
        {
            string title = a[0];
            string source = a[1];
            string url = a[2];
            string urlPicture = a[3];
            try
            {
                string connectionString = "server=localhost; database=appinfo; uid=root; pwd=mysql";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT MAX(id) FROM archive", connection);
                ArrayList valuesList = new ArrayList();
                connection.Open();
                MySqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    valuesList.Add(Convert.ToInt32(dataReader[0].ToString()));
                }
                dataReader.Close();
                var index = Int32.Parse(valuesList[0].ToString());
                cmd = new MySqlCommand(
                    "INSERT INTO `archive` (`id`,`title`,`source`,`url`,`urlPicture`) VALUES(" + (index + 1) + "," +
                    "N'" + title + "',N'" + source + "',N'" + url + "',N'" + urlPicture + "');", connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Article enregistré!");
                connection.Close();

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Archivate(Object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string[] a = RecoverArticle(b);
            InsertIntoDB(a);
        }

        private void Hlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
