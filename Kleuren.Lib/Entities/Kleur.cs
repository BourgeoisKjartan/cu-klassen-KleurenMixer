using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kleuren.Lib.Entities
{
    public class Kleur
    {
        public int Id { get; set; }

        private string naam;

        public string Naam
        {
            get { return naam; }
            set
            {
                naam = (value.Trim().Length >= 3) ?
                        naam = value :
                        throw new Exception("De naam moet minstens 3 letters tellen");
            }
        }

        private int[] rgb;

        public int[] RGB
        {
            get { return rgb; }
            set
            {
                foreach (int waarde in value)
                {
                    if (waarde < 0 || waarde > 255) throw new Exception("De RGB-waarden moeten tussen 0 en 255 liggen");
                }
                rgb = value;
            }
        }

        //public int[] RGB { get; set; } = new int[3];

        public static int MaxId { get; private set; }

        public Kleur(string naam, int[] rgb = null, int id = 0)
        {
            Naam = naam;
            RGB = rgb;
            if (id <= 0) Id = ++MaxId;
            else Id = id;
            BepaaldMaxId();
        }

        public override string ToString()
        {
            string rgb = String.Join(",", RGB);
            string info = $"{Id} - {Naam}\n\tRGB: {rgb}";
            return info;
        }

        void BepaaldMaxId()
        {
            if (Id > MaxId) MaxId = Id;
        }

        string GeefHexCode()
        {
            string hexCode = "#";
            foreach (int rgbWaarde in RGB)
            {
                //rgb wordt omgezet naar hexadecimaal en er wordt een 0 voor geplaatst
                string hexRGB = "0" + rgbWaarde.ToString("X");
                //de laatste twee tekens van hexRGB worden behouden
                hexRGB = hexRGB.Substring(hexRGB.Length - 2, 2);
                hexCode += hexRGB;
            }
            return hexCode;
        }

        public Brush GeefBrush()
        {
            BrushConverter bc = new BrushConverter();
            Brush brush = (Brush)bc.ConvertFrom(GeefHexCode());
            return brush;
        }
    }
}
