using QPINSampleCode.DTOs;
using System;
using System.Threading.Tasks;

namespace QPINSampleCode.Service
{
    public interface IQPINClient
    {
        Task<PINOffsetGenerationResponse> GeneratePINOffset(QPINInput input);
        Task<PINOffsetVerificationResponse> VerifyPINOffset(QPINInput input);
    }
}