using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.models;

namespace WindowsFormsApp1.Functions
{
    public class FunctionD : BaseFuntions
    {
        public FunctionD(int numberFunction)
        {
            ComandoModel commad = comandosD().Single(x => x.id == numberFunction);
            switch (commad.tipoInstrucao)
            {
                case TipoInstrucao.processo:
                    System.Diagnostics.Process.Start(commad.instrucao);
                    break;
                case TipoInstrucao.tecla:
                    SendKeys.Send(commad.instrucao);
                    break;
                case TipoInstrucao.atalho:
                    SendKeys.SendWait(commad.instrucao);
                    break;
                default:
                    break;
            }
        }

    }
}
