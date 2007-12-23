using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BlackFox.S3
{
    public class S3Object : S3BaseType
    {
        string m_key;
        public string Key { get { return m_key; } }

        int m_size;
        public int Size { get { return m_size; } }
        string m_eTag;
        public string ETag { get { return m_eTag; } }

        DateTime m_lastModified;
        DateTime LastModified { get { return m_lastModified; } }

        /// <summary>
        /// Construct an S3Object from the XML result of a GET request on a S3Bucket.
        /// </summary>
        internal S3Object(S3Connection connection, XmlNode node)
            : base(connection)
        {
            m_key = S3Utils.SelectSingleString(node, "s3:Key");
            m_eTag = S3Utils.SelectSingleString(node, "s3:ETag");
            m_size = int.Parse(S3Utils.SelectSingleString(node, "s3:Size"));
            m_lastModified = S3Utils.ParseDate(S3Utils.SelectSingleString(node, "s3:LastModified"));
        }

        public override string ToString()
        {
            return m_key;
        }
    }
}
