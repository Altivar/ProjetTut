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
    /// Logique d'interaction pour ClavierVirtuel.xaml
    /// </summary>
    public partial class ClavierVirtuel : ScatterViewItem
    {

        private bool isMovementEnabled = true;

        public ClavierVirtuel()
        {
            InitializeComponent();
            
            this.Text.IsEnabled = false;

            this.Cadenas.PreviewTouchDown += new EventHandler<TouchEventArgs>(Cadenas_PreviewTouchDown);

            this.A.PreviewTouchDown += new EventHandler<TouchEventArgs>(A_PreviewTouchDown);
            this.Z.PreviewTouchDown += new EventHandler<TouchEventArgs>(Z_PreviewTouchDown);
            this.E.PreviewTouchDown += new EventHandler<TouchEventArgs>(E_PreviewTouchDown);
        }

        void Cadenas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            isMovementEnabled = !isMovementEnabled;
            this.CanMove = isMovementEnabled;
            this.CanRotate = isMovementEnabled;
            this.CanScale = isMovementEnabled;
        }

        void E_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            this.Text.AppendText("E");
        }

        void Z_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            this.Text.AppendText("Z");
        }

        void A_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            this.Text.AppendText("A");
        }



        

    }
}
