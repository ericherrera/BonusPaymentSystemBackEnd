using BonusPaymentSystem.Api.Models.Constants;
using BonusPaymentSystem.Commons.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Common
{
    public static class PageListHelper
    {
        public static readonly IDictionary PageHeaders = new Dictionary<string, string>()
        {
            { PAGE_NUMBER, "" },
            { PAGE_SIZE, "" }, 
            { TOTAL_ITEM, "" }, 
            { PAGE_MAX_SIZE, "" }
        };

        public const string PAGE_NUMBER = "pageNumber";
        public const string PAGE_SIZE = "pageSize";
        public const string TOTAL_ITEM = "totalSize";
        public const string PAGE_MAX_SIZE = "pageMaxSize";

        public const int PAGE_MAX_SIZE_VALUE = 50;


        public static int GetPageSize(int pageSize)
        {
          return pageSize > PAGE_MAX_SIZE_VALUE ? PAGE_MAX_SIZE_VALUE : pageSize;
        }

        public static void SetHeaderParamPage(IHeaderDictionary headers)
        {
            foreach (var varNameObj in PageHeaders.Keys)
            {
                var varName = varNameObj.ToString();
                if (headers.ContainsKey(varName))
                    headers[varName] = PageHeaders[varName].ToString();
                else
                    headers.Add(varName, PageHeaders[varName].ToString());

            }

        }
    }
}
