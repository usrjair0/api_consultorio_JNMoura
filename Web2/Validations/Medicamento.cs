using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Validations
{
    public class Medicamento
    {
        public static bool VencimentoMaiorQueDataFabricacao(Models.Medicamento medicamento)
        {
            return medicamento.DataVencimento > medicamento.DataFabricacao;
        }
    }
}