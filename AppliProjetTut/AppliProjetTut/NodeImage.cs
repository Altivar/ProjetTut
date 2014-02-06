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
        ImageBrush ImgNode;

        public NodeImage(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {

            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            imageChoice = new ListeImages(this);

            ImgNode = new ImageBrush();
            base.MainGrid.Background = new SolidColorBrush(Colors.Gray);

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
        //
        //
        public void onCloseImagesList()
        {
            base.AddonGrid.Items.Remove(imageChoice);
            isEditing = false;
        }
        public void onValidateChoice(string newPath)
        {
            if (newPath == "NONE")
            {
                base.MainGrid.Background = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(".\\Resources\\Images\\" + newPath, UriKind.Relative);
                bi.EndInit();
                ImgNode.ImageSource = bi;
                base.MainGrid.Background = ImgNode;
            }
        }


    }
}
