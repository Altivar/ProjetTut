using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace AppliProjetTut
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        // liste de node
        List<NodeText> listNode = new List<NodeText>();

        // liste de ligne inter-node
        List<Line> listLine = new List<Line>();

        //Départ du hold
        int start;

        //Position du Node initial
        Point initP = new Point(800, 400);
        

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();


            // ajout de Claviers
            // AddClavier();

            // ajout de Nodes
            //AddNode(null, initP);

            PreviewTouchMove += new EventHandler<TouchEventArgs>(OnPreviewTouchMove);
            PreviewTouchDown += new EventHandler<TouchEventArgs>(OnPreviewTouchDown);
            PreviewTouchUp += new EventHandler<TouchEventArgs>(OnPreviewTouchUp);

            ManipulationInertiaStarting += new EventHandler<ManipulationInertiaStartingEventArgs>(SurfaceWindow1_ManipulationInertiaStarting);

        }

        void SurfaceWindow1_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            
        }

        

        

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        public void AddNode(NodeText parent, Point pt)
        {
            NodeText text = new NodeText(this, parent);
            text.Center = pt;
            
            this.MainScatterView.Items.Add(text);
            listNode.Add(text);
        }

        private void OnPreviewTouchDown(object sender, TouchEventArgs e)
        {
            start = e.Timestamp;
        }
        
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {
            start = e.Timestamp;
            for (int i = 0; i < listLine.Count; i++)
            {
                this.MainGrid.Children.Remove(listLine.ElementAt(i));
            }
            listLine.Clear();
            for (int i = 1; i < listNode.Count; i++)
            {
                Line tempLine = listNode.ElementAt(i).getLineToParent();
                listLine.Add(tempLine);
                Line line1 = new Line();
                line1.Stroke = System.Windows.Media.Brushes.PaleVioletRed;
                line1.StrokeThickness = 2;
                line1.X1 = tempLine.X2 + 30;
                line1.Y1 = tempLine.Y2 + 30;
                line1.X2 = tempLine.X2 - 30;
                line1.Y2 = tempLine.Y2 - 30;
                listLine.Add(line1);
                Line line2 = new Line();
                line2.Stroke = System.Windows.Media.Brushes.PaleVioletRed;
                line2.StrokeThickness = 2;
                line2.X1 = tempLine.X2 - 30;
                line2.Y1 = tempLine.Y2 + 30;
                line2.X2 = tempLine.X2 + 30;
                line2.Y2 = tempLine.Y2 - 30;
                listLine.Add(line2);

                this.MainGrid.Children.Add(line1);
                this.MainGrid.Children.Add(line2);
                this.MainGrid.Children.Add(tempLine);

            }
        }


        private void OnPreviewTouchUp(object sender, TouchEventArgs e)
        {
            //créé un nouveau node après 2 secondes
            if (e.Timestamp - start > 2000)
            {
                Point pt = e.TouchDevice.GetPosition(this);
                AddNode(null, pt);
                start = 0;
            }
        }
        
    }
}