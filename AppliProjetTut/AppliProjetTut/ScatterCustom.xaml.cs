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


        public ScatterCustom(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;

            this.MainMenu.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnActivationMenu);

            CanScale = false;
        }


        // lorsque la node est chargée
        protected void OnNodeLoaded(object sender, RoutedEventArgs e)
        {
            // TODO : a implementer dans les classes héritée
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







        //
        //  OPEN MENU
        //
        void OnActivationMenu(object sender, TouchEventArgs e)
        {
            Surface.MenuIsOpened(this);
        }

        // SELECTION MENU
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

        public void OnRemoveSelection(object sender, RoutedEventArgs e)
        {
            Surface.RemoveNode(this, true);
        }

        public void OnSeparateSelection(object sender, RoutedEventArgs e)
        {
            parent = null;
        }






        // SET - GET

        public ScatterCustom GetParent()
        {
            return parent;
        }
        public void SetParent(ScatterCustom newParent)
        {
            if (this.parent == null)
            {
                this.parent = newParent;
            }
            else if (newParent == null)
            {
                this.parent = null;
            }
        }

        public Point GetOrigin()
        {
            Point pt = this.PointFromScreen(this.ActualCenter);
            pt.Y -= this.Height / 2;
            return this.PointToScreen(pt);
        }

        

    }
}
