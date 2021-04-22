using System;
using System.Collections.Generic;
using System.Text;

namespace QPINSampleCode.DTOs
{
    public class QwickPINError
    {
        /// <summary>
        ///  Error Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }
    }
}
