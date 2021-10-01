using System;
using System.Collections.Generic;
using System.Text;

namespace Hangman
{
    internal class LetterAlreadyInUseException : Exception
    {
        public LetterAlreadyInUseException(string message) : base(message)
        {

        }
    }
}
