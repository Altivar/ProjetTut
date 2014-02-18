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
        List<ScatterCustom> listNode = new List<ScatterCustom>();

        // liste de menu de création de node
        List<KeyValuePair<MenuCreation, Timer>> listMenu = new List<KeyValuePair<MenuCreation, Timer>>();

        // liste de ligne inter-node (trait rose avec triangle)
        List<Line> listLine = new List<Line>();
        List<Polygon> listPoly = new List<Polygon>();

        // gestion de rattache à un parent
        List<KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>> listLigneRattache = new List<KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>>();
        // gestion des poly de rattache à un parent
        List<KeyValuePair<Polygon, ScatterCustom>> listRattache = new List<KeyValuePair<Polygon, ScatterCustom>>();

        // gestion du multi-touch
        List<KeyValuePair<int, KeyValuePair<int, Point>>> listTouch = new List<KeyValuePair<int, KeyValuePair<int, Point>>>();
        int mLimiteNbrTouch = 4;

        //Position du Node initial
        Point initP = new Point(800, 400);

        // timer
        Timer timeRefresh = new Timer();
        
        //Liste des Cercle Chargeur
        List<KeyValuePair<LoadCircle, Timer>> listLoadCircle = new List<KeyValuePair<LoadCircle, Timer>>();









        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // ajout de Nodes
            AddNode(null, initP, "Text");
            AddNode(null, initP, "Image");

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

        void ChoiceMenuTimeExpired(object sender, EventArgs e)
        {
            //
            // SUPPRIME LE MENU EXPIRE
            //
            Timer menuTimer = (Timer)sender;
            if (menuTimer == null)
                return;

            for (int i = 0; i < listMenu.Count; i++)
            {
                if (listMenu.ElementAt(i).Value == menuTimer)
                {
                    this.MainScatterView.Items.Remove(listMenu.ElementAt(i).Key);
                    listMenu.RemoveAt(i);
                    return;
                }
            }
        }

        void circleTimeOut(object sender, EventArgs e)
        {
            Timer circleTimer = (Timer)sender;
            for (int i = 0; i < listLoadCircle.Count; i++)
            {
                if (listLoadCircle.ElementAt(i).Value == circleTimer)
                {
                    this.MainScatterView.Items.Remove(listLoadCircle.ElementAt(i).Key);
                    for (int j = 0; j < listTouch.Count; j++)
                    {
                        if (listLoadCircle.ElementAt(i).Key.Id == listTouch.ElementAt(j).Key)
                        {
                            listTouch.RemoveAt(j);
                            break;
                        }
                    }
                    listLoadCircle.RemoveAt(i);
                    return;
                }
            }
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

        




        //
        //  EVENEMENT
        //
        private void OnPreviewTouchDown(object sender, TouchEventArgs e)
        {
            bool LoadEnable = true;

            //Pour chaque node, verification de la position du touch
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode.ElementAt(i).AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                {
                    LoadEnable = false;
                }
            }

            //Pour chaque node, verification de la position du touch
            for (int i = 0; i < listRattache.Count; i++)
            {
                if (listRattache.ElementAt(i).Key.AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                {
                    LoadEnable = false;
                }
            }


            if (LoadEnable && listTouch.Count < mLimiteNbrTouch)
            {
                // Timer du cercle de chargement
                Timer circleTimer = new Timer();
                circleTimer.Interval = 5000;    // durée de vie maximum : 5s
                circleTimer.Tick += new EventHandler(circleTimeOut);
                circleTimer.Start();
                // Cercle de chargement
                LoadCircle mLCircle = new LoadCircle();
                mLCircle.Id = e.TouchDevice.Id;
                mLCircle.Center = e.TouchDevice.GetPosition(this);

                // Pair du cercle et de son timer
                KeyValuePair<LoadCircle, Timer> myPair = new KeyValuePair<LoadCircle, Timer>(mLCircle, circleTimer);

                listLoadCircle.Add(myPair);
                MainScatterView.Items.Add(mLCircle);


                // on ajoute cette instance de point avec : 
                // son ID
                // sa position
                // son heure d'apparition
                KeyValuePair<int, Point> statTouch = new KeyValuePair<int, Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
                KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int, KeyValuePair<int, Point>>(e.TouchDevice.Id, statTouch);
                listTouch.Add(pairTouch);
            }
        }

        
        

        /// <summary>
        /// Evenement de mouvement de Touch sur la table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {

            // met a jour la ligne de rattache si l'id du touch correspond
            bool isRattache = false;
            for (int i = 0; i < listLigneRattache.Count && !isRattache; i++)
            {
                if (e.TouchDevice.Id == listLigneRattache.ElementAt(i).Value.Key)
                {
                    listLigneRattache.ElementAt(i).Value.Value.X2 = e.TouchDevice.GetPosition(this).X;
                    listLigneRattache.ElementAt(i).Value.Value.Y2 = e.TouchDevice.GetPosition(this).Y;
                    isRattache = true;
                }
            }
            
            
            
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
                            if (listLoadCircle.ElementAt(j).Key.Id == e.TouchDevice.Id)
                            {
                                // on supprime l'ancien cercle
                                MainScatterView.Items.Remove(listLoadCircle.ElementAt(j).Key);
                                listLoadCircle.RemoveAt(j);

                                if (!isRattache)
                                {
                                    // On ajoute le nouveau
                                    //

                                    // Timer du cercle de chargement
                                    Timer circleTimer = new Timer();
                                    circleTimer.Interval = 5000;    // durée de vie maximum : 5s
                                    circleTimer.Tick += new EventHandler(circleTimeOut);
                                    circleTimer.Start();
                                    // Cercle de chargement
                                    LoadCircle mLCircle = new LoadCircle();
                                    mLCircle.Id = e.TouchDevice.Id;
                                    mLCircle.Center = e.TouchDevice.GetPosition(this);

                                    // Pair du cercle et de son timer
                                    KeyValuePair<LoadCircle, Timer> myPair = new KeyValuePair<LoadCircle, Timer>(mLCircle, circleTimer);

                                    listLoadCircle.Add(myPair);
                                    MainScatterView.Items.Add(mLCircle);
                                }

                                done = true;
                            }
                        }
                        listTouch.RemoveAt(i);
                        if (!isRattache)
                        {
                            KeyValuePair<int, Point> statTouch = new KeyValuePair<int, Point>(e.Timestamp, e.TouchDevice.GetPosition(this));
                            KeyValuePair<int, KeyValuePair<int, Point>> pairTouch = new KeyValuePair<int, KeyValuePair<int, Point>>(e.TouchDevice.Id, statTouch);
                            listTouch.Add(pairTouch);
                        }
                        return;
                    }
                }
            }
        }


        private void OnPreviewTouchUp(object sender, TouchEventArgs e)
        {

            for (int i = 0; i < listLigneRattache.Count; i++)
            {
                if (e.TouchDevice.Id == listLigneRattache.ElementAt(i).Value.Key)
                {
                    

                    double length = -1;
                    ScatterCustom nearestText = null;
                    for (int j = 0; j < listNode.Count; j++) // on teste la distance de chaque node
                    {
                        if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j)) // on ne test pas la node a laquelle la ligne est rattachée
                        {
                            if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j).GetParent()) // on test si la node n'a pas pour parent celle a laquelle la ligne est rattachée
                            {
                                double diffXCarre = (listNode.ElementAt(j).ActualCenter.X - listLigneRattache.ElementAt(i).Value.Value.X2);
                                diffXCarre *= diffXCarre;
                                double diffYCarre = (listNode.ElementAt(j).ActualCenter.Y - listLigneRattache.ElementAt(i).Value.Value.Y2);
                                diffYCarre *= diffYCarre;
                                double testLenght = Math.Sqrt(diffXCarre + diffYCarre);
                                if (length == -1 || length > testLenght) // si la node est plus proche que la précédente on la retient
                                {
                                    length = testLenght;
                                    nearestText = listNode.ElementAt(j);
                                }
                            }
                        }
                    }

                    if (length != -1) // si la node la plus proche est assez près
                    {
                        double dimension = (nearestText.Width > nearestText.Height) ? nearestText.Width : nearestText.Height;
                        if (length < dimension / 3 * 2)
                        {
                            listLigneRattache.ElementAt(i).Key.SetParent(nearestText);
                        }
                    }

                    // ensuite on supprime la ligne
                    listLigneRattache.RemoveAt(i);

                }
            }


            for (int i = 0; i < listLoadCircle.Count; i++)
            {
                if (listLoadCircle.ElementAt(i).Key.Id == e.TouchDevice.Id)
                {
                    MainScatterView.Items.Remove(listLoadCircle.ElementAt(i).Key);
                    listLoadCircle.RemoveAt(i);
                }
            }
            
            for (int i = 0; i < listTouch.Count; i++)
            {
                if (listTouch.ElementAt(i).Key == e.TouchDevice.Id)
                {
                    if (e.Timestamp - listTouch.ElementAt(i).Value.Key > 2000)  // si l'appui a duré +2s
                    {
                        MenuCreation ChoiceNode = new MenuCreation(this);
                        ChoiceNode.Center = e.TouchDevice.GetCenterPosition(this);
                        ChoiceNode.Orientation = e.TouchDevice.GetOrientation(this)+90.0;
                        MainScatterView.Items.Add(ChoiceNode);

                        Timer menuLifeTime = new Timer();
                        menuLifeTime.Interval = 3000;
                        menuLifeTime.Tick += new EventHandler(ChoiceMenuTimeExpired);
                        menuLifeTime.Start();

                        KeyValuePair<MenuCreation, Timer> myPair = new KeyValuePair<MenuCreation, Timer>(ChoiceNode, menuLifeTime);
                        listMenu.Add(myPair);
                    }
                    listTouch.RemoveAt(i);
                }
            }

        }



        //
        //  FONCTION DE GESTION DES NODES
        //
        /// <summary>
        /// Ajoute un NODE du type passé en paramètre
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pt"></param>
        /// <param name="typeNode"></param>
        public void AddNode(ScatterCustom parent, Point pt, String typeNode)
        {
            switch (typeNode)
            {
                case "Text":
                    NodeText text = new NodeText(this, parent);
                    text.Center = pt;
                    this.MainScatterView.Items.Add(text);
                    listNode.Add(text);
                    break;
                case "Image":
                    NodeImage image = new NodeImage(this, parent);
                    image.Center = pt;
                    this.MainScatterView.Items.Add(image);
                    listNode.Add(image);
                    break;

            }

        }
        /// <summary>
        /// Supprime le NODE passé en paramètre
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="confirmation"></param>
        public void RemoveNode(ScatterCustom parent, bool confirmation)
        {
            bool conf = confirmation;

            for (int i = 0; i < listLigneRattache.Count; i++)
            {
                if (listLigneRattache.ElementAt(i).Key == parent)
                {
                    listLigneRattache.RemoveAt(i);
                    break;
                }
            }
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

                        // on separe de son parent avant d'effectuer la suppression des enfants
                        // utile dans le cas d'une boucle
                        parent.SetParent(null);

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


        //
        //  REFRESH
        //
        /// <summary>
        /// Fonction de Refresh de la table appelée a chaque tick du timer
        /// </summary>
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


            //
            //  DESSIN DES LIGNES DE RATTACHE
            //
            this.LinkParentLineGrid.Children.RemoveRange(0, this.LinkParentLineGrid.Children.Count);
            this.NearParentEllipseCanvas.Children.RemoveRange(0, this.NearParentEllipseCanvas.Children.Count);
            for (int i = 0; i < listLigneRattache.Count; i++)
            {
                listLigneRattache.ElementAt(i).Value.Value.X1 = listLigneRattache.ElementAt(i).Key.GetOrigin().X;
                listLigneRattache.ElementAt(i).Value.Value.Y1 = listLigneRattache.ElementAt(i).Key.GetOrigin().Y;

                this.LinkParentLineGrid.Children.Add(listLigneRattache.ElementAt(i).Value.Value);


                double length = -1;
                ScatterCustom nearestText = null;
                for (int j = 0; j < listNode.Count; j++) // on teste la distance de chaque node
                {
                    if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j)) // on test si la node est differente de celle a laquelle la ligne est rattachée 
                    {
                        if (listLigneRattache.ElementAt(i).Key != listNode.ElementAt(j).GetParent()) // on test si la node n'a pas pour parent celle a laquelle la ligne est rattachée
                        {
                            double diffXCarre = (listNode.ElementAt(j).ActualCenter.X - listLigneRattache.ElementAt(i).Value.Value.X2);
                            diffXCarre *= diffXCarre;
                            double diffYCarre = (listNode.ElementAt(j).ActualCenter.Y - listLigneRattache.ElementAt(i).Value.Value.Y2);
                            diffYCarre *= diffYCarre;
                            double testLenght = Math.Sqrt(diffXCarre + diffYCarre);
                            if (length == -1 || length > testLenght) // si la node est plus proche que la précédente on la retient
                            {
                                length = testLenght;
                                nearestText = listNode.ElementAt(j);
                            }
                        }
                    }
                }

                if (length != -1) // si la node la plus proche est assez près
                {
                    double dimension = (nearestText.Width > nearestText.Height) ? nearestText.Width : nearestText.Height;
                    if (length < dimension / 3 * 2)
                    {
                        Ellipse ell = new Ellipse();

                        ell.Width = dimension * 3 / 2 - dimension / 6;
                        ell.Height = dimension * 3 / 2 - dimension / 6;
                        ell.Fill = new SolidColorBrush(Colors.Transparent);
                        ell.Stroke = new SolidColorBrush(Colors.DarkGreen);
                        ell.StrokeThickness = 9;

                        DoubleCollection dColl = new DoubleCollection();
                        dColl.Add(10);
                        dColl.Add(5);
                        ell.StrokeDashArray = dColl;

                        this.NearParentEllipseCanvas.Children.Add(ell);
                        Canvas.SetLeft(ell, nearestText.ActualCenter.X - dimension / 3 * 2);
                        Canvas.SetTop(ell, nearestText.ActualCenter.Y - dimension / 3 * 2);
                    }
                }

            }


            //
            //  DESSIN DES LIAISONS INTER-NODES
            //
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
                    pt1.Y -= listNode.ElementAt(i).Height / 2 - 20;
                    pt1 = listNode.ElementAt(i).PointToScreen(pt1);
                    // placement du premier point de la pseudo-ellipse
                    pt2.X -= 50;
                    pt2.Y -= listNode.ElementAt(i).Height / 2 + 20;
                    pt2 = listNode.ElementAt(i).PointToScreen(pt2);
                    // placement du premier point de la pseudo-ellipse
                    pt3.X -= 20;
                    pt3.Y -= listNode.ElementAt(i).Height / 2 + 40;
                    pt3 = listNode.ElementAt(i).PointToScreen(pt3);
                    // placement du premier point de la pseudo-ellipse
                    pt4.X += 20;
                    pt4.Y -= listNode.ElementAt(i).Height / 2 + 40;
                    pt4 = listNode.ElementAt(i).PointToScreen(pt4);
                    // placement du premier point de la pseudo-ellipse
                    pt5.X += 50;
                    pt5.Y -= listNode.ElementAt(i).Height / 2 + 20;
                    pt5 = listNode.ElementAt(i).PointToScreen(pt5);
                    // placement du premier point de la pseudo-ellipse
                    pt6.X += 60;
                    pt6.Y -= listNode.ElementAt(i).Height / 2 - 20;
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
                    KeyValuePair<Polygon, ScatterCustom> myPair = new KeyValuePair<Polygon, ScatterCustom>(poly, listNode.ElementAt(i));
                    listRattache.Add(myPair);
                    this.LinkParentGrid.Children.Add(poly);

                }
            }


        }
        //
        //  FIN REFRESH



        /// <summary>
        /// Evenement Touch sur un Cercle Vert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGreenCirclePreviewTouchDown(object sender, TouchEventArgs e)
        {

            ScatterCustom text = null;
            for (int i = 0; i < listRattache.Count; i++)
            {
                if (listRattache.ElementAt(i).Key == (Polygon)sender)
                {
                    text = listRattache.ElementAt(i).Value;
                }
            }

            if (text != null)
            {
                
                bool isSet = false;
                for (int i = 0; i < listLigneRattache.Count && !isSet; i++)
                {
                    if (listLigneRattache.ElementAt(i).Key == text)
                    {
                        isSet = true;
                    }
                }

                if (!isSet)
                {
                    Line ligne = new Line();
                    ligne.X1 = text.GetOrigin().X;
                    ligne.Y1 = text.GetOrigin().Y;
                    ligne.X2 = e.TouchDevice.GetPosition(this).X;
                    ligne.Y2 = e.TouchDevice.GetPosition(this).Y;

                    ligne.Stroke = new SolidColorBrush(Colors.DarkGreen);
                    ligne.StrokeThickness = 6;

                    KeyValuePair<int, Line> myFirstPair = new KeyValuePair<int, Line>(e.TouchDevice.Id, ligne);
                    KeyValuePair<ScatterCustom, KeyValuePair<int, Line>> myPair = new KeyValuePair<ScatterCustom, KeyValuePair<int, Line>>(text, myFirstPair);

                    listLigneRattache.Add(myPair);
                }

            }

        }




        //
        // MENU SELECTION TYPE NODE
        //
        public void MenuIsClicked(MenuCreation menu, String choice)
        {
            AddNode(null, menu.ActualCenter, choice);

            try
            {
                MainScatterView.Items.Remove(menu);
            }
            catch { }
        }





        ///////////////////
            //FIN DES FONCTIONS !!!
    }
}