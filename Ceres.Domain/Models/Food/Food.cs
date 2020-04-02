using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Food : AggregationRoot
    {
        protected Food()
        { }

        public Food(Guid oid,string classify, string type, string code, string coding, string name, string image,int click,int status)
        {
            OID = oid;
            Classify = classify;
            Type = type;
            Code = code;
            Coding = coding;
            Name = name;
            Image = image;
            Click = click;
            Status = status;
        }

        public Food(Guid oid, string classify, string type, string code, string name, string image, int click, int status)
        {
            OID = oid;
            Classify = classify;
            Type = type;
            Code = code;
            Name = name;
            Image = image;
            Click = click;
            Status = status;
        }

        public string Classify { get; private set; }
        public string Type { get; private set; }
        public string Code { get; private set; }
        public string Coding { get; private set; }
        public string Name { get; private set; }
        public string Image { get; private set; }
        public int Click { get; private set; }
        public int Status { get; private set; }
    }
}
