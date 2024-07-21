//using Azure.Core;
//using Dapper;
//using IntegrationAPI.Common;
//using IntegrationAPI.Domain.Entities;
//using IntegrationAPI.Domain.Request;
//using IntegrationAPI.Domain.Response.DALResponse;
//using IntegrationAPI.Repository.Abstract;
//using IntegrationAPI.Repository.DbConstants;
//using KastlePMIntegration.Common;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics.CodeAnalysis;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using GooleWalletUtilities = IntegrationAPI.Common.GooleWalletUtilities;

//namespace TaskManagementAPI.Repository.Concrete
//{
//    public class TaskManagementRepository : ITaskManagementRepository
//    {
//        /// <summary>
//        /// The logger
//        /// </summary>
//        private readonly IKastleLogger _logger;

//        /// <summary>
//        /// The object SQL
//        /// </summary>
//        private readonly ISqlHelper _objSql;

//        public ConnectionStrings _connectionStrings { get; set; }

//        [ExcludeFromCodeCoverage]
//        /// <summary>
//        /// Save Kastle Pet Info
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        public TaskManagementRepository(IKastleLogger logger, IOptions<ConnectionStrings> connectionStrings, IOptionsSnapshot<AppSettings> appSettings)
//        {
//            _logger = logger;
//            _connectionStrings = connectionStrings.Value;
//            _objSql = new SqlHelper(_connectionStrings.NCAdb, appSettings.Value.SQLCommandTimeOut, maxpoolsize: appSettings.Value.DbMaxPoolSize);
//        }

//        public ValidateExternalNumberDalResponse ValidateExternalNumber(string externalNumber)
//        {

//            _logger.LogInformationStart($"[ValidateExternalNumber] :: Request: {externalNumber}");
//            // Get Db connection key
//            var response = new ValidateExternalNumberDalResponse { IsSuccess = true };
//            try
//            {
//                var InlineString = InLineQueries.VALIDATEEXTERNALNUMBER;
//                List<SqlParameter> paramcollection = new List<SqlParameter>
//                {
//                    new SqlParameter(DBParamNames.EXTERNALNUMBER, SqlDbType.VarChar)
//                    {
//                        Value = externalNumber
//                    }

//                };
//                SqlParameter recordCount = new SqlParameter(DBParamNames.RECORDCOUNT, SqlDbType.Int);
//                recordCount.Direction = ParameterDirection.Output;
//                paramcollection.Add(recordCount);
//                int externalCount ;
//                _objSql.ExecuteDataSetQueryStringDB(InlineString, paramcollection);
//                response.ExternalNumberCount = int.TryParse(Convert.ToString(recordCount.Value), out externalCount) ? externalCount : 1;

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"[ValidateExternalNumber] :: Exception: {JsonConvert.SerializeObject(ex)}");
//                response = new ValidateExternalNumberDalResponse { IsSuccess = false, Message = ApiError.InternalError , ExternalNumberCount = -1 };
//            }
//            _logger.LogInformationEnd();
//            return response;
//        }

//        public async Task<InsertCardDetailDalResponse> CreateDigitalCardAsync(NCCardDalRequest request)
//        {

