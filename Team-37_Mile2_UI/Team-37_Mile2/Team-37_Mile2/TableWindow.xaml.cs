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
    /// Interaction logic for TableWindow.xaml
    /// </summary>
    public partial class TableWindow : Window
    {
        public TableWindow()
        {
            InitializeComponent();
            addColumnsToGrid();
        }

        public void addColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Date";
            col1.Binding = new Binding("date");
            ReviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "User Name";
            col2.Binding = new Binding("name");
            ReviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Stars";
            col3.Binding = new Binding("stars");
            ReviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";

            //Set the rows to adjust their height to allow the review text to wordwrap.
            //Referenced: https://stackoverflow.com/questions/44507929/wpf-datagrid-how-to-set-specifc-setter-in-c-sharp-in-stead-of-xaml
            //  and https://msdn.microsoft.com/en-us/library/system.windows.controls.textblock.textwrapping(v=vs.110).aspx
            Style newStyle = new Style(typeof(TextBlock));
            newStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            col4.ElementStyle = newStyle;

            col4.Binding = new Binding("text");
            //col4.Width = DataGridLength.Auto; //causes the width to extend to however long needed to fit all of the text on one line.
            col4.Width = 550;
            ReviewGrid.Columns.Add(col4);

        }
    }
}
