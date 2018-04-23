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
            public double distance { get; set; }
            public double bus_lat { get; set; }
            public double bus_long { get; set; }
            public string address { get; set; }
            public int review_count { get; set; }
            public int num_checkins { get; set; }
            public double review_rating { get; set; }
            public string id { get; set; }

            public double calc_dist(double latitude, double longitude)
            {
                return Math.Abs(Math.Sqrt(Math.Pow(bus_lat - latitude, 2) + Math.Pow(bus_long - longitude, 2)));
            }
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
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            BusinessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            BusinessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state_var");
            BusinessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Distance";
            col5.Binding = new Binding("distance");
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
            col8.Header = "Average Rating";
            col8.Binding = new Binding("review_rating");
            BusinessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Checkins";
            col9.Binding = new Binding("num_checkins");
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
                        //following commented out section only used in Milestone 2 also used as basic format for search button
                        /*cmd.CommandText = "SELECT * FROM business_table WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business() { name = reader.GetString(1), state_var = reader.GetString(2), city = reader.GetString(3), stars = reader.GetDouble(6), zip = reader.GetString(8), address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                            }
                        }*/

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

        //remove this event catagories only added by add button
        /*private void catagoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        }*/

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            BusinessGrid.Items.Clear();
            //utilize stringbuilder to build select statement based on the options selected
            //each string builder will contain the section for WHERE clause 
            StringBuilder sb_cat = new StringBuilder();
            StringBuilder sb_att = new StringBuilder();
            StringBuilder sb_price = new StringBuilder();//in attributes: make index for prices
            StringBuilder sb_meal = new StringBuilder();//also in attributes: make index for meal
            StringBuilder sb_open = new StringBuilder();
            
            //build each string here

            for(int i = selected_categories.Items.Count; i > 0; i--)//builds category string
            {
                sb_cat.Append(" AND category = '" + selected_categories.Items[i-1] + "'");
            }

            //if()

            if (credit_cards.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'BusinessAcceptsCreditCards' AND A.val = 'true')");
            }
            //like the above for Filter by Attributes box and filter by meal box
            if (reservations.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'RestrauntsReservations' AND A.val = 'true')");
            }
            if (wheelchair.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'WheelchairAccessible' AND A.val = 'true')");
            }
            if (outdoor_seat.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'OutdoorSeating' AND A.val = 'true')");
            }
            if (kid_friendly.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'GoodForKids' AND A.val = 'true')");
            }
            if (group_friendly.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'RestaurantsGoodForGroups' AND A.val = 'true')");
            }
            if (delivery.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'RestaurantsDelivery' AND A.val = 'true')");
            }
            if (takeout.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'RestaurantsTakeOut' AND A.val = 'true')");
            }
            if (wifi.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'WiFi' AND A.val = 'true')");
            }
            if (bikeparking.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'BikeParking' AND A.val = 'true')");
            }

            if(day != null)//works with set up of "10:00-20:00"
            {
                if (opened != null && closed != null)
                {
                    sb_open.Append(" AND O." + day.SelectedItem.ToString().ToLower() + " = '" + opened.SelectedItem.ToString() + "-" + closed.SelectedItem.ToString() + "'");
                }
            }

            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder("SELECT B.name, B.address, B.city, B.state_var, B.stars, B.review_count, B.review_rating, B.latitude, B.longitude, B.business_id FROM business_table as B ");

                    //adds join statements as needed
                    if (sb_cat.Length > 0)
                    {
                         sql.Append("JOIN business_category_table as C ON B.business_id=C.business_id ");
                    }
                    if (sb_att.Length > 0)
                    {
                        sql.Append("JOIN business_attribute_table as A ON B.business_id=A.business_id ");
                    }
                    if (sb_open.Length > 0)
                    {
                        sql.Append("JOIN hours_open_table as O ON B.business_id=O.business_id ");
                    }
                    
                    
                    sql.Append("WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString()+"'");
                    
                    //add other strings to sql statement here
                    if (sb_cat.Length > 0) { sql.Append(" " + sb_cat.ToString()); }
                    if (sb_att.Length > 0) { sql.Append(" " + sb_att.ToString()); }
                    if (sb_meal.Length > 0) { sql.Append(" " + sb_meal.ToString()); }
                    if (sb_open.Length > 0) { sql.Append(" " + sb_open.ToString()); }
                    if (sb_price.Length > 0) { sql.Append(" " + sb_price.ToString()); }

                    sql.Append(";");//adds semicolon to end of sql statement
                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Business temp = new Business();
                            temp.name = reader.GetString(0);
                            temp.address = reader.GetString(1);
                            temp.city = reader.GetString(2);
                            temp.state_var = reader.GetString(3);
                            temp.stars = reader.GetDouble(4);
                            temp.review_count = reader.GetInt32(5);
                            temp.review_rating = reader.GetDouble(6);
                            temp.bus_lat = reader.GetDouble(7);
                            temp.bus_long = reader.GetDouble(8);
                            temp.id = reader.GetString(9);
                            //temp.distance = temp.calc_dist(usr_lat,usr_long);//needs user long and lat to calculate
                            BusinessGrid.Items.Add(temp);//adds record to display


                            //BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), state_var = reader.GetString(1), city = reader.GetString(2), stars = reader.GetDouble(3), distance = , address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                        }
                    }
                }
                comm.Close();
            }
        }

        //changes selected business at bottom of screen allowing checkin and addition of review
        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Business tempbus = (Business)BusinessGrid.SelectedItem;
            Name.Text = tempbus.name;
        }

        //adds review using user info and filled out info. check for empty review text
        private void submit_review_Click(object sender, RoutedEventArgs e)
        {
            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    //cmd.CommandText = "SELECT business_table.* FROM business_table JOIN business_category_table ON business_table.business_id=business_category_table.business_id WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "' AND category ='" + categoryList.SelectedItem.ToString() + "';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //BusinessGrid.Items.Add(new Business() { name = reader.GetString(1), state_var = reader.GetString(2), city = reader.GetString(3), stars = reader.GetDouble(6), zip = reader.GetString(8), address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                        }
                    }
                }
                comm.Close();
            }
        }

        //adds selected category to list
        private void add_cat_Click(object sender, RoutedEventArgs e)
        {
            selected_categories.Items.Add(categoryList.SelectedItem);
        }

        //removes selected cetegory from list
        private void remove_cat_Click(object sender, RoutedEventArgs e)
        {
            selected_categories.Items.Remove(selected_categories.SelectedItem);
        }
    }
}
