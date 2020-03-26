using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLC12k20P2
{
    class Analizador
    {

        public static List<ExpresionRegular> expRegulares = new List<ExpresionRegular>();
        public static List<Conjunto> conjuntos = new List<Conjunto>();
        public static List<Entrada> entradas = new List<Entrada>();

        public Analizador()
        {

        }

        public String limpiarArchivo(string texto, Form1 form)
        {
            char[] cadenaEntrada = texto.ToCharArray();

            StringBuilder sbLimpio = new StringBuilder();

            string limpio;
            string reporteTokens = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Reporte</title><link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css\" integrity=\"sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh\" crossorigin=\"anonymous\"></head><body><div style=\"text-align: center;\"><h1 class=\"display-1\">Reporte de Tokens</h1></div><div style=\"margin-left: 5%; margin-right: 5%;\"><table class=\"table\"><thead class=\"thead-dark\"><tr><th scope=\"col\">#</th><th scope=\"col\">ASCII</th><th scope=\"col\">Simbolo</th><th scope=\"col\">Descripción</th></tr></thead><tbody>";
            bool comentarioM = false;
            bool comentarioU = false;
            bool falsaAlarma = false;
            int auxAscii;
            int asciiChar;
            int contadorChar = 1;
            for(int i = 0; i < cadenaEntrada.Length; i++)
            {
                asciiChar = (int)cadenaEntrada[i];
                
                if(!comentarioM && !comentarioU)
                {
                    if (asciiChar == 60)
                    {
                        auxAscii = (int)cadenaEntrada[i + 1];
                        if (auxAscii == 33) comentarioM = true;
                        else falsaAlarma = true;
                    } else if (asciiChar == 47)
                    {
                        auxAscii = (int)cadenaEntrada[i + 1];
                        if (auxAscii == 47) comentarioU = true;
                        else falsaAlarma = true;
                    } else if(asciiChar != 10 && (asciiChar < 32 || asciiChar > 128))
                    {
                        //Form1.consola.AppendText("Error lexico con el caracter: " + cadenaEntrada[i] + "\n");
                    } else
                    {
                        sbLimpio.Append(cadenaEntrada[i]);
                        if((asciiChar > 32 && asciiChar < 48) || (asciiChar > 57 && asciiChar < 65) || (asciiChar > 90 && asciiChar < 97) || (asciiChar > 122 && asciiChar < 128))
                        { // caracter
                            reporteTokens += "<tr><th scope=\"row\">" + contadorChar +
                                "</th><th>" + asciiChar + "</th><th>" + cadenaEntrada[i] +
                                "</th><th>Caracter especial</th></tr>";
                        } else if(asciiChar > 47 && asciiChar < 58)
                        { // digito
                            reporteTokens += "<tr><th scope=\"row\">" + contadorChar +
                                "</th><th>" + asciiChar + "</th><th>" + cadenaEntrada[i] +
                                "</th><th>Digito</th></tr>";
                        } else if(asciiChar > 64 && asciiChar < 91)
                        { // mayusculas
                            reporteTokens += "<tr><th scope=\"row\">" + contadorChar +
                                "</th><th>" + asciiChar + "</th><th>" + cadenaEntrada[i] +
                                "</th><th>Letra mayuscula</th></tr>";
                        } else if(asciiChar > 96 && asciiChar < 123)
                        { // minusculas
                            reporteTokens += "<tr><th scope=\"row\">" + contadorChar +
                                "</th><th>" + asciiChar + "</th><th>" + cadenaEntrada[i] +
                                "</th><th>Letra minuscula</th></tr>";
                        }
                        contadorChar++;
                    }
                    if (falsaAlarma)
                    {
                        sbLimpio.Append(cadenaEntrada[i]);
                        reporteTokens += "<tr><th scope=\"row\">" + contadorChar +
                                "</th><th>" + asciiChar + "</th><th>" + cadenaEntrada[i] +
                                "</th><th>Caracter especial</th></tr>";
                        contadorChar++;
                        falsaAlarma = false;
                    }
                }

                if(comentarioU)
                {
                    if(asciiChar == 10)
                    {
                        comentarioU = false;
                        //sbLimpio.AppendLine();
                    }
                }
                if(comentarioM)
                {
                    if(asciiChar == 62)
                    {
                        auxAscii = (int)cadenaEntrada[i - 1];
                        if (auxAscii == 33) comentarioM = false;
                    }
                }
            }
            limpio = sbLimpio.ToString();
            // creando reporte de tokens 
            try
            {
                reporteTokens += "</tbody></table></div><script src=\"https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js\" integrity=\"sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6\" crossorigin=\"anonymous\"></script></body></html>";
                Random random = new Random();
                string nombreReporte = @"C:\Users\Pistacho\Desktop\reportesP2\reporteTokens" + random.Next(100, 999) + ".html";

                File.WriteAllText(nombreReporte, reporteTokens);

                form.consola.Text += "Se creo reporte de tokens: " + nombreReporte + "\n\r\n\r";
            } catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            return limpio;
        }

        public void analizadorLexico(string strLimpio, Form1 form)
        {
            string[] lineas = strLimpio.Split(';');
            StringBuilder sbToken = new StringBuilder();
            int asciiActual;
            int contadorComillas = 0;
            char[] lineaActual;
            string token;
            bool conj = false;
            bool lexe = false;

            int aux = 0;
            for (int i = 0; i < lineas.Length; i++)
            {
                Conjunto conjuntoNuevo = new Conjunto();
                ExpresionRegular exprNuevo = new ExpresionRegular();
                Entrada lexema = new Entrada();

                lineaActual = lineas[i].ToCharArray();

                for(int j = 0; j < lineaActual.Length; j++)
                {
                    asciiActual = (int)lineaActual[j];
                    if (lineaActual.Length < 5) break;
                    if(asciiActual == 58)
                    {
                        token = sbToken.ToString();
                        if(token.Equals("CONJ"))
                        {
                            conj = true;
                            j = j + 1;
                            sbToken.Clear();
                        }
                    }
                    if(sbToken.Length > 2 && asciiActual == 32 && conj == true && contadorComillas == 0)
                    { // es un conjunto
                        conjuntoNuevo.setNombre(sbToken.ToString().Replace(" ", "").Replace("->",""));
                        //form.consola.Text += "Conjunto -> " + sbToken.ToString();
                        sbToken.Clear();
                    } else if(sbToken.Length > 2 && asciiActual == 45 && conj == false && contadorComillas == 0)
                    { // es una expresion regular
                        exprNuevo.setNombre(sbToken.ToString());
                        //form.consola.Text += "Expresion -> " + sbToken.ToString();
                        sbToken.Clear();
                    } else if(sbToken.Length > 2 && asciiActual == 58 && contadorComillas == 0 && !conj)
                    { // es un lexema
                        //form.consola.Text += "Lexema -> " + sbToken.ToString();
                        lexema.setNombre(sbToken.ToString());
                        sbToken.Clear();
                        lexe = true;
                    }
                    if(asciiActual > 32 && asciiActual < 128)
                    {
                        sbToken.Append(lineaActual[j]);
                        if (asciiActual == 34 && (int)lineaActual[j - 1] != 92) contadorComillas++;
                    } else if(asciiActual == 37)
                    {
                        //nada
                    }
                    if (contadorComillas == 2) contadorComillas = 0;
                    if (contadorComillas > 0 && asciiActual == 32) sbToken.Append(lineaActual[j]);
                    if (sbToken.ToString().Replace(" ", ",").Equals("->") ||
                        (sbToken.ToString().Replace(" ","").Equals(":") &&
                        contadorComillas == 0))
                    {
                        sbToken.Clear();
                    }
                } // en estos if debemos agregar a las listas segun corresponda
                if(conj && sbToken.Length > 2)
                {
                    conjuntoNuevo.setCadena(sbToken.ToString());
                    conjuntos.Add(conjuntoNuevo);
                    //form.consola.Text += " -- " + sbToken.ToString().Replace(" ", "") + "\n\r\n\r";
                } else if(!conj && !lexe && sbToken.Length > 2)
                {
                    exprNuevo.setExpresion(sbToken.ToString(), form);
                    expRegulares.Add(exprNuevo);
                    aux++;
                    //form.consola.Text += " -- " + sbToken.ToString() + "\n\r\n\r";
                } else if(lexe && sbToken.Length > 2)
                {
                    lexema.setCadena(sbToken.ToString());
                    lexema.setColumna(i);
                    entradas.Add(lexema);
                    //form.consola.Text += " -- " + sbToken.ToString() + "\n\r\n\r";
                }
                conj = false;
                lexe = false;
                sbToken.Clear();
                //form.consola.Text += lineas[i] + "\n\r";
            }
            //Console.WriteLine("nepe; " + aux);
        }
    }
}
