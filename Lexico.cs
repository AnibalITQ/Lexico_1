using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico_1
{
    public class Lexico : Token, IDisposable
    {
        StreamReader archivo;
        StreamWriter log;

        protected int linea;

        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.txt no existe", log, linea);
            }
        }

        public Lexico(string nombre)
        {
            linea = 1;
            log = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".log");
            log.AutoFlush = true;
            if (Path.GetExtension(nombre) != ".cpp")
            {
                throw new Error("El archvio " + nombre + " no tiene extension CPP", log, linea);
            }
            if (File.Exists(nombre))
            {
                archivo = new StreamReader(nombre);
            }
            else
            {
                throw new Error("El archvio " + nombre + " no existe", log, linea);
            }
        }

        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }

        public void nextToken()
        {
            char c;
            string buffer = "";

            while (char.IsWhiteSpace(c = (char)archivo.Read()) && !archivo.EndOfStream)
            {
                if (c == '\n')
                {
                    linea++;
                }
            }
            buffer += c;
            if (char.IsLetter(c))
            {
                setClasificacion(Tipos.Identificador);
                while (char.IsLetterOrDigit(c = (char)archivo.Peek()))
                {
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (char.IsDigit(c))
            {
                setClasificacion(Tipos.Numero);
                while (char.IsDigit(c = (char)archivo.Peek()))
                {
                    archivo.Read();
                    buffer += c;
                }
                if (c == '.')
                {
                    archivo.Read();
                    buffer += c;
                    if (char.IsDigit((c = (char)archivo.Peek())))
                    {
                        while (char.IsDigit(c = (char)archivo.Peek()))
                        {
                            archivo.Read();
                            buffer += c;
                        }
                    }
                    else
                    {
                        throw new Error("lexico, se espera un digito", log, linea);
                    }
                }
            }
            else if (c == ':')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.Inicializacion);
                    archivo.Read();
                    buffer += c;
                }

            }

            else if (c == '=')
            {
                setClasificacion(Tipos.Asignacion);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OpRelacional);
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == ';')
            {
                setClasificacion(Tipos.FinSentencia);
            }
            else if (c == '&')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '&')
                {
                    setClasificacion(Tipos.OpLogico);
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == '|')
            {
                setClasificacion(Tipos.Caracter);
                if ((c = (char)archivo.Peek()) == '|')
                {
                    setClasificacion(Tipos.OpLogico);
                    archivo.Read();
                    buffer += c;
                }
            }

            else if (c == '!')
            {
                setClasificacion(Tipos.OpLogico);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.OpRelacional);
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == '>')
            {
                setClasificacion(Tipos.OpRelacional);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    archivo.Read();
                    buffer += c;
                }

            }
            else if (c == '<')
            {
                setClasificacion(Tipos.OpRelacional);
                if ((c = (char)archivo.Peek()) == '=' || (c = (char)archivo.Peek()) == '>')
                {
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == '+')
            {
                setClasificacion(Tipos.OpTermino);
                if ((c = (char)archivo.Peek()) == '=' || (c = (char)archivo.Peek()) == '+')
                {
                    setClasificacion(Tipos.IncTermino);
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == '-')
            {
                setClasificacion(Tipos.OpTermino);
                if ((c = (char)archivo.Peek()) == '=' || (c = (char)archivo.Peek()) == '-')
                {
                    setClasificacion(Tipos.IncTermino);
                    archivo.Read();
                    buffer += c;
                }
            }
            else if (c == '*' || c == '/' || c == '%')
            {
                setClasificacion(Tipos.OpFactor);
                if ((c = (char)archivo.Peek()) == '=')
                {
                    setClasificacion(Tipos.IncFactor);
                    archivo.Read();
                    buffer += c;
                }

            }
            else if (c == '?')
            {
                setClasificacion(Tipos.OpTernario);
            }
            else if (c == '"')
            {

                while ((c = (char)archivo.Peek()) != '"' && !archivo.EndOfStream)
                {
                    archivo.Read();
                    buffer += c;

                }
                if (archivo.EndOfStream)
                {
                    throw new Error("Cadena no cerrada",log, linea);
                }
                archivo.Read();
                buffer += c;
                setClasificacion(Tipos.Cadena);
            }
            else if (c == '\'')
            {

                while ((c = (char)archivo.Peek()) != '\'' && !archivo.EndOfStream)
                {
                    archivo.Read();
                    buffer += c;
                }
                if (archivo.EndOfStream)
                {
                    throw new Error("Cadena no cerrada",log, linea);
                }
                archivo.Read();
                buffer += c;
                setClasificacion(Tipos.Cadena);
            }
            else if (c == '{'){
                setClasificacion(Tipos.Inicio);
            }
            else if (c == '}'){
                setClasificacion(Tipos.Fin);
            }
            else
            {
                setClasificacion(Tipos.Caracter);
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + " | " + getClasificacion());
        }


        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}