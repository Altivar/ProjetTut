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
        public ListeImages()
        {
            InitializeComponent();

            CanScale = false;
            CanMove = false;
            CanRotate = false;


            InitListView();


        }



        public void InitListView()
        {

            listview.Items.Clear();

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
                        listview.Items.Add(nomPartition.Last());
                    }
                }
            }
            catch { };

            
        }


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
                    break;
                default:
                    return false;
                    break;
            }


        }




        // FIN CLASSE
        //

    }
}
