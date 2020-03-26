using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class ExpresionRegular
    {

        string nombre;
        string expresion;
        List<string> lista;
        Grafo grafo;
        public MetodoThompson mt;

        public ExpresionRegular()
        {
            this.lista = new List<string>();
            this.grafo = new Grafo();
        }

        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public string getNombre()
        {
            return this.nombre;
        }

        public void setExpresion(string expresion, Form1 form)
        {
            this.expresion = expresion;

            StringBuilder token = new StringBuilder();
            char[] cadena = this.expresion.ToCharArray();
            for(int i = 0; i < cadena.Length; i++)
            {
                string nuevo;
                if (cadena[i] == '.' || cadena[i] == '+'  || cadena[i] == '*' 
                    || cadena[i] == '?' || cadena[i] == '|')
                {
                    nuevo = cadena[i] + ".";
                    lista.Add(nuevo);
                } else if(cadena[i] == '\"')
                {
                    for(int j = i+1; j < cadena.Length; j++)
                    {
                        if(cadena[j] == '\"' && (int)cadena[j-1] != 92)
                        {
                            i = j;
                            nuevo = "\\\"" + token.ToString() + "\\\"";
                            this.lista.Add(nuevo);
                            token.Clear();
                            break;
                        }
                        token.Append(cadena[j]);
                    }
                } else if(cadena[i] == '{')
                {
                    for(int j = i+1; j < cadena.Length; j++)
                    {
                        if(cadena[j] == '}')
                        {
                            i = j;
                            nuevo = token.ToString();
                            lista.Add(nuevo);
                            token.Clear();
                            break;
                        }
                        token.Append(cadena[j]);
                    }
                }
                
            }
            this.grafo.listaER = new List<string>(this.lista);
            this.grafo.hacerGrafo();
            this.grafo.dibujarGrafo(form);
            this.grafo.limpiarLista();
            this.mt = new MetodoThompson(this.grafo);
            //Console.WriteLine("---------------------- esto es nuevo --------------------");
            this.mt.buscarCabeza();
            this.mt.dibujarAfd(form);
            this.mt.dibujarTabla(this.grafo.listaER, form);
            this.lista.Clear();
        }
    }
}
