using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kleuren.Lib.Entities;

namespace Kleuren.Lib.Services
{
    public class KleurenBeheer
    {
        public static List<Kleur> Kleuren { get; set; } = new List<Kleur>();

        public static void MaakVoorbeeldKleuren()
        {
            Kleur rood = new Kleur("Rood", new int[] { 255, 0, 0 });
            Kleur groen = new Kleur("Groen", new int[] { 0, 255, 0 });
            Kleur blauw = new Kleur("Blauw", new int[] { 0, 0, 255 });

            Kleuren = new List<Kleur> { rood, groen, blauw };
        }

        public static void Verwijder(Kleur teVerwijderen)
        {
            if (teVerwijderen != null && IsBestaandIdInKleuren(teVerwijderen))
            {
                Kleuren.Remove(teVerwijderen);
            }
            else throw new Exception
            ("Geef een geldige kleur door om te verwijderen");
        }

        static bool IsBestaandIdInKleuren(Kleur teChecken)
        {
            bool gevonden = false;
            foreach (Kleur kleur in Kleuren)
            {
                if (kleur.Id == teChecken.Id)
                {
                    gevonden = true;
                    break;
                }
            }
            return gevonden;
        }

        static void CheckUniciteitInKleuren(Kleur teChecken)
        {
            foreach (Kleur kleur in Kleuren)
            {
                if (kleur.Naam == teChecken.Naam) throw new Exception($"{kleur.Naam} bestaat reeds");
                if (kleur.RGB.Equals(teChecken.RGB)) throw new Exception($"{String.Join(", ",kleur.RGB)} bestaat reeds");
            }
        }

        //

        public static void SlaOp(Kleur opTeSlaan)
        {
            if (opTeSlaan == null) throw new Exception("Geef een geldige kleur door");
            else if (!IsBestaandIdInKleuren(opTeSlaan))
            {
                CheckUniciteitInKleuren(opTeSlaan);
                Kleuren.Add(opTeSlaan);
            }
            else
            {
                int indexKleur = GeefIndexInKleuren(opTeSlaan);
                Kleuren[indexKleur] = opTeSlaan;
            }
        }

        static int GeefIndexInKleuren(Kleur teChecken)
        {
            int index = -1;
            for (int i = 0; i < Kleuren.Count; i++)
            {
                if (Kleuren[i].Id == teChecken.Id)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        /*






                */
    }
}
