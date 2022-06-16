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
    /// Logika interakcji dla klasy Edit.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Editor()
        {
            InitializeComponent();
            LoadGrid();
        }

        internal SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-UKPP3P6;Initial Catalog=Animals;Integrated Security=True");

        public void LoadGrid()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM zwierzeta", conn);
            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            AnimalsTable.ItemsSource = dataTable.DefaultView;
        }

        private void ReadMenu(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"SELECT * FROM zwierzeta WHERE id like '{AnimalName.Text}'", conn);
            if (AnimalName.Text == string.Empty)
                command = new SqlCommand($"SELECT * FROM zwierzeta", conn);



            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            AnimalsTable.ItemsSource = dataTable.DefaultView;
            ClearData();
        }



        public void ClearData()
        {
           AnimalName.Clear();
            AnimalsNutrition.Clear();
            AnimalsLocation.Clear();
            AnimalsSize.Clear();
            AnimalsWeight.Clear();
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
        private void DeleteMenu(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand command = new SqlCommand($"DELETE FROM Menu WHERE id like '{AnimalName.Text}'", conn);
            try
            {
                if (AnimalName.Text != string.Empty)
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Record has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    conn.Close();
                    ClearData();
                    LoadGrid();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Not deleted: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void UpdateMenu(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand command = new SqlCommand($"UPDATE zwierzeta SET " +
                $"Nutration = '{AnimalsNutrition.Text}'," +
                $"Location = '{AnimalsLocation.Text}'," +
                $"Size = '{AnimalsSize.Text}'," +
                $"Weight = '{AnimalsWeight.Text}'" +
                $"WHERE Name like '{AnimalName.Text}'", conn);

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Record has been updated successfully", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                ClearData();
                LoadGrid();
            }
        }
        private void InsertMenu(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand command = new SqlCommand("INSERT INTO zwierzeta VALUES(@id ,@Name, @Products, @MikroMarko, @KCAL, @Price)", conn);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Name", AnimalName.Text);
                    command.Parameters.AddWithValue("@Nutrition", AnimalsNutrition.Text);
                    command.Parameters.AddWithValue("@Location", AnimalsLocation.Text);
                    command.Parameters.AddWithValue("@Size", AnimalsSize.Text);
                    command.Parameters.AddWithValue("@Weight", AnimalsWeight.Text);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully added", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool isValid()
        {
            if (AnimalsNutrition.Text == string.Empty)
            {
                MessageBox.Show("Name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
