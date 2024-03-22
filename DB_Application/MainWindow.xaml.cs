using MySql.Data.MySqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DB_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }
        private void InitializeDatabaseConnection()
        {
            string connectionString = "server=mariadb.vamk.fi;port=3306;database=e2101083_courses;uid=e2101083;password=9SbjzjcK6hQ;";
            connection = new MySqlConnection(connectionString);
        }

        private void FetchData()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM course";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                StringBuilder coursesInfo = new StringBuilder();

                while (reader.Read())
                {
                    string courseName = reader.GetString(1);
                    string teacherName = reader.GetString(2);

                    // Append fetched data to the StringBuilder
                    coursesInfo.AppendLine($"{courseName} - {teacherName}");
                }

                reader.Close();
                connection.Close();
                txtBlockCourse.Text = coursesInfo.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void SaveData(string courseName, string teacherName)
        {
            try
            {
                connection.Open();

                string insertQuery = "INSERT INTO course (courseName, teacherName) VALUES (@courseName, @teacherName)";
                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);

                insertCommand.Parameters.AddWithValue("@courseName", courseName);
                insertCommand.Parameters.AddWithValue("@teacherName", teacherName);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                MessageBox.Show("Rows affected: " + rowsAffected);

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnFetchData_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            string courseName = txtCourseName.Text;
            string teacherName = txtTeacherName.Text;
            if (!string.IsNullOrEmpty(courseName) || !string.IsNullOrEmpty(teacherName))
            {
                SaveData(courseName, teacherName);
            }
            else
            {
                MessageBox.Show("Please enter a course name.");
            }
        }
    }
}