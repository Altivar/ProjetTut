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
using System.Windows.Forms;
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
        List<Polygon> listPoly = new List<Polygon>();

        //Départ du hold
        Point ptStartHold;
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

            // ajout de Nodes
            AddNode(null, initP);

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
        public void RemoveNode(NodeText parent, bool confirmation)
        {
            bool conf = confirmation;
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode.ElementAt(i).GetParent() == parent)
                {
                    if (conf)
                    {
                        // demande de confirmation
                        string title = "Warning";
                        string message = "Remove this Text will remove automatically its children. Remove?";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult res;

                        res = System.Windows.Forms.MessageBox.Show(message, title, buttons);

                        if (res == System.Windows.Forms.DialogResult.No)
                        {
                            // On annule la suppression
                            return;
                        }
                        conf = false;
                    }
                    RemoveNode(listNode.ElementAt(i), false);
                    i--;
                }
            }
            listNode.Remove(parent);
            this.MainScatterView.Items.Remove(parent);
            if (confirmation)
            {
                RefreshImage();
            }

        }

        private void OnPreviewTouchDown(object sender, TouchEventArgs e)
        {
            start = e.Timestamp;
            ptStartHold = e.TouchDevice.GetPosition(this);
        }
        
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {
            double diffX = ptStartHold.X - e.TouchDevice.GetPosition(this).X;
            double diffY = ptStartHold.Y - e.TouchDevice.GetPosition(this).Y;

            if (diffX * diffX + diffY * diffY > 900)
            {
                start = e.Timestamp;
            }

            RefreshImage();
            
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


        private void RefreshImage()
        {
            for (int i = 0; i < listLine.Count; i++)
            {
                this.LineGrid.Children.Remove(listLine.ElementAt(i));
            }
            listLine.Clear();
            for (int i = 0; i < listPoly.Count; i++)
            {
                this.LineGrid.Children.Remove(listPoly.ElementAt(i));
            }
            listPoly.Clear();
            for (int i = 0; i < listNode.Count; i++)
            {
                Line tempLine = listNode.ElementAt(i).getLineToParent();
                if (!(tempLine.X1 == 0 && tempLine.Y1 == 0 && tempLine.X2 == 0 && tempLine.Y2 == 0))
                {
                    listLine.Add(tempLine);
                    this.LineGrid.Children.Add(tempLine);
                    Polygon triangle = new Polygon();
                    double pourcentage = Math.Sqrt(100 / ((tempLine.X1 - tempLine.X2) * (tempLine.X1 - tempLine.X2) + (tempLine.Y1 - tempLine.Y2) * (tempLine.Y1 - tempLine.Y2)));
                    double Xplus = (tempLine.X1 + tempLine.X2) / 2 - pourcentage * (tempLine.X2 - tempLine.X1);
                    double Yplus = (tempLine.Y1 + tempLine.Y2) / 2 - pourcentage * (tempLine.Y2 - tempLine.Y1);

                    double XVect = Xplus - (tempLine.X1 + tempLine.X2) / 2;
                    double YVect = Yplus - (tempLine.Y1 + tempLine.Y2) / 2;

                    Point ptMilieu = new Point((tempLine.X1 + tempLine.X2) / 2, (tempLine.Y1 + tempLine.Y2) / 2);
                    Point ptFleche1 = new Point(Xplus + YVect, Yplus - XVect);
                    Point ptFleche2 = new Point(Xplus - YVect, Yplus + XVect);

                    PointCollection triangleCollection = new PointCollection();
                    triangleCollection.Add(ptMilieu);
                    triangleCollection.Add(ptFleche1);
                    triangleCollection.Add(ptFleche2);
                    triangleCollection.Add(ptMilieu);
                    triangle.Points = triangleCollection;
                    triangle.Stroke = Brushes.PaleVioletRed;
                    triangle.Fill = Brushes.PaleVioletRed;
                    listPoly.Add(triangle);

                    this.LineGrid.Children.Add(triangle);
                }
            }
        }
        
    }
}