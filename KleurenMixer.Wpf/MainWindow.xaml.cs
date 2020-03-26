using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kleuren.Lib.Services;
using Kleuren.Lib.Entities;


namespace KleurenMixer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum EditModes { editing, readOnly, canSave }   

    public partial class MainWindow : Window
    {
        Kleur huidigeKleur;
        

        public MainWindow()
        {
            InitializeComponent();
            KleurenBeheer.MaakVoorbeeldKleuren();
        }

        void KoppelDynamischeLijsten()
        {
            lstKleuren.ItemsSource = KleurenBeheer.Kleuren;
            lstKleuren.Items.Refresh();
        }

        void KoppelStatischeLijsten()
        {
            for (int i = 0; i < 256; i++)
            {
                cmbGroen.Items.Add(i);
            }
        }

        void PasEditModeToe(EditModes editMode)
        {
            WijzigBruikbaarheidControls(grdKleur);
            switch (editMode)
            {
                case EditModes.editing:
                    btnOpslaan.IsEnabled = false;
                    break;
                case EditModes.readOnly:
                    WijzigBruikbaarheidControls(grdKleur, false);
                    btnNieuw.IsEnabled = true;
                    break;
                case EditModes.canSave:
                    break;
                default:
                    WijzigBruikbaarheidControls(grdKleur, false);
                    break;
            }
        }

        void WijzigBruikbaarheidControls(Panel panel, bool ingeschakeld = true)
        {
            foreach (object control in panel.Children)
            {
                ((Control)control).IsEnabled = ingeschakeld;
            }
        }

        void ToonDetails(Kleur gekozenKleur)
        {
            lblId.Content = gekozenKleur.Id;
            txtNaam.Text = gekozenKleur.Naam;
            txtRood.Text = gekozenKleur.RGB[0].ToString();
            cmbGroen.SelectedItem = gekozenKleur.RGB[1];
            sldBlauw_ValueChanged(null, null);
            sldBlauw.Value = gekozenKleur.RGB[2];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KoppelStatischeLijsten();
            KoppelDynamischeLijsten();
            PasEditModeToe(EditModes.readOnly);
            lstKleuren.SelectedIndex = 0;
            txtRood.TextChanged += IntegerTextBox_TextChanged;
        }

        private void sldBlauw_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblBlauw.Content = $"Blauw\n{sldBlauw.Value}";
        }

        private void lstKleuren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstKleuren.SelectedItem != null)
            {
                PasEditModeToe(EditModes.canSave);
                huidigeKleur = (Kleur)lstKleuren.SelectedItem;
                ToonDetails(huidigeKleur);
                lblKleur.Background = huidigeKleur.GeefBrush();
            }
            else
            {
                VerwijderInput();
                
            }
        }

        void VerwijderInput()
        {
            ClearPanel(grdKleur);
            huidigeKleur = null;
            cmbGroen.SelectedIndex = 0;
            PasEditModeToe(EditModes.readOnly);
            tbkFeedback.Visibility = Visibility.Hidden;
            lblKleur.Background = Brushes.White;
        }

        Kleur GeefKleur(int id = 0)
        {
            Kleur kleur = null;
            string naam = txtNaam.Text;
            int groen = (int)cmbGroen.SelectedItem;
            int blauw = (int)sldBlauw.Value;
            try
            {
                int rood = int.Parse(txtRood.Text);
                kleur = new Kleur(naam, new int[] { rood, groen, blauw }, id);
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
            return kleur;
        }

        void CheckGeldigeInput()
        {
            tbkFeedback.Visibility = Visibility.Hidden;
            if (GeefKleur() == null) PasEditModeToe(EditModes.editing);
            else PasEditModeToe(EditModes.canSave);
        }

        private void txtNaam_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckGeldigeInput();
        }

        private void txtRood_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckGeldigeInput();
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            tbkFeedback.Visibility = Visibility.Hidden;
            if (huidigeKleur == null) ToonMelding("Kies de te verwijderen kleur");
            else
            {
                KleurenBeheer.Verwijder(huidigeKleur);
                KoppelDynamischeLijsten();
            } 
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = (huidigeKleur == null) ? 0 : huidigeKleur.Id;
                string naam;
                huidigeKleur = GeefKleur(id);
                naam = huidigeKleur.Naam;
                KleurenBeheer.SlaOp(huidigeKleur);
                lstKleuren.SelectedItem = null;
                KoppelDynamischeLijsten();
                VerwijderInput();
                ToonMelding($"{naam} is opgeslagen", true);

            }
            catch (Exception ex) 
            {
                ToonMelding(ex.Message);
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            lstKleuren.SelectedItem = null;
            PasEditModeToe(EditModes.editing);
            txtNaam.Focus();
            tbkFeedback.Visibility = Visibility.Hidden;
        }
    }
}
