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

        public NodeImage(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {

            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

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
        private void OnImageChoiceSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.Height = 200;
                base.Width = 300;
                base.MainGrid.Width = 300;
                base.MainGrid.Height = 200;
                base.AddonGrid.Items.Add(imageChoice);
                imageChoice.InitListView();

                isEditing = true;
            }
        }


        //
        //  EVENT de changement de taille
        //
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


            


            /*if (currentSize.X > 300)
            {
                base.MainGrid.Width = base.Width;
                base.Height = (base.Width - 300) * ecartXparY + 200;
            }
            else if (currentSize.Y > 200)
            {
                base.MainGrid.Height = base.Height;
            }

            //
            //  GESTION taille base.MainGrid
            //
            if (currentSize.X < base.MainGrid.Width)
            {
                base.MainGrid.Width = currentSize.X;
            }
            if (currentSize.Y < base.MainGrid.Height)
            {
                base.MainGrid.Height = currentSize.Y;
            }
            if (300 > base.MainGrid.Width)
            {
                base.MainGrid.Width = 300;
            }
            if (200 > base.MainGrid.Height)
            {
                base.MainGrid.Height = 200;
            }

            //
            //  GESTION taille Node
            //
            if (currentSize.X < base.Width)
            {
                base.Width = currentSize.X;
            }
            if (currentSize.Y < base.Height)
            {
                base.Height = currentSize.Y;
            }*/

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
            base.MainMenu.Margin = new Thickness((base.Width - base.MainGrid.Width) / 2, base.MainGrid.Height + borderHeight, (base.Width - base.MainGrid.Width) / 2, -(borderHeight + 50));

            previousSize = new Point(base.Width, base.Height);

        }




        //
        //  GESTION IMAGE PAR LISTE D'IMAGE
        //
        public void onCloseImagesList()
        {
            base.AddonGrid.Items.Remove(imageChoice);
            base.MainGrid.Background = currentImage;
            isEditing = false;
            tempSize = new Point(300, 200);
        }
        public void onChoice(Brush newPath, Point dimension)
        {
            base.MainGrid.Background = newPath;
            tempSize = dimension;
        }
        public void onValidateChoice()
        {
            currentImage = base.MainGrid.Background;
            currentSize = tempSize;
            mise_a_echelle();
        }


        //
        //  FONCTION de mise a echelle du Node
        //
        void mise_a_echelle()
        {
            // taille du Node
            base.Height = 200;
            base.Width = 300;
            
            // taille minimum du Node
            //base.MinHeight = 200;
            //base.MinWidth = 300;

            //// taille maximum du Node
            // reglage sur les X
            if (currentSize.X > 300)
            {
                //base.MaxWidth = currentSize.X;
                base.MainGrid.Width = base.Width;
            }
            else
            {
                //base.MaxWidth = base.MinWidth;
                base.MainGrid.Width = currentSize.X;
            }
            // reglage sur les Y
            if (currentSize.Y > 200)
            {
                //base.MaxHeight = currentSize.Y;
                base.MainGrid.Height = base.Height;
            }
            else
            {
                //base.MaxHeight = base.MinHeight;
                base.MainGrid.Height = currentSize.Y;
            }

            double borderHeight = (base.Height - base.MainGrid.Height) / 2;
            base.MainMenu.Margin = new Thickness((base.Width - base.MainGrid.Width) / 2, base.MainGrid.Height + borderHeight, (base.Width - base.MainGrid.Width) / 2, -(borderHeight + 50));

            previousSize = new Point(base.Width, base.Height);



        }



        //
        //
        //
        public Brush GetImage()
        {
            return currentImage;
        }
        public Point GetSize()
        {
            return currentSize;
        }


    }
}
