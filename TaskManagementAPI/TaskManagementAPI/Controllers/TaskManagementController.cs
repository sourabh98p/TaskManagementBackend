//using Microsoft.AspNetCore.Mvc;
//using TaskManagementAPI.Controllers.Public;

//namespace TaskManagementAPI.Controllers
//{
//    /// <summary>
//    /// Values Controller
//    /// </summary>
//    [Route("kiapi/v1/[controller]")]
//    [ApiController]
//    public class TaskManagementController : SecureController
//    {

//        private readonly IGoogleWalletManager _googleWalletManager;
//        /// <summary>
//        /// Google Wallet Controller
//        /// </summary>
//        public TaskManagementController(IGoogleWalletManager googleWalletManager)
//        {
//            _googleWalletManager = googleWalletManager;
//        }

//        /// <summary>
//        /// Create Digital Card
//        /// </summary>
//        /// <returns></returns>

//        [HttpPost]
//        [Route("CreateDigitalCard")]
//        public async Task<IActionResult> CreateDigitalCard(CreateDigitalCardRequest request)
//        {
//            var response = await _googleWalletManager.CreateDigitalCardAsync(request);
//            return Ok(response);
//        }
//        /// <summary>
//        /// Update Aliro CardStatus
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost]
//        [Route("UpdateAliroCardStatus")]
//        public async Task<IActionResult> UpdateAliroCardStatus(UpdateAliroCardRequest request)
//        {
//            var response = await _googleWalletManager.UpdateAliroCardStatus(request);
//            return Ok(response);
//        }

//        /// <summary>
//        /// Get IntUpdated Cardbundle Details
//        /// </summary>
//        /// <returns></returns>
//        /// 
//        [HttpPost]
//        [Route("GetUpdatedCardbundleDetails")]
//        public async Task<IActionResult> GetUpdatedCardbundleDetails(CreateDigitalCardRequest request)
//        {
//            var response = await _googleWalletManager.GetUpdatedCardbundleDetailsAsyn(request);
//            return Ok(response);
//        }

//    }
//}
