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
        // Nombre maximal de caractères
        int MaxLength = -1;
        // Si le cadenas est activé
        bool isLocked = false;

        SurfaceScrollViewer SScrollViewer;

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

            base.SetTypeOfNode("Text");

            STextBox = new SurfaceTextBox();
            STextBox.Name = "TextBoxNode";
            STextBox.IsEnabled = false;
            STextBox.TextWrapping = TextWrapping.Wrap;
            base.TypeScatter.Children.Add(STextBox);

            SScrollViewer = new SurfaceScrollViewer();
            SScrollViewer.Width = 300;
            SScrollViewer.Height = 200;
            SScrollViewer.Content = STextBox;
            SScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            SScrollViewer.ScrollToEnd();

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
                currentColor = new SolidColorBrush(Colors.Black);
            }
            STextBox.Background = currentColor;

        }


        //
        //   EVENT de selection du menu
        //
        private void OnEditSelection(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                base.AddonGrid.Items.Add(clavier);

                // on fait apparaitre le "curseur"
                STextBox.AppendText("|\r");
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


        //
        //   INTERACTION AVEC LE CLAVIER
        //
        public void AjoutTexte(string str)
        {
            if (str.Equals("Close"))
            {
                this.AddonGrid.Items.Remove(clavier);

                // on enlève le "curseur"
                string test = STextBox.Text;
                if (MaxLength == -1)
                {
                    test = test.Remove(test.Length - 2);
                }
                else
                {
                    test = test.Remove(test.Length - 1);
                }
                STextBox.Clear();
                STextBox.AppendText(test);

                isMoveEnable(true);
                isEditing = false;
            }
            else if (str.ToLower().Equals("backspace"))
            {
                if (MaxLength == -1)
                {
                    if (STextBox.Text.Length > 2)
                    {
                        string test = STextBox.Text;
                        test = test.Remove(test.Length - 3);
                        STextBox.Clear();
                        STextBox.AppendText(test);
                        if (STextBox.Text.Length < 1 && MaxLength != -1)
                        {
                            clavier.EnableEnterKeys(false);
                        }
                        STextBox.AppendText("|\r");

                        // le fichier a été modifié
                        Surface.Modification(true);
                    }
                }
                else
                {
                    if (STextBox.Text.Length > 1)
                    {
                        string test = STextBox.Text;
                        test = test.Remove(test.Length - 2);
                        STextBox.Clear();
                        STextBox.AppendText(test);
                        if (STextBox.Text.Length < 1 && MaxLength != -1)
                        {
                            clavier.EnableEnterKeys(false);
                        }
                        STextBox.AppendText("|");
                    }
                }
            }
            else
            {

                if (STextBox.Text.Length - 2 >= MaxLength && MaxLength != -1)
                    return;

                if (MaxLength == -1)
                {
                    string test = STextBox.Text;
                    test = test.Remove(test.Length - 2);
                    STextBox.Clear();
                    STextBox.AppendText(test);
                    STextBox.AppendText(str);
                    STextBox.AppendText("|\r");
                    clavier.EnableEnterKeys(true);
                    // le fichier a été modifié
                    Surface.Modification(true);
                }
                else
                {
                    string test = STextBox.Text;
                    test = test.Remove(test.Length - 1);
                    STextBox.Clear();
                    STextBox.AppendText(test);
                    STextBox.AppendText(str);
                    STextBox.AppendText("|");
                    clavier.EnableEnterKeys(true);
                }

            }

            if (STextBox.LineCount > 8 && isLocked)
            {
                STextBox.Width = 250;
                SScrollViewer.ScrollToBottom();
            }
            else
            {
                STextBox.Width = 300;
            }

        }
        


        //
        //   INTERACTION AVEC LA PALETTE
        //
        public void SetBackGroundColor(Brush color)
        {
            currentColor = color;
            STextBox.Background = currentColor;
            STextBox.BorderBrush = currentColor;
            Surface.Modification(true);
        }
        public void ClosePalette()
        {
            this.AddonGrid.Items.Remove(palette);
            isEditing = false;
        }



        //
        // autres fonctions utiles
        //
        public Brush GetColor()
        {
            return currentColor;
        }

        public void isMoveEnable(bool enable)
        {
            CanMove = enable;
            CanRotate = enable;
            isLocked = !enable;

            if (STextBox.LineCount > 8 && isLocked)
            {
                STextBox.Width = 250;
                SScrollViewer.ScrollToBottom();
            }
            else
            {
                STextBox.Width = 300;
            }


            if (!enable)
            {
                //si le node est locké on peut utiliser la scrollbar sur le textbox
                base.TypeScatter.Children.Remove(STextBox);
                SScrollViewer.Content = STextBox;
                base.TypeScatter.Children.Add(SScrollViewer);
            }
            else
            {
                try
                {
                    //si le node n'est pas locké pas de scrollbar
                    SScrollViewer.Content = null;
                    base.TypeScatter.Children.Remove(SScrollViewer);
                    base.TypeScatter.Children.Add(STextBox);
                }
                catch { }
            }
        }

        public ClavierVirtuel GetClavier()
        {
            return clavier;
        }

        public string GetText()
        {
            return STextBox.Text.ToString();
        }

        //
        //  prevu pour la sauvegarde de fichier
        //
        public void TransformToFileSaver()
        {
            // on adapte la taille
            base.Height = 0;
            base.MainGrid.Height = 50;
            // on limite le nombre de ligne
            STextBox.MaxLines = 1;
            // nombre de caractères limité à 20
            MaxLength = 20;
            // on modifie la couleur de base
            base.Background = new SolidColorBrush(Colors.Black);
            STextBox.Background = new SolidColorBrush(Colors.Black);
            STextBox.BorderBrush = new SolidColorBrush(Colors.Black);

            // on desactive les caracteres speciaux du clavier
            clavier.DisableSpecialCarac();

            base.AddonGrid.Margin = new Thickness(150, 100, 150, -100);

            // on cache le menu
            base.MainMenu.Visibility = System.Windows.Visibility.Hidden;

            // on active le clavier
            base.AddonGrid.Items.Add(clavier);
            // on fait apparaitre le "curseur"
            STextBox.AppendText("|");
            clavier.CanMove = false;
            clavier.CanScale = false;
            clavier.CanRotate = false;
            isEditing = true;

        }


        //
        //  CHARGEMENT du NodeText
        //
        public void LoadText(string str)
        {
            STextBox.Clear();
            STextBox.AppendText(str);
        }

    }
}
