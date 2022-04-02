using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chardonnay1erCru
{
    public class Bouteille
    {
        private string nom;

        private int quantite;



        public string Nom 
        {
            get { return nom; }
            set { nom = value; }
        }

        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }



    }
}
