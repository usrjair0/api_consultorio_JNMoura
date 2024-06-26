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