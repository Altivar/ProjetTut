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
        //private List<NodeText> listeNode = new List<NodeText>();

        // parent
        NodeText parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // clavier virtuel
        private ClavierVirtuel clavier;

        public NodeText(SurfaceWindow1 parentSurface, NodeText parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;
            clavier = new ClavierVirtuel(this);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNodeLoaded(object sender, RoutedEventArgs e)
        {
            // initialisation du texte et de la couleur de fond
            this.TextBoxNode.IsEnabled = false;
            this.Background = new SolidColorBrush(Colors.LightBlue);
            CanScale = false;
        }


        // evenement de selection du menu
        private void OnEditSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                this.MainScatter.Items.Add(clavier);

                // on fait apparaitre le "curseur"
                this.TextBoxNode.AppendText("|");
                clavier.CanMove = false;
                clavier.CanScale = false;
                clavier.CanRotate = false;
                isEditing = true;
            }
        }

        private void OnAddNodeSelection(object sender, RoutedEventArgs e)
        {
            Point pt = new Point(150, 150);
            Surface.AddNode(this, pt);
        }

        private void OnRemoveSelection(object sender, RoutedEventArgs e)
        {
            //
        }



        // ajout de texte
        public void AjoutTexte(string str)
        {

            if (str.Equals("close"))
            {
                this.MainScatter.Items.Remove(clavier);
                
                // on enlève le "curseur"
                string test = this.TextBoxNode.Text;
                test = test.Remove(test.Length - 1);
                this.TextBoxNode.Clear();
                this.TextBoxNode.AppendText(test);

                isMoveEnable(true);
                isEditing = false;
            }
            else if (str.Equals("Backspace"))
            {
                if (this.TextBoxNode.Text.Length > 1)
                {
                    string test = this.TextBoxNode.Text;
                    test = test.Remove(test.Length - 2);
                    this.TextBoxNode.Clear();
                    this.TextBoxNode.AppendText(test);
                    this.TextBoxNode.AppendText("|");
                }
            }
            else
            {
                string test = this.TextBoxNode.Text;
                test = test.Remove(test.Length - 1);
                this.TextBoxNode.Clear();
                this.TextBoxNode.AppendText(test);
                this.TextBoxNode.AppendText(str);
                this.TextBoxNode.AppendText("|");
            }
            
        }

        public void isMoveEnable(bool enable)
        {
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
