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
    /// Logique d'interaction pour ScatterCustom.xaml
    /// </summary>
    public partial class ScatterCustom : ScatterViewItem
    {

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;

        // type du node
        string thisType;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parentSurface"></param>
        /// <param name="parentNode"></param>
        public ScatterCustom(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            CanScale = false;
        }


        // lorsque la node est chargée
        protected void OnNodeLoaded(object sender, RoutedEventArgs e)
        {
            // TODO : a implementer dans les classes héritée
        }






        /// <summary>
        /// Retourne la ligne de liason entre le Node et son parent
        /// </summary>
        /// <returns></returns>
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







        

        // SELECTION MENU
        /// <summary>
        /// Lorsque le Menu de selection de NodeText est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAddNodeTextSelection(object sender, RoutedEventArgs e)
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
            Surface.AddNode(this, pt, "Text");
        }
        /// <summary>
        /// Lorsque le Menu de selection de NodeImage est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAddNodeImageSelection(object sender, RoutedEventArgs e)
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
            Surface.AddNode(this, pt, "Image");
        }
        /// <summary>
        /// Lorsque le Menu de suppression de Node est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnRemoveSelection(object sender, RoutedEventArgs e)
        {
            Surface.RemoveNode(this, true);
        }
        /// <summary>
        /// Lorsque le Menu de séparation du parent est activé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSeparateSelection(object sender, RoutedEventArgs e)
        {
            parent = null;

            // le fichier a été modifié
            Surface.Modification(true);
        }






        // SET - GET
        /// <summary>
        /// Retourne le parent
        /// </summary>
        /// <returns></returns>
        public ScatterCustom GetParent()
        {
            return parent;
        }
        /// <summary>
        /// Rattache au Node passé en paramètre comme enfant
        /// </summary>
        /// <param name="newParent"></param>
        public void SetParent(ScatterCustom newParent)
        {
            if (newParent == null)
            {
                this.parent = null;
                // le fichier a été modifié
                Surface.Modification(true);
            }
            else if (this.parent == null)
            {
                this.parent = newParent;
                // le fichier a été modifié
                Surface.Modification(true);
            }
        }
        /// <summary>
        /// Retourne le centre du Node
        /// </summary>
        /// <returns></returns>
        public Point GetOrigin()
        {
            Point pt = this.PointFromScreen(this.ActualCenter);
            pt.Y -= this.Height / 2;
            return this.PointToScreen(pt);
        }

        /// <summary>
        /// Indique le type du Node
        /// </summary>
        /// <param name="str"></param>
        public void SetTypeOfNode(string str)
        {
            thisType = str;
        }
        /// <summary>
        /// Retourne le type du Node
        /// </summary>
        /// <returns></returns>
        public string GetTypeOfNode()
        {
            return thisType;
        }

    }
}
