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
using System.ComponentModel;
using System.Globalization;

namespace Team37_Mile2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double usrlong;
        private double usrlat;

        private void set_usrlocation(string usrlongstr,string usrlatstr)
        {
            if (usrlongstr.StartsWith("-"))
            {
                usrlongstr.TrimStart('-');
                usrlong = Double.Parse(usrlongstr);
                usrlong *= -1;
            }
            else
            {
                usrlong = Double.Parse(usrlongstr);
            }
            if (usrlatstr[0] == '-')
            {
                usrlatstr.TrimStart('-');
                usrlat = Double.Parse(usrlatstr);
                usrlat *= -1;
            }
            else
            {
                usrlat = Double.Parse(usrlatstr);
            }

        }

        public class Business
        {
            public string bus_id { get; set; }
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

        public class Review
        {
            public String date { get; set; }
            public String user_name { get; set; }
            public int stars { get; set; }
            public string text { get; set; }

            public string business_name { get; set; }
            public string city { get; set; }
        }

        public class User
        {
            public string user_id { get; set; }
            public string name { get; set; }
            public double average_stars { get; set; }
            public string yelping_since { get; set; }
            public int funny { get; set; }
            public int cool { get; set; }
            public int useful { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumnsToBusinessGrid();
            addColumnsToTipGrid();
            addColumnsToFriendGrid();
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

        public void addColumnsToBusinessGrid()
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

        public void addColumnsToTipGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("user_name");
            TipGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business Name";
            col2.Binding = new Binding("business_name");
            col2.Width = 255;
            TipGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            TipGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = DataGridLength.Auto;
            TipGrid.Columns.Add(col4);

        }

        public void addColumnsToFriendGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            //col1.Width = 255;
            FriendGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Average Stars";
            col2.Binding = new Binding("average_stars");
            FriendGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Yelping Since";
            col3.Binding = new Binding("yelping_since");
            FriendGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Funny";
            col4.Binding = new Binding("funny");
            col4.Width = DataGridLength.Auto;
            FriendGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Cool";
            col5.Binding = new Binding("cool");
            FriendGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Useful";
            col6.Binding = new Binding("useful");
            FriendGrid.Columns.Add(col6);

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


            //filters for attributes
            if (credit_cards.IsChecked == true)
            {
                sb_att.Append(" AND (A.attribute_name = 'BusinessAcceptsCreditCards' AND A.val = 'true')");
            }
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
            //end filter for attributes

            //filters for prices
            if (!(price1.IsChecked == false && price2.IsChecked == false && price3.IsChecked == false && price4.IsChecked == false))
            {
                sb_price.Append(" AND (");
                if (price1.IsChecked == true && price2.IsChecked == true && price3.IsChecked == true && price4.IsChecked == true)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '1' OR RestaurantsPriceRange2 = '2' OR RestaurantsPriceRange2 = '3' OR RestaurantsPriceRange2 = '4')");
                }
                else if (price1.IsChecked == true && price2.IsChecked == true && price3.IsChecked == true && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '1' OR RestaurantsPriceRange2 = '2' OR RestaurantsPriceRange2 = '3')");
                }
                else if (price1.IsChecked == true && price2.IsChecked == true && price3.IsChecked == false && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '1' OR RestaurantsPriceRange2 = '2')");
                }
                else if (price1.IsChecked == true && price2.IsChecked == false && price3.IsChecked == false && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '1')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == true && price3.IsChecked == true && price4.IsChecked == true)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '2' OR RestaurantsPriceRange2 = '3' OR RestaurantsPriceRange2 = '4')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == true && price3.IsChecked == true && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '2' OR RestaurantsPriceRange2 = '3')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == true && price3.IsChecked == false && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '2')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == false && price3.IsChecked == true && price4.IsChecked == true)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '3' OR RestaurantsPriceRange2 = '4')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == false && price3.IsChecked == true && price4.IsChecked == false)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '3')");
                }
                else if (price1.IsChecked == false && price2.IsChecked == false && price3.IsChecked == false && price4.IsChecked == true)
                {
                    sb_price.Append("RestaurantsPriceRange2 = '4')");
                }
            }
            //end filter for prices


            //filters for meals
            if (anymeal.IsChecked == false)
            {
                sb_meal.Append(" AND (A.attribute_name = 'GoodForMeal' AND A.val = '{''dessert'': ");
                if (Dessert.IsChecked == true)
                {
                    sb_meal.Append("True, ");
                }
                else
                {
                    sb_meal.Append("False, ");
                }
                sb_meal.Append("''latenight'': ");
                if (Late_Night.IsChecked == true)
                {
                    sb_meal.Append("True, ");
                }
                else
                {
                    sb_meal.Append("False, ");
                }
                sb_meal.Append("''lunch'': ");
                if (Lunch.IsChecked == true)
                {
                    sb_meal.Append("True, ");
                }
                else
                {
                    sb_meal.Append("False, ");
                }
                sb_meal.Append("''dinner'': ");
                if (Dinner.IsChecked == true)
                {
                    sb_meal.Append("True, ");
                }
                else
                {
                    sb_meal.Append("False, ");
                }
                sb_meal.Append("''breakfast'': ");
                if (Breakfast.IsChecked == true)
                {
                    sb_meal.Append("True, ");
                }
                else
                {
                    sb_meal.Append("False, ");
                }
                sb_meal.Append("''brunch'': ");
                if (Brunch.IsChecked == true)
                {
                    sb_meal.Append("True}'");
                }
                else
                {
                    sb_meal.Append("False}')");
                }
                sb_meal.Append(")");
            }
            //end filter by meal

            if (day.SelectedItem != null)//works with set up of "10:00-20:00"
            {
                if (opened.SelectedItem != null && closed.SelectedItem != null)
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
                    StringBuilder sql = new StringBuilder("SELECT B.business_id, B.name, B.address, B.city, B.state_var, B.stars, B.review_count, B.review_rating, B.latitude, B.longitude FROM business_table as B ");

                    //adds join statements as needed
                    if (sb_cat.Length > 0)
                    {
                         sql.Append("JOIN business_category_table as C ON B.business_id=C.business_id ");
                    }
                    if (sb_att.Length > 0 || sb_meal.Length > 0)
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

                    //If the Sort By selection is not "Nearest" or none...
                    if (sortby.SelectedIndex != 5 && sortby.SelectedIndex != -1)
                    {
                        sql.Append(" ORDER BY ");
                        switch (sortby.SelectedIndex)
                        {
                            case 0:
                                {
                                    sql.Append("B.name");
                                    break;
                                }
                            case 1:
                                {
                                    sql.Append("B.stars DESC");
                                    break;
                                }
                            case 2:
                                {
                                    sql.Append("B.review_count DESC");
                                    break;
                                }
                            case 3:
                                {
                                    sql.Append("B.review_rating DESC");
                                    break;
                                }
                            case 4:
                                {
                                    sql.Append("B.num_checkins DESC");
                                    break;
                                }
                        }
                    }


                    sql.Append(";");//adds semicolon to end of sql statement
                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        //var format = new NumberFormatInfo();
                        //format.NegativeSign = "-";
                        //format.NumberDecimalSeparator = ".";
                        while (reader.Read())
                        {
                            Business temp = new Business();
                            temp.bus_id = reader.GetString(0);
                            temp.name = reader.GetString(1);
                            temp.address = reader.GetString(2);
                            temp.city = reader.GetString(3);
                            temp.state_var = reader.GetString(4);
                            temp.stars = reader.GetDouble(5);
                            temp.review_count = reader.GetInt32(6);
                            temp.review_rating = reader.GetDouble(7);
                            temp.bus_lat = reader.GetDouble(8);
                            temp.bus_long = reader.GetDouble(9);
                            temp.distance = temp.calc_dist(usrlat,usrlong);//needs user long and lat to calculate
                            //BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), state_var = reader.GetString(1), city = reader.GetString(2), stars = reader.GetDouble(3), distance = , address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                            BusinessGrid.Items.Add(temp);//adds record to display


                            //BusinessGrid.Items.Add(new Business() { name = reader.GetString(0), state_var = reader.GetString(1), city = reader.GetString(2), stars = reader.GetDouble(3), distance = , address = reader.GetString(9), review_count = reader.GetInt32(10), num_checkins = reader.GetInt32(11), review_rating = reader.GetDouble(12) });
                        }
                    }

                    //If the user chose to sort by distance, do this locally
                    //  (since we don't collect the distances through a SQL query)
                    if(sortby.SelectedIndex == 5)
                    {
                        ItemCollection tempItems = BusinessGrid.Items;
                        tempItems.SortDescriptions.Add(new SortDescription("distance", ListSortDirection.Ascending));
                        tempItems.Refresh();
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

        private static Random random = new Random();
        public static string RandomId(int length)//modified from quick google to generate id
        {
            const string chars = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ0123456789_-";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //adds review using user info and filled out info. check for empty review text
        private void submit_review_Click(object sender, RoutedEventArgs e)
        {
            string rev_id = "";
            bool goodrevid = false;
            while (!goodrevid)
            {
                rev_id = RandomId(22);
                using (var comm = new NpgsqlConnection(buildConnectString()))//checks if id already exists
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;
                        cmd.CommandText = "SELECT COUNT(review_id) FROM review_table WHERE review_id = '" + rev_id + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if(reader.GetInt32(0) == 0)
                                {
                                    goodrevid = true;
                                }
                            }
                        }
                    }
                    comm.Close();
                }
            }
            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    cmd.CommandText = "INSERT INTO review_table (review_id, user_id, business_id, date, text, stars, funny, cool, useful) VALUES ('"+rev_id+"','"+((User)listBox_CurrentUserIDMatch.SelectedItem).user_id+"','"+((Business)BusinessGrid.SelectedItem).bus_id+"','"+DateTime.Now.ToString("yyy-MM-dd")+"','"+review.ToString()+"',"+Convert.ToInt32(rating.ToString())+",0,0,0);";//INCOMPLETE
                    using (var reader = cmd.ExecuteReader())//needed to execute the command
                    {
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

            Business selectedBusiness = new Business();
            //If the user has selected a business in the GUI...
            if (this.BusinessGrid.SelectedItem != null)
            {
                selectedBusiness = (Business)this.BusinessGrid.SelectedItem;

                //Replace the placeholder '[Business Name]' text with the business' name.
                win2.checkinChart.Title = win2.checkinChart.Title.ToString().Replace("[Business Name]", selectedBusiness.name);


                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;

                        cmd.CommandText = "SELECT business_table.name, checkin_table.business_id, day_var, SUM(count_var) FROM business_table JOIN checkin_table ON business_table.business_id=checkin_table.business_id GROUP BY business_table.name, checkin_table.business_id, day_var";
                        cmd.CommandText += " HAVING ";

                        cmd.CommandText += "checkin_table.business_id = '" + selectedBusiness.bus_id + "';";


                        using (var reader = cmd.ExecuteReader())
                        {
                            List<KeyValuePair<string, int>> dataList = new List<KeyValuePair<string, int>>();
                            while (reader.Read())
                            {
                                //For each result, add the value pair to the list that we'll pass to the graph window.
                                dataList.Add(new KeyValuePair<string, int>(reader.GetString(2), reader.GetInt32(3)));
                            }

                            win2.SetChart(dataList);
                        }
                    }
                    comm.Close();
                }

                //Show() must go after SetChart() - if not, this (fairly strange) exception is thrown:
                //    "Cannot modify the logical children for this node at this time because a tree walk is in progress"
                win2.Show();
            }
            else
            {
                //No business was clicked - show an error window?
            }
        }

        private void buttonReviews_Click(object sender, RoutedEventArgs e)
        {
            //Make the table window, but don't display it yet.
            TableWindow win2 = new TableWindow();

            Business selectedBusiness = new Business();
            //If the user has selected a business in the GUI...
            if (this.BusinessGrid.SelectedItem != null)
            {
                selectedBusiness = (Business)this.BusinessGrid.SelectedItem;


                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;

                        cmd.CommandText = "SELECT review_table.date, user_table.name, review_table.stars, review_table.text FROM business_table JOIN review_table ON review_table.business_id=business_table.business_id JOIN user_table ON user_table.user_id=review_table.user_id";
                        cmd.CommandText += " WHERE ";

                        cmd.CommandText += "business_table.business_id = '" + selectedBusiness.bus_id + "';";


                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //For each result, create a Review object that stores all of the data we need
                                //  from the database.
                                Review review = new Review();

                                review.date = reader.GetString(0);
                                review.user_name = reader.GetString(1);
                                review.stars = reader.GetInt32(2);
                                review.text = reader.GetString(3);

                                //Add the object to the table.
                                win2.ReviewGrid.Items.Add(review);
                            }
                        }
                    }
                    comm.Close();
                }

                win2.Show();
            }
            else
            {
                //No business was clicked - show an error window?
            }
        }

        private void buttonNumBusinessPerZip_Click(object sender, RoutedEventArgs e)
        {
            //Make the graph window, but don't display it yet.
            GraphWindow win2 = new GraphWindow();

            string selectedCity;
            //If the user has selected a business in the GUI...
            if (this.cityList.SelectedItem != null)
            {
                selectedCity = this.cityList.SelectedItem.ToString();

                //Replace the placeholder '[Business Name]' text with the business' name.
                win2.checkinChart.Title = "Number of Businesses per Zipcode for " + selectedCity;


                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;

                        cmd.CommandText = "SELECT postal_code, COUNT(business_id) FROM business_table WHERE city = '" + selectedCity + "' GROUP BY postal_code;";


                        using (var reader = cmd.ExecuteReader())
                        {
                            List<KeyValuePair<string, int>> dataList = new List<KeyValuePair<string, int>>();
                            while (reader.Read())
                            {
                                //For each result, add the value pair to the list that we'll pass to the graph window.
                                dataList.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                            }

                            win2.SetChart(dataList);
                        }
                    }
                    comm.Close();
                }

                //Show() must go after SetChart() - if not, this (fairly strange) exception is thrown:
                //    "Cannot modify the logical children for this node at this time because a tree walk is in progress"
                win2.Show();
            }
            else
            {
                //No city was selected - show an error window?
            }
        }

        private void button_CurrentUserSearch_Click(object sender, RoutedEventArgs e)
        {
            //Get the name entered by the user.
            string name = textBox_CurrentUser.Text;

            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder("SELECT U.user_id FROM user_table as U ");
                    sql.Append("WHERE name ='" + name + "';");


                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox_CurrentUserIDMatch.Items.Add(reader.GetString(0));//adds record to display
                        }
                    }
                }
                comm.Close();
            }
        }

        private void listBox_CurrentUserIDMatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the ID clicked on by the user.
            string id = listBox_CurrentUserIDMatch.SelectedItem.ToString();

            //Clear all of the info tables and text boxes.
            FriendGrid.Items.Clear();
            TipGrid.Items.Clear();
            textBox_UserInformationName.Clear();
            textBox_UserInformationStars.Clear();
            textBox_UserInformationFans.Clear();
            textBox_UserInformationYelpingSince.Clear();
            textBox_UserInformationVotesFunny.Clear();
            textBox_UserInformationVotesCool.Clear();
            textBox_UserInformationVotesUseful.Clear();
            textBox_UserLocationLatitude.Clear();
            textBox_UserLocationLongitude.Clear();


            using (var comm = new NpgsqlConnection(buildConnectString()))
            {
                comm.Open();

                //Run query for user's info
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder(@"
                        SELECT name, average_stars, fans, yelping_since, funny, cool, useful FROM user_table
                        WHERE user_table.user_id='" + id + "';");


                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            textBox_UserInformationName.Text = reader.GetString(0);
                            textBox_UserInformationStars.Text = reader.GetDouble(1).ToString();
                            textBox_UserInformationFans.Text = reader.GetInt32(2).ToString();
                            textBox_UserInformationYelpingSince.Text = reader.GetString(3);
                            textBox_UserInformationVotesFunny.Text = reader.GetInt32(4).ToString();
                            textBox_UserInformationVotesCool.Text = reader.GetInt32(5).ToString();
                            textBox_UserInformationVotesUseful.Text = reader.GetInt32(6).ToString();
                        }
                    }
                }

                //Run query for user's friends
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder(@"
                        SELECT user_table.user_id, user_table.name, user_table.average_stars, user_table.yelping_since, user_table.funny, user_table.cool, user_table.useful FROM friendship_table
                        JOIN user_table ON user_table.user_id=friendship_table.friend2_id
                        WHERE friend1_id='" + id + "';");


                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //For each result, create a User object that stores all of the data we need
                            //  from the database.
                            User user = new User();

                            user.user_id = reader.GetString(0);
                            user.name = reader.GetString(1);
                            user.average_stars = reader.GetDouble(2);
                            user.yelping_since = reader.GetString(3);
                            user.funny = reader.GetInt32(4);
                            user.cool = reader.GetInt32(5);
                            user.useful = reader.GetInt32(6);

                            FriendGrid.Items.Add(user);//adds record to display
                        }
                    }
                }

                //Run query for user's friends' reviews
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = comm;
                    StringBuilder sql = new StringBuilder(@"
                        SELECT F.name AS user_name, business_table.name AS business_name, business_table.city, review_table.text FROM
                            (SELECT * FROM friendship_table
                            JOIN user_table ON friendship_table.friend2_id=user_table.user_id
                            WHERE friend1_id='" + id + @"')
                        AS F JOIN review_table ON F.user_id=review_table.user_id
                        JOIN business_table ON business_table.business_id=review_table.business_id;");


                    cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //For each result, create a Review object that stores all of the data we need
                            //  from the database.
                            Review review = new Review();

                            review.user_name = reader.GetString(0);
                            review.business_name = reader.GetString(1);
                            review.city = reader.GetString(2);
                            review.text = reader.GetString(3);

                            TipGrid.Items.Add(review);//adds record to display
                        }
                    }
                }

                comm.Close();
            }
        }

        private void button_RemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            //Get the current user.
            string id = listBox_CurrentUserIDMatch.SelectedItem.ToString();

            if (FriendGrid.SelectedItem != null)
            {
                using (var comm = new NpgsqlConnection(buildConnectString()))
                {
                    comm.Open();

                    //Run query for user's info
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = comm;

                        StringBuilder sql = new StringBuilder(@"
                        DELETE FROM friendship_table
                        WHERE (friend1_id='" + id + @"' AND friend2_id='" + ((User)FriendGrid.SelectedItem).user_id + @"') OR (friend1_id='" + ((User)FriendGrid.SelectedItem).user_id + @"' AND friend2_id='" +  id + "');");


                        cmd.CommandText = sql.ToString();//puts contents of sql string builder into communication with db
                        using (cmd.ExecuteReader()) { }  //If called without something like using, the next command will fail, stating one is already running.

                    }

                    comm.Close();
                }

                FriendGrid.Items.Remove(FriendGrid.SelectedItem);
            }
        }

        private void checkin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_UserLocationSet_Click(object sender, RoutedEventArgs e)
        {
            set_usrlocation(textBox_UserLocationLongitude.Text, textBox_UserLocationLatitude.Text);
        }
    }

}
