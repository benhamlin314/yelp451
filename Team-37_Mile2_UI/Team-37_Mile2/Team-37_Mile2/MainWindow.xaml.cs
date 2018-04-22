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
            //-- DEBUG -- (Jared: my postgres user has a different password, not sure how to change it)
            return "Server=localhost; Port=5432; Username=postgres; Password=password; Database=yelp451";
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

            if (credit_cards.IsChecked == true)
            {
                sb_att.Append(" AND A.attribute_name = 'Accepts Credit Cards' AND A.val = 'TRUE'");
            }
            //like the above for Filter by Attributes box and filter by meal box

            //If a day isn't specified, we ignore it - SelectedIndex is -1 in this case
            if(day.SelectedIndex != -1)
            {
                sb_open.Append(" AND O." + day.SelectedItem.ToString().ToLower() + " = '" + opened.SelectedItem.ToString() + "-" + closed.SelectedItem.ToString() + "'");
            }

            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder("SELECT B.name, B.address, B.city, B.state_var, B.stars, B.review_count, B.review_rating, B.latitude, B.longitude FROM business_table as B ");

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
                            //temp.distance = temp.calc_dist()//needs user long and lat to calculate
                            //BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), state_var = reader.GetString(1), city = reader.GetString(2), stars = reader.GetDouble(3), distance = , address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                            BusinessGrid.Items.Add(temp);//adds record to display
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

        private void buttonCheckin_Click(object sender, RoutedEventArgs e)
        {
            //Make the graph window, but don't display it yet.
            GraphWindow win2 = new GraphWindow();
            win2.Show();

            //Save the list of 'business_id's to a list for later.
            List<string> businessIDList = new List<string>();

            //----------------------------------
            //All of this is a copy of the Submit button code, except for
            //    - what we're SELECTing (the IDs)
            //    - what we're doing with the data (instead of making Business
            //        objects and adding them to the Data Grid on the Main Window,
            //        we're just holding on to the reader object and then making
            //        a second request where we take the results of the first and
            //        grab the checkin counts WHERE the id is one of those returned
            //        above.
            //
            //This should probably all be split into functions, but trying to figure
            //    out how made my head hurt and ultimately seemed like a bad idea
            //    for the time being.
            //----------------------------------

            //utilize stringbuilder to build select statement based on the options selected
            //each string builder will contain the section for WHERE clause 
            StringBuilder sb_cat = new StringBuilder();
            StringBuilder sb_att = new StringBuilder();
            StringBuilder sb_price = new StringBuilder();//in attributes: make index for prices
            StringBuilder sb_meal = new StringBuilder();//also in attributes: make index for meal
            StringBuilder sb_open = new StringBuilder();

            //build each string here

            for (int i = selected_categories.Items.Count; i > 0; i--)//builds category string
            {
                sb_cat.Append(" AND category = '" + selected_categories.Items[i - 1] + "'");
            }

            if (credit_cards.IsChecked == true)
            {
                sb_att.Append(" AND A.attribute_name = 'Accepts Credit Cards' AND A.val = 'TRUE'");
            }
            //like the above for Filter by Attributes box and filter by meal box

            //If a day isn't specified, we ignore it - SelectedIndex is -1 in this case
            if (day.SelectedIndex != -1)
            {
                sb_open.Append(" AND O." + day.SelectedItem.ToString().ToLower() + " = '" + opened.SelectedItem.ToString() + "-" + closed.SelectedItem.ToString() + "'");
            }

            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    //Only grab the IDs this time.
                    StringBuilder sql = new StringBuilder("SELECT B.business_id FROM business_table as B ");

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


                    sql.Append("WHERE state_var ='" + stateList.SelectedItem.ToString() + "' AND city ='" + cityList.SelectedItem.ToString() + "' AND postal_code ='" + zipList.SelectedItem.ToString() + "'");

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
                            //For each ID returned, add it to the list of IDs.
                            businessIDList.Add(reader.GetString(0));
                        }
                    }
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;

                    cmd.CommandText = "SELECT business_table.name, checkin_table.business_id, COUNT(day_var) FROM business_table JOIN checkin_table ON business_table.business_id=checkin_table.business_id GROUP BY business_table.name, checkin_table.business_id";

                    if (businessIDList.Count > 0)
                    {
                        cmd.CommandText += " HAVING ";

                        foreach (string i in businessIDList)
                        {
                            cmd.CommandText += "checkin_table.business_id = '" + i + "' OR ";
                        }

                        cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 4);
                        cmd.CommandText += ";";
                    }
                    


                    using (var reader = cmd.ExecuteReader())
                    {
                        List<KeyValuePair<string, int>> dataList = new List<KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            //For each result, add the value pair to the list that we'll pass to the graph window.
                            dataList.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(2)));
                        }

                        //This doesn't work right now - an exception is thrown.
                        win2.checkinChart.DataContext = dataList;
                    }
                }


                comm.Close();
            }
        }

        private void buttonReviews_Click(object sender, RoutedEventArgs e)
        {
            //show a TableWindow
        }

        private void buttonNumBusinessPerZip_Click(object sender, RoutedEventArgs e)
        {
            GraphWindow win2 = new GraphWindow();
            win2.Show();
        }
    }
   
}
