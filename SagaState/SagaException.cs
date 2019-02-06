﻿using System;

namespace SagaState
{
    public class SagaException : Exception
    {
        public SagaException() : base()
        {

        }
        public SagaException(string message) : base(message)
        {
            
        }

        public SagaException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}