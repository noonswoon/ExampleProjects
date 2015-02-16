using System;

namespace BeginningNHibernate.NhSessionFactory
{
    public class NhUniqueKeyException : ApplicationException
    {
        private readonly string _sqlMessage;
        public override string Message
        {
            get
            {
                return _sqlMessage;
            }
        }

        public NhUniqueKeyException(string sqlMessage)
        {
            _sqlMessage = sqlMessage;
        }

        public  int StatusCode
        {
            get { return 512; }
        }

        public  string StringToDisplay
        {
            get { return "error in trying to save duplicated key"; }
        }
    }
}