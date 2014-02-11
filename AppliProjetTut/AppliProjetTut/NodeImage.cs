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
        Point tempSize = new Point(-1, -1);

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
                currentSize = new Point(-1, -1);
            }
            base.MainGrid.Background = currentImage;

            ElementMenuItem MenuItem1 = new ElementMenuItem();
            MenuItem1.Header = "Image choice";
            MenuItem1.Click += new RoutedEventHandler(OnImageChoiceSelection);
            base.MainMenu.Items.Add(MenuItem1);

            ElementMenuItem MenuItem2 = new ElementMenuItem();
            MenuItem2.Header = "Aggrandir";
            MenuItem2.Click += new RoutedEventHandler(OnBiggerSelection);
            base.MainMenu.Items.Add(MenuItem2);

            ElementMenuItem MenuItem3 = new ElementMenuItem();
            MenuItem3.Header = "Réduire";
            MenuItem3.Click += new RoutedEventHandler(OnSmallerSelection);
            base.MainMenu.Items.Add(MenuItem3);


        }



        //
        //   EVENT de selection du menu
        //
        private void OnImageChoiceSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.AddonGrid.Items.Add(imageChoice);
                imageChoice.InitListView();

                isEditing = true;
            }
        }

        private void OnBiggerSelection(object sender, RoutedEventArgs e)
        {
            if (currentSize.X != -1 && currentSize.Y != -1)
            {
                //base.CanScale = true;
                base.MainGrid.Width = currentSize.X;
                base.MainGrid.Height = currentSize.Y;
                base.Width = currentSize.X;
                base.Height = currentSize.Y;
                //base.CanScale = false;
            }
        }

        private void OnSmallerSelection(object sender, RoutedEventArgs e)
        {
            base.MainGrid.Width = 300;
            base.MainGrid.Height = 200;
            base.Width = 300;
            base.Height = 200;

            if (base.ActualCenter.X < 100)
            {
                if (base.ActualCenter.Y < 100)
                {
                    base.Center = new Point(100, 100);
                }
                else if (base.ActualCenter.Y > Surface.Height - 100)
                {
                    base.Center = new Point(100, Surface.Height - 100);
                }
                else
                {
                    base.Center = new Point(100, base.ActualCenter.Y - 100);
                }
            }
            else if (base.ActualCenter.X > Surface.Width - 100)
            {
                if (base.ActualCenter.Y < 100)
                {
                    base.Center = new Point(Surface.Width - 100, 100);
                }
                else if (base.ActualCenter.Y > Surface.Height - 100)
                {
                    base.Center = new Point(Surface.Width - 100, Surface.Height - 100);
                }
                else
                {
                    base.Center = new Point(Surface.Width - 100, base.ActualCenter.Y - 100);
                }
            }

            if (base.ActualCenter.Y < 100)
            {
                base.Center = new Point(base.ActualCenter.X - 100, 100);
            }
            else if (base.ActualCenter.Y > Surface.Height - 100)
            {
                base.Center = new Point(base.ActualCenter.X - 100, Surface.Height - 100);
            }
        }


        //
        //  GESTION IMAGE AR LISTE D'IMAGE
        //
        public void onCloseImagesList()
        {
            base.AddonGrid.Items.Remove(imageChoice);
            base.MainGrid.Background = currentImage;
            isEditing = false;
            tempSize = new Point(-1, -1);
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