//            _logger.LogInformationStart("GetReaderGroupIdAndReaderPublicKey - > Request: {Request})", JsonConvert.SerializeObject(request));
//            // Get Db connection key
//            var response = new InsertCardDetailDalResponse { IsSuccess = true };
//            try
//            {
//                var GetCardidSting = InLineQueries.GETCARDID;
//                List<SqlParameter> paramcollectionForGetCard = new List<SqlParameter>();
//                SqlParameter CardId = new SqlParameter(DBParamNames.CARDID, SqlDbType.Int);
//                CardId.Direction = ParameterDirection.Output;
//                paramcollectionForGetCard.Add(CardId);
//                int uniqueCardId;
//                await _objSql.ExecuteDataSetQueryStringDB(GetCardidSting, paramcollectionForGetCard);
//                response.CardId = int.TryParse(Convert.ToString(CardId.Value), out uniqueCardId) ? uniqueCardId : 0;
//                var InlineString = InLineQueries.INSERTCARDDETAILS;
//                var NCCredetialbyte = GooleWalletUtilities.HexStringToByteArray(request.NCCredential);
//                List<SqlParameter> paramcollection = new List<SqlParameter>
//                {   new SqlParameter("@NCCardId", SqlDbType.Int) { Value = response.CardId },
//                    new SqlParameter("@NCGeneration", SqlDbType.Int) { Value = request.NCGeneration },
//                    new SqlParameter("@NCActiveType", SqlDbType.VarChar) { Value = request.NCActiveType },
//                    new SqlParameter("@NCRestoreType", SqlDbType.VarChar) { Value = request.NCRestoreType },
//                    new SqlParameter("@NCActiveClass", SqlDbType.VarChar) { Value = request.NCActiveClass },
//                    new SqlParameter("@NCRestoreClass", SqlDbType.VarChar) { Value = request.NCRestoreClass },
//                    new SqlParameter("@NCLockoutClass", SqlDbType.VarChar) { Value = request.NCLockoutClass },
//                    new SqlParameter("@NCLockoutMap", SqlDbType.VarChar) { Value = request.NCLockoutMap },
//                    new SqlParameter("@NCPriority", SqlDbType.Int) { Value = request.NCPriority },
//                    new SqlParameter("@NCEffective", SqlDbType.DateTime) { Value = request.NCEffective },
//                    new SqlParameter("@NCExpire", SqlDbType.DateTime) { Value = request.NCExpire},
//                    new SqlParameter("@NCCityCode", SqlDbType.Int) { Value = request.NCCityCode },
//                    new SqlParameter("@NCOfflineCode", SqlDbType.Int) { Value = request.NCOfflineCode },
//                    new SqlParameter("@NCCardNumber", SqlDbType.Int) { Value = request.NCCardNumber },
//                    new SqlParameter("@NCExternalNumber", SqlDbType.VarChar) { Value = request.NCExternalNumber },
//                    new SqlParameter("@NCAuthGroupID", SqlDbType.Int) { Value =  request.NCAuthGroupID},
//                    new SqlParameter("@NCFCGroupID", SqlDbType.Int) { Value = request.NCFCGroupID },
//                    new SqlParameter("@NCCardHolderID", SqlDbType.Int) { Value = request.NCCardHolderID },
//                    new SqlParameter("@NCStatus", SqlDbType.VarChar) { Value = request.NCStatus },
//                    new SqlParameter("@NCLastUpdate", SqlDbType.DateTime) { Value = request.NCLastUpdate },
//                    new SqlParameter("@NCUpdateBy", SqlDbType.Int) { Value = request.NCUpdateBy },
//                    new SqlParameter("@NCAuthorizerID", SqlDbType.Int) { Value = request.NCAuthorizerID },
//                    new SqlParameter("@NCCityID", SqlDbType.Int) { Value = request.NCCityID },
//                    new SqlParameter("@NCFlags", SqlDbType.Int) { Value = request.NCFlags },
//                    new SqlParameter("@NCProcessed", SqlDbType.DateTime) { Value = request.NCLastUpdate },
//                    new SqlParameter("@NCFuture", SqlDbType.VarChar) { Value = request.NCFuture },
//                    new SqlParameter("@NCReaderTechId", SqlDbType.Int) { Value = request.NCReaderTechId },
//                    new SqlParameter("@NCCredential", SqlDbType.VarBinary) { Value = NCCredetialbyte },
//                    new SqlParameter("@NCCardFormatID", SqlDbType.Int) { Value = request.NCCardFormatId }
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(InlineString, paramcollection);
               
