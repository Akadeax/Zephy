using System;
using System.Collections.Generic;
using System.Text;

namespace Server.exceptions
{
    public class InvalidIDException : Exception
    {
        public string Message { get; set; } 

        public InvalidIDException(string message) :base(message)
        {
            Message = message;
        }
    }
}
