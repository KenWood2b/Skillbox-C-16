using System;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace ADOWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string mssqlConnectionString = @"Server=desktop-r55hp68\mssqlserver01;Database=ShopDB;Trusted_Connection=True;";
        string accessConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AccessDatabase.accdb;Persist Security Info=False;";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LoadClients();
            LoadPurchases();
        }

        private void LoadClients()
        {
            using (SqlConnection conn = new SqlConnection(mssqlConnectionString))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Clients", conn);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);
                ClientsGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void LoadPurchases()
        {
            using (OleDbConnection conn = new OleDbConnection(accessConnectionString))
            {
                conn.Open();
                var command = new OleDbCommand("SELECT * FROM Products", conn);
                var adapter = new OleDbDataAdapter(command);
                var dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);
                PurchasesGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(mssqlConnectionString))
            {
                conn.Open();
                var command = new SqlCommand("INSERT INTO Clients (LastName, FirstName, MiddleName, PhoneNumber, Email) VALUES (@LastName, @FirstName, @MiddleName, @PhoneNumber, @Email)", conn);
                command.Parameters.AddWithValue("@LastName", "Фамилия");
                command.Parameters.AddWithValue("@FirstName", "Имя");
                command.Parameters.AddWithValue("@MiddleName", "Отчество");
                command.Parameters.AddWithValue("@PhoneNumber", "123456789");
                command.Parameters.AddWithValue("@Email", "email@example.com");
                command.ExecuteNonQuery();
            }
            LoadClients();
        }

        private void UpdateClient_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(mssqlConnectionString))
            {
                conn.Open();
                var command = new SqlCommand("UPDATE Clients SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName, PhoneNumber = @PhoneNumber WHERE ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", 1); // Replace with selected client's ID
                command.Parameters.AddWithValue("@LastName", "Обновленная Фамилия");
                command.Parameters.AddWithValue("@FirstName", "Обновленное Имя");
                command.Parameters.AddWithValue("@MiddleName", "Обновленное Отчество");
                command.Parameters.AddWithValue("@PhoneNumber", "987654321");
                command.ExecuteNonQuery();
            }
            LoadClients();
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(mssqlConnectionString))
            {
                conn.Open();
                var command = new SqlCommand("DELETE FROM Clients WHERE ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", 1); // Replace with selected client's ID
                command.ExecuteNonQuery();
            }
            LoadClients();
        }

        private void AddPurchase_Click(object sender, RoutedEventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(accessConnectionString))
            {
                conn.Open();
                var command = new OleDbCommand("INSERT INTO Products (Email, ProductCode, ProductName) VALUES (@Email, @ProductCode, @ProductName)", conn);
                command.Parameters.AddWithValue("@Email", "email@example.com");
                command.Parameters.AddWithValue("@ProductCode", "123");
                command.Parameters.AddWithValue("@ProductName", "Новый Товар");
                command.ExecuteNonQuery();
            }
            LoadPurchases();
        }

        private void DeletePurchase_Click(object sender, RoutedEventArgs e)
        {
            using (OleDbConnection conn = new OleDbConnection(accessConnectionString))
            {
                conn.Open();
                var command = new OleDbCommand("DELETE FROM Products WHERE ID = @ID", conn);
                command.Parameters.AddWithValue("@ID", 1); // Replace with selected purchase's ID
                command.ExecuteNonQuery();
            }
            LoadPurchases();
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(mssqlConnectionString))
            {
                conn.Open();
                var command = new SqlCommand("DELETE FROM Clients", conn);
                command.ExecuteNonQuery();
            }

            using (OleDbConnection conn = new OleDbConnection(accessConnectionString))
            {
                conn.Open();
                var command = new OleDbCommand("DELETE FROM Products", conn);
                command.ExecuteNonQuery();
            }

            LoadData();
        }
    }
}

