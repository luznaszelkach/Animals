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
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Animals"/> class.
        /// </summary>
        public Editor()
        {
            InitializeComponent();
            LoadGrid();
        }
        /// <summary>
        /// Defines the conn.
        /// </summary>
        internal SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-UKPP3P6;Initial Catalog=Animals;Integrated Security=True");
        /// <summary>                                         
        /// The LoadGrid.
        /// </summary>
        public void LoadGrid()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM zwierzeta", conn);
            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            StudentsTable.ItemsSource = dataTable.DefaultView;
        }
        /// <summary>
        /// The InsertStudent.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void InsertStudent(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand command = new SqlCommand("INSERT INTO zwierzeta VALUES(@Name, @Nutrition, @Location, @Size, @Weight)", conn);
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
        /// <summary>
        /// The ReadStudents.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void ReadStudents(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"SELECT * FROM zwierzeta WHERE id like '{AnimalsID.Text}'", conn);

            if (AnimalsID.Text == string.Empty)
                command = new SqlCommand($"SELECT * FROM zwierzeta", conn);

            DataTable dataTable = new DataTable();
            conn.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            dataTable.Load(dataReader);
            conn.Close();
            StudentsTable.ItemsSource = dataTable.DefaultView;
            ClearData();
        }
        /// <summary>
        /// The DeleteStudent.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void DeleteStudent(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand command = new SqlCommand($"DELETE FROM students WHERE student_id like '{AnimalsID.Text}'", conn);
            try
            {
                if (AnimalsID.Text != string.Empty)
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
        /// <summary>
        /// The UpdateStudent.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/>.</param>
        private void UpdateStudent(object sender, RoutedEventArgs e)
        {
            conn.Open();
            SqlCommand command = new SqlCommand($"UPDATE zwierzeta SET " +
                $"Name = '{AnimalName.Text}'," +
                $"Nutrition = '{AnimalsNutrition.Text}'," +
                $"Location = '{AnimalsLocation.Text}'," +
                $"Size = '{AnimalsSize.Text}'" +
                   $"Weight = '{AnimalsWeight.Text}'" +
                $"WHERE student_id like '{AnimalsID.Text}'", conn);

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
        /// <summary>
        /// The isValid.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool isValid()
        {
            if (AnimalName.Text == string.Empty)
            {
                MessageBox.Show("Surname is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        /// <summary>
        /// The ClearData.
        /// </summary>
        public void ClearData()
        {
            AnimalsID.Clear();
            AnimalName.Clear();
            
        }
        /// <summary>
        /// The StudentsIDInputValidation.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="TextCompositionEventArgs"/>.</param>
        private void StudentsIDInputValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// The StudentsNameInputValidation.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="TextCompositionEventArgs"/>.</param>
        private void StudentsNameInputValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-zZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
