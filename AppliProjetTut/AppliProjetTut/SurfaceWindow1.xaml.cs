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

using System.IO;

namespace AppliProjetTut
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        //
        //  GESTION DU FICHIER
        //

        // nom du fichier ouvert
        string nomFichier = "<>";
        bool isModified = false;
        

        //
        ////

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
        List<KeyValuePair<Ellipse, ScatterCustom>> listRattache = new List<KeyValuePair<Ellipse, ScatterCustom>>();

        // gestion du multi-touch
        List<KeyValuePair<int, KeyValuePair<int, Point>>> listTouch = new List<KeyValuePair<int, KeyValuePair<int, Point>>>();
        int mLimiteNbrTouch = 4;

        // Position du Node initial
        Point initP = new Point(800, 400);

        // timer
        Timer timeRefresh = new Timer();
        
        //Liste des Cercle Chargeur
        List<KeyValuePair<LoadCircle, Timer>> listLoadCircle = new List<KeyValuePair<LoadCircle, Timer>>();


        // tag du menu principal
        MenuPrincipal menuPrincipal;






        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            // si le dossier de sauvegarde n'existe pas : on le crée
            DirectoryInfo SavesDir = new DirectoryInfo(".\\Saves\\");
            if (!SavesDir.Exists)
                SavesDir.Create();

            // ajout de Nodes
            AddNode(null, initP, "Text");
            AddNode(null, initP, "Image");
            Modification(false);

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

            if (e.TouchDevice.GetIsFingerRecognized())
            {
                
                // Verification pour le menu principal si le touch est dessus
                if (menuPrincipal != null)
                {
                    if (menuPrincipal.AreAnyTouchesOver)
                    {
                        return;
                    }
                }

                //Pour chaque node, verification de la position du touch
                for (int i = 0; i < listNode.Count; i++)
                {
                    if (listNode.ElementAt(i).AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                    {
                        return;
                    }
                }

                //Pour chaque node, verification de la position du touch
                for (int i = 0; i < listRattache.Count; i++)
                {
                    if (listRattache.ElementAt(i).Key.AreAnyTouchesOver)//Si il est dessus, le chargement est impossible
                    {
                        return;
                    }
                }


                if (listTouch.Count < mLimiteNbrTouch)
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
        }

        
        

        /// <summary>
        /// Evenement de mouvement de Touch sur la table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
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
                                Modification(true);
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
                            ChoiceNode.Orientation = e.TouchDevice.GetOrientation(this) + 90.0;
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
        }


        ////                         ////
        //                             //
        //  GESTION du MENU PRINCIPAL  //
        //                             //
        ////                         ////
        /// <summary>
        /// Appelé lorsqu'on ajoute un tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnVisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            // gestion du tag
            MenuPrincipal mainMenu = (MenuPrincipal)e.TagVisualization;
            
            if (mainMenu != null)
            {
                menuPrincipal = mainMenu;
                menuPrincipal.SaveAsButton.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnSaveAsButtonPreviewTouchUp);
                menuPrincipal.SaveButton.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnSaveButtonPreviewTouchUp);
                menuPrincipal.OpenButton.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnOpenButtonPreviewTouchUp);
            }
        }

        

        

        /// <summary>
        /// Appelé lorsqu'on enleve un tag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TagVisualizer_VisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            if (menuPrincipal == (MenuPrincipal)e.TagVisualization)
            {
                menuPrincipal = null;
            }
        }

        /// <summary>
        /// Appelé lorsque le bouton de sauvegarde du menu principal est appuyé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSaveAsButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (menuPrincipal == null)
                return;

            NodeText SaveFileNameEntrance = new NodeText(this, null);
            SaveFileNameEntrance.TransformToFileSaver();
            SaveFileNameEntrance.SetParent(SaveFileNameEntrance);
            SaveFileNameEntrance.GetClavier().Enter.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnValidateFileName);
            SaveFileNameEntrance.GetClavier().Entrer.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnValidateFileName);
            SaveFileNameEntrance.GetClavier().close.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnClosePreviewTouchDown);

            menuPrincipal.FormGrid.Children.Clear();
            menuPrincipal.FormGrid.Children.Add(SaveFileNameEntrance);
        }

        /// <summary>
        /// Appelé lorsque le bouton de sauvegarde du menu principal est appuyé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSaveButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (menuPrincipal == null)
                return;

            if (nomFichier == "<>")
            {
                NodeText SaveFileNameEntrance = new NodeText(this, null);
                SaveFileNameEntrance.TransformToFileSaver();
                SaveFileNameEntrance.SetParent(SaveFileNameEntrance);
                SaveFileNameEntrance.GetClavier().Enter.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnValidateFileName);
                SaveFileNameEntrance.GetClavier().Entrer.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnValidateFileName);
                SaveFileNameEntrance.GetClavier().close.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnClosePreviewTouchDown);

                menuPrincipal.FormGrid.Children.Clear();
                menuPrincipal.FormGrid.Children.Add(SaveFileNameEntrance);
            }
            else
            {
                CreateDirectory(nomFichier);
            }
        }


        /// <summary>
        /// Appelé lorsque le bouton d'ouverture de fichier est appuyé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOpenButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {

            if (menuPrincipal == null)
                return;

            ListeSauvegarde listSave = new ListeSauvegarde(this);
            
            menuPrincipal.FormGrid.Children.Clear();
            menuPrincipal.FormGrid.Children.Add(listSave);

        }


        /// <summary>
        /// Appelé lorsque le nom du fichier de suavegarde est validé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnValidateFileName(object sender, TouchEventArgs e)
        {

            NodeText saveFileNameText;
            string NomSauvegarde = "<>";
            for (int i = 0; i < menuPrincipal.FormGrid.Children.Count; i++)
            {
                saveFileNameText = (NodeText)menuPrincipal.FormGrid.Children[i];
                if (saveFileNameText != null)
                {
                    NomSauvegarde = saveFileNameText.GetText();
                }
            }

            if (NomSauvegarde == "<>")
            {
                return;
            }
            else
            {
                char[] separator = { '\r' };
                NomSauvegarde = NomSauvegarde.Split(separator).First();
            }

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves");
            try
            {
                DirectoryInfo[] DirFiles = dirInfo.GetDirectories();
                foreach (DirectoryInfo dir in DirFiles)
                {
                    if (dir.Name == NomSauvegarde)
                    {
                        // demande de confirmation
                        string title = "Attention";
                        string message = "Ecraser le dossier de sauvegarde existant?";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult res;

                        res = System.Windows.Forms.MessageBox.Show(message, title, buttons);

                        if (res == System.Windows.Forms.DialogResult.Yes)
                        {
                            // on supprime le dossier existant
                            dir.Delete(true);
                        }
                        else
                        {
                            // On annule la suppression
                            return;
                        }
                    }
                }

                CreateDirectory(NomSauvegarde);

            }
            catch { }
        }

        /// <summary>
        /// Appelé lorsque le bouton "Close" du clavier de sauvegarde est appuyé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClosePreviewTouchDown(object sender, TouchEventArgs e)
        {
            menuPrincipal.FormGrid.Children.Clear();
        }

        /// <summary>
        /// Crée le dossier de la sauvegarde
        /// </summary>
        /// <param name="path"></param>
        void CreateDirectory(string path)
        {


            DirectoryInfo newDir = new DirectoryInfo(".\\Saves\\" + path);
            newDir.Create();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < listNode.Count; i++)
            {

                string type = listNode.ElementAt(i).GetTypeOfNode();
                switch (type)
                {
                    case "Text":
                        {
                            NodeText myText = (NodeText)listNode.ElementAt(i);
                            if (myText == null)
                                break;
                            sb.AppendLine("<node type='txt' id='" + i + "' color='" + myText.GetColor().ToString() + "'>");
                            int numParent = GetNumOfParent(listNode.ElementAt(i));
                            if (numParent != -1)
                                sb.AppendLine("<parent id='" + numParent + "'/>");
                            if(myText.GetText() != "")
                            {
                                sb.AppendLine("<text>");
                                sb.AppendLine(myText.GetText());
                                sb.AppendLine("</text>");
                            }
                            sb.AppendLine("</node>");
                            break;
                        }
                    case "Image":
                        {
                            NodeImage myImage = (NodeImage)listNode.ElementAt(i);
                            if (myImage == null)
                                break;
                            sb.AppendLine("<node type='img' id='" + i + "'>");
                            int numParent = GetNumOfParent(listNode.ElementAt(i));
                            if (numParent != -1)
                                sb.AppendLine("<parent id='" + numParent + "'/>");
                            if (myImage.GetImagePath() != "NONE")
                                sb.AppendLine("<image path='" + myImage.GetImagePath() + "'>");
                            sb.AppendLine("</node>");
                            try
                            {
                                SaveImageToFile(myImage.GetImagePath(), path);
                            }
                            catch { };
                            break;
                        }
                }

                sb.AppendLine();

            }
            
            FileStream myFile = new FileStream(".\\Saves\\" + path + "\\saveFile.mms", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(myFile);
            sw.Write(sb.ToString());
            sw.Close();

            menuPrincipal.FormGrid.Children.Clear();

            Modification(false);
            nomFichier = path;

        }

        /// <summary>
        /// Récupère la position du parent dans la liste
        /// </summary>
        /// <param name="scatt"></param>
        /// <returns></returns>
        private int GetNumOfParent(ScatterCustom scatt)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if (scatt.GetParent() == listNode.ElementAt(i))
                {
                    return i;
                }
            }

            return -1;
        }

        private void SaveImageToFile(string imgPath, string savePath)
        {
            if (imgPath == "NONE")
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves\\"+savePath+"\\Images");
            if (!dirInfo.Exists)
                dirInfo.Create();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(".\\Resources\\Images\\" + imgPath, UriKind.Relative);
            bi.EndInit();

            string separator = ".";
            string imgFormat = imgPath.Split(separator.ToCharArray()).Last();
            
            string imgLocation = ".\\Saves\\" + savePath + "\\Images\\" + imgPath;
            FileStream fileStr = new FileStream(imgLocation, FileMode.Create);

            switch (imgFormat)
            { 
                case "jpg":
                    JpegBitmapEncoder encoderJPG = new JpegBitmapEncoder();
                    encoderJPG.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                    encoderJPG.Save(fileStr);
                    break;
                case "png":
                    PngBitmapEncoder encoderPNG = new PngBitmapEncoder();
                    encoderPNG.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                    encoderPNG.Save(fileStr);
                    break;
                case "gif":
                    GifBitmapEncoder encoderGIF = new GifBitmapEncoder();
                    encoderGIF.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                    encoderGIF.Save(fileStr);
                    break;
                case "bmp":
                    BmpBitmapEncoder encoderBMP = new BmpBitmapEncoder();
                    encoderBMP.Frames.Add(BitmapFrame.Create((BitmapImage)bi));
                    encoderBMP.Save(fileStr);
                    break;
            }

        }

        /// <summary>
        /// Appelé lors de la selection d'un fichier a ouvrir
        /// </summary>
        /// <param name="filename"></param>
        public void OpenFile(string filename)
        {

            if (filename == "<Annuler>")
            {
                menuPrincipal.FormGrid.Children.Clear();
                return;
            }

            listNode.Clear();
            MainScatterView.Items.Clear();

            string line = "";
            string filePath = ".\\Saves\\" + filename + "\\saveFile.mms";
            StreamReader sr = new StreamReader(filePath);

            List<KeyValuePair<KeyValuePair<int, int>, ScatterCustom>> listParent = new List<KeyValuePair<KeyValuePair<int, int>, ScatterCustom>>();
            string currentType = "";
            int compteur = 0;
            bool isFirstLine = true;
            bool isInText = false;
            KeyValuePair<int, int> myPair = new KeyValuePair<int, int>();
            ScatterCustom currentNode = new ScatterCustom(this, null);


            //
            // Chargement des nodes
            //
            while ((line = sr.ReadLine()) != null)
            {
                
                if(line.Contains("<node"))
                {
                    compteur++;
                    myPair = new KeyValuePair<int, int>(-1, -1);
                    string[] typeLine = line.Split("'".ToCharArray());
                    string type = "NULL";
                    for (int i = 0; i < typeLine.Count()-1; i++)
                    {
                        if (typeLine[i].Contains("type"))
                        {
                            type = typeLine[i + 1];
                            break;
                        }
                    }
                    int id = -1;
                    for (int i = 0; i < typeLine.Count() - 1; i++)
                    {
                        if (typeLine[i].Contains("id"))
                        {
                            string strId = typeLine[i + 1];
                            id = Convert.ToInt32(strId);
                            break;
                        }
                    }
                    myPair = new KeyValuePair<int, int>(id, -1);

                    switch (type)
                    { 
                        case "txt":
                            currentType = "txt";
                            NodeText myText = new NodeText(this, null);
                            string color = "NULL";
                            for (int i = 0; i < typeLine.Count()-1; i++)
                            {
                                if (typeLine[i].Contains("color"))
                                {
                                    color = typeLine[i + 1];
                                    break;
                                }
                            }
                            if (color != "NULL")
                                myText.SetColor(color);

                            myText.Center = initP;
                            this.MainScatterView.Items.Add(myText);
                            listNode.Add(myText);
                            currentNode = myText;
                            break;
                        case "img":
                            currentType = "img";
                            NodeImage myImage = new NodeImage(this, null);
                            myImage.Center = initP;
                            this.MainScatterView.Items.Add(myImage);
                            listNode.Add(myImage);
                            currentNode = myImage;
                            break;
                    }
                    continue;
                }


                if (line.Contains("<parent") && line.Contains("/>"))
                {
                    compteur++;
                    string[] typeLine = line.Split("'".ToCharArray());
                    int id = -1;
                    for (int i = 0; i < typeLine.Count() - 1; i++)
                    {
                        if (typeLine[i].Contains("id"))
                        {
                            string strId = typeLine[i + 1];
                            id = Convert.ToInt32(strId);
                            break;
                        }
                    }
                    myPair = new KeyValuePair<int, int>(myPair.Key, id);
                }


                if (line.Contains("<image"))
                {
                    NodeImage myImage = (NodeImage)currentNode;
                    if (myImage == null)
                        continue;
                    string[] imageLine = line.Split("'".ToCharArray());
                    string path = "NONE";
                    for (int i = 0; i < imageLine.Count() - 1; i++)
                    {
                        if (imageLine[i].Contains("path"))
                        {
                            path = imageLine[i + 1];
                            break;
                        }
                    }
                    if (path != "NONE")
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri(".\\Saves\\" + filename + "\\Images\\" + path, UriKind.Relative);
                        bi.EndInit();
                        Point dim = new Point(bi.Width, bi.Height);
                        Brush bru = new ImageBrush(bi);
                        myImage.LoadImage(bru, dim, path);
                    }
                    continue;
                }

                

                if(line.Contains("</text>"))
                {
                    NodeText myText = (NodeText)currentNode;
                    if (myText == null)
                        continue;
                    string[] textLine = line.Split("</texte>".ToCharArray());
                    string text = "";
                    try
                    {
                        text = textLine.First();
                    }
                    catch { }
                    if (text != "")
                    {
                        myText.LoadText("\r", false);
                        if (!isFirstLine)
                            myText.LoadText(text, false);
                        else
                        {
                            myText.LoadText(text, true);
                            isFirstLine = false;
                        }
                    }
                    isInText = false;
                }

                if (isInText)
                {
                    NodeText myText = (NodeText)currentNode;
                    if (myText == null)
                        continue;
                    myText.LoadText("\r", false);
                    if (!isFirstLine)
                        myText.LoadText(line, false);
                    else
                    {
                        myText.LoadText(line, true);
                        isFirstLine = false;
                    }
                }

                if (line.Contains("<text>"))
                {
                    NodeText myText = (NodeText)currentNode;
                    if (myText == null)
                        continue;
                    string[] textLine = line.Split("<texte>".ToCharArray());
                    string text = "";
                    try
                    {
                        text = textLine.Last();
                    }
                    catch { }
                    if (text != "")
                    {
                        myText.LoadText(text, true);
                        isFirstLine = false;
                    }
                    isInText = true;

                }


                if (line.Contains("</node>"))
                {
                    if (currentType != "" && myPair.Key != -1)
                    {
                        KeyValuePair<KeyValuePair<int, int>, ScatterCustom> triple = new KeyValuePair<KeyValuePair<int, int>, ScatterCustom>(myPair, currentNode);
                        listParent.Add(triple);
                    }
                    currentType = "";
                    isFirstLine = true;
                    isInText = false;
                    currentNode = new ScatterCustom(this, null);
                }

            }


            //
            // Chargement des liens de parenté
            //
            for (int i = 0; i < listParent.Count; i++)
            {
                if (listParent.ElementAt(i).Key.Value != -1)
                {
                    for (int j = 0; j < listParent.Count; j++)
                    {
                        if (listParent.ElementAt(i).Key.Value == listParent.ElementAt(j).Key.Key)
                        {
                            listParent.ElementAt(i).Value.SetParent(listParent.ElementAt(j).Value);
                        }
                    }
                }
                else
                    listParent.ElementAt(i).Value.SetParent(null);
            }

            nomFichier = filename;
            isModified = false;

            menuPrincipal.FormGrid.Children.Clear();
        
        
        
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
                default:
                    return;

            }
            Modification(true);

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
                Modification(true);
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

                    Ellipse ell = new Ellipse();

                    ell.Width = 150;
                    ell.Height = 150;
                    ell.Fill = new SolidColorBrush(Colors.Green);
                    ell.Stroke = new SolidColorBrush(Colors.DarkGreen);
                    ell.StrokeThickness = 3;

                    this.LinkParentGrid.Children.Add(ell);
                    
                    Point ellPointUL = listNode.ElementAt(i).PointFromScreen(listNode.ElementAt(i).ActualCenter);
                    ellPointUL.Y -= listNode.ElementAt(i).Height / 3;
                    ellPointUL = listNode.ElementAt(i).PointToScreen(ellPointUL);

                    Canvas.SetLeft(ell, ellPointUL.X - ell.Width / 2);
                    Canvas.SetTop(ell, ellPointUL.Y - ell.Height / 2);
                    
                    ell.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnGreenCirclePreviewTouchDown);
                    KeyValuePair<Ellipse, ScatterCustom> myPair = new KeyValuePair<Ellipse,ScatterCustom>(ell, listNode.ElementAt(i));
                    listRattache.Add(myPair);

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

            if (!e.TouchDevice.GetIsFingerRecognized())
                return;

            ScatterCustom text = null;
            for (int i = 0; i < listRattache.Count; i++)
            {
                if (listRattache.ElementAt(i).Key == (Ellipse)sender)
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

        





        //
        //  MODIFICATION du FICHIER
        //
        public void Modification(bool modif)
        {
            isModified = modif;
        }

        ///////////////////
            //FIN DES FONCTIONS !!!
    }
}