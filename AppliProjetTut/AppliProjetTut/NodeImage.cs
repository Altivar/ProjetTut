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
using Microsoft.Surface.Presentation.Controls;

namespace AppliProjetTut
{

    /// <summary>
    /// Noeud représentant une image
    /// </summary>
    public partial class NodeImage : ScatterCustom
    {


        // en edition
        private bool isEditing = false;

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // liste de choix de l'image
        ListeImages imageChoice;

        // Image du ImageNode
        Brush currentImage;
        Point currentSize;
        Point tempSize = new Point(300, 200);
        Point previousSize = new Point(300, 200);
        string tempPath = "NONE";
        string currentPath = "NONE";

        /// <summary>
        /// Defalut Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public NodeImage(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {

            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            base.SetTypeOfNode("Image");

            imageChoice = new ListeImages(this);

            try
            {
                currentImage = ((NodeImage)parent).GetImage();
                currentSize = ((NodeImage)parent).GetSize();
            }
            catch
            {
                currentImage = new SolidColorBrush(Colors.Gray);
                currentSize = new Point(300, 200);
            }
            mise_a_echelle();
            base.MainGrid.Background = currentImage;

            base.CanScale = true;
            base.SizeChanged += new SizeChangedEventHandler(OnNodeImageSizeChanged);

            ElementMenuItem MenuItem1 = new ElementMenuItem();
            MenuItem1.Header = "Image choice";
            MenuItem1.Click += new RoutedEventHandler(OnImageChoiceSelection);
            base.MainMenu.Items.Add(MenuItem1);
        }

        



        //
        //   EVENT de selection du menu
        //
        /// <summary>
        /// Appelé lors de la selection du Menu de choix d'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImageChoiceSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.Height = 200;
                base.Width = 300;
                base.MainGrid.Width = (currentSize.X > 300) ? 300 : currentSize.X;
                base.MainGrid.Height = (currentSize.Y > 200) ? 200 : currentSize.Y;

                base.AddonGrid.Items.Add(imageChoice);
                imageChoice.InitListView();

                double borderHeight = (base.Height - base.MainGrid.Height) / 2;
                base.AddonGrid.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
                base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                CanScale = false;
                isEditing = true;
            }
        }


        //
        //  EVENT de changement de taille
        //
        /// <summary>
        /// Evenement de changement de taille réadapté aux images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnNodeImageSizeChanged(object sender, SizeChangedEventArgs e)
        {

            double ecartX = currentSize.X - 300;
            double ecartY = currentSize.Y - 200;
            double ecartXparY = ecartX / ecartY;

            if (currentSize.X <= 300 && currentSize.Y <= 200)
            {
                base.Width = previousSize.X;
                base.Height = previousSize.Y;
            }
            else if (currentSize.X > 300 && currentSize.Y > 200)
            {
                if (ecartX > ecartY)
                {
                    double newHeight = (base.Width - 300) / ecartX * ecartY;
                    base.Height = newHeight + 200;
                }
                else if (ecartY > ecartX)
                {
                    double newWidth = (base.Height - 200) / ecartY * ecartX;
                    base.Width = newWidth + 300;
                }
                else // si ecartX == ecartY
                { 
                
                }
            }
            else if (currentSize.X <= 300 && currentSize.Y > 200)
            {
                base.Width = previousSize.X;
            }
            else if (currentSize.X > 300 && currentSize.Y <= 200)
            {
                base.Height = previousSize.Y;
            }
            else
            { 
                // pas normal !!! 
            }



            // si la dimensiondu Node est trop grande
            if (base.Width > currentSize.X)
            {
                base.Width = currentSize.X;
            }
            if (base.Height > currentSize.Y)
            {
                base.Height = currentSize.Y;
            }

            // si la dimension du Node est trop petite
            if (base.Width <= 300)
            {
                base.Width = 300;
            }
            if (base.Height <= 200)
            {
                base.Height = 200;
            }

            // reglage de la Grid
            if (currentSize.X <= 300)
            {
                base.MainGrid.Width = currentSize.X;
            }
            else
            {
                base.MainGrid.Width = base.Width;
            }
            if (currentSize.Y <= 200)
            {
                base.MainGrid.Height = currentSize.Y;
            }
            else
            {
                base.MainGrid.Height = base.Height;
            }

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness(base.MainGrid.Width/2, base.MainGrid.Height + borderHeight, base.MainGrid.Width/2, -(borderHeight + 50));
            base.MainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            previousSize = new Point(base.Width, base.Height);

        }




        //
        //  GESTION IMAGE PAR LISTE D'IMAGE
        //
        /// <summary>
        /// Appelé lors de la fermeture de la liste d'image
        /// </summary>
        public void onCloseImagesList()
        {
            base.AddonGrid.Items.Remove(imageChoice);
            base.MainGrid.Background = currentImage;
            base.MainGrid.Width = (currentSize.X > 300) ? 300 : currentSize.X;
            base.MainGrid.Height = (currentSize.Y > 200) ? 200 : currentSize.Y;

            CanScale = true;
            isEditing = false;
            
            tempSize = new Point(300, 200);

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.MainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        }

        /// <summary>
        /// Appelé lors de l'appui sur une image de la liste
        /// </summary>
        /// <param name="newPath"></param>
        /// <param name="dimension"></param>
        /// <param name="path"></param>
        public void onChoice(Brush newPath, Point dimension, string path)
        {
            base.MainGrid.Background = newPath;
            tempSize = dimension;
            base.MainGrid.Width = (tempSize.X > 300) ? 300 : tempSize.X;
            base.MainGrid.Height = (tempSize.Y > 200) ? 200 : tempSize.Y;

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.MainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            base.AddonGrid.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            tempPath = path;

        }
        /// <summary>
        /// Appéle lors de la validation du choix de l'image
        /// </summary>
        public void onValidateChoice()
        {
            currentImage = base.MainGrid.Background;
            currentSize = tempSize;
            mise_a_echelle();

            currentPath = tempPath;


            // le fichier a été modifié
            Surface.Modification(true);
        }


        //
        //  FONCTION de mise a echelle du Node
        //
        /// <summary>
        /// Met le NodeImage à l'echelle de l'image qu'il contient
        /// </summary>
        void mise_a_echelle()
        {
            // taille du Node
            base.Height = 200;
            base.Width = 300;

            //// taille maximum du Node
            // reglage sur les X
            if (currentSize.X > 300)
            {
                base.MainGrid.Width = base.Width;
            }
            else
            {
                base.MainGrid.Width = currentSize.X;
            }
            // reglage sur les Y
            if (currentSize.Y > 200)
            {
                base.MainGrid.Height = base.Height;
            }
            else
            {
                base.MainGrid.Height = currentSize.Y;
            }

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.MainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            previousSize = new Point(base.Width, base.Height);



        }



        //
        //  GESTION chargement image
        //
        /// <summary>
        /// Charge l'image passé en paramètre avec les caractéristique de l'image
        /// </summary>
        /// <param name="newPath"></param>
        /// <param name="dimension"></param>
        /// <param name="path"></param>
        public void LoadImage(Brush newPath, Point dimension, string path)
        {
            base.MainGrid.Background = newPath;
            currentSize = dimension;
            base.MainGrid.Width = (tempSize.X > 300) ? 300 : tempSize.X;
            base.MainGrid.Height = (tempSize.Y > 200) ? 200 : tempSize.Y;

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.MainMenu.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            base.AddonGrid.Margin = new Thickness(base.MainGrid.Width / 2, base.MainGrid.Height + borderHeight, base.MainGrid.Width / 2, -(borderHeight + 50));
            base.AddonGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            currentPath = path;

            currentImage = base.MainGrid.Background;
            mise_a_echelle();
        }


        //
        //
        //
        /// <summary>
        /// Retourne le chemin vers l'image
        /// </summary>
        /// <returns></returns>
        public string GetImagePath()
        {
            return currentPath;
        }
        /// <summary>
        /// Retourne l'image
        /// </summary>
        /// <returns></returns>
        public Brush GetImage()
        {
            return currentImage;
        }
        /// <summary>
        /// Retourne la taille maximale de l'image du NodeImage
        /// </summary>
        /// <returns></returns>
        public Point GetSize()
        {
            return currentSize;
        }


    }
}
