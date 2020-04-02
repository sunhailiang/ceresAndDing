using Ceres.WebApi.OSSHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ceres.WebApi
{
    public class PhotoGraphHelper
    {
        public static void PhotoGraphQuestionType(string imageBaseUrl, Application.ViewModels.Ding ding)
        {
            //JSON 数组或者不带引号的字符串
            try
            {
                //为Json数组
                string[] oldArray = JsonConvert.DeserializeObject<string[]>(ding.Answer);
                string[] newArray = new string[oldArray.Length];

                for (int i = 0; i < oldArray.Length; i++)
                {
                    //查找腾讯云中是否存在数据
                    string temp = "/Image/MercuryAnswer" + "/" + oldArray[i] + ".jpg";
                    if (!TencentHelper.CheckTencentFileIsExists(temp))
                    {
                        temp = "/Image/MercuryAnswer" + "/" + oldArray[i] + ".jpeg";
                        if (!TencentHelper.CheckTencentFileIsExists(temp))
                        {
                            temp = "/Image/MercuryAnswer" + "/" + oldArray[i] + ".png";
                        }
                    }
                    newArray[i] = imageBaseUrl + temp;
                }
                ding.Answer = JsonConvert.SerializeObject(newArray);
            }
            catch (Exception)
            {
                //数据为字符串
                string[] newArray = new string[1];
                string temp = "/Image/MercuryAnswer" + "/" + ding.Answer + ".jpg";
                if (!TencentHelper.CheckTencentFileIsExists(temp))
                {
                    temp = "/Image/MercuryAnswer" + "/" + ding.Answer + ".jpeg";
                    if (!TencentHelper.CheckTencentFileIsExists(temp))
                    {
                        temp = "/Image/MercuryAnswer" + "/" + ding.Answer + ".png";
                    }
                }
                newArray[0] = imageBaseUrl + temp;
                ding.Answer = JsonConvert.SerializeObject(newArray);
            }
        }

        public static void OtherQuestionType(Application.ViewModels.Ding ding)
        {
            try
            {
                ding.Answer = JsonConvert.DeserializeObject<string>(ding.Answer);
            }
            catch (Exception)
            {
            }
        }
    }
}
