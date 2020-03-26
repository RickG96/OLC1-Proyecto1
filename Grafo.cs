using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class Grafo
    {

        public List<NodoThompson> listaNodos;
        public List<string> listaER;
        public int contadorNodos;

        public Grafo()
        {
            this.listaNodos = new List<NodoThompson>();
            this.contadorNodos = 0;
        }

        public void dibujarGrafo(Form1 form)
        {
            StringBuilder strGrafo = new StringBuilder();

            strGrafo.Append("digraph G {");
            strGrafo.AppendLine();
            strGrafo.Append("rankdir=LR");
            strGrafo.AppendLine();

            for(int i = 0; i < listaNodos.Count; i ++)
            {
                strGrafo.Append("nodo" + listaNodos[i].identificador + "[shape=circle,color=firebrick3,label=" + listaNodos[i].identificador + "];");
                strGrafo.AppendLine();
            }

            for(int i = 0; i < listaNodos.Count; i ++)
            {
                if(listaNodos[i].getIrA() != null)
                {
                    strGrafo.Append("nodo" + listaNodos[i].getIdentificador() + " -> nodo" + listaNodos[i].getIrA().getIdentificador() + " [color=firebrick3,label=\"" + listaNodos[i].getAristaA() + "\"];");
                    strGrafo.AppendLine();
                }
                if(listaNodos[i].getIrB() != null)
                {
                    strGrafo.Append("nodo" + listaNodos[i].getIdentificador() + " -> nodo" + listaNodos[i].getIrB().getIdentificador() + " [color=firebrick3,label=\"" + listaNodos[i].getAristaB() + "\"];");
                    strGrafo.AppendLine();
                }
            }

            strGrafo.Append("}");

            try
            {
                Random random = new Random();
                int numero = random.Next(1000, 9999);
                string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\grafoAFN" + numero + ".txt";
                string auxImg = @"C:\Users\Pistacho\Desktop\reportesP2\grafoAFN" + numero + ".png";
                File.WriteAllText(grafoDot, strGrafo.ToString());
                Thread.Sleep(500);
                ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                startInfo.Arguments = "-Tpng " + grafoDot + " -o " + auxImg;
                form.consola.Text += "Se creo la imagen del grafo: grafoAFN" + numero + ".png\n\r\n\r"; 
                Process.Start(startInfo);
                //Thread.Sleep(500);
                //Form1.consola.Text += "Se creo reporte de tokens: " + grafoDot + "\n\r\n\r";
            } catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void hacerGrafo()
        {
            Stack<string> pila1 = new Stack<string>();
            Stack<string> pila2 = new Stack<string>();
            //Console.WriteLine(String.Join(",", listaER));
            string aux1;
            string aux2;
            //Console.WriteLine("" + listaER.Count);
            for (int i = listaER.Count - 1; i >= 0; i--)
            {
                //Console.WriteLine("" + i);
                if (listaER[i].Equals(".."))
                {
                    pila1.Push(listaER[i]);

                    if (pila2.Count >= 2)
                    {
                        pila1.Pop();
                        aux1 = pila2.Pop();
                        aux2 = pila2.Pop();
                        if (aux1.Equals("runi2k") || aux2.Equals("runi2k"))
                        {
                            if (aux1.Equals("runi2k") && aux2.Equals("runi2k"))
                            {
                                this.concatenarGrafoToken("", true, false);
                            }
                            else
                            {
                                if (aux1.Equals("runi2k"))
                                {
                                    this.concatenarGrafoToken(aux2, false, false);
                                } else
                                {
                                    this.concatenarGrafoToken(aux1, false, true);
                                }
                            }
                        } else
                        {
                            this.nuevoConcatenar(aux1, aux2);
                        }
                        pila2.Push("runi2k");
                    }
                } else if (listaER[i].Equals("|."))
                {
                    pila1.Push(listaER[i]);

                    if (pila2.Count >= 2)
                    {
                        pila1.Pop();
                        aux1 = pila2.Pop();
                        aux2 = pila2.Pop();
                        if (aux1.Equals("runi2k") || aux2.Equals("runi2k"))
                        {
                            if (aux1.Equals("runi2k") && aux2.Equals("runi2k"))
                            {
                                this.oGrafoToken("", true);
                            }
                            else
                            {
                                if (aux1.Equals("runi2k"))
                                {
                                    this.oGrafoToken(aux2, false);
                                }
                                else
                                {
                                    this.oGrafoToken(aux1, false);
                                }
                            }
                        }
                        else
                        {
                            this.nuevoO(aux1, aux2);
                        }
                        pila2.Push("runi2k");
                    }
                } else if (listaER[i].Equals("+."))
                {
                    pila1.Push(listaER[i]);

                    if (pila2.Count > 0)
                    {
                        pila1.Pop();
                        if (!pila2.Peek().Equals("runi2k"))
                        {
                            this.nuevoUnoMas(pila2.Pop());
                        } else
                        {
                            pila2.Pop();
                            this.unomasGrafo();
                        }
                        pila2.Push("runi2k");
                    }
                } else if (listaER[i].Equals("*."))
                {
                    pila1.Push(listaER[i]);

                    if (pila2.Count > 0)
                    {
                        pila1.Pop();
                        if (!pila2.Peek().Equals("runi2k"))
                        {
                            this.nuevoCeroMas(pila2.Pop());
                        }
                        else
                        {
                            pila2.Pop();
                            this.ceromasGrafo();
                        }
                        pila2.Push("runi2k");
                    }
                } else if (listaER[i].Equals("?."))
                {
                    pila1.Push(listaER[i]);

                    if (pila2.Count > 0)
                    {
                        pila1.Pop();
                        if (!pila2.Peek().Equals("runi2k"))
                        {
                            this.nuevoCeroUno(pila2.Pop());
                        }
                        else
                        {
                            pila2.Pop();
                            this.cerounoGrafo();
                        }
                        pila2.Push("runi2k");
                    }
                } else
                {
                    pila2.Push(listaER[i]);
                }
            }
            //Console.WriteLine(pila1.Count);
            //Console.WriteLine(pila2.Count);
        }

        public void nuevoConcatenar(string aristaA, string aristaB) // .
        {
            // crea 3 nodos
            NodoThompson nodo1 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(this.contadorNodos);
            contadorNodos++;

            nodo1.setAristaA(aristaA);
            nodo1.setIrA(nodo2);
            nodo1.setCabeza(true);

            nodo2.setAristaA(aristaB);
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setCola(true);

            listaNodos.Add(nodo1);
            listaNodos.Add(nodo2);
            listaNodos.Add(nodo3);
        }

        public void nuevoO(string aristaA, string aristaB) // | dos parametros de string
        {
            // crea 6 nodos
            NodoThompson nodo1 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo4 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo5= new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo6 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            
            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            nodo1.setIrA(nodo2);
            nodo1.setIrB(nodo4);
            nodo1.setCabeza(true);

            nodo2.setAristaA(aristaA);
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setAristaA("ε");
            nodo3.setIrA(nodo6);
            nodo3.setCuerpo(true);

            nodo4.setAristaA(aristaB);
            nodo4.setIrA(nodo5);
            nodo4.setCuerpo(true);

            nodo5.setAristaA("ε");
            nodo5.setIrA(nodo6);
            nodo5.setCuerpo(true);

            nodo6.setCola(true);

            this.listaNodos.Add(nodo1);
            this.listaNodos.Add(nodo2);
            this.listaNodos.Add(nodo3);
            this.listaNodos.Add(nodo4);
            this.listaNodos.Add(nodo5);
            this.listaNodos.Add(nodo6);
        }

        public void nuevoCeroMas(string arista) // * un parametro string
        {
            // crea 4 nodos
            NodoThompson nodo1 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo4 = new NodoThompson(this.contadorNodos);
            contadorNodos++;

            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            nodo1.setIrA(nodo2);
            nodo1.setIrB(nodo4);
            nodo1.setCabeza(true);

            nodo2.setAristaA(arista);
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setAristaA("ε");
            nodo3.setAristaB("ε");
            nodo3.setIrA(nodo4);
            nodo3.setIrB(nodo2);
            nodo4.setCuerpo(true);

            nodo4.setCola(true);

            this.listaNodos.Add(nodo1);
            this.listaNodos.Add(nodo2);
            this.listaNodos.Add(nodo3);
            this.listaNodos.Add(nodo4);
        }

        public void nuevoUnoMas(string arista) // + un parametro string
        {
            // crea 5 nodos
            NodoThompson nodo5 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo1 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo4 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            

            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            nodo1.setIrA(nodo2);
            nodo1.setIrB(nodo4);
            nodo1.setCuerpo(true);

            nodo2.setAristaA(arista);
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setAristaA("ε");
            nodo3.setAristaB("ε");
            nodo3.setIrA(nodo4);
            nodo3.setIrB(nodo2);
            nodo4.setCuerpo(true);

            nodo4.setCola(true);

            nodo5.setAristaA(arista);
            nodo5.setIrA(nodo1);
            nodo5.setCabeza(true);

            this.listaNodos.Add(nodo5);
            this.listaNodos.Add(nodo1);
            this.listaNodos.Add(nodo2);
            this.listaNodos.Add(nodo3);
            this.listaNodos.Add(nodo4);
        }

        public void nuevoCeroUno(string aristaA) // ? un parametro string
        {
            // crea 6 nodos
            NodoThompson nodo1 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo4 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo5 = new NodoThompson(this.contadorNodos);
            contadorNodos++;
            NodoThompson nodo6 = new NodoThompson(this.contadorNodos);
            contadorNodos++;

            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            nodo1.setIrA(nodo2);
            nodo1.setIrB(nodo4);
            nodo1.setCabeza(true);

            nodo2.setAristaA(aristaA);
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setAristaA("ε");
            nodo3.setIrA(nodo6);
            nodo3.setCuerpo(true);

            nodo4.setAristaA("ε");
            nodo4.setIrA(nodo5);
            nodo4.setCuerpo(true);

            nodo5.setAristaA("ε");
            nodo5.setIrA(nodo6);
            nodo5.setCuerpo(true);

            nodo6.setCola(true);

            this.listaNodos.Add(nodo1);
            this.listaNodos.Add(nodo2);
            this.listaNodos.Add(nodo3);
            this.listaNodos.Add(nodo4);
            this.listaNodos.Add(nodo5);
            this.listaNodos.Add(nodo6);
        }

        public void oGrafoToken(string arista, bool dosGrafos)
        {
            
            if(dosGrafos == false)
            {
                NodoThompson nodo1 = new NodoThompson(contadorNodos);
                contadorNodos++;
                NodoThompson nodo2 = new NodoThompson(contadorNodos);
                contadorNodos++;
                NodoThompson nodo3 = new NodoThompson(contadorNodos);
                contadorNodos++;
                NodoThompson nodo4 = new NodoThompson(contadorNodos);
                contadorNodos++;

                nodo1.setAristaA("ε");
                //lo apuntamos a la cabeza del grafo en la lista, y esta sera la nueva cabeza
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCabeza())
                    {
                        nodo1.setIrA(listaNodos[i]);
                        listaNodos[i].setCabeza(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }
                nodo1.setAristaB("ε");
                nodo1.setIrB(nodo2);
                nodo1.setCabeza(true);

                nodo2.setAristaA(arista);
                nodo2.setIrA(nodo3);
                nodo2.setCuerpo(true);

                nodo3.setAristaA("ε");
                nodo3.setIrA(nodo4);
                nodo3.setCuerpo(true);

                nodo4.setCola(true);
                //buscamos la cola del grafo y la apuntamos al nodo 4, la nueva cola
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCola())
                    {
                        listaNodos[i].setAristaA("ε");
                        listaNodos[i].setIrA(nodo4);
                        listaNodos[i].setCola(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }

                listaNodos.Add(nodo1);
                listaNodos.Add(nodo2);
                listaNodos.Add(nodo3);
                listaNodos.Add(nodo4);
            } else
            {
                NodoThompson nodo1 = new NodoThompson(contadorNodos);
                contadorNodos++;
                NodoThompson nodo2 = new NodoThompson(contadorNodos);
                contadorNodos++;


                nodo1.setAristaA("ε");
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCabeza())
                    {
                        nodo1.setIrA(listaNodos[i]);
                        listaNodos[i].setCabeza(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }
                nodo1.setAristaB("ε");
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCabeza())
                    {
                        nodo1.setIrB(listaNodos[i]);
                        listaNodos[i].setCabeza(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }
                nodo1.setCabeza(true);


                nodo2.setCola(true);
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCola())
                    {
                        listaNodos[i].setAristaA("ε");
                        listaNodos[i].setIrA(nodo2);
                        listaNodos[i].setCola(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }
                for (int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCola())
                    {
                        listaNodos[i].setAristaA("ε");
                        listaNodos[i].setIrA(nodo2);
                        listaNodos[i].setCola(false);
                        listaNodos[i].setCuerpo(true);
                        break;
                    }
                }

                listaNodos.Add(nodo1);
                listaNodos.Add(nodo2);
            }
        }

        public void concatenarGrafoToken(string arista, bool dosGrafos, bool primeroArista)
        {
            if(dosGrafos == false)
            {
                NodoThompson nodo = new NodoThompson(contadorNodos);
                contadorNodos++;

                if(primeroArista == true)
                {
                    nodo.setCabeza(true);
                    nodo.setAristaA(arista);
                    for(int i = listaNodos.Count - 1; i >= 0; i--)
                    {
                        if (listaNodos[i].getCabeza())
                        {
                            nodo.setIrA(listaNodos[i]);
                            listaNodos[i].setCabeza(false);
                            listaNodos[i].setCuerpo(true);
                            break;
                        }
                    }
                } else
                {
                    nodo.setCola(true);
                    for(int i = listaNodos.Count - 1; i >= 0; i--)
                    {
                        if(listaNodos[i].getCola())
                        {
                            listaNodos[i].setCola(false);
                            listaNodos[i].setCuerpo(true);
                            listaNodos[i].setAristaA(arista);
                            listaNodos[i].setIrA(nodo);
                            break;
                        }
                    }
                }

                listaNodos.Add(nodo);
            } else
            {
                for(int i = listaNodos.Count - 1; i >= 0; i--)
                {
                    if (listaNodos[i].getCola())
                    {
                        listaNodos[i].setCola(false);
                        listaNodos[i].setCuerpo(true);
                        int auxCabezas = 0;
                        for(int j = listaNodos.Count - 1; j >= 0; j--)
                        {
                            if (listaNodos[j].getCabeza()) auxCabezas++;
                            if(auxCabezas == 2)
                            {
                                listaNodos[j].setCabeza(false);
                                listaNodos[j].setCuerpo(true);
                                listaNodos[i].setAristaA("ε");
                                listaNodos[i].setIrA(listaNodos[j]);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        // probar de aqui pa abajo
        public void cerounoGrafo()
        {
            NodoThompson nodo1 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo4 = new NodoThompson(contadorNodos);
            contadorNodos++;

            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            nodo1.setIrB(nodo2);
            // buscar setIrA
            for(int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if(listaNodos[i].getCabeza())
                {
                    listaNodos[i].setCabeza(false);
                    listaNodos[i].setCuerpo(true);
                    nodo1.setIrA(listaNodos[i]);
                    break;
                }
            }
            nodo1.setCabeza(true);

            nodo2.setAristaA("ε");
            nodo2.setIrA(nodo3);
            nodo2.setCuerpo(true);

            nodo3.setAristaA("ε");
            nodo3.setIrA(nodo4);
            nodo3.setCuerpo(true);

            for (int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if (listaNodos[i].getCola())
                {
                    listaNodos[i].setCola(false);
                    listaNodos[i].setCuerpo(true);
                    listaNodos[i].setIrA(nodo4);
                    break;
                }
            }
            nodo4.setCola(true);

            listaNodos.Add(nodo1);
            listaNodos.Add(nodo2);
            listaNodos.Add(nodo3);
            listaNodos.Add(nodo4);
        }

        public void ceromasGrafo()
        {
            NodoThompson nodo1 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(contadorNodos);
            contadorNodos++;

            for(int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if(listaNodos[i].getCola())
                {
                    for(int j = listaNodos.Count - 1; j >= 0; j--)
                    {
                        if (listaNodos[j].getCabeza())
                        {
                            listaNodos[i].setAristaB("ε");
                            listaNodos[i].setIrB(listaNodos[j]);
                            break;
                        }
                    }
                    break;
                }
            }

            nodo1.setAristaA("ε");
            nodo1.setAristaB("ε");
            for (int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if (listaNodos[i].getCabeza())
                {
                    listaNodos[i].setCabeza(false);
                    listaNodos[i].setCuerpo(true);
                    nodo1.setIrA(listaNodos[i]);
                    break;
                }
            }
            nodo1.setIrB(nodo2);
            nodo1.setCabeza(true);


            for (int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if (listaNodos[i].getCola())
                {
                    listaNodos[i].setCola(false);
                    listaNodos[i].setCuerpo(true);
                    listaNodos[i].setAristaA("ε");
                    listaNodos[i].setIrA(nodo2);
                    break;
                }
            }
            nodo2.setCola(true);

            listaNodos.Add(nodo1);
            listaNodos.Add(nodo2);
        }

        public void unomasGrafo()
        {
            NodoThompson nodo1 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo2 = new NodoThompson(contadorNodos);
            contadorNodos++;
            NodoThompson nodo3 = new NodoThompson(contadorNodos);
            contadorNodos++;
            int banderaCola = 0;
            int banderaCabeza = 0;
            List<NodoThompson> copiarGrafo = new List<NodoThompson>();
            int nodoCop = 0; 
            for (int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if (listaNodos[i].getCabeza()) banderaCabeza++;
                if (listaNodos[i].getCola()) banderaCola++;

                if ((banderaCabeza == 1 && banderaCola == 1) || (banderaCabeza == 0 && banderaCola == 1) || (banderaCabeza == 1 && banderaCola == 0))
                {
                    NodoThompson nodo = new NodoThompson(listaNodos[i].getIdentificador());
                    nodo.setAristaA(listaNodos[i].getAristaA());
                    nodo.setAristaB(listaNodos[i].getAristaB());
                    nodo.setCabeza(listaNodos[i].getCabeza());
                    nodo.setCuerpo(listaNodos[i].getCuerpo());
                    nodo.setCola(listaNodos[i].getCola());
                    copiarGrafo.Add(nodo);
                    Console.WriteLine("nodo: " + listaNodos[i].getIdentificador() + "cabeza: " + listaNodos[i].getCabeza() + " cuerpo: " + listaNodos[i].getCuerpo() + " cola: " + listaNodos[i].getCola());
                    nodoCop++;
                }
            }

            for (int i = 0; i < copiarGrafo.Count; i++)
            {
                if(copiarGrafo[i].getCola())
                {
                    for(int j = 0; j < copiarGrafo.Count; j++)
                    {
                        if(copiarGrafo[j].getCabeza())
                        {
                            copiarGrafo[i].setAristaB("ε");
                            copiarGrafo[i].setIrB(copiarGrafo[j]);
                            break;
                        }
                    }
                    break;
                }
            }

            nodo1.setAristaA("ε");
            for(int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if(listaNodos[i].getCabeza())
                {
                    listaNodos[i].setCabeza(false);
                    listaNodos[i].setCuerpo(true);
                    nodo1.setIrA(listaNodos[i]);
                    break;
                }
            }
            nodo1.setCabeza(true);

            for(int i = listaNodos.Count - 1; i >= 0; i--)
            {
                if (listaNodos[i].getCola())
                {
                    listaNodos[i].setCuerpo(true);
                    listaNodos[i].setCola(false);
                    listaNodos[i].setIrA(nodo2);
                    listaNodos[i].setAristaA("ε");
                    break;
                }
            }
            nodo2.setAristaA("ε");
            for(int i = 0; i < copiarGrafo.Count; i++)
            {
                if (copiarGrafo[i].getCabeza())
                {
                    copiarGrafo[i].setCabeza(false);
                    copiarGrafo[i].setCuerpo(true);
                    nodo2.setIrA(copiarGrafo[i]);
                    break;
                }
            }
            nodo2.setAristaB("ε");
            nodo2.setIrB(nodo3);

            for(int i = 0; i < copiarGrafo.Count; i++)
            {
                if (copiarGrafo[i].getCola())
                {
                    copiarGrafo[i].setCola(false);
                    copiarGrafo[i].setCuerpo(true);
                    copiarGrafo[i].setAristaA("ε");
                    copiarGrafo[i].setIrA(nodo3);
                    break;
                }
            }
            nodo3.setCola(true);

            copiarGrafo.Reverse();
            //////////
            for(int i = 0; i < copiarGrafo.Count; i++)
            {
                for(int j = 0; j < listaNodos.Count; j++)
                {
                    if(copiarGrafo[i].getIdentificador() == listaNodos[j].getIdentificador())
                    {
                        if(listaNodos[j].getIrA() != null)
                        {
                            for (int x = 0; x < copiarGrafo.Count; x++)
                            {
                                if (listaNodos[j].getIrA().getIdentificador() == copiarGrafo[x].getIdentificador())
                                {
                                    copiarGrafo[i].setIrA(copiarGrafo[x]);
                                    break;
                                }
                            }
                        }
                        if (listaNodos[j].getIrB() != null)
                        {
                            for (int x = 0; x < copiarGrafo.Count; x++)
                            {
                                if (listaNodos[j].getIrB().getIdentificador() == copiarGrafo[x].getIdentificador())
                                {
                                    copiarGrafo[i].setIrB(copiarGrafo[x]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //////////
            listaNodos.Add(nodo1);
            listaNodos.Add(nodo2);
            //Console.WriteLine("auxiliar: " + copiarGrafo.Count);
            for(int i = 0; i < copiarGrafo.Count; i++)
            {
                copiarGrafo[i].setIdentificador(contadorNodos);
                contadorNodos++;
                listaNodos.Add(copiarGrafo[i]);
            }
            listaNodos.Add(nodo3);
            Console.WriteLine(nodoCop + " hola");
        }

        public void limpiarLista()
        {
            this.listaER.RemoveAll(u => u.StartsWith(".."));
            this.listaER.RemoveAll(u => u.StartsWith("|."));
            this.listaER.RemoveAll(u => u.StartsWith("+."));
            this.listaER.RemoveAll(u => u.StartsWith("*."));
            this.listaER.RemoveAll(u => u.StartsWith("?."));
            this.listaER = this.listaER.Distinct().ToList();
        }
        
        
    }
}
