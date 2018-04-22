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
using System.Windows.Shapes;

namespace Team37_Mile2
{
    /// <summary>
    /// Interaction logic for GraphWindow.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        public GraphWindow()
        {
            InitializeComponent();
        }

        public void SetChart(List<KeyValuePair<string, int>> data) {
            this.checkinChart.DataContext = data;

            // -- DEBUG --
            /*List<KeyValuePair<string, int>> myChartData = new List<KeyValuePair<string, int>>();
            myChartData.Add(new KeyValuePair<string, int>("Sun", 50));

            this.checkinChart.DataContext = myChartData;*/

        }
    }
}
