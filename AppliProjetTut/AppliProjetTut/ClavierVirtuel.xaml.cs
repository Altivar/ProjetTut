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
using Microsoft.Surface.Presentation.Input;
using System.Windows.Forms;

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

        // Gestion du LowerCase - UpperCase
        private bool TempLockedCaps = true;
        private bool LockedCaps = false;


        // Gestion d'un appui long sur le BackSpace
        Timer backSpaceTimer;
        bool isBackSpaceDown = false;
        int compteurBackSpace = 0;


        public ClavierVirtuel(NodeText parent)
        {
            InitializeComponent();

            // initialize nodeparent
            NodeParent = parent;

            this.CanScale = false;



            backSpaceTimer = new Timer();
            backSpaceTimer.Tick += new EventHandler(backSpaceTimer_Tick);
            backSpaceTimer.Interval = 100;
            backSpaceTimer.Start();


            
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

            this.close.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnClosePreviewTouchDown);

            this.Backspace.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnLetterPreviewTouchDown);
            this.Backspace.PreviewTouchUp += new EventHandler<TouchEventArgs>(Backspace_PreviewTouchUp);
        }



        //
        //  TIMER POUR LE BACKSPACE
        //
        void backSpaceTimer_Tick(object sender, EventArgs e)
        {
            if (isBackSpaceDown)
            {
                if (compteurBackSpace < 4)
                {
                    compteurBackSpace++;
                }
                else
                {
                    NodeParent.AjoutTexte("backspace");
                }
            }
        }
        void Backspace_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            compteurBackSpace = 0;
            isBackSpaceDown = false;
        }



        //
        //  GESTION FERMETURE
        //
        void OnClosePreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne ferme la fenêtre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                this.Cadenas.Foreground = new SolidColorBrush(Colors.Black);
                this.Cadenas.Background = new SolidColorBrush(Colors.LightBlue);
                isMovementEnabled = true;
                NodeParent.isMoveEnable(isMovementEnabled);
                NodeParent.AjoutTexte("Close");
            }
        }

        void Cadenas_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne bloque la fenêtre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                isMovementEnabled = !isMovementEnabled;
                NodeParent.isMoveEnable(isMovementEnabled);
                if (isMovementEnabled)
                {
                    this.Cadenas.Foreground = new SolidColorBrush(Colors.Black);
                    this.Cadenas.Background = new SolidColorBrush(Colors.LightBlue);
                }
                else
                {
                    this.Cadenas.Foreground = new SolidColorBrush(Colors.White);
                    this.Cadenas.Background = new SolidColorBrush(Colors.Teal);
                }
            }
        }

        

        void OnLetterPreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne tape une lettre que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                if (((SurfaceButton)sender).Content.ToString().ToLower() == "backspace")
                {
                    isBackSpaceDown = true;
                }

                if (TempLockedCaps)
                {
                    NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
                    TempLockedCaps = false;
                    this.Caps_Lock.Foreground = new SolidColorBrush(Colors.Black);
                    this.Caps_Lock.Background = new SolidColorBrush(Colors.Lavender);
                }
                else if (LockedCaps)
                {
                    NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
                }
                else
                {
                    NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString().ToLower());
                }
            }
        }

        void Enter_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne passe à la ligne que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }

        void Space_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne fais un espace que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }

        void Caps_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //n'utilise le caps qu'avec le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                if (LockedCaps)
                {
                    LockedCaps = false;
                    this.Caps_Lock.Foreground = new SolidColorBrush(Colors.Black);
                    this.Caps_Lock.Background = new SolidColorBrush(Colors.Lavender);
                    return;
                }
                if (TempLockedCaps)
                {
                    LockedCaps = true;
                    TempLockedCaps = false;
                    this.Caps_Lock.Foreground = new SolidColorBrush(Colors.White);
                    this.Caps_Lock.Background = new SolidColorBrush(Colors.DarkGoldenrod);
                    return;
                }

                TempLockedCaps = true;
                if (TempLockedCaps)
                {
                    this.Caps_Lock.Foreground = new SolidColorBrush(Colors.White);
                    this.Caps_Lock.Background = new SolidColorBrush(Colors.BurlyWood);
                }
            }
        }

        void Tab_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            //ne fait de tabulation que si on utilise le doigt
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                NodeParent.AjoutTexte(((SurfaceButton)sender).Content.ToString());
            }
        }






        //
        //  DESACTIVE les caractères SPECIAUX
        //
        public void DisableSpecialCarac()
        {
            dbl_quotes.IsEnabled = false;
            quote.IsEnabled = false;
            par1.IsEnabled = false;
            par1bis.IsEnabled = false;
            Tab.IsEnabled = false;
            Stars.IsEnabled = false;
            shift.IsEnabled = false;
            interogation.IsEnabled = false;

            Enter.IsEnabled = false;
            Entrer.IsEnabled = false;

            KeyGrid.Children.Remove(Cadenas);
        }
        public void EnableEnterKeys(bool enable)
        {
            Enter.IsEnabled = enable;
            Entrer.IsEnabled = enable;
        }


    }
}
