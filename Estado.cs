using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class Estado
    {

        public List<int> irInt;
        public List<int> moverEstado;
        public List<string> irString;
        public int numeroEstado;

        public Estado(int noEstado)
        {
            this.irInt = new List<int>();
            this.moverEstado = new List<int>();
            this.irString = new List<string>();
            this.numeroEstado = noEstado;
        }


    }
}
