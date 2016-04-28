using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KundeKartotekJA
{
    public class Kunder
    {
        private int kundeID;
        private string kundeNavn;
        private string kundeFirma;
        private int kundeKontaktTLF;
        private string kundeEmail;

        public Kunder(){}
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
        public int KundeKontaktTLF
        {
            get { return kundeKontaktTLF; }
            set { kundeKontaktTLF = value; }
        }
        public string KundeEmail
        {
            get { return kundeEmail; }
            set { kundeEmail = value; }
        }
    }
}