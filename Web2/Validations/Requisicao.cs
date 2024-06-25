using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Validations
{
    public class Requisicao
    {
        public static bool IdRequisicaoIgualAoIdCorpoRequisicao(int idRequisicao, int idCorpoRequisicao)
        {
            return idRequisicao == idCorpoRequisicao;
        }
    }
}