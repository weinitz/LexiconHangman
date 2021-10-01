using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    internal class WordAlreadyInUseException : Exception
    {
        public WordAlreadyInUseException(string message) : base(message)
        {

        }
    }
}
