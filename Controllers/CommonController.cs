using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BizWebAPI.Areas.ERP.Dtos;
using BizWebAPI.Areas.ERP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BizWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommonController : ControllerBase
    {
        [HttpPost]
        [Route("Encryption")]
        public string Encryption(CryptoLibraryDto cryptoLibraryDto)
        {
            CryptoLibrary crypto = new CryptoLibrary();
            var Encryption = crypto.Encrypt(cryptoLibraryDto.PlainText, cryptoLibraryDto.SecurityKey);

            return Encryption;
        }

        [HttpPost]
        [Route("Decryption")]
        public string Decryption(CryptoLibraryDto cryptoLibraryDto)
        {
            CryptoLibrary cryptoLibrary = new CryptoLibrary();
            var Decryption = cryptoLibrary.Decrypt(cryptoLibraryDto.PlainText, cryptoLibraryDto.SecurityKey);

            return Decryption;
        }
    }
}