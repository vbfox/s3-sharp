using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affirma.ThreeSharp;
using Affirma.ThreeSharp.Model;
using System.Xml;
using Affirma.ThreeSharp.Query;


namespace BlackFox.S3
{
    public class S3Connection
    {
        public IEnumerable<S3Bucket> Buckets
        {
            get
            {
                BucketListRequest request = null;
                BucketListResponse response = null;
                try
                {
                    request = new BucketListRequest(null);
                    response = m_service.BucketList(request);

                    XmlDocument bucketXml = response.StreamResponseToXmlDocument();

                    return bucketXml.SelectS3Nodes("//s3:Bucket").Select(n => new S3Bucket(this, n));
                }
                finally
                {
                    if (response != null && response.DataStream != null)
                        response.DataStream.Close();
                }
            }
        }

        ThreeSharpConfig m_config;
        IThreeSharp m_service;
        public IThreeSharp Service { get { return m_service; } }

        public S3Connection(String awsAccessKeyId, String awsSecretAccessKey)
        {
            m_config = new ThreeSharpConfig();
            m_config.AwsAccessKeyID = awsAccessKeyId;
            m_config.AwsSecretAccessKey = awsSecretAccessKey;
            m_config.IsSecure = false;

            m_service = new ThreeSharpQuery(m_config);
        }
    }
}
