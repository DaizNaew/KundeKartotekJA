using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KundeKartotekJA
{
    class Kunde
    {

        private int kundeID;
        private string kundeNavn;
        private string kundeFirma;
        private string kundeTLF;
        private string kundeEmail;


        public Kunde() { }

        public int KundeID
        {
            get { return kundeID; }
            set { kundeID = value; }
        }
        public string KundeNavn
        {
            get { return kundeNavn; }
            set { kundeNavn = value; }
        }
        public string KundeFirma
        {
            get { return kundeFirma; }
            set { kundeFirma = value; }
        }
        public string KundeTLF
        {
            get { return kundeTLF; }
            set { kundeTLF = value; }
        }
        public string KundeEmail
        {
            get { return kundeEmail; }
            set { kundeEmail = value; }
        }
    }
}