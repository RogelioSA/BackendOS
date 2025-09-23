namespace nest.core.dominio.Excepciones
{
    public class IdentityException : Exception
    {
        public List<IdentityError> Errors { get; set; }
        public IdentityException(List<IdentityError> Errors)
        {
            this.Errors = Errors;
        }
    }

    public class IdentityError 
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
