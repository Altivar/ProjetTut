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

        // mouvement
        private bool isMovementEnabled = true;

        // Node
        private NodeText NodeParent;


        public ClavierVirtuel(NodeText parent)
        {
            InitializeComponent();

            // initialize nodeparent
            NodeParent = parent;

            this.CanScale = false;

            //this.Text.IsEnabled = false;

            this.Cadenas.PreviewTouchDown += new EventHandler<TouchEventArgs>(Cadenas_PreviewTouchDown);

            this.Carré.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.et.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.é.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.dbl_quotes.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.quote.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.par.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.par1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.è.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.under.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.ç.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.à.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.parbis.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.par1bis.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.arobase.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Backspace.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Tab.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Circonflexe.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Dollars.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.ù.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Stars.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.virgule.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.interogation.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.point.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.pointvrgl.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.dble.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.exclamation.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.plus.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.moins.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.multiplier.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.diviser.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.égal.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Point.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);

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
            this.zero.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.un.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.deux.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.trois.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.quatre.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.cinq.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.six.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.sept.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.huit.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.neuf.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);

            this.Enter.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Entrer.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Space.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);

            this.Caps_Lock.PreviewTouchDown += new EventHandler<TouchEventArgs>(Caps_PreviewTouchDown);
            this.Tab.PreviewTouchDown += new EventHandler<TouchEventArgs>(Tab_PreviewTouchDown);


            this.close.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
        }

        void Cadenas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            isMovementEnabled = !isMovementEnabled;
            NodeParent.isMoveEnable(isMovementEnabled);
        }

        void OnLetterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
        }

        void Enter_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
        }

        void Space_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
        }

        void Caps_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
        }
        void Tab_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
        }

    }
}
