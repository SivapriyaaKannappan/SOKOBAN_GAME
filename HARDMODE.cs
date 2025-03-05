using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SOKOBAN_ASSESSMENT
{
    class HARDMODE : Window
    {
        private Canvas windowCanvas { get; set; }
        private Button returnButton { get; set; }
        private TextBlock instructionBlock { get; set; }
        private Border gridBorder { get; set; }
        public Grid appGrid { get; set; }





        public HARDMODE(string windowName)
        {
            this.Title = windowName;
            initializeWindow();
        }

        private void initializeWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowCanvas = new Canvas();
            createGrid();
            createSidePanel();
            appGrid.Focus();

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

            for (int i = 0; i < 15; i++)
                appGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 15; i++)
                appGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void createSidePanel()
        {
            instructionBlock = new TextBlock();
            instructionBlock.FontSize = 25;
            instructionBlock.Text = "Use Arrow keys to move around the screen!";

            returnButton = new Button();
            returnButton.Height = 30;
            returnButton.Width = 245;
            returnButton.FontSize = 25;
            returnButton.Focusable = false;
            returnButton.Content = "Return to the start page.";

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
