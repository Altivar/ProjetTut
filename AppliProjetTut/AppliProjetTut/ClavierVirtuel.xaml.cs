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

            this.A.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Z.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.E.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.R.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.T.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Y.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.U.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.I.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.O.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.P.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Q.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.S.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.D.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.F.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.G.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.H.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.J.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.K.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.L.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.M.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.W.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.X.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.C.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.V.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.B.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.N.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
        }   

        void Cadenas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            isMovementEnabled = !isMovementEnabled;
            this.CanMove = isMovementEnabled;
            this.CanRotate = isMovementEnabled;
            this.CanScale = isMovementEnabled;
        }

        void OnLetterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            this.Text.AppendText(((SurfaceButton)sender).Content.ToString());
        }

        



        

    }
}