//                var GetInstID = InLineQueries.GETINSTID;
//                List<SqlParameter> paramcollectionForIstID = new List<SqlParameter>
//                {  
//                    new SqlParameter("@NCCardHolderID", SqlDbType.Int) { Value = request.NCCardHolderID }
//                };
//                DataSet resultSet = await _objSql.ExecuteDataSetQueryStringDB(GetInstID, paramcollectionForIstID);
//                int instID=0;
//                if (resultSet != null && resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
//                {
//                    object result = resultSet.Tables[0].Rows[0][0];

//                    if (result != DBNull.Value)
//                    {
//                        instID = Convert.ToInt32(result);
//                    }
//                }
//                var GetSourceId = InLineQueries.GETSOURCEID;
//                DataSet dataSet = await _objSql.ExecuteDataSetQueryStringDB(GetSourceId, null);
//                int SourceID = 0;
//                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
//                {
//                    object result = dataSet.Tables[0].Rows[0][1];

//                    if (result != DBNull.Value)
//                    {
//                        SourceID = Convert.ToInt32(result);
//                    }
//                }
//                var InlineStringforNCCredentialMapping = InLineQueries.INSERTCREDENTIALMAPPING;
//                // Parameter collection for the NCCredentialMapping table
//                Guid GuidId = Guid.NewGuid();
//                var requestCredentialMapping = new
//                {
//                    NCAltGUId = GuidId,
//                    NCKastleCredentialGUID = GuidId,
//                    NCSourceId = SourceID,
//                    NCInstID = instID, 
//                    NCCredentialFormatMappingID = (int?)null,
//                    NCFlags = request.NCFlags,
//                    NCLastUpdate = DateTime.Now,
//                    NCLastUpdateBy = request.NCUpdateBy,
//                    NCValueEncoding = (int?)null
//                };
//                // Creating SqlParameter list for the NCCredentialMapping table
//                List<SqlParameter> paramcollectionCredentialMapping = new List<SqlParameter>
//                {
//                    new SqlParameter("@NCCardId", SqlDbType.Int) { Value = response.CardId },
//                    new SqlParameter("@NCAltGUId", SqlDbType.UniqueIdentifier) { Value = requestCredentialMapping.NCAltGUId },
//                    new SqlParameter("@NCKastleCredentialGUID", SqlDbType.UniqueIdentifier) { Value = requestCredentialMapping.NCKastleCredentialGUID },
//                    new SqlParameter("@NCSourceId", SqlDbType.Int) { Value = requestCredentialMapping.NCSourceId },
//                    new SqlParameter("@NCInstID", SqlDbType.Int) { Value = requestCredentialMapping.NCInstID },
//                    new SqlParameter("@NCCredentialFormatMappingID", SqlDbType.Int) { Value = (object)requestCredentialMapping.NCCredentialFormatMappingID  ?? DBNull.Value },
//                    new SqlParameter("@NCFlags", SqlDbType.Int) { Value = requestCredentialMapping.NCFlags },
//                    new SqlParameter("@NCLastUpdate", SqlDbType.DateTime) { Value = requestCredentialMapping.NCLastUpdate },
//                    new SqlParameter("@NCLastUpdateBy", SqlDbType.Int) { Value = requestCredentialMapping.NCLastUpdateBy },
//                    new SqlParameter("@NCValueEncoding", SqlDbType.Int) { Value = (object)requestCredentialMapping.NCValueEncoding ?? DBNull.Value } // Handling null values
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(InlineStringforNCCredentialMapping, paramcollectionCredentialMapping);

