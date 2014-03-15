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
    /// Logique d'interaction pour ListeSauvegarde.xaml
    /// </summary>
    public partial class ListeSauvegarde : ScatterViewItem
    {

        // lien entre la liste et la liste des fichiers
        SurfaceWindow1 SurfaceParent;

        // liste de bouton
        List<SurfaceButton> listButton = new List<SurfaceButton>();

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public ListeSauvegarde(SurfaceWindow1 parent)
        {
            InitializeComponent();

            SurfaceParent = parent;

            InitSaveList();

            this.BoutonAnnuler.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnFileButtonPreviewTouchUp);
        }

        /// <summary>
        /// Initialise les liste de Sauvegarde
        /// </summary>
        private void InitSaveList()
        {

            DirectoryInfo dirInfo = new DirectoryInfo(".\\Saves\\");
            
            DirectoryInfo[] DirSaves = dirInfo.GetDirectories();
            foreach (DirectoryInfo dir in DirSaves)
            {   
                try
                {
                    string nomSauvegarde = dir.FullName;
                    char[] separator = { '\\' };
                    string[] nomPartition = nomSauvegarde.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    SurfaceButton btnImg = new SurfaceButton();
                    btnImg.Width = 200;
                    btnImg.Height = 30;
                    btnImg.Content = nomPartition.Last();

                    listButton.Add(btnImg);
                }
                catch { };
             }
            

            ButtonListGrid.Height = listButton.Count * 30;
            ButtonListGrid.Width = 200;
            for (int i = 0; i < listButton.Count; i++)
            {
                SurfaceButton btn = listButton.ElementAt(i);
                btn.Margin = new Thickness(0, i * 30, 0, (listButton.Count - (i+1)) * 30);
                if (i % 2 == 0)
                {
                    btn.Background = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    btn.Background = new SolidColorBrush(Colors.DarkGray);
                }

                btn.PreviewTouchUp += new EventHandler<TouchEventArgs>(OnFileButtonPreviewTouchUp);
                ButtonListGrid.Children.Add(btn);
            }
        }

        /// <summary>
        /// Lors de la sélection d'un bouton : ouvre le fichier correspondant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFileButtonPreviewTouchUp(object sender, TouchEventArgs e)
        {
            SurfaceButton senderBtn = (SurfaceButton)sender;
            if (senderBtn == null)
                return;

            
            string fileContent = (string)senderBtn.Content;
            if (senderBtn == null)
                return;
            
            string fileName = senderBtn.Name;
            if (fileName == "BoutonAnnuler" && fileContent == "Annuler")
            {
                SurfaceParent.OpenFile("<Annuler>");
                return;
            }
                    

            SurfaceParent.OpenFile(fileContent);

        }

    }
}
