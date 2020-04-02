using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.WebApi.OSSHelper
{
    public class TencentHelper
    {
        static CosXmlConfig config;
        static string secretId = "AKIDVveJrE2QJ1Z0IPQBvGUbdVTL3Yr1uPVR";   //云 API 密钥 SecretId
        static string secretKey = "uc4GJAwY1JQTQEuVwFNJ8qPpGYgeT4d4"; //云 API 密钥 SecretKey
        static long durationSecond = 600;          //每次请求签名有效时长，单位为秒
        static QCloudCredentialProvider qCloudCredentialProvider;
        static CosXml cosXml;
        static string bucket = "ceres-image-1258925102"; //存储桶，格式：BucketName-APPID

        //构造函数
        static TencentHelper()
        {

        }

        /// <summary>
        /// 上传文件到腾讯云中
        /// </summary>
        /// <param name="srcFilePath">本地绝对路径，包括文件后缀，比如 C:/Test/test.jpg</param>
        /// <param name="desFilePath">腾讯云相对路径，存储桶后面的路径，包括文件后缀，比如 /Test/test.jpg</param>
        /// <returns></returns>
        public static int UploadToTencent(string srcFilePath, string desFilePath)
        {
            int result = -1;
            try
            {
                config = new CosXmlConfig.Builder()
                  .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
                  .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
                  .IsHttps(true)  //设置默认 HTTPS 请求
                  .SetAppid("1258925102") //设置腾讯云账户的账户标识 APPID
                  .SetRegion("ap-shanghai") //设置一个默认的存储桶地域
                  .Build();

                qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId,
                  secretKey, durationSecond);

                cosXml = new CosXmlServer(config, qCloudCredentialProvider);


                string key = desFilePath; //对象在存储桶中的位置，即称对象键
                string srcPath = srcFilePath;//本地文件绝对路径
                //srcPath = "TestUpload" + "/"+ "76fe6f60-d78b-41a5-a8cf-46ac3229dbd9.png";
                if (!System.IO.File.Exists(srcPath))
                {
                    // 如果不存在目标文件，创建一个临时的测试文件
                    System.IO.File.WriteAllBytes(srcPath, new byte[1024]);
                }

                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult uploadResult = cosXml.PutObject(request);
                //对象的 eTag
                string eTag = uploadResult.eTag;
                result = 0;
            }
            catch (COSXML.CosException.CosClientException)
            {
                //请求失败
                //Console.WriteLine("CosClientException: " + clientEx);
                return result;
            }
            catch (COSXML.CosException.CosServerException)
            {
                //请求失败
                //Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                return result;
            }
            return result;
        }

        /// <summary>
        /// 查询腾讯云中，文件是否存在
        /// </summary>
        /// <param name="desFilePath">腾讯云相对路径，存储桶后面的路径，包括文件后缀，比如 /Test/test.jpg</param>
        /// <returns></returns>
        public static bool CheckTencentFileIsExists(string desFilePath)
        {
            bool result = false;
            try
            {
                config = new CosXmlConfig.Builder()
                  .SetConnectionTimeoutMs(60000)  //设置连接超时时间，单位毫秒，默认45000ms
                  .SetReadWriteTimeoutMs(40000)  //设置读写超时时间，单位毫秒，默认45000ms
                  .IsHttps(true)  //设置默认 HTTPS 请求
                  .SetAppid("1258925102") //设置腾讯云账户的账户标识 APPID
                  .SetRegion("ap-shanghai") //设置一个默认的存储桶地域
                  .Build();

                qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId,
                  secretKey, durationSecond);

                cosXml = new CosXmlServer(config, qCloudCredentialProvider);
                string key = desFilePath; //对象在存储桶中的相对位置，即称对象键

                HeadObjectRequest request = new HeadObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                HeadObjectResult objectResult = cosXml.HeadObject(request);

                if (objectResult != null)
                {
                    result = true;
                }
            }
            catch (COSXML.CosException.CosClientException)
            {
                //请求失败
                //Console.WriteLine("CosClientException: " + clientEx);
                return result;
            }
            catch (COSXML.CosException.CosServerException)
            {
                //请求失败
                //Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                return result;
            }
            return result;
        }
    }
}