//                var InlineStringforNCCardOwnerInstMapping = InLineQueries.NCCARDOWNERINST;
//                // Parameter collection for the NCCredentialMapping table
//                var requestNCCardOwnerInstMapping = new
//                {
//                    NCOwnerInst = instID,
//                    NCFlags = request.NCFlags,
//                    NCUpdateBy = request.NCUpdateBy,
//                    NCUpdateDate = DateTime.UtcNow,
//                };
//                // Creating SqlParameter list for the NCCredentialMapping table
//                List<SqlParameter> paramcollectionNCCardOwnerInstMapping = new List<SqlParameter>
//                {
//                    new SqlParameter("@NCCardId", SqlDbType.Int) { Value = response.CardId },
//                    new SqlParameter("@NCOwnerInst", SqlDbType.Int) { Value = requestNCCardOwnerInstMapping.NCOwnerInst },
//                    new SqlParameter("@NCFlags", SqlDbType.Int) { Value = requestNCCardOwnerInstMapping.NCFlags },
//                    new SqlParameter("@NCUpdateBY", SqlDbType.Int) { Value = requestNCCardOwnerInstMapping.NCUpdateBy },
//                    new SqlParameter("@NCLastUpdate", SqlDbType.DateTime) { Value = requestNCCardOwnerInstMapping.NCUpdateDate },
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(InlineStringforNCCardOwnerInstMapping, paramcollectionNCCardOwnerInstMapping);
//                //Creating inline for INSERT INTO CHANGEHISTORY TABLE
//                var InlineChangeHistory = InLineQueries.INSERTINTOCHANGEHISTORY;
//                //getting the NCID 
//                var UpdateNCid = InLineQueries.UpdateNCID;
//                List<SqlParameter> objParams = new List<SqlParameter>();
//                {
//                    objParams.Add(new SqlParameter("@NCTableId", SqlDbType.Int)
//                    {
//                        Value = Convert.ToInt32(CommonConstants.NCTableId)
//                    }) ;
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(UpdateNCid, objParams);

//                var GetNcid = InLineQueries.GetNCID;
//                List<SqlParameter> paramcollectionForNCID = new List<SqlParameter>();
//                SqlParameter NCId = new SqlParameter("@NCTableId", SqlDbType.Int) { Value = Convert.ToInt32(CommonConstants.NCTableId) };
//                paramcollectionForGetCard.Add(NCId);
//                SqlParameter ID = new SqlParameter("@nextid", SqlDbType.Int);
//                ID.Direction = ParameterDirection.Output;
//                paramcollectionForGetCard.Add(ID);
//                await _objSql.ExecuteDataSetQueryStringDB(GetNcid, paramcollectionForNCID);
//                int NcID;
//                response.NCid = int.TryParse(Convert.ToString(ID.Value), out NcID) ? NcID : 0;

//                List<SqlParameter> ChangeHistorycollection = new List<SqlParameter>
//                {
//                    new SqlParameter("@NCId", SqlDbType.Int) { Value = response.NCid },
//                    new SqlParameter("@NCWho", SqlDbType.Int) { Value = request.NCCardHolderID },
//                    new SqlParameter("@NCWhen", SqlDbType.DateTime) { Value = DateTime.Now },
//                    new SqlParameter("@NCChangedType", SqlDbType.Int) { Value = Convert.ToInt32(CommonConstants.NCChangedType) },
//                    new SqlParameter("@NCChangedObject", SqlDbType.Int) { Value = request.NCCardId },
//                    new SqlParameter("@NCChange", SqlDbType.Int) { Value = Convert.ToInt32(CommonConstants.AddCard) },
//                    new SqlParameter("@NCSubType", SqlDbType.Int) { Value = Convert.ToInt32(CommonConstants.NCSubType) },
//                    new SqlParameter("@NCSubObject", SqlDbType.Int) { Value = Convert.ToInt32(CommonConstants.NCSubType) },
//                    new SqlParameter("@NCLog", SqlDbType.VarChar) { Value = CommonConstants.CardAdded },
//                    new SqlParameter("@NCProcessed", SqlDbType.DateTime) { Value = DateTime.Now},
//                    new SqlParameter("@NCGeneratedBy", SqlDbType.Int) { Value = request.NCCityCode },
//                    new SqlParameter("@NCServerTime", SqlDbType.DateTime) { Value =  DateTime.Now},
                  
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(InlineChangeHistory, ChangeHistorycollection);

