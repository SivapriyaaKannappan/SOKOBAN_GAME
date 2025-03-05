using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SOKOBAN_ASSESSMENT
{
    class MEDMODE : Window
    {
        private Canvas windowCanvas { get; set; }
        private Button returnButton { get; set; }
        private TextBlock instructionBlock { get; set; }
        private Border gridBorder { get; set; }
        public Grid appGrid { get; set; }

        private PopulateGrid populatedGrid1 { get; set; }
        public int penguinRow { get; set; }
        public int penguinColumn { get; set; }




        public MEDMODE(string windowName)
        {
            this.Title = windowName;
            initializeWindow1();
        }

        private void initializeWindow1()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowCanvas = new Canvas();
            createGrid();
            createSidePanel();
            appGrid.Focus();

            //populatedGrid1 = new PopulateGrid(this);
            populatedGrid1.drawGrid1();

            this.Content = this.windowCanvas;

            setupPageEvents();
        }

        private void createGrid()
        {
            gridBorder = new Border();
            gridBorder.BorderThickness = new Thickness(11.00);
            gridBorder.BorderBrush = Brushes.Black;

            appGrid = new Grid();
            appGrid.Width = appGrid.Height = 400;
            appGrid.HorizontalAlignment = HorizontalAlignment.Left;
            appGrid.VerticalAlignment = VerticalAlignment.Top;
            appGrid.Focusable = true;
            gridBorder.Child = appGrid;

            for (int i = 0; i < 10; i++)
                appGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 10; i++)
                appGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void createSidePanel()
        {
            instructionBlock = new TextBlock();
            instructionBlock.FontSize = 25;
            instructionBlock.FontFamily = new FontFamily("Goudy Stout");
            instructionBlock.Text = "Welcome to Medium mode";
            instructionBlock.HorizontalAlignment = HorizontalAlignment.Left;

            returnButton = new Button();
            returnButton.Height = 50;
            returnButton.Width = 350;
            returnButton.FontSize = 15;
            returnButton.Focusable = false;
            returnButton.Content = "Return to Main";
            returnButton.FontFamily = new FontFamily("Goudy Stout");
            arrangeOnCanvas();

        }

        private void arrangeOnCanvas()
        {
            windowCanvas.Children.Add(instructionBlock);
            windowCanvas.Children.Add(returnButton);
            windowCanvas.Children.Add(gridBorder);

            Canvas.SetLeft(instructionBlock, 490);
            Canvas.SetTop(instructionBlock, 100);

            Canvas.SetLeft(returnButton, 490);
            Canvas.SetTop(returnButton, 380);
        }

        protected void returnButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void setupPageEvents()
        {
            returnButton.Click += returnButton_Click;
        }

    }


}


