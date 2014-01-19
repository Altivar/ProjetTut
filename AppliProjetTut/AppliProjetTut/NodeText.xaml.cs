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

        // parent
        NodeText parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // clavier virtuel
        private ClavierVirtuel clavier;
        // palette de couleur
        private PaletteCouleurs palette;
        // couleur actuelle
        private Brush currentColor;

        public NodeText(SurfaceWindow1 parentSurface, NodeText parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;
            clavier = new ClavierVirtuel(this);
            palette = new PaletteCouleurs(this);
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
            currentColor = new SolidColorBrush(Colors.LightBlue);
            this.TextBoxNode.Background = currentColor;
            this.TextBoxNode.BorderBrush = currentColor;
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

            Point pt = ActualCenter;
            if (pt.Y < Surface.Height - 300)
            {
                pt.Y += 200;
            }
            else
            {
                pt.Y -= 200;
            }
            Surface.AddNode(this, pt);
        }

        private void OnRemoveSelection(object sender, RoutedEventArgs e)
        {
            Surface.RemoveNode(this, true);
        }

        private void OnColorSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                this.MainScatter.Items.Add(palette);

                palette.SetFirstColor(currentColor);
                palette.CanMove = false;
                palette.CanScale = false;
                palette.CanRotate = false;
                isEditing = true;
            }
        }

        private void OnSeparateSelection(object sender, RoutedEventArgs e)
        {
            parent = null;
        }

        // ajout de texte
        public void AjoutTexte(string str)
        {

            if (str.Equals("Close"))
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

        // changement de couleur
        public void SetBackGroundColor(Brush color)
        {
            currentColor = color;
            this.TextBoxNode.Background = currentColor;
            this.TextBoxNode.BorderBrush = currentColor;
        }
        public void ClosePalette()
        {
            this.MainScatter.Items.Remove(palette);
            isEditing = false;
        }

        public void isMoveEnable(bool enable)
        {
            CanMove = enable;
            CanRotate = enable;
        }

        public Line getLineToParent()
        {
            Line line = new Line();

            if (parent != null)
            {

                Point pt1 = this.ActualCenter;
                Point pt2 = parent.ActualCenter;

                line.Stroke = Brushes.PaleVioletRed;

                line.X1 = pt1.X;
                line.Y1 = pt1.Y;
                line.X2 = pt2.X;
                line.Y2 = pt2.Y;
                line.StrokeThickness = 2;

            }
            else
            {
                line.Stroke = Brushes.Transparent;
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = 0;
                line.StrokeThickness = 0;
            }

            return line;
        }

        public NodeText GetParent()
        {
            return parent;
        }

        
    }
}
