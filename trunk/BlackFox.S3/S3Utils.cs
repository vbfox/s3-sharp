using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BlackFox.S3
{
    public static class S3Utils
    {
        public static IEnumerable<XmlNode> SelectNodes(XmlNode node, string xpath)
        {
            foreach (XmlNode selectedNode in node.SelectNodes(xpath, GetNS(node)))
            {
                yield return selectedNode;
            }
        }

        public static XmlNode SelectSingleNode(XmlNode node, string xpath)
        {
            return node.SelectSingleNode(xpath, GetNS(node));
        }

        public static string SelectSingleString(XmlNode node, string xpath)
        {
            return SelectSingleNode(node, xpath).InnerXml;
        }

        public static XmlNamespaceManager GetNS(XmlNode node)
        {
            return node.OwnerDocument == null ? GetNS((XmlDocument)node) : GetNS(node.OwnerDocument);
        }

        public static XmlNamespaceManager GetNS(XmlDocument doc)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("s3", @"http://s3.amazonaws.com/doc/2006-03-01/");
            return ns;
        }

        public static DateTime ParseDate(string dateString)
        {
            return DateTime.ParseExact(dateString, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK",
                            System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
