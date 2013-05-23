using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace MSR.LST.Net.Rtp
{
    public class EncryptionTransform : PacketTransform
    {
        #region PacketTransform Members

        private static readonly byte[] salt = { 0xaa, 0xbb, 0xcc, 0x01, 0x02, 0x03, 0xab, 0xfa };

        private static readonly String paranoidSaltString = "Paranoid Salt String";

        private ICryptoTransform decrypter;
        private ICryptoTransform encrypter;


        public EncryptionTransform(String password) {
            PasswordDeriveBytes passwordBytes = 
                new PasswordDeriveBytes(password + paranoidSaltString,salt);

            // Create a TripleDESCryptoServiceProvider object.
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Mode = CipherMode.ECB; 

            // Create the key and add it to the Key property.
            tdes.Key = passwordBytes.CryptDeriveKey("TripleDES", "SHA1", 192, tdes.IV);

            decrypter = tdes.CreateDecryptor();
            encrypter = tdes.CreateEncryptor();
        }
       


        public void Encode(RtpPacket packet)
        {
            Debug.WriteLine("ENCODING: Original length: " + packet.Payload.Length);
            Debug.Assert(packet.ReservedPaddingBytes == RtpPacket.MAX_CRYPTO_BLOCK_SIZE);

   

            BufferChunk chunk = SymmetricEncryption(packet, encrypter);

            // set the payload; this method indicates that we are allowed to use
            // the portion of the payload that is reserved for padding
            packet.SetPaddedPayload(chunk);

            Debug.WriteLine("New length: " + packet.Payload.Length);
        }

        private BufferChunk SymmetricEncryption(RtpPacket packet,ICryptoTransform crypto)
        {
            BufferChunk payload = packet.Payload;

            byte[] data = crypto.TransformFinalBlock(payload.Buffer, payload.Index, payload.Length);

            return new BufferChunk(data);
        }

        public void Decode(RtpPacket packet)
        {
            Debug.WriteLine("DECODING: Original length: " + packet.Payload.Length);

            packet.Payload = SymmetricEncryption(packet,decrypter);

            Debug.WriteLine("New length: " + packet.Payload.Length);
        }

        #endregion
    }
}
