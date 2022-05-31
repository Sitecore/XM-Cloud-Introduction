using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvp.Feature.People
{
    public class Constants
    {
        public const string MVP_PEOPLE_ROOT_ITEM_SHORT_ID = "64F31E3A20404E69B9A76830CBE669D2";
        public const string MVP_PERSON_TEMPLATE_SHORT_ID = "AD9C783786604360BA2B7ADDF4163685";

        public static class QueryParameters
        {
            public const string FacetPrefix = "fc_";
            public const string FacetAward = "fc_Type";
            public const string FacetYear = "fc_Year"; 
            public const string FacetCountry = "fc_Country";
            public const string Page = "pg";
            public const string Query = "q";
        }
    }
}
