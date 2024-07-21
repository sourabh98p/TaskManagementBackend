
//namespace TaskManagementAPI.Manager.Concrete
//{
//    public class TaskManagementManager : ITaskManagementManager
//    {
//        public TaskManagementManager()
//        {

//        }
//        public async Task<ServiceResponse<CreateDigitalCardResponse>> CreateDigitalCardAsync(CreateDigitalCardRequest request)
//        {
//            _logger.LogInformationStart();
//            var response = new CreateDigitalCardResponse() { IsSuccess = false, };
//            _logger.LogInformation("CreateDigitalCardAsync is calling request : {@Request}", JsonConvert.SerializeObject(request));

//            Guid nfcExternalcardGuid;
//            string nfcExternalNumber, nfcCredential;
//            int OfflineCode, CardNumber;
//            InitializeVariables(out nfcExternalcardGuid, out nfcExternalNumber, out nfcCredential, out OfflineCode, out CardNumber);

//            try
//            {
//                if (request.CardholderId <= 0)
//                {
//                    response.IsSuccess = false;
//                    response.Message = _resources.FunctionSucessMsg;
//                }
//                else
//                {
//                    this.CretaeNFCCard(request, ref nfcExternalcardGuid, ref nfcExternalNumber, ref nfcCredential);

//                    var ByteOfCredential = GooleWalletUtilities.HexStringToByteArray(nfcCredential);

//                    Write logic to add cards details in NCA nccard table

//                    GetOfflineAndCardNumber(ByteOfCredential, ref OfflineCode, ref CardNumber);

//                    NCCardDalRequest dalRequest = new NCCardDalRequest()
//                    {
//                        NCCardFormatId = request.CardFormatId,
//                        NCCardHolderID = request.CardholderId,
//                        NCCredential = "0x" + nfcCredential,
//                        NCOfflineCode = OfflineCode,
//                        NCCardNumber = CardNumber,
//                        NCExternalNumber = nfcExternalNumber,
//                        NCLastUpdate = DateTime.UtcNow,
//                        NCUpdateBy = request.CardholderId,
//                        NCAuthorizerID = request.CardholderId,

//                    };

//                    InsertCardDetailDalResponse dalResponse;
//                    dalResponse = await _googleWalletRepository.CreateDigitalCardAsync(dalRequest);
//                    var ResGroupIDAndPublic = await _googleWalletRepository.GetReaderGroupIdAndReaderPublicKey(request.CardholderId);
//                    if (dalResponse != null && dalResponse.IsSuccess)
//                    {
//                        _logger.LogInformation("Card Details push successfully in NCA DB");
//                        response.CardDetails = new CardDetails() { ExternalNumber = dalRequest.NCExternalNumber, CardId = dalResponse.CardId };
//                        response.IsSuccess = dalResponse.IsSuccess;
//                    }
//                    if (ResGroupIDAndPublic.IsSuccess)
//                    {
//                        response.groupIdAndPublicKey = ResGroupIDAndPublic.groupIdAndPublicKey;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                response.Message = "Some Error Occured";
//                _logger.LogError($"Error occured in CreateDigitalCardManager:CreateDigitalCardAsync and CardHolderId - {request.CardholderId}, Exception: {ex}, StackTrace {ex.StackTrace}");
//                response.IsSuccess = false;
//            }

//            return new ServiceResponse<CreateDigitalCardResponse>() { Data = response };
//        }

//    }
//}
