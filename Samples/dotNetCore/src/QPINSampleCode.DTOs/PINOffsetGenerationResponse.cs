using System;
using System.Collections.Generic;
using System.Text;

namespace QPINSampleCode.DTOs
{
    public class PINOffsetGenerationResponse
    {
        /// <summary>
        /// The gererated PIN Offset.
        /// </summary>
        public string PINOffset { get; set; }

        /// <summary>
        /// Magensa Transaction ID.
        /// </summary>
        public string MagTranID { get; set; }
    }
}
