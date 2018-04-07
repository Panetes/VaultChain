using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TrusChain.Storage;

namespace ConsoleApp1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ShareFileService : IServiceContract
    {
        public int ShareFile(string path, string address, string walletAddress)
        {

            var plainFile = path;
            var encrpFile = path + ".enc";

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {

                myRijndael.GenerateKey();
                myRijndael.GenerateIV();
                // Encrypt the string to an array of bytes. 
                byte[] encrypted = AESExample.AES.EncryptStringToBytes(File.ReadAllText(plainFile), myRijndael.Key, myRijndael.IV);

                // Decrypt the bytes to a string. 
                //string roundtrip = AESExample.AES.DecryptStringFromBytes(encrypted, myRijndael.Key, myRijndael.IV);

                File.WriteAllBytes(encrpFile, encrypted);
            }

            var hashString = IpfsWrapper.Add(encrpFile).Result;
            var hashString2 = IpfsWrapper.Add(plainFile).Result;
            
            if (hashString != null) return 1;

            return -1;
        }
    }
}
