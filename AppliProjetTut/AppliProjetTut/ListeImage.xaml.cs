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

using System.IO;

namespace AppliProjetTut
{
    
    /// <summary>
    /// Logique d'interaction pour ListeImages.xaml
    /// </summary>
    public partial class ListeImages : ScatterViewItem
    {

        int imgSize = 100;

        int imgBorder = 5;

        // NodeImage a laquelle il est rattaché
        NodeImage nodeParent;

        // liste de bouton
        List<SurfaceButton> listButton = new List<SurfaceButton>();

        public ListeImages(NodeImage parent)
        {
            InitializeComponent();

            nodeParent = parent;

            CanScale = false;
            CanMove = false;
            CanRotate = false;

            ButtonListGrid.Height = 100;

            InitListView();


        }


        //
        //   FONCTION D'INITIALISATION DE LA LISTE D'IMAGE
        //
        public void InitListView()
        {
            listButton.Clear();

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Resources\\Images");
            try
            {
                FileInfo[] ImgFiles = dirInfo.GetFiles();
                foreach (FileInfo image in ImgFiles)
                {
                string nomImage = image.FullName;
                char[] separator = { '\\' };
                string[] nomPartition = nomImage.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    if (isImage(nomPartition.Last()))
                    {
                        SurfaceButton btnImg = new SurfaceButton();
                        btnImg.Width = imgSize;
                        btnImg.Height = imgSize;
                        
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri(".\\Resources\\Images\\" + nomPartition.Last(), UriKind.Relative);
                        bi.EndInit();
                        btnImg.Background = new ImageBrush(bi);

                        listButton.Add(btnImg);
                    }
                }
                SurfaceButton btnNone = new SurfaceButton();
                btnNone.Width = imgSize;
                btnNone.Height = imgSize;

                btnNone.Background = new SolidColorBrush(Colors.Gray);

                listButton.Add(btnNone);

            }
            catch { };

            ButtonListGrid.Width = listButton.Count * imgSize;
            for (int i = 0 ; i < listButton.Count; i++)
            { 
                SurfaceButton btn = listButton.ElementAt(i);
                if (i == listButton.Count - 1)
                {
                    btn.Margin = new Thickness(imgSize * i + imgBorder, imgBorder, imgSize * (listButton.Count - 1 - i) + imgBorder, 100 - imgSize + imgBorder);
                }
                else
                {
                    btn.Margin = new Thickness(imgSize * i + imgBorder, imgBorder, imgSize * (listButton.Count - 1 - i), 100 - imgSize + imgBorder);
                }
                
                btn.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnButtonPreviewTouchUp);
                ButtonListGrid.Children.Add(btn);
            }
            
        }


        void OnButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            SurfaceButton button = (SurfaceButton)sender;
            if (button != null)
            {
                Brush imgBrush = (Brush)button.Background;
                nodeParent.onChoice(imgBrush);
            }
        }

        //
        //   VERIFIE SI LE FICHIER EST UNE IMAGE
        //   true : extension d'image reconnue
        //   false : extension autre que d'une image reconnue
        private bool isImage(string nom)
        {

            char[] separator = { '.' };
            string[] nomSplitte = nom.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            switch (nomSplitte.Last())
            {
                case "png":
                case "jpg":
                case "bmp":
                case "gif":
                    return true;
                default:
                    return false;
            }


        }

        
        
        
        
        
        //
        //   INTERACTION AVEC LA LISTE D'IMAGES
        //
        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            nodeParent.onValidateChoice();
            nodeParent.onCloseImagesList();
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            nodeParent.onCloseImagesList();
        }




        // FIN CLASSE
        //

    }
}
