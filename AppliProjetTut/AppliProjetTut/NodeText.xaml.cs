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
    /// Logique d'interaction pour NodeText.xaml
    /// </summary>
    public partial class NodeText : ScatterViewItem
    {


        // en edition
        private bool isEditing = false;

        // liste de nodes enfant
        private List<NodeText> listeNode = new List<NodeText>();

        // clavier virtuel
        private ClavierVirtuel clavier;

        public NodeText()
        {
            InitializeComponent();

            clavier = new ClavierVirtuel(this);


            // initialisation du texte et de la couleur de fond
            this.Text.IsEnabled = false;
            this.Background = new SolidColorBrush(Colors.LightBlue);

            this.Text.AppendText("// TODO \\\\");

            

        }


        // evenement de selection du menu
        private void OnEditSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                this.MainScatter.Items.Add(clavier);
                clavier.CanMove = false;
                clavier.CanScale = false;
                clavier.CanRotate = false;
                isEditing = true;
            }
        }

        private void OnAddNodeSelection(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnRemoveSelection(object sender, RoutedEventArgs e)
        {
            //
        }



        // ajout de texte
        public void AjoutTexte(string str)
        {

            if (str == "close")
            {
                this.MainScatter.Items.Remove(clavier);
                isEditing = false;
            }
            else
            {
                this.Text.AppendText(str);
            }
            
        }
        public void isMoveEnable(bool enable)
        {
            CanRotate = enable;
            CanMove = enable;
            CanRotate = enable;
            if (enable)
            {
                this.Background = new SolidColorBrush(Colors.LightBlue);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.Teal);
            }
        }

        
    }
}
