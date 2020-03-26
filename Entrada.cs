using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class Entrada
    {
        public string nombre;
        public string cadena;
        public int columna;

        public Entrada()
        {

        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public void setCadena(string cadena)
        {
            this.cadena = cadena;
        }

        public string getCadena()
        {
            return this.cadena;
        }

        public void setColumna(int columna)
        {
            this.columna = columna;
        }

        public int getColumna()
        {
            return this.columna;
        }
    }
}
