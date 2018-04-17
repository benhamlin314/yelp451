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
using Npgsql;

namespace Team37_Mile2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string name { get; set; }
            public string state_var { get; set; }
            public string city { get; set; }
            public double stars { get; set; }
            public string zip { get; set; }
            public string address { get; set; }
            public int review_count { get; set; }
            public int num_checkins { get; set; }
            public double review_rating { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumnsToGrid();
        }

        private string buildConnectString()
        {
            return "Server=localhost; Port=5432; Username=postgres; Password=1234; Database=yelp451";
        }

        public void addStates()
        {
            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT distinct state_var FROM business_table ORDER BY state_var;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stateList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }

        public void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            BusinessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("state_var");
            BusinessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            BusinessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Address";
            col4.Binding = new Binding("address");
            BusinessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Zip";
            col5.Binding = new Binding("zip");
            BusinessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("stars");
            BusinessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Reviews";
            col7.Binding = new Binding("review_count");
            BusinessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Checkins";
            col8.Binding = new Binding("num_checkins");
            BusinessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Rating";
            col9.Binding = new Binding("review_rating");
            BusinessGrid.Columns.Add(col9);

        }

        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityList.Items.Clear();
            zipList.Items.Clear();
            categoryList.Items.Clear();
            BusinessGrid.Items.Clear();
            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "SELECT distinct city FROM business_table WHERE state_var='"+stateList.SelectedItem.ToString()+"' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cityList.Items.Add(reader.GetString(0));
                        }
                    }
                }
                comm.Close();
            }
        }

        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zipList.Items.Clear();
            categoryList.Items.Clear();
            BusinessGrid.Items.Clear();
            if (!(cityList.SelectedIndex == -1))
            {
                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "SELECT distinct postal_code FROM business_table WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                zipList.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                    comm.Close();
                }
            }
        }

        private void zipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            categoryList.Items.Clear();
            BusinessGrid.Items.Clear();
            if (!(zipList.SelectedIndex == -1)) {
                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "SELECT * FROM business_table WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business() { name = reader.GetString(1), state_var = reader.GetString(2), city = reader.GetString(3), stars = reader.GetDouble(6), zip = reader.GetString(8), address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                            }
                        }

                        cmd.CommandText = "SELECT distinct category FROM business_category_table JOIN business_table ON business_category_table.business_id=business_table.business_id WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryList.Items.Add(reader.GetString(0));
                            }
                        }

                    }
                    comm.Close();
                }
            }
            else
            {

            }
        }

        private void catagoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            if (!(categoryList.SelectedIndex == -1))
            {
                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "SELECT business_table.* FROM business_table JOIN business_category_table ON business_table.business_id=business_category_table.business_id WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "' AND category ='" + categoryList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business() { name = reader.GetString(1), state_var = reader.GetString(2), city = reader.GetString(3), stars = reader.GetDouble(6), zip = reader.GetString(8), address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                            }
                        }
                    }
                    comm.Close();
                }
            }
            else { }
        }
    }

    
}
