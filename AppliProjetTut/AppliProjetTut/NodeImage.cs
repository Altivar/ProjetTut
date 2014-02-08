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
            }
            catch
            {
                currentImage = new SolidColorBrush(Colors.Gray);
            }
            base.MainGrid.Background = currentImage;

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
                base.AddonGrid.Items.Add(imageChoice);
                imageChoice.InitListView();

                isEditing = true;
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
        }
        public void onChoice(Brush newPath)
        {
            base.MainGrid.Background = newPath;
        }
        public void onValidateChoice()
        {
            currentImage = base.MainGrid.Background;
        }


        //
        //
        //
        public Brush GetImage()
        {
            return currentImage;
        }


    }
}
