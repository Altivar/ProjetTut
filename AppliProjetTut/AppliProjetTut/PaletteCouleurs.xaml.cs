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
    /// Logique d'interaction pour PaletteCouleurs.xaml
    /// </summary>
    public partial class PaletteCouleurs : ScatterViewItem
    {

        // NodeText parent;
        private NodeText parentNode;

        // couleur d'origine
        private Brush firstColor = new SolidColorBrush(Colors.LightBlue);

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="parent"></param>
        public PaletteCouleurs(NodeText parent)
        {
            InitializeComponent();

            parentNode = parent;

            this.Rouge.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Rouge1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Rouge2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Orange.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Orange1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Orange2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Jaune.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Jaune1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Jaune2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Vert.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Vert1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Vert2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Bleu.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Bleu1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Bleu2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Violet.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Violet1.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);
            this.Violet2.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnColorPreviewTouchDown);

            this.Annuler.PreviewTouchDown += new EventHandler<TouchEventArgs>(OnAnnulerPreviewTouchDown);
            this.Valider.PreviewTouchDown += new EventHandler<TouchEventArgs>(Valider_PreviewTouchDown);
        }

        /// <summary>
        /// Appelé lorsque le choix est validé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Valider_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            firstColor = ((SurfaceButton)sender).Background;
            parentNode.ClosePalette();
        }
        /// <summary>
        /// Appelé lorsque le choix est annulé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAnnulerPreviewTouchDown(object sender, TouchEventArgs e)
        {
            parentNode.SetBackGroundColor(firstColor);
            parentNode.ClosePalette();
        }
        /// <summary>
        /// Appelé lors de l'appui d'une touche couleur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnColorPreviewTouchDown(object sender, TouchEventArgs e)
        {
            parentNode.SetBackGroundColor( ((SurfaceButton)sender).Background );
        }
        /// <summary>
        /// Récupère la couleur avant que la palette de couleurs ne s'ouvre
        /// </summary>
        /// <param name="col"></param>
        public void SetFirstColor(Brush col)
        {
            firstColor = col;
        }



    }
}