//                //Insert into DUP table
//                var InsertDup = InLineQueries.INSERTDUP;
//                List<SqlParameter> Dupcollection = new List<SqlParameter>
//                {
//                    new SqlParameter("@GetDate", SqlDbType.DateTime) { Value = DateTime.Now },
//                    new SqlParameter("@UpdateBy", SqlDbType.Int) { Value = request.NCCardHolderID }
//                };
//                await _objSql.ExecuteDataSetQueryStringDB(InsertDup, Dupcollection);

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"[CreateDigitalCardAsync] :: Exception: {JsonConvert.SerializeObject(ex)}");
//                response = new InsertCardDetailDalResponse { IsSuccess = false, Message = ApiError.InternalError };
//            }

//            _logger.LogInformationEnd();
//            return response;
//        }

//        public async Task<ReaderGroupIdAndPublicKeyDalResponse> GetReaderGroupIdAndReaderPublicKey(int cardholderid)
//        {

//            _logger.LogInformationStart("GetReaderGroupIdAndReaderPublicKey - > Request: {Cardholderid}" , cardholderid.ToString());
//            // Get Db connection key
//            var response = new ReaderGroupIdAndPublicKeyDalResponse { IsSuccess = true , groupIdAndPublicKey = new List<GroupIdAndPublicKey>() };
//            try
//            {
//                var InlineString = InLineQueries.GetReaderGroupIdAndReaderPublicKey;
//                List<SqlParameter> paramcollection = new List<SqlParameter>
//                {
//                    new SqlParameter(DBParamNames.CardHolderID, SqlDbType.Int)
//                    {
//                        Value = cardholderid
//                    }

//                };
//                var dalRes = await _objSql.ExecuteDataSetQueryStringDB(InlineString, paramcollection);

//                // Iterate through each DataTable in the DataSet
//                if (dalRes != null && dalRes.Tables !=null)
//                {
//                    foreach (DataTable dataTable in dalRes.Tables)
//                    {
//                        // Iterate through each row in the current DataTable
//                        foreach (DataRow row in dataTable.Rows)
//                        {
//                            var Data = new GroupIdAndPublicKey();
//                            // Access data from the first column using the indexer [0]
//                            Data.ReaderGroupId= row[0].ToString(); // Assuming the data type is string
//                            Data.PublicKeys = row[1].ToString(); // Assuming the data type is string
//                            response.groupIdAndPublicKey.Add(Data);
//                        }
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"[GetReaderGroupIdAndReaderPublicKey] :: Exception: {JsonConvert.SerializeObject(ex)}");
//                response = new ReaderGroupIdAndPublicKeyDalResponse { IsSuccess = false, Message = ApiError.InternalError };
//            }
//            _logger.LogInformationEnd();
//            return response;
//        }
//        public async Task<OperationStatus> UpdateAliroCardStatus(UpdateAliroCardRequest request)
//        {

//            _logger.LogInformationStart("UpdateAliroCardStatus - > Request: {Request}", JsonConvert.SerializeObject(request));
//            // Get Db connection key
//            var response = new OperationStatus { IsSuccess = true , Message = SuccessMessage.AliroCardUpdateSuccessMessage };
//            try
//            {
//                var InlineString = string.Empty;
//                List<SqlParameter> paramcollection = new List<SqlParameter>();
//                if (string.IsNullOrEmpty(request.ExternalNumber))
//                {
//                    InlineString = InLineQueries.UpdateAliroCardByCardholderId;
//                    SqlParameter commonParameter = new SqlParameter(DBParamNames.CARDHOLDERID, SqlDbType.Int);
//                    commonParameter.Value = request.CardholderId;
//                    paramcollection.Add(commonParameter);
//                }
//                else
//                {
//                    InlineString = InLineQueries.UpdateAliroCardByExternalNumber;
//                    SqlParameter commonParameter = new SqlParameter(DBParamNames.EXTERNALNUMBER, SqlDbType.VarChar);
//                    commonParameter.Value = request.ExternalNumber;
//                    paramcollection.Add(commonParameter);
//                }

