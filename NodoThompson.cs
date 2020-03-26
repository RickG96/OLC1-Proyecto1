using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class NodoThompson
    {

        public NodoThompson irA;
        public NodoThompson irB;

        public int identificador;

        public bool esCabeza;
        public bool esCola;
        public bool esCuerpo;

        public string aristaA;
        public string aristaB;

        public NodoThompson(int id)
        {
            this.irA = null;
            this.irB = null;
            this.identificador = id;
            this.esCabeza = false;
            this.esCola = false;
            this.esCuerpo = false;
            this.aristaA = "";
            this.aristaB = "";
        }

        public void setIrA(NodoThompson ir)
        {
            this.irA = ir;
        }

        public NodoThompson getIrA()
        {
            return this.irA;
        }

        public void setIrB(NodoThompson ir)
        {
            this.irB = ir;
        }

        public NodoThompson getIrB()
        {
            return this.irB;
        }

        public void setIdentificador(int id)
        {
            this.identificador = id;
        }

        public int getIdentificador()
        {
            return this.identificador;
        }

        public void setCabeza(bool confirmar)
        {
            this.esCabeza = confirmar;
        }

        public bool getCabeza()
        {
            return this.esCabeza;
        }

        public void setCuerpo(bool confirmar)
        {
            this.esCuerpo = confirmar;
        }

        public bool getCuerpo()
        {
            return this.esCuerpo;
        }

        public void setCola(bool confirmar)
        {
            this.esCola = confirmar;
        }

        public bool getCola()
        {
            return this.esCola;
        }

        public void setAristaA(string arista)
        {
            this.aristaA = arista;
        }

        public string getAristaA()
        {
            return this.aristaA;
        }

        public void setAristaB(string arista)
        {
            this.aristaB = arista;
        }

        public string getAristaB()
        {
            return this.aristaB;
        }
    }
}
