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
    public partial class NodeText : ScatterCustom
    {


        // en edition
        private bool isEditing = false;

        // parent
        ScatterCustom parent;
        // surfacewindow
        SurfaceWindow1 Surface;


        // TextBox du TextNode
        SurfaceTextBox STextBox;


        // clavier virtuel
        private ClavierVirtuel clavier;
        // palette de couleur
        private PaletteCouleurs palette;

        // couleur actuelle
        private Brush currentColor;

        public NodeText(SurfaceWindow1 parentSurface, ScatterCustom parentNode)
            : base(parentSurface, parentNode)
        {
            InitializeComponent();

            parent = parentNode;
            Surface = parentSurface;
            clavier = new ClavierVirtuel(this);
            palette = new PaletteCouleurs(this);

            STextBox = new SurfaceTextBox();
            STextBox.Name = "TextBoxNode";
            STextBox.IsEnabled = false;
            STextBox.TextWrapping = TextWrapping.Wrap;
            STextBox.MaxLines = 6;
            base.TextScatter.Children.Add(STextBox);

            ElementMenuItem MenuItem1 = new ElementMenuItem();
            MenuItem1.Header = "Color choice";
            MenuItem1.Click += new RoutedEventHandler(OnColorSelection);
            base.MainMenu.Items.Add(MenuItem1);

            ElementMenuItem MenuItem2 = new ElementMenuItem();
            MenuItem2.Header = "Edit";
            MenuItem2.Click += new RoutedEventHandler(OnEditSelection);
            base.MainMenu.Items.Add(MenuItem2);

            try
            {
                currentColor = ((NodeText)parent).GetColor();
            }
            catch
            {
                currentColor = new SolidColorBrush(Colors.LightBlue);
            }
            STextBox.Background = currentColor;

        }



        // evenement de selection du menu
        private void OnEditSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.AddonGrid.Items.Add(clavier);

                // on fait apparaitre le "curseur"
                STextBox.AppendText("|");
                clavier.CanMove = false;
                clavier.CanScale = false;
                clavier.CanRotate = false;
                isEditing = true;
            }
        }

        void OnColorSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                this.AddonGrid.Items.Add(palette);

                palette.SetFirstColor(currentColor);
                palette.CanMove = false;
                palette.CanScale = false;
                palette.CanRotate = false;
                isEditing = true;
            }
        }

        // ajout de texte
        public void AjoutTexte(string str)
        {

            if (str.Equals("Close"))
            {
                this.AddonGrid.Items.Remove(clavier);

                // on enlève le "curseur"
                string test = STextBox.Text;
                test = test.Remove(test.Length - 1);
                STextBox.Clear();
                STextBox.AppendText(test);

                isMoveEnable(true);
                isEditing = false;
            }
            else if (str.ToLower().Equals("backspace"))
            {
                if (STextBox.Text.Length > 1)
                {
                    string test = STextBox.Text;
                    test = test.Remove(test.Length - 2);
                    STextBox.Clear();
                    STextBox.AppendText(test);
                    STextBox.AppendText("|");
                }
            }
            else
            {
                string test = STextBox.Text;
                test = test.Remove(test.Length - 1);
                STextBox.Clear();
                STextBox.AppendText(test);
                STextBox.AppendText(str);
                STextBox.AppendText("|");
            }

        }

        // changement de couleur
        public void SetBackGroundColor(Brush color)
        {
            currentColor = color;
            STextBox.Background = currentColor;
            STextBox.BorderBrush = currentColor;
        }
        public void ClosePalette()
        {
            this.AddonGrid.Items.Remove(palette);
            isEditing = false;
        }
        public Brush GetColor()
        {
            return currentColor;
        }

        public void isMoveEnable(bool enable)
        {
            CanMove = enable;
            CanRotate = enable;
        }


    }
}
