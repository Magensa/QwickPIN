using System;
using System.Collections.Generic;
using System.Text;

namespace QPINSampleCode.DTOs
{
    public class PINOffsetVerificationResponse
    {
        /// <summary>
        /// Verification Result.
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Magensa Transaction ID.
        /// </summary>
        public string MagTranID { get; set; }
    }
}
