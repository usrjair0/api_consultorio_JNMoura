namespace Web2.Validations
{
    public class Medico
    {
        public static bool UniqueCRM {get; set;}
        
        public static bool CheckUniqueCRM(int existingCRMCount)
        {
            if(existingCRMCount > 0)
            {
                UniqueCRM = false;
                return false;
            }

            UniqueCRM = true;
            return true;
        }
    }
}