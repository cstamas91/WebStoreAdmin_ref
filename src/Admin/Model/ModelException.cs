using System;
namespace ITStore.Admin.Model
{
    public class ModelException : Exception
    {
        public ModelException() { }

        public ModelException(Exception innerException) : base(String.Empty, innerException) { }
    }
}
