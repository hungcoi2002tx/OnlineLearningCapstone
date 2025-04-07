﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Other.ExceptionModel
{
    public class UnauthorizedException : Exception
    {
        public int ErrorCode { get; set; }
        public UnauthorizedException(int errorCode)
        {
            ErrorCode = errorCode;
        }

        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
