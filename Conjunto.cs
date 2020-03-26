using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class Conjunto
    {
        string nombre;
        int tipo;// 1 - por comas, 2 - por rango, 3 - todos
        public List<char> caracteres;
        string cadena;

        public Conjunto()
        {
            caracteres = new List<char>();
        }

        public int getTipo()
        {
            return this.tipo;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public void setTipo(int tipo)
        {
            this.tipo = tipo;
        }

        public void setCadena(string cadena)
        {
            this.cadena = cadena;

            char[] chars = this.cadena.ToCharArray();

            int asciiActual = (int)chars[1];
            if(asciiActual == 44)
            {
                setTipo(1);
                string[] charsComa = this.cadena.Split(',');
                for(int i = 0; i < charsComa.Length; i++)
                {
                    this.setChar(charsComa[i].ToCharArray()[0]);
                }
            } else if(asciiActual == 126)
            {
                setTipo(2);
                int charA = (int)chars[0];
                int charB = (int)chars[2];
                for(int i = charA; i <= charB; i++)
                {
                    char c = (char)i;
                    this.setChar(c);
                }
            } else
            {
                setTipo(3);
                for(int i = 33; i < 128; i++)
                {
                    char c = (char)i;
                    this.setChar(c);
                }
            }
        }


        public void setChar(char caracter)
        {
            this.caracteres.Add(caracter);
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }
    }
}
