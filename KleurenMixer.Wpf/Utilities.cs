using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

//Pas de namespace aan naar die van je code behind
namespace KleurenMixer.Wpf
{
    public partial class MainWindow
    {
        void ToonMelding(string melding, bool isSucces = false)
        {
            tbkFeedback.Visibility = Visibility.Visible;
            tbkFeedback.Text = melding;
            tbkFeedback.Background = isSucces == true ? Brushes.DarkGreen : Brushes.Red;
        }

        bool IsGeldigGetal(TextBox textBox, Type type)
        {
            string getal;
            string typeNaam = "";
            bool inputCorrect = false;

            getal = textBox.Text;

            if (string.IsNullOrEmpty(getal.Trim()))
            {
                textBox.Focus();
                textBox.SelectAll();
                throw new Exception("Vul een getal in");
            }
            try
            {
                switch (type.Name)
                {
                    case "Int32":
                        typeNaam = "geheel getal";
                        int.Parse(getal);
                        break;
                    case "Single":
                        typeNaam = "kommagetal";
                        float.Parse(getal);
                        break;
                    case "Decimal":
                        typeNaam = "decimaal getal";
                        decimal.Parse(getal);
                        break;
                    default:
                        break;
                }
                inputCorrect = true;
            }
            catch (Exception ex)
            {
                textBox.Focus();
                textBox.SelectAll();
                throw new Exception($"{getal} is geen geldig {typeNaam}\n" + ex.Message);
            }
            finally
            {
                MarkeerTextBox(textBox, inputCorrect);
            }
            return inputCorrect;
        }

        void MarkeerTextBox(TextBox textBox, bool inputCorrect)
        {
            if (inputCorrect)
            {
                textBox.BorderThickness = new Thickness(1);
                textBox.BorderBrush = new SolidColorBrush(Colors.Gray);
                textBox.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                textBox.BorderThickness = new Thickness(3);
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                textBox.Background = new SolidColorBrush(Colors.LightPink);
            }
        }

        private void IntegerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput((TextBox)sender, typeof(int));
        }

        private void FloatTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput((TextBox)sender, typeof(float));
        }

        private void DecimalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput((TextBox)sender, typeof(decimal));
        }

        void CheckInput(TextBox textBox, Type type)
        {
            try
            {
                if (IsGeldigGetal(textBox, type))
                {
                    tbkFeedback.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                ToonMelding(ex.Message);
            }
        }

        void ClearPanel(Panel toClear)
        {
            foreach (object control in toClear.Children)
            {
                Console.WriteLine(control.ToString());
                if (control is TextBox)
                {
                    TextBox castedControl = (TextBox)control;
                    castedControl.Text = string.Empty;
                }
                else if (control is Label)
                {
                    Label castedControl = (Label)control;
                    if (castedControl.Name.Length > 0)
                    {
                        castedControl.Content = "";
                    }
                }
                else if (control is TextBlock)
                {
                    TextBlock castedControl = (TextBlock)control;
                    if (castedControl.Name.Length > 0)
                    {
                        castedControl.Text = "";
                    }
                }
                else if (control is Selector)
                {
                    Selector castedControl = (Selector)control;
                    castedControl.SelectedIndex = -1;
                }
                else if (control is DatePicker)
                {
                    DatePicker castedControl = (DatePicker)control;
                    castedControl.SelectedDate = DateTime.Today;
                }
                else if (control is Slider)
                {
                    Slider castedControl = (Slider)control;
                    castedControl.Value = castedControl.Minimum;
                }
                else if (control is CheckBox)
                {
                    CheckBox castedControl = (CheckBox)control;
                    castedControl.IsChecked = false;
                }
            }

        }

        void BlockPanel(Panel toClear)
        {
            foreach (object control in toClear.Children)
            {
                ((Control)control).IsEnabled = false;
            }
        }
    }
}
