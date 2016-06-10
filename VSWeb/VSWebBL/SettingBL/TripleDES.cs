using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using VSWebDO;
using VSWebDAL;

namespace VSWebBL.SettingBL
{
   public class TripleDES
    {
       private static TripleDES _self = new TripleDES();

        /// <summary>
        /// Used to call the functions using class name instead of object
        /// </summary>
       public static TripleDES Ins
        {
            get { return _self; }
        }
       private Byte[] key = { 8, 2, 11, 4, 5, 6, 7, 8, 4, 10, 11, 12, 13, 21, 15, 16, 17, 18, 2, 20, 21, 16, 16, 24 };
       private Byte[] iv = { 65, 110, 68, 1, 69, 178, 200, 235 };

       public Byte[] Encrypt(String plainText)
       {
           //Declare a UTF8Encoding object so we may use the GetByte
           // method to transform the plainText into a Byte array.

           UTF8Encoding utf8encoder= new  UTF8Encoding();
           Byte[] inputInBytes=utf8encoder.GetBytes(plainText);

        // Create a new TripleDES service provider
           TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

        // The ICryptTransform interface uses the TripleDES
        // crypt provider along with encryption key and init vector
        // information
           ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(key, iv);

        // All cryptographic functions need a stream to output the
        //encrypted information. Here we declare a memory stream
        //for this purpose.

           MemoryStream encryptedStream = new MemoryStream();
           CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);

        // Write the encrypted information to the stream. Flush the information
        // when done to ensure everything is out of the buffer.
           cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
           cryptStream.FlushFinalBlock();
           encryptedStream.Position = 0;
           //  Read the stream back into a Byte array and return it to the calling method.
           int l=int.Parse(encryptedStream.Length.ToString());
           Byte[] result;
           result = BitConverter.GetBytes(encryptedStream.Length - 1);
        encryptedStream.Read(result, 0,l );
        cryptStream.Close();
          return result;
   

       }

       public string Decrypt(Byte[] inputInBytes)
       {
           if(inputInBytes==null)
           {
               
           }

           // UTFEncoding is used to transform the decrypted Byte Array
           //information back into a string.
           UTF8Encoding utf8encoder = new UTF8Encoding();
           TripleDESCryptoServiceProvider tdesProvider=new TripleDESCryptoServiceProvider();

        //  As before we must provide the encryption/decryption key along with
        // the init vector.
           ICryptoTransform cryptoTransform = tdesProvider.CreateDecryptor(key, iv);

        // Provide a memory stream to decrypt information into
        MemoryStream   decryptedStream= new MemoryStream();
        CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write);
        cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
        cryptStream.FlushFinalBlock();
        decryptedStream.Position = 0;

      // Read the memory stream and convert it back into a string
        Byte[] result; 
        int l=int.Parse(decryptedStream.Length.ToString());
        result = BitConverter.GetBytes(decryptedStream.Length - 1);
        decryptedStream.Read(result, 0, l);
        cryptStream.Close();
        UTF8Encoding myutf = new UTF8Encoding();
         return myutf.GetString(result);


       }

      
    }
}