//                var UpdateStatusString = string.Empty;
//                GetUpdateStringData(request.Cardstatus,ref UpdateStatusString, ref paramcollection);
//                var dalRes = await _objSql.ExecuteDataSetQueryStringDB(string.Format(InlineString, UpdateStatusString), paramcollection);
//                var RowAffected = 0;
//                if(dalRes.Tables.Count > 0)
//                {
//                    RowAffected = dalRes.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dalRes.Tables[0].Rows[0][0]) : 0;
//                }
//                if(RowAffected == 0) {
//                    response.Message = SuccessMessage.NORecord;
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"[UpdateAliroCardStatus] :: Exception: {JsonConvert.SerializeObject(ex)}");
//                response = new ReaderGroupIdAndPublicKeyDalResponse { IsSuccess = false, Message = ApiError.InternalError };
//            }
//            _logger.LogInformationEnd();
//            return response;
//        }
//        private static void GetUpdateStringData(string CardStatus , ref string status, ref List<SqlParameter> paramcollection)
//        {
//            var effectiveDate = DateTime.ParseExact("1990-01-01 00:00:00.000", "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
//            var expirationDate = DateTime.ParseExact("3999-12-31 23:59:59.000", "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
//            if (CardStatus == Cardstatus.ACTIVE.ToString())
//            {
//                SqlParameter parameter1 = new SqlParameter(DBParamNames.NCFlags, SqlDbType.Int);
//                parameter1.Value = 256;
//                paramcollection.Add(parameter1);
//                SqlParameter parameter2 = new SqlParameter(DBParamNames.NCEffective, SqlDbType.DateTime);
//                parameter2.Value = effectiveDate;
//                paramcollection.Add(parameter2);
//                SqlParameter parameter3 = new SqlParameter(DBParamNames.NCExpire, SqlDbType.DateTime);
//                parameter3.Value = expirationDate;
//                paramcollection.Add(parameter3);
//                SqlParameter parameter4 = new SqlParameter(DBParamNames.NCLastUpdate, SqlDbType.DateTime);
//                parameter4.Value = DateTime.UtcNow;
//                paramcollection.Add(parameter4);
//                status = "NCStatus='Authorized',NCFlags = @NCFlags, NCEffective = @NCEffective, NCExpire = @NCExpire , NCLastUpdate = @NCLastUpdate" ;
//            }
//            else if(CardStatus == Cardstatus.SUSPENDED.ToString())
//            {
//                SqlParameter parameter1 = new SqlParameter(DBParamNames.NCEffective, SqlDbType.DateTime);
//                parameter1.Value = effectiveDate;
//                paramcollection.Add(parameter1);
//                SqlParameter parameter2 = new SqlParameter(DBParamNames.NCExpire, SqlDbType.DateTime);
//                parameter2.Value = expirationDate;
//                paramcollection.Add(parameter2);
//                SqlParameter parameter3 = new SqlParameter(DBParamNames.NCLastUpdate, SqlDbType.DateTime);
//                parameter3.Value = DateTime.Now;
//                paramcollection.Add(parameter3);
//                status = "NCStatus='REVOKE' ,NCEffective = @NCExpire, NCExpire = @NCEffective, NCLastUpdate = @NCLastUpdate";
//            }
//            else if(CardStatus == Cardstatus.DELETED.ToString())
//            {
//                SqlParameter parameter1 = new SqlParameter(DBParamNames.NCLastUpdate, SqlDbType.DateTime);
//                parameter1.Value = DateTime.Now;
//                paramcollection.Add(parameter1);
//                status = "NCFlags = NCFlags + 1 ,NCLastUpdate = @NCLastUpdate";
//            }
//        }
//    }
//}
