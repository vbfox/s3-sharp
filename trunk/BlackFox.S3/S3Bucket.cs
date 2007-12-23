using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Affirma.ThreeSharp.Model;

namespace BlackFox.S3
{
    public class S3Bucket : S3BaseType
    {
        string m_name;
        public string Name { get { return m_name; } }

        DateTime m_creationDate;
        public DateTime CreationDate { get { return m_creationDate; } }

        internal S3Bucket(S3Connection connection, XmlNode node)
            : base(connection)
        {
            m_name = S3Utils.SelectSingleString(node, "s3:Name");
            var creationDateStr = S3Utils.SelectSingleString(node, "s3:CreationDate");
            m_creationDate = S3Utils.ParseDate(creationDateStr);
        }

        public IEnumerable<S3Object> Keys
        {
            get
            {
                BucketListRequest request = null;
                BucketListResponse response = null;
                try
                {
                    // The first thing we need to do is check for the presence of a Temporary Redirect.  These occur for a few
                    // minutes after an EU bucket is created, while S3 creates the DNS entries.  If we get one, we need to pull
                    // the bucket listing from the redirect URL

                    string redirectUrl = null;
                    BucketListRequest testRequest = new BucketListRequest(m_name);
                    testRequest.Method = "HEAD";
                    BucketListResponse testResponse = Service.BucketList(testRequest);
                    testResponse.DataStream.Close();
                    if (testResponse.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                    {
                        redirectUrl = testResponse.Headers["Location"].ToString();
                    }

                    bool isTruncated = true;
                    string marker = string.Empty;

                    // The while-loop is here because S3 will only return a maximum of 1000 items at a time, so if the list
                    // was truncated, we need to make another call and get the rest
                    while (isTruncated)
                    {
                        request = new BucketListRequest(m_name);
                        request.RedirectUrl = redirectUrl;
                        if (!string.IsNullOrEmpty(marker))
                        {
                            request.QueryList.Add("marker", marker);
                        }
                        response = Service.BucketList(request);

                        XmlNode responseNode = S3Utils.SelectSingleNode(response.StreamResponseToXmlDocument(), "/s3:ListBucketResult");

                        foreach (var objNode in S3Utils.SelectNodes(responseNode, "s3:Contents"))
                        {
                            S3Object obj = new S3Object(Connection, objNode);
                            yield return obj;
                            marker = obj.Key;
                        }

                        isTruncated = bool.Parse(S3Utils.SelectSingleString(responseNode, "s3:IsTruncated"));
                    }
                }
                finally
                {
                    if (response != null && response.DataStream != null) response.DataStream.Close();
                }
            }
        }

        public override string ToString()
        {
            return m_name;
        }
    }
}
