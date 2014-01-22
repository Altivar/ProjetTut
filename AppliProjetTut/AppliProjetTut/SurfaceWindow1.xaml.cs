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

        // liste de ligne inter-node (trait rose avec triangle)
        List<Line> listLine = new List<Line>();
        List<Polygon> listPoly = new List<Polygon>();

        // gestion de rattache à un parent
        // gestion des poly de rattache à un parent
        List<KeyValuePair<Polygon, NodeText>> listRattache = new List<KeyValuePair<Polygon, NodeText>>();

        // gestion du multi-touch
        List<KeyValuePair<int, KeyValuePair<int, Point>>> listTouch = new List<KeyValuePair<int, KeyValuePair<int, Point>>>();

        //Position du Node initial
        Point initP = new Point(800, 400);

        // timer
        Timer timeRefresh = new Timer();
        
        //Liste des Cercle Chargeur
        List<LoadCircle> listLoadCircle = new List<LoadCircle>();

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

            timeRefresh.Interval = 30;
            timeRefresh.Tick += new EventHandler(TimerRefresh);
            timeRefresh.Start();

        }

        void TimerRefresh(object sender, EventArgs e)
        {
            RefreshImage();
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
            //Cercle de chargement
            LoadCircle mLCircle = new LoadCircle();
            mLCircle.Id = e.TouchDevice.Id;
            mLCircle.Center = e.TouchDevice.GetPosition(this);
            listLoadCircle.Add(mLCircle);
            MainScatterView.Items.Add(mLCircle);


            // on ajoute cette instance de point avec : 
            // son ID
            // sa position
            // son heure d'apparition
            KeyValuePair<int, Point> statTouch = new KeyValuePair<int,Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
            KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int,KeyValuePair<int,Point>>(e.TouchDevice.Id, statTouch);
            listTouch.Add(pairTouch);
        }
        
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {
            for (int i = 0; i < listTouch.Count; i++)
            {
                if (listTouch.ElementAt(i).Key == e.TouchDevice.Id)
                {
                    double diffX = listTouch.ElementAt(i).Value.Value.X - e.TouchDevice.GetPosition(this).X;
                    double diffY = listTouch.ElementAt(i).Value.Value.Y - e.TouchDevice.GetPosition(this).Y;
                    if (diffX * diffX + diffY * diffY > 900)    // si le déplacement depuis le point de départ est plus grand que 30pxl, on reinit le timer
                    {
                        bool done = false;
                        for (int j = 0; j < listLoadCircle.Count && !done; j++)
                        {
                            if (listLoadCircle.ElementAt(j).Id == e.TouchDevice.Id)
                            {
                                // on supprime l'ancien cercle
                                MainScatterView.Items.Remove(listLoadCircle.ElementAt(j));
                                listLoadCircle.RemoveAt(j);
                                // on ajoute le nouveau
                                LoadCircle mLCircle = new LoadCircle();
                                
                                mLCircle.Id = e.TouchDevice.Id;
                                mLCircle.Center = e.TouchDevice.GetPosition(this);
                                listLoadCircle.Add(mLCircle);
                                MainScatterView.Items.Add(mLCircle);

                                done = true;
                            }
                        }
                        listTouch.RemoveAt(i);
                        KeyValuePair<int, Point> statTouch = new KeyValuePair<int, Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
                        KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int, KeyValuePair<int, Point>>(e.TouchDevice.Id, statTouch);
                        listTouch.Add(pairTouch);
                        return;
                    }
                }
            }
        }


        private void OnPreviewTouchUp(object sender, TouchEventArgs e)
        {
            for (int i = 0; i < listLoadCircle.Count; i++)
            {
                if (listLoadCircle.ElementAt(i).Id == e.TouchDevice.Id)
                {
                    MainScatterView.Items.Remove(listLoadCircle.ElementAt(i));
                    listLoadCircle.RemoveAt(i);
                }
            }
            
            for (int i = 0; i < listTouch.Count; i++)
            {
                if (listTouch.ElementAt(i).Key == e.TouchDevice.Id)
                {
                    if (e.Timestamp - listTouch.ElementAt(i).Value.Key > 2000)  // si l'appui a duré +2s
                    {
                        Point pt = e.TouchDevice.GetPosition(this);             // on crée un NodeText
                        AddNode(null, pt);
                    }
                    listTouch.RemoveAt(i);
                }
            }

        }


        private void RefreshImage()
        {
            // on efface toutes les lignes des liens internode
            this.LineGrid.Children.RemoveRange(0, this.LineGrid.Children.Count);

            // on efface tous les cercles de chargement verts
            listPoly.Clear();
            this.LinkParentGrid.Children.RemoveRange(0, this.LinkParentGrid.Children.Count);

            
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
                    // on dessine la ligne
                    this.LineGrid.Children.Add(tempLine);

                    Polygon triangle = new Polygon();
                    double pourcentage = Math.Sqrt(400 / ((tempLine.X1 - tempLine.X2) * (tempLine.X1 - tempLine.X2) + (tempLine.Y1 - tempLine.Y2) * (tempLine.Y1 - tempLine.Y2)));
                    double Xplus = (tempLine.X1 + tempLine.X2) / 2 - pourcentage * (tempLine.X2 - tempLine.X1);
                    double Yplus = (tempLine.Y1 + tempLine.Y2) / 2 - pourcentage * (tempLine.Y2 - tempLine.Y1);

                    double XVect = Xplus - (tempLine.X1 + tempLine.X2) / 2;
                    double YVect = Yplus - (tempLine.Y1 + tempLine.Y2) / 2;

                    Point ptMilieu = new Point((tempLine.X1 + tempLine.X2) / 2, (tempLine.Y1 + tempLine.Y2) / 2);
                    Point ptFleche1 = new Point(Xplus + YVect, Yplus - XVect);
                    Point ptFleche2 = new Point(Xplus - YVect, Yplus + XVect);

                    // on crée le triangle a partir des points calculés précedemment
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
                else
                {

                    Polygon poly = new Polygon();

                    Point pt1 = listNode.ElementAt(i).PointFromScreen(listNode.ElementAt(i).ActualCenter);
                    Point pt2 = pt1;
                    Point pt3 = pt1;
                    Point pt4 = pt1;
                    Point pt5 = pt1;
                    Point pt6 = pt1;
                    // placement du premier point de la pseudo-ellipse
                    pt1.X -= 60;
                    pt1.Y -= 80;
                    pt1 = listNode.ElementAt(i).PointToScreen(pt1);
                    // placement du premier point de la pseudo-ellipse
                    pt2.X -= 50;
                    pt2.Y -= 120;
                    pt2 = listNode.ElementAt(i).PointToScreen(pt2);
                    // placement du premier point de la pseudo-ellipse
                    pt3.X -= 20;
                    pt3.Y -= 140;
                    pt3 = listNode.ElementAt(i).PointToScreen(pt3);
                    // placement du premier point de la pseudo-ellipse
                    pt4.X += 20;
                    pt4.Y -= 140;
                    pt4 = listNode.ElementAt(i).PointToScreen(pt4);
                    // placement du premier point de la pseudo-ellipse
                    pt5.X += 50;
                    pt5.Y -= 120;
                    pt5 = listNode.ElementAt(i).PointToScreen(pt5);
                    // placement du premier point de la pseudo-ellipse
                    pt6.X += 60;
                    pt6.Y -= 80;
                    pt6 = listNode.ElementAt(i).PointToScreen(pt6);

                    // création de la PointCollection qui génerera la forme
                    PointCollection polyCollection = new PointCollection();
                    polyCollection.Add(pt1);
                    polyCollection.Add(pt2);
                    polyCollection.Add(pt3);
                    polyCollection.Add(pt4);
                    polyCollection.Add(pt5);
                    polyCollection.Add(pt6);
                    polyCollection.Add(pt1);

                    // on ajoute les points au poly
                    poly.Points = polyCollection;

                    poly.Fill = new SolidColorBrush(Colors.Green);
                    poly.Stroke = new SolidColorBrush(Colors.DarkGreen);
                    poly.StrokeThickness = 3;



                    poly.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnGreenCirclePreviewTouchDown);

                    // on dessine le poly
                    KeyValuePair<Polygon, NodeText> myPair = new KeyValuePair<Polygon, NodeText>(poly, listNode.ElementAt(i));
                    listRattache.Add(myPair);
                    this.LinkParentGrid.Children.Add(poly);

                }
            }
        }




        /// <summary>
        /// Evenement Touch sur un Cercle Vert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGreenCirclePreviewTouchDown(object sender, TouchEventArgs e)
        {
            


        }







        ///////////////////
            //FIN DES FONCTIONS !!!
    }
}