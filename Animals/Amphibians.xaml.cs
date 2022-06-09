using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Animals
{
    /// <summary>
    /// Interaction logic for Amphibians.xaml
    /// </summary>
    public partial class Amphibians : UserControl
    {
        public Amphibians()
        {
            InitializeComponent();
            LoadGrid();
        }

        internal SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-UKPP3P6;Initial Catalog=Animals;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Amphibians", conn);
            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            AnimalTable.ItemsSource = dataTable.DefaultView;
        }

        private void ReadAnimals(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"SELECT * FROM Amphibians WHERE id like '{AnimalID.Text}'", conn);
            if (AnimalID.Text == string.Empty)
                command = new SqlCommand($"SELECT * FROM Amphibians", conn);



            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            AnimalTable.ItemsSource = dataTable.DefaultView;
            ClearData();
        }



        public void ClearData()
        {
            AnimalID.Clear();
            Name.Clear();
            Nutrition.Clear();
            Location.Clear();
            Size.Clear();
            Weight.Clear();
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LetterValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-zZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
