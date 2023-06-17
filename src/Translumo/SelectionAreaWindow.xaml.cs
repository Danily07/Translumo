using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Translumo
{
    public partial class SelectionAreaWindow : Window
    {
        public Point MouseInitialPos { get; private set; }
        public Point MouseEndPos { get; private set; }

        private bool _mouseIsDown = false; // Set to 'true' when mouse is held down.
        private Point _relativeInitialPos; // The point where the mouse button was clicked down.

        public SelectionAreaWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                CloseDialog(true);
                return;
            }

            // Capture and track the mouse.
            _mouseIsDown = true;
            _relativeInitialPos = e.GetPosition(this);
            MouseInitialPos = this.PointToScreen(_relativeInitialPos);

            theGrid.CaptureMouse();

            // Initial placement of the drag selection box.         
            Canvas.SetLeft(selectionBox, _relativeInitialPos.X);
            Canvas.SetTop(selectionBox, _relativeInitialPos.Y);
            selectionBox.Width = 0;
            selectionBox.Height = 0;

            selectionBox.Fill = new SolidColorBrush(Color.FromRgb(191, 255, 40));

            // Make the drag selection box visible.
            selectionBox.Visibility = Visibility.Visible;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture and stop tracking it.
            _mouseIsDown = false;
            theGrid.ReleaseMouseCapture();

            // Hide the drag selection box.
            selectionBox.Visibility = Visibility.Collapsed;

            MouseEndPos = this.PointToScreen(e.GetPosition(this));

            CloseDialog(false);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
            {
                // When the mouse is held down, reposition the drag selection box.

                Point mousePos = e.GetPosition(theGrid);

                if (_relativeInitialPos.X < mousePos.X)
                {
                    Canvas.SetLeft(selectionBox, _relativeInitialPos.X);
                    selectionBox.Width = mousePos.X - _relativeInitialPos.X;
                }
                else
                {
                    Canvas.SetLeft(selectionBox, mousePos.X);
                    selectionBox.Width = _relativeInitialPos.X - mousePos.X;
                }

                if (_relativeInitialPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(selectionBox, _relativeInitialPos.Y);
                    selectionBox.Height = mousePos.Y - _relativeInitialPos.Y;
                }
                else
                {
                    Canvas.SetTop(selectionBox, mousePos.Y);
                    selectionBox.Height = _relativeInitialPos.Y - mousePos.Y;
                }
            }
        }

        private void CloseDialog(bool cancellation)
        {
            this.DialogResult = !cancellation;
            Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }
    }
}
