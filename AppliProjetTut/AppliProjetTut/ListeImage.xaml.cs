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
        List<KeyValuePair<KeyValuePair<SurfaceButton, Point>, string>> listButton = new List<KeyValuePair<KeyValuePair<SurfaceButton, Point>, string>>();
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
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
        /// <summary>
        /// Initialise la liste d'image
        /// </summary>
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

                        Point dim = new Point(bi.Width, bi.Height);
                        KeyValuePair<SurfaceButton, Point> myPair = new KeyValuePair<SurfaceButton, Point>(btnImg, dim);
                        KeyValuePair<KeyValuePair<SurfaceButton, Point>,string> triple = new KeyValuePair<KeyValuePair<SurfaceButton,Point>,string>(myPair, nomPartition.Last());

                        listButton.Add(triple);
                    }
                }
                SurfaceButton btnNone = new SurfaceButton();
                btnNone.Width = imgSize;
                btnNone.Height = imgSize;

                btnNone.Background = new SolidColorBrush(Colors.Gray);
                
                Point dimNone = new Point(300, 200);
                KeyValuePair<SurfaceButton, Point> myPairNone = new KeyValuePair<SurfaceButton, Point>(btnNone, dimNone);
                KeyValuePair<KeyValuePair<SurfaceButton, Point>,string> tripleNone = new KeyValuePair<KeyValuePair<SurfaceButton,Point>,string>(myPairNone, "NONE");

                listButton.Add(tripleNone);

            }
            catch { };

            ButtonListGrid.Width = listButton.Count * imgSize;
            for (int i = 0 ; i < listButton.Count; i++)
            { 
                SurfaceButton btn = listButton.ElementAt(i).Key.Key;
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

        /// <summary>
        /// Lors de l'appui sur un bouton image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            SurfaceButton button = (SurfaceButton)sender;
            if (button != null)
            {
                for (int i = 0; i < listButton.Count; i++)
                {
                    if (listButton.ElementAt(i).Key.Key == button)
                    { 
                        Brush imgBrush = (Brush)button.Background;
                        nodeParent.onChoice(imgBrush, listButton.ElementAt(i).Key.Value, listButton.ElementAt(i).Value);
                    }
                }
                
            }
        }

        //
        //   VERIFIE SI LE FICHIER EST UNE IMAGE
        //   true : extension d'image reconnue
        //   false : extension autre que d'une image reconnue
        /// <summary>
        /// Vérifie si le fichier passé en paramètre est une image
        /// true : extension d'image reconnue
        /// false : extension non reconnue
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Valide le choix de l'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            nodeParent.onValidateChoice();
            nodeParent.onCloseImagesList();
        }
        /// <summary>
        /// Annule le choix de l'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            nodeParent.onCloseImagesList();
        }




        // FIN CLASSE
        //

    }
}
