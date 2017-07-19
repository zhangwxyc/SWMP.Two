using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace SWMP.LikeMatch
{
    public class TwoWordService
    {
        /// <summary>
        /// 两个
        /// </summary>
        public Func<string, string, string> TwoWordFunc;

        /// <summary>
        /// 单个
        /// </summary>
        public Func<string, bool, string> OneWordFunc;

        /// <summary>
        /// 0个
        /// </summary>
        public Func<bool, string> NoWordFunc;


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="srcText"></param>
        ///// <param name="config"></param>
        ///// <returns></returns>
        //public (bool isRedirect, string txt) Process(string srcText, NameValueCollection config = null)
        //{
        //    var info = new TwoWordLineInfo(srcText, config);
        //    if (!info.IsSuccess)
        //    {
        //        return ValueTuple.Create(false, srcText);
        //    }
        //    string responseTxt = "";
        //    if (info.LeftWord.WType != WordType.空值 && info.RightWord.WType != WordType.空值)
        //    {
        //        if (TwoWordFunc == null)
        //        {
        //            return ValueTuple.Create(false, srcText);
        //        }
        //        responseTxt = TwoWordFunc(info.LeftWord.Content, info.RightWord.Content);
        //    }
        //    else if (info.LeftWord.WType == WordType.空值 && info.RightWord.WType == WordType.空值)
        //    {
        //        if (NoWordFunc == null)
        //        {
        //            return ValueTuple.Create(false, srcText);
        //        }
        //        responseTxt = NoWordFunc(info.IsDis);
        //    }
        //    else
        //    {
        //        if (OneWordFunc == null)
        //        {
        //            return ValueTuple.Create(false, srcText);
        //        }
        //        var oneWord = info.LeftWord.WType == WordType.空值 ? info.RightWord.Content : info.LeftWord.Content;
        //        responseTxt = OneWordFunc(oneWord, info.IsDis);
        //    }
        //    return ValueTuple.Create(true, responseTxt);
        //}
    }
}
