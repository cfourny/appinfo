using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;

namespace NewsAPITest
{
    /// <summary>
    /// Interaction logic for Archive.xaml
    /// </summary>
    public partial class Archive : Window
    {
        public Archive()
        {
            InitializeComponent();
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "server=localhost; database=appinfo; uid=root; pwd=mysql";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM archive", connection);
                connection.Open();

                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                dataGridArchive.DataContext = ds;

                /*
                DataTable data = new DataTable();
                data.Load(cmd.ExecuteReader());
                connection.Close();
                dataGridArchive.DataContext = data;
                */

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /*
        public ObservableCollection<Client> GetAllClients()
        {
            ObservableCollection<Client> clients = new ObservableCollection<Client>();
            // Chaîne de connexion
            string connectionString =
            "server=yqozyoncedricfou.mysql.db;database=yqozyoncedricfou;uid=yqozyoncedricfou;pwd=Ingetis123;";
            // Créer une connexion à la base de données​​
            string query = "SELECT * FROM archive";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Créer un objet Command
                MySqlCommand command = new MySqlCommand(query, connection);
                try
                {
                    // Ouvrir la connexion
                    connection.Open();
                    // Exécuter la requête SQL et récupérer les données dans un DataReader
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Parcourir les lignes de données
                        while (reader.Read())
                        {
                            // Récupérer les valeurs des colonnes de la ligne actuelle
                            int Id = reader.GetInt32(0);
                            string Atitle = reader.GetString(1);
                            string Asource = reader.GetString(2);
                            string Aurl = reader.GetString(3);
                            string AurlPicture = reader.GetString(4);
                            Client client = new Client { title = Atitle, source = Asource, id = Id, url = Aurl, urlPicture = AurlPicture };
                            clients.Add(client);
                        }
                    }
            }
                catch (Exception ex)
                {
                    // Gérer les erreurs de connexion ou d'exécution de la requête SQL
                    Console.WriteLine("Erreur : " + ex.Message);
                }
            }
            return clients;
        }
        */
        /*
        private void LoadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM archive", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                dataGridArchive.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
            
        }
        */
    }
}