using System;
using System.Collections.Generic;

namespace ReportServiceCore
{
    public class ErrorRepository
    {
        public List<Error> GetErrors()
        {
            return new List<Error> 
            {
                new Error{ Id=1, Date=DateTime.Now, ErrorMessage="Błąd testowy 1"},
                new Error{ Id=2, Date=DateTime.Now, ErrorMessage="Błąd testowy 2"}
            };
        }
    }
}
