/******************************************************************************* 
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0.html 
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  Joel Wetzel
 *  Affirma Consulting
 *  jwetzel@affirmaconsulting.com
 * 
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace Affirma.ThreeSharp.Model
{
    /// <summary>
    /// The base class for all Request objects
    /// </summary>
    public class Request : Transfer
    {
        protected int timeout = 50000;
        protected String contentType = "";
        protected SortedList queryList;
        protected SortedList metaData;

        protected String redirectUrl;

        public Request()
        {
            this.Method = "PUT";
            this.queryList = new SortedList();
            this.metaData = new SortedList();
        }

        public int Timeout
        {
            get { return this.timeout; }
        }

        public String ContentType
        {
            get { return this.contentType; }
        }

        public SortedList QueryList
        {
            get { return this.queryList; }
        }

        public SortedList MetaData
        {
            get { return this.metaData; }
        }

        public String RedirectUrl
        {
            get { return this.redirectUrl; }
            set { this.redirectUrl = value; }
        }

        public void LoadStreamWithBytes(byte[] bytes)
        {
            this.dataStream = new MemoryStream(bytes.Length);
            this.dataStream.Write(bytes, 0, bytes.Length);
            this.dataStream.Position = 0;

            this.contentType = "text/plain";
            this.BytesTotal = bytes.Length;
        }

        public void LoadStreamWithString(String data)
        {
            ASCIIEncoding ae = new ASCIIEncoding();
            byte[] bytes = ae.GetBytes(data);
            this.LoadStreamWithBytes(bytes);
        }

        public void LoadStreamWithFile(String localfile)
        {
            this.dataStream = File.OpenRead(localfile);

            this.contentType = ThreeSharpUtils.ConvertExtensionToMimeType(Path.GetExtension(localfile));
            this.BytesTotal = ((FileStream)this.dataStream).Length;
        }

        public void EncryptStream(string encryptionKey, string encryptionIV)
        {
            Stream existingStream = this.dataStream;

            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            cryptoServiceProvider.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey);
            cryptoServiceProvider.IV = ASCIIEncoding.ASCII.GetBytes(encryptionIV);
            CryptoStream cryptoStream = new CryptoStream(existingStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Read);
            this.dataStream = cryptoStream;

            // The encryption algorithm can pad the data by a few bytes, so this line corrects for that.
            this.BytesTotal = Convert.ToInt64(Math.Ceiling(Convert.ToDouble(this.BytesTotal) / 8.0) * 8.0);
            this.BytesTotal = this.BytesTotal > 0L ? this.BytesTotal : 8L; 
        }

    }
}
