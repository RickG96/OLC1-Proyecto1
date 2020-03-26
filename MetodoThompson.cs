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
    class MetodoThompson
    {

        Grafo grafoThompson;
        List<Estado> listaEstados;
        int cabeza;
        int final;
        Queue<int> cola1;
        Queue<int> cola2;
        Queue<int> cola3;

        public MetodoThompson(Grafo grafo)
        {
            this.grafoThompson = grafo;
            this.listaEstados = new List<Estado>();
            this.cola1 = new Queue<int>();
            this.cola2 = new Queue<int>();
            this.cola3 = new Queue<int>();
        }

        public void buscarCabeza()
        {
            for(int i = 0; i < grafoThompson.listaNodos.Count; i++)
            {
                if (grafoThompson.listaNodos[i].getCabeza())
                {
                     cabeza = grafoThompson.listaNodos[i].getIdentificador();
                }
                if (grafoThompson.listaNodos[i].getCola())
                {
                    final = grafoThompson.listaNodos[i].getIdentificador();
                }
            }

            empezarEstados(cabeza, 9999);
        }

        public void empezarEstados(int inicio, int last)
        {
            //cola2.Enqueue(inicio);
            cola3.Enqueue(inicio);
            int contador = 0;
            while(cola3.Count > 0)
            {
                if(cola3.Peek() != final)
                {
                    Estado nuevoEstado = new Estado(contador);
                    contador++;
                    cola2.Enqueue(cola3.Dequeue());
                    while (cola2.Count > 0)
                    {
                        mover(cola2.Dequeue(), nuevoEstado);
                    }
                    listaEstados.Add(nuevoEstado);
                    
                } else
                {
                    cola3.Dequeue();
                }
                
            }

            Estado ultimo = new Estado(contador);
            ultimo.moverEstado.Add(final);
            listaEstados.Add(ultimo);
            
        }

        public void mover(int estadoActual, Estado estado)
        {
            for(int i = 0; i < grafoThompson.listaNodos.Count; i++)
            {
                if(estadoActual == grafoThompson.listaNodos[i].getIdentificador())
                {
                    estado.moverEstado.Add(estadoActual);
                    if (grafoThompson.listaNodos[i].getIrA() != null)
                    {
                        if(grafoThompson.listaNodos[i].getAristaA().Equals("ε"))
                        {
                            if (cola2.Contains(grafoThompson.listaNodos[i].getIrA().getIdentificador()) == false || estadoActual != grafoThompson.listaNodos[i].getIdentificador())
                            {
                                cola2.Enqueue(grafoThompson.listaNodos[i].getIrA().getIdentificador());
                            } 
                        } else
                        {
                            if(cola3.Contains(grafoThompson.listaNodos[i].getIrA().getIdentificador()) == false && agregado(grafoThompson.listaNodos[i].getIrA().getIdentificador()) == false && estado.moverEstado[0] != grafoThompson.listaNodos[i].getIrA().getIdentificador()) cola3.Enqueue(grafoThompson.listaNodos[i].getIrA().getIdentificador());

                            estado.irInt.Add(grafoThompson.listaNodos[i].getIrA().getIdentificador());
                            estado.irString.Add(grafoThompson.listaNodos[i].getAristaA());
                        }
                    }
                    if (this.grafoThompson.listaNodos[i].getIrB() != null)
                    {
                        if (this.grafoThompson.listaNodos[i].getAristaB().Equals("ε"))
                        {
                            if (cola2.Contains(grafoThompson.listaNodos[i].getIrB().getIdentificador()) == false || estadoActual != grafoThompson.listaNodos[i].getIdentificador())
                            {
                                cola2.Enqueue(grafoThompson.listaNodos[i].getIrB().getIdentificador());
                            } 
                        } else
                        {
                            if (cola3.Contains(grafoThompson.listaNodos[i].getIrB().getIdentificador()) == false && agregado(grafoThompson.listaNodos[i].getIrB().getIdentificador()) == false && estado.moverEstado[0] != grafoThompson.listaNodos[i].getIrB().getIdentificador()) cola3.Enqueue(grafoThompson.listaNodos[i].getIrB().getIdentificador());
                            estado.irInt.Add(grafoThompson.listaNodos[i].getIrB().getIdentificador());
                            estado.irString.Add(grafoThompson.listaNodos[i].getAristaB());
                        }
                    }
                    
                }
            }
        }

        public bool agregado(int estado)
        {
            if(listaEstados.Count > 0)
            {
                for(int i = 0; i < listaEstados.Count; i++)
                {
                    if(listaEstados[i].moverEstado[0] == estado)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void dibujarAfd(Form1 form)
        {
            //Console.WriteLine("dibujando afd.....");
            StringBuilder strGrafo = new StringBuilder();

            strGrafo.Append("digraph G {");
            strGrafo.AppendLine();
            bool estadoFinal = false;
            bool irUltimo = false;
            for (int i = 0; i < listaEstados.Count-1; i++)
            {
                for(int j = 0; j < listaEstados[i].moverEstado.Count; j ++)
                {
                    if(listaEstados[i].moverEstado[j] == listaEstados[listaEstados.Count - 1].moverEstado[0])
                    {
                        estadoFinal = true;
                    }
                }
                for(int x = 0; x < listaEstados[i].irInt.Count; x++)
                {
                    if (listaEstados[i].irInt[x] == listaEstados[listaEstados.Count - 1].moverEstado[0]) irUltimo = true;
                }
                if (estadoFinal == true)
                {
                    strGrafo.Append("nodo" + listaEstados[i].moverEstado[0] + "[shape=circle,color=darkorchid2,peripheries=2,label=S" + listaEstados[i].numeroEstado + "];");
                    strGrafo.AppendLine();
                }
                else
                {
                    strGrafo.Append("nodo" + listaEstados[i].moverEstado[0] + "[shape=circle,color=darkorchid2,label=S" + listaEstados[i].numeroEstado + "];");
                    strGrafo.AppendLine();
                }
                estadoFinal = false;
            }

            if(irUltimo == true)
            {
                strGrafo.Append("nodo" + listaEstados[listaEstados.Count - 1].moverEstado[0] + "[shape=circle,color=darkorchid2,peripheries=2,label=S" + listaEstados[listaEstados.Count - 1].numeroEstado + "];");
                strGrafo.AppendLine();
            }

            for(int i = 0; i < listaEstados.Count; i ++)
            {
                for(int j = 0; j < listaEstados[i].irInt.Count; j++)
                {
                    strGrafo.Append("nodo" + listaEstados[i].moverEstado[0] + " -> nodo" + listaEstados[i].irInt[j] + " [color=darkorchid2,label=\"" + listaEstados[i].irString[j] + "\"];");
                    strGrafo.AppendLine();
                }
            }
            strGrafo.Append("}");
            
            try
            {
                Random random = new Random();
                int numero = random.Next(1000, 9999);
                string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\grafoAFD" + numero + ".txt";
                string auxImg = @"C:\Users\Pistacho\Desktop\reportesP2\grafoAFD" + numero + ".png";
                File.WriteAllText(grafoDot, strGrafo.ToString());
                Thread.Sleep(500);
                ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                startInfo.Arguments = "-Tpng " + grafoDot + " -o " + auxImg;
                form.consola.Text += "se creo la imagen del grafo: grafoAFD" + numero + ".png\n\r\n\r";
                Process.Start(startInfo);
                //Thread.Sleep(500);
                //Form1.consola.Text += "Se creo reporte de tokens: " + grafoDot + "\n\r\n\r";
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void dibujarTabla(List<string> lista, Form1 form)
        {
            StringBuilder strTabla = new StringBuilder();

            strTabla.AppendLine("digraph g {");
            strTabla.AppendLine("aHtmlTable [");
            strTabla.AppendLine("shape=plaintext");
            strTabla.AppendLine("color=deeppink4");
            strTabla.AppendLine("label=<");

            strTabla.AppendLine("<table border='1' cellborder='1'>");
            string aux = "";

            for(int i = 0; i < lista.Count; i++)
            {
                aux += "<td>" + lista[i].Replace("\\", "").Replace("<", "mq").Replace(">","MQ") + "</td>";
            }

            strTabla.AppendLine("<tr><td>Estado</td>" + aux + "</tr>");
            aux = "";
            bool encontrado = false;
            bool irUltimo = false;
            for(int i = 0; i < listaEstados.Count-1; i++)
            {
                aux += "<td>S" + listaEstados[i].numeroEstado + "</td>";
                for (int y = 0; y < listaEstados[i].irInt.Count; y++)
                {
                    if (listaEstados[i].irInt[y] == listaEstados[listaEstados.Count - 1].moverEstado[0]) irUltimo = true;
                }
                for (int j = 0; j < lista.Count; j++)
                {
                    for(int k = 0; k < listaEstados[i].irInt.Count; k++)
                    {
                        if (lista[j].Equals(listaEstados[i].irString[k]))
                        {
                            encontrado = true;
                            for (int x = 0; x < listaEstados.Count; x++)
                            {
                                if (listaEstados[i].irInt[k] == listaEstados[x].moverEstado[0])
                                {
                                    aux += "<td>S" + listaEstados[x].numeroEstado + "</td>";
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    if (encontrado == false)
                    {
                        aux += "<td>-</td>";
                    }
                    encontrado = false;
                }
                strTabla.AppendLine("<tr>" + aux + "</tr>");
                aux = "";
            }
            if (irUltimo == true)
            {
                aux += "<td>S" + listaEstados[listaEstados.Count - 1].numeroEstado + "</td>";
                for(int i = 0; i < lista.Count; i++)
                {
                    aux += "<td>-</td>";
                }
                strTabla.AppendLine("<tr>" + aux + "</tr>");
                aux = "";
            }
            strTabla.AppendLine("</table>");
            strTabla.AppendLine(">];");
            strTabla.AppendLine("}");

            try
            {
                Random random = new Random();
                int numero = random.Next(1000, 9999);
                string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\estados" + numero + ".txt";
                string auxImg = @"C:\Users\Pistacho\Desktop\reportesP2\estados" + numero + ".png";
                File.WriteAllText(grafoDot, strTabla.ToString());
                Thread.Sleep(500);
                ProcessStartInfo startInfo = new ProcessStartInfo("dot.exe");
                startInfo.Arguments = "-Tpng " + grafoDot + " -o " + auxImg;
                Process.Start(startInfo);
                form.consola.Text += "Se creo la imagen de tabla de estados: estados" + numero + ".png\n\r\n\r";
                Thread.Sleep(500);
                //Form1.consola.Text += "Se creo reporte de tokens: " + grafoDot + "\n\r\n\r";
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }

        public void leerEntrada(string entrada, int columna, Form1 form)
        {
            bool exitoso = false; // cuando la cadena es aceptada entonces se vuelve verdadera
            bool error = false; 
            bool arista = false;
            int estadoActual = listaEstados[0].moverEstado[0]; // siempre empezamos desde el estado inicial
            char[] charActual;
            int fila = 0;
            StringBuilder strTokens = new StringBuilder();
            StringBuilder strError = new StringBuilder();
            strTokens.AppendLine("<ListaTokens>");
            string cadenaRestante = entrada;
            while(exitoso == false && error == false)
            {
                for (int i = 0; i < listaEstados.Count; i++)
                {// movemos entre estados
                    if (estadoActual == listaEstados[i].moverEstado[0])
                    {
                        for (int j = 0; j < listaEstados[i].irInt.Count; j++)
                        {// movemos entre aristas
                            charActual = listaEstados[i].irString[j].ToCharArray();
                            //miramos si es conjunto o cadena la arista
                            if (charActual[0] == '\\' && charActual[1] == '"')
                            { // la arista es cadena
                                string cadenaEvaluar = listaEstados[i].irString[j].Substring(2, listaEstados[i].irString[j].Length - 4);
                                //Console.WriteLine(cadenaEvaluar);
                                if(cadenaRestante.Substring(0, cadenaEvaluar.Length).Equals(cadenaEvaluar))
                                {
                                    cadenaRestante = cadenaRestante.Substring(cadenaEvaluar.Length);
                                    arista = true;
                                    estadoActual = listaEstados[i].irInt[j];
                                    strTokens.AppendLine("  <Token>");
                                    strTokens.AppendLine("      <Nombre>Cadena</Nombre");
                                    strTokens.AppendLine("      <Valor>" + cadenaEvaluar + "</Valor>");
                                    strTokens.AppendLine("      <Fila>" + fila + "</Fila>");
                                    strTokens.AppendLine("      <Columna>" + columna + "</Columna>");
                                    strTokens.AppendLine("  </Token>");
                                    fila = fila + cadenaEvaluar.Length;
                                }
                            }
                            else
                            { // la arista es un conjunto
                              //Console.WriteLine("es un conjunto");
                                char[] evalChar = cadenaRestante.ToCharArray();
                                for (int x = 0; x < Analizador.conjuntos.Count; x++)
                                {
                                    if(listaEstados[i].irString[j].Equals(Analizador.conjuntos[x].getNombre()))
                                    {
                                        for(int y = 0; y < Analizador.conjuntos[x].caracteres.Count; y++)
                                        {
                                            if(evalChar[0] == Analizador.conjuntos[x].caracteres[y])
                                            {
                                                cadenaRestante = cadenaRestante.Substring(1, cadenaRestante.Length - 1);
                                                arista = true;
                                                estadoActual = listaEstados[i].irInt[j];
                                                strTokens.AppendLine("  <Token>");
                                                strTokens.AppendLine("      <Nombre>" + Analizador.conjuntos[x].getNombre() + "</Nombre");
                                                strTokens.AppendLine("      <Valor>" + evalChar[0] + "</Valor>");
                                                strTokens.AppendLine("      <Fila>" + fila + "</Fila>");
                                                strTokens.AppendLine("      <Columna>" + columna + "</Columna>");
                                                strTokens.AppendLine("  </Token>");
                                                fila++;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            if (arista == true) break;
                        }
                        if (arista == true)
                        {
                            arista = false;
                            if(cadenaRestante.Length == 0)
                            {
                                if(exitoso == false)
                                {
                                    for (int x = 0; x < listaEstados.Count; x++)
                                    {
                                        if (listaEstados[x].moverEstado[0] == estadoActual)
                                        {
                                            for(int y = 0; y < listaEstados[x].moverEstado.Count; y++)
                                            {
                                                if(listaEstados[x].moverEstado[y] == listaEstados[listaEstados.Count - 1].moverEstado[0])
                                                {
                                                    form.consola.Text += "lexema leido exitosamente: " + entrada + System.Environment.NewLine;
                                                    exitoso = true;
                                                    strTokens.AppendLine("</ListaTokens>");
                                                    Random random = new Random();
                                                    int numero = random.Next(1000, 9999);
                                                    string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\repTokens" + numero + ".xml";
                                                    File.WriteAllText(grafoDot, strTokens.ToString());
                                                    form.consola.Text += "Se creo reporte de tokens: repTokens" + numero + ".xml\n\r\n\r";
                                                    Thread.Sleep(200);
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                                if(exitoso == false)
                                {
                                    form.consola.Text += "cadena incompleta " + entrada + System.Environment.NewLine;
                                    strError.AppendLine("<ListaErrores>");
                                    strError.AppendLine("   <Error>");
                                    strError.AppendLine("       <Valor>No es estado de aceptacion</Valor>");
                                    strError.AppendLine("       <Fila>" + fila + "</Fila>");
                                    strError.AppendLine("       <Columna>" + fila + "</Columna>");
                                    strError.AppendLine("   </Error>");
                                    strError.AppendLine("</ListaErrores>");
                                    Random random = new Random();
                                    int numero = random.Next(1000, 9999);
                                    string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\repError" + numero + ".xml";
                                    File.WriteAllText(grafoDot, strError.ToString());
                                    form.consola.Text += "Se creo reporte de error de tokens: repError" + numero + ".xml\n\r\n\r";
                                    Thread.Sleep(200);
                                    error = true;
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            // no se encontro a donde ir, por lo tanto la cadena es invalida
                            // se esperaba alguna de las aristas... 
                            form.consola.Text += "error, no se sabe a donde ir " + cadenaRestante + System.Environment.NewLine;
                            strError.AppendLine("<ListaErrores>");
                            strError.AppendLine("   <Error>");
                            strError.AppendLine("       <Valor>" + cadenaRestante + "</Valor>");
                            strError.AppendLine("       <Fila>" + fila + "</Fila>");
                            strError.AppendLine("       <Columna>" + fila + "</Columna>");
                            strError.AppendLine("   </Error>");
                            strError.AppendLine("</ListaErrores>");
                            Random random = new Random();
                            int numero = random.Next(1000, 9999);
                            string grafoDot = @"C:\Users\Pistacho\Desktop\reportesP2\repError" + numero + ".xml";
                            File.WriteAllText(grafoDot, strError.ToString());
                            form.consola.Text += "Se creo reporte de error de tokens: repError" + numero + ".xml\n\r\n\r";
                            Thread.Sleep(500);
                            error = true;
                            break;
                        }
                    }
                    arista = false;
                }
            }
            

        }
        
    }
}
