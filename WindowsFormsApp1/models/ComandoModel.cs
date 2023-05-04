using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.models
{
    public class ComandoModel
    {
        public int id { get; set; }
        public string instrucao { get; set; }
        public TipoInstrucao tipoInstrucao { get; set; }
    }
    public enum TipoInstrucao
    {
        processo,
        tecla,
        atalho
    }
}
