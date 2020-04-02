using Ceres.Domain.Core.Models;
using System;

namespace Ceres.Domain.Models
{
    public class Component : AggregationRoot
    {
        protected Component()
        { }

        public Component(Guid oid, string name,string englishName, string nameCode, string unit,int important)
        {
            OID = oid;
            Name = name;
            EnglishName = englishName;
            NameCode = nameCode;
            Unit = unit;
            Important = important;
        }

        public string Name { get; private set; }
        public string EnglishName { get; private set; }
        public string NameCode { get; private set; }
        public string Unit { get; private set; }
        public int Important { get; private set; }
    }
}
