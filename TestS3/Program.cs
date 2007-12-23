using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Affirma.ThreeSharp.Model;
using System.Xml;
using Affirma.ThreeSharp;
using Affirma.ThreeSharp.Query;

namespace BlackFox.S3
{
    /*
    static class Keys
    {
        public static readonly string AwsAccessKeyId = "";
        public static readonly string AwsSecretAccessKey = "";
    }
    */
    
    class Program
    {
        static void Main(string[] args)
        {
            S3Connection s3 = new S3Connection(Keys.AwsAccessKeyId, Keys.AwsSecretAccessKey);

            foreach (var bucket in s3.Buckets)
            {
                Console.WriteLine("{0} -- created : {1}", bucket.Name, bucket.CreationDate);
                
                foreach (var s in bucket.Keys)
                {
                    Console.WriteLine("\t{0} - {1} kB", s.Key, s.Size / 1024);
                }
            }
            Console.ReadLine();
        }
    }
}
