using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico_1
{
    public class Token
    {
        public enum Tipos { Identificador, Numero, Asignacion, Inicializacion,
                            FinSentencia, Caracter, OpLogico, OpRelacional, OpTermino,OpFactor,IncTermino,
                            IncFactor,Cadena,OpTernario,Inicio,Fin};
        private string Contenido;
        private Tipos   Clasificacion;

        public Token()
        {
            Contenido = "";
            Clasificacion = Tipos.Identificador;
        }

        public void setContenido(string Contenido)
        {
            this.Contenido = Contenido;
        }
        public void setClasificacion(Tipos Clasificacion)
        {
            this.Clasificacion = Clasificacion;
        }
        public string getContenido()
        {
            return this.Contenido;
        }
        public Tipos getClasificacion()
        {
            return this.Clasificacion;
        }
    }
}