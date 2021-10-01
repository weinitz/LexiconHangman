using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    internal class IsNotLetterException : Exception
    {
        public IsNotLetterException(string message) : base(message)
        {

        }
    }
}

