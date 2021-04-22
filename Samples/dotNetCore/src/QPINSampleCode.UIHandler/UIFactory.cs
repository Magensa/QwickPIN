using Newtonsoft.Json;
using QPINSampleCode.DTOs;
using QPINSampleCode.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QPINSampleCode.UIHandler
{
    public class UIFactory : IUIFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public UIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void ShowUI(UI ui)
        {
            switch (ui)
            {
                case UI.GENERATEPINOFFSET:
                    ShowGeneratePINOffset();
                    break;
                case UI.VERIFYPINOFFSET:
                    ShowVerifyPINOffset();
                    break;
            }
        }
        
        private void ShowGeneratePINOffset()
        {
            PINData pinData = new PINData();
            QPINInput input = new QPINInput();
            try
            {
                Array enumPANDataTypeArr = Enum.GetValues(typeof(PANDataType)).OfType<PANDataType>().ToArray();
                input.PANDataType = Read_DataType_Input("Enter number or Type for PANDataType:", enumPANDataTypeArr);
                if (input.PANDataType == "MagTekTrack2Encrypted")
                {
                    PANDataTrack2 panTrack2 = new PANDataTrack2();
                    panTrack2.track2 = Read_LongString_Input("Enter Track2 Data:", false); 
                    panTrack2.ksn = Read_String_Input("Enter PIN KSN:", false);
                    input.PANData = JsonConvert.SerializeObject(panTrack2);
                }
                else
                {
                    input.PANData = Read_LongString_Input("Enter PAN Data:", false);
                }
                
                Array enumPINDataTypeArr = Enum.GetValues(typeof(PINDataType)).OfType<PINDataType>().ToArray();
                input.PINDataType = Read_DataType_Input("Enter PINDataType:", enumPINDataTypeArr);

                // If you would like to input EPB, KSN for MagTekARQC, remove condition part.
                if (input.PANDataType != "MagTekARQC")
                {
                    pinData.epb = Read_String_Input("Enter EPB :", false); 
                    pinData.ksn = Read_String_Input("Enter PIN KSN:", false); 
                }
                
                if (!string.IsNullOrWhiteSpace(pinData.epb) && !string.IsNullOrWhiteSpace(pinData.ksn))
                    input.PINData = JsonConvert.SerializeObject(pinData);

                input.RefID = Read_String_Input("Enter RefID:", true); 
                Console.WriteLine("Please wait...");
                var svc = _serviceProvider.GetService<IQPINClient>();
                var result = svc.GeneratePINOffset(input).Result;
                if (!string.IsNullOrWhiteSpace(result.MagTranID))
                {
                    Console.WriteLine("=====================Response Start======================");
                    Console.WriteLine("PinOffset: " + result.PINOffset);
                    Console.WriteLine("MagTranID: " + result.MagTranID);
                    Console.WriteLine("=====================Response End======================");
                }                
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while GeneratePinOffset : " + ex.Message.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
           
        }

        private void ShowVerifyPINOffset()
        {
            PINData pinData = new PINData();
            QPINInput input = new QPINInput();
            try
            {                                
                Array enumPANDataTypeArr = Enum.GetValues(typeof(PANDataType)).OfType<PANDataType>().ToArray();
                input.PANDataType = Read_DataType_Input("Enter number or Type for PANDataType:", enumPANDataTypeArr);
                if (input.PANDataType == "MagTekTrack2Encrypted")
                {
                    PANDataTrack2 panTrack2 = new PANDataTrack2();
                    panTrack2.track2 = Read_LongString_Input("Enter Track2 Data:", false);
                    panTrack2.ksn = Read_String_Input("Enter PIN KSN:", false);
                    input.PANData = JsonConvert.SerializeObject(panTrack2);
                }
                else
                {
                    input.PANData = Read_LongString_Input("Enter PAN Data:", false); 
                }

                Array enumPINDataTypeArr = Enum.GetValues(typeof(PINDataType)).OfType<PINDataType>().ToArray();
                input.PINDataType = Read_DataType_Input("Enter PINDataType:", enumPINDataTypeArr);

                // If you would like to input EPB, KSN for MagTekARQC, remove condition part.
                if (input.PANDataType != "MagTekARQC")
                {
                    pinData.epb = Read_String_Input("Enter EPB :", false); 
                    pinData.ksn = Read_String_Input("Enter PIN KSN:", false);
                }

                if (!string.IsNullOrWhiteSpace(pinData.epb) && !string.IsNullOrWhiteSpace(pinData.ksn))
                    input.PINData = JsonConvert.SerializeObject(pinData);
               
                input.RefPINOffset = Read_String_Input("Enter RefPINOffset:", false); 
                input.RefID = Read_String_Input("Enter RefID:", true); 
                Console.WriteLine("Please wait...");
                var svc = _serviceProvider.GetService<IQPINClient>();
                var result = svc.VerifyPINOffset(input).Result;
                if (!string.IsNullOrWhiteSpace(result.MagTranID))
                {
                    Console.WriteLine("=====================Response Start======================");
                    Console.WriteLine("Is PinOffset(" + input.RefPINOffset + ") Verified: " + result.Success);
                    Console.WriteLine("MagTranID: " + result.MagTranID);
                    Console.WriteLine("=====================Response End======================");
                }
                    
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error while VerifyPinOffset : " + ex.Message.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        #region Helper Functions

        /// <summary>
        /// list up enum and select it by number for convenience.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="enumArr"></param>
        /// <returns></returns>
        private static string Read_DataType_Input(string question, Array enumArr)
        {
            int index;
            Console.Write(question + Environment.NewLine + "(ex. ");
            int totalCount = enumArr.Length;
            string[] keyTypes = new string[totalCount];
            int cnt = 0;
            foreach (var dataType in enumArr)
            {
                Console.Write(cnt.ToString() + ":" + dataType.ToString());
                keyTypes[cnt] = dataType.ToString();
                cnt++;
                if (cnt != totalCount) Console.Write(", ");
            }
            Console.WriteLine(")");
            var ans = Console.ReadLine();
            Regex regex = new Regex(@"^\d$");
            if (regex.IsMatch(ans) == true)
            {
                index = Convert.ToInt32(ans);
                if (index >= totalCount)
                {
                    Console.WriteLine("Invalid Input.");
                    return Read_DataType_Input(question, enumArr);
                }
            }
            else
                index = Array.FindIndex(keyTypes, x => x.Equals(ans, StringComparison.OrdinalIgnoreCase));

            if (index != -1)
                return keyTypes[index];
            else
            {
                Console.WriteLine("Invalid Input.");
                return Read_DataType_Input(question, enumArr);
            }
        }

        /// <summary>
        /// accepts default string input. For large string input, use the Read_LongString_Input
        /// </summary>
        /// <param name="question"></param>
        /// <param name="isOptional"></param>
        /// <returns></returns>
        private static string Read_String_Input(string question, bool isOptional)
        {
            Console.WriteLine(question);
            var ans = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(ans))
            {
                return Read_String_Input(question, isOptional);
            }
            return ans;
        }
        /// <summary>
        /// Accepts large string input, as the default string implemenattion has limitations.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="isOptional"></param>
        /// <returns></returns>
        private static string Read_LongString_Input(string userMessage, bool isOptional)
        {
            Console.WriteLine(userMessage);
            byte[] inputBuffer = new byte[262144];
            Stream inputStream = Console.OpenStandardInput(262144);
            Console.SetIn(new StreamReader(inputStream, Console.InputEncoding, false, inputBuffer.Length));
            string strInput = Console.ReadLine();
            if ((!isOptional) && string.IsNullOrWhiteSpace(strInput))
            {
                return Read_LongString_Input(userMessage, isOptional);
            }
            return strInput;
        }
        
        #endregion

    }
}
