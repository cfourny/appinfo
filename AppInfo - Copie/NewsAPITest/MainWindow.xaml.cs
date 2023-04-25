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


namespace NewsAPITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        class NewsResponse
        {
            public string status { get; set; }
            public int TotalResults { get; set; }
            public List<Article> Articles { get; set; }
        }
        class Article
        {
            public string Title { get; set; }
            public Uri urlToImage { get; set; }
            public Uri Url { get; set; }
        }
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
        }

        HttpClient client = new HttpClient();


        //Fonction de recherche des articles
        private async Task GetArticles()
        {
            //_______________________ETAPE 1___________________________
            //Code de création du lien de recherche et application des filtres
            //Lien vierge
            string link = "$https://newsapi.org/v2/top-headlines?";

            //Filtres
            int co = 0;
            int ca = 0;

            //Filtre du pays 
            var index = (ComboBoxItem)CountryFilter.SelectedValue;
            var Country = (string)index.Content;
            if (Country != "Tous les pays")
            {
                link = link + "country=" + Country;
                co++;
            }

            //Filtre de la catégorie
            index = (ComboBoxItem)CategoryFilter.SelectedValue;
            var Category = (string)index.Content;
            if (Category != "Toutes catégories")
            {
                if (co == 1)
                {
                    link = link + "&";
                }
                link = link + "category=" + Category;
                ca++;
            }

            //Filtre du mot-clef
            string Keyword = KeywordBox.Text;
            if (Keyword != "")
            {
                if (co == 1 || ca == 1)
                {
                    link = link + "&";
                }
                link = link + "q=" + Keyword;
            }
            //Ajout de la clef API à la fin du lien
            link = link + "&apiKey=b154243766fe463a870105f1213a9c4a";

            //_______________________ETAPE 2___________________________
            Confirm.Text = "Recherche...";
            //Recherche des articles avec l'api et les filtres
            string response = await client.GetStringAsync(link);

            /*
            // Boucle qui relance la recherche en cas d'échec, jusqu'a 5 fois
            int i = 0;
            do
            {
                response = await client.GetStringAsync(
                    $"https://newsapi.org/v2/top-headlines?country=us&apiKey=b154243766fe463a870105f1213a9c4a");
                i++;
            }
            while (NewsResponse.status != "ok" || i > 5);

            if(i == 5)
            {
                Confirm.Text = "Échec de la recherche.";
            }
            */

            Confirm.Text = "Traitement...";

            //Traitement des articles trouvés
            NewsResponse newsObject = JsonConvert.DeserializeObject<NewsResponse>(response);


            Confirm.Text = "Affichage...";

            //Ajout du résultat de la recherche dans une nouvelle ligne

            foreach (var article in newsObject.Articles)
            {
                //Création de la ligne
                RowDefinition DynRow = new RowDefinition();
                DynRow.Height = new GridLength(30);
                GridNews.RowDefinitions.Add(DynRow);

                TextBlock txtb = new TextBlock();
                Hyperlink hlink = new Hyperlink();
                //Création du lien portant le titre de l'article, renvoyant l'URL de
                //l'article lorsque l'ont clique dessus
                hlink.SetValue(Grid.RowProperty, 2);
                hlink.NavigateUri = article.Url;
                hlink.Inlines.Add(article.Title);
                hlink.RequestNavigate += Hyperlink_RequestNavigate;
                txtb.Width = 800;
                txtb.TextAlignment = TextAlignment.Left;
                txtb.SetValue(Grid.RowProperty, 2);
                txtb.Inlines.Add(hlink);

                /*
                TextBlock txtb = new TextBlock();
                txtb.Width = 800;
                txtb.TextAlignment = TextAlignment.Left;
                txtb.SetValue(Grid.RowProperty, 2);
                txtb.Text = article.Title;
                */

                GridNews.Children.Add(txtb);
                Grid.SetRow(txtb, GridNews.RowDefinitions.Count - 1);
            }

            Confirm.Text = "Terminé!";
        }

        //Lancement de la recherche lors de l'appui sur le boutton
        private void getnews(object sender, RoutedEventArgs e)
        {
            _ = GetArticles();
        }
    }

}
