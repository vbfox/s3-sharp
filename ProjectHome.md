This project is dedicated to host my experiments on amazon S3 usage in **C#**.

My current target is to have a simple, object-oriented wrapper around S3 using (for now) the  [TreeSharp](http://www.codeplex.com/ThreeSharp/) library for the REST communication.

## Sample ##
```
var s3 = new S3Connection(Keys.AwsAccessKeyId, Keys.AwsSecretAccessKey);

foreach(var bucket in s3.Buckets)
{
    Console.WriteLine(bucket.Name);

    foreach(var key in bucket.Keys)
    {
        Console.WriteLine("\t{0}", key.Name);
    }
}
```

## WARNING ##
This code use **C# 3.0** features and the solution is a **Visual Studio 2008** one.