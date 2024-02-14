using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinq_Parallell.Models
{
    public class Usuario
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string gender { get; set; }
        public string company { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string dni { get; set; }


        public bool comprova_nom()
        {
            return Name.Length > 0;
        }

        public bool comprova_dni(){
            if (dni.Length != 9 || !dni.Substring(0, 8).All(char.IsDigit))
            {
                return false;
            }

            string numbers = dni.Substring(0, 8);

            char letter = dni.ToUpper()[8];

            string validLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

            int remainder = int.Parse(numbers) % 23;

            return letter == validLetters[remainder];
        }

        public bool comprova_mail()
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            int indexOfAt = email.IndexOf('@');
            int lastIndexOfDot = email.LastIndexOf('.');

            return indexOfAt >= 1 && indexOfAt < email.Length - 1 &&
                   lastIndexOfDot > indexOfAt;
        }

    }
}

