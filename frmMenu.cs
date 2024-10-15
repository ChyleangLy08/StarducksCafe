using System;
using System.Drawing;
using System.Windows.Forms;
using StarducksManagementSystem.Components;

namespace StarducksManagementSystem
{
    public partial class frmMenu : Form
    {
        private Orders ordersControl;

        public frmMenu()
        {
            InitializeComponent();

            // Initialize the ordersControl here
            ordersControl = new Orders();

            // Add it to the form or a specific panel
            ordersControl.Dock = DockStyle.Fill; // Optional - set docking style
            this.Controls.Add(ordersControl);    // Add to form if not added via designer
        }


        private void frmMenu_Load(object sender, EventArgs e)
        {
            AddItemsToFlowLayout();
            this.ControlBox = false;
        }

        private void AddItemsToFlowLayout()
        {
            WidgetItems icedLatte = CreateWidgetItem("Iced Latte", @"C:\Computer Science\Computer Science Year4\Semester I\OOAD and Prog\Assignment\StarducksManagementSystem\Assets\Group 5626.png", "$3.00", "$3.50", "$4.00");
            flowLayoutPanel1.Controls.Add(icedLatte);
        }

        private WidgetItems CreateWidgetItem(string title, string imagePath, string sPrice, string mPrice, string lPrice)
        {
            WidgetItems widgetItem = new WidgetItems
            {
                ItemTile = title,
                ItemImage = Image.FromFile(imagePath),
                SPrice = sPrice,
                MPrice = mPrice,
                LPrice = lPrice
            };

            widgetItem.ItemAdded += AddOrders_ItemAdded;
            return widgetItem;
        }

        private void AddOrders_ItemAdded(object sender, EventArgs e)
        {
            if (sender is WidgetItems control)
            {
                string selectedSize = control.SelectedSize;
                string title = control.ItemTile;
                Image itemImage = control.ItemImage;
                string price = selectedSize == "S" ? control.SPrice :
                               selectedSize == "M" ? control.MPrice :
                               control.LPrice;

                ordersControl.AddOrderItem(itemImage, title, selectedSize, price);
            }
        }
    }
}