using System;
using System.Collections.Generic;
using System.Text;

namespace QPINSampleCode.DTOs
{
    public class QPINInput
    {

        /// <summary>
        /// PAN Data
        /// </summary>
        public string PANData { get; set; }
        /// <summary>
        /// PAN Data Type (e.g MagTekARQC, MagTekTrack2Encrypted, DukptPAN )
        /// </summary>
        public string PANDataType { get; set; }
        /// <summary>
        /// PIN Data
        /// </summary>
        public string PINData { get; set; }
        /// <summary>
        /// PIN Block Format (e.g ISO_0, ISO_1, ISO_2, ISO_3)
        /// </summary>
        public string PINDataType { get; set; }
        /// <summary>
        /// Reference PINOffset
        /// </summary>
        public string RefPINOffset { get; set; }
        /// <summary>
        /// Reference ID 
        /// </summary>
        public string RefID { get; set; }
    }
}
