using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DAL.EFSource;
using SW.MiddlePlugin;

namespace SWMP.LikeMatch
{

    public class LikeMatch : SW.MiddlePlugin.IMiddlePlugin
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

        public LikeMatch()
        {
            DAL.EFSource.HEEntities context = new HEEntities();
            int noWordCount = 3;
            NoWordFunc = (bool isNo) =>
            {
                int count = context.RelationShip.Count(x => x.RType == (isNo ? 0 : 1));
                var startIndex = DateTime.Now.Ticks % (count - noWordCount);
                var list = context.RelationShip.Where(x => x.RType == (isNo ? 0 : 1)).OrderBy(x => x.Id).Skip((int)startIndex).Take(noWordCount).ToList();
                if (list.Count == 0)
                {
                    return "?";
                }
                StringBuilder ret = new StringBuilder();
                string lineFormat = "{0}与{1}" + (isNo ? "不" : "") + "宜搭配：{2};\n";
                foreach (var item in list)
                {
                    var matchPInfoF = context.Product.FirstOrDefault(x => x.Id == item.PA);
                    var matchPInfoS = context.Product.FirstOrDefault(x => x.Id == item.PB);
                    if (matchPInfoF == null || matchPInfoS == null)
                    {
                        continue;
                    }
                    ret.AppendFormat(lineFormat, matchPInfoF.Name, matchPInfoS.Name, item.Description.TrimEnd(new char[] { ',', '.', '，', '。' }));
                }
                return ret.ToString().TrimEnd('\n');

            };
            OneWordFunc = (string key, bool isNo) =>
            {
                DAL.EFSource.Product pInfo = context.Product.FirstOrDefault(x => x.Name == key || ("、" + x.NickNames + "、").Contains("、" + key + "、"));
                if (pInfo == null)
                {
                    //模糊查找
                    pInfo = context.Product.FirstOrDefault(x => x.Name.StartsWith(key));

                    if (pInfo == null)
                    {
                        pInfo = context.Product.FirstOrDefault(x => x.Name.Contains(key));
                    }
                }
                if (pInfo == null)
                {
                    return string.Format("没有找到 {0} 相关的", key);
                }
                var list = context.RelationShip.Where(x => x.PA == pInfo.Id && x.RType == (isNo ? 0 : 1)).ToList();
                StringBuilder ret = new StringBuilder();
                string lineFormat = "{0}与{1}" + (isNo ? "不" : "") + "宜搭配：{2};\n";
                foreach (var item in list)
                {
                    var matchPInfo = context.Product.FirstOrDefault(x => x.Id == item.PB);
                    if (matchPInfo == null)
                    {
                        continue;
                    }
                    ret.AppendFormat(lineFormat, pInfo.Name, matchPInfo.Name, item.Description.TrimEnd(new char[] { ',', '.', '，', '。' }));
                }
                return ret.ToString().TrimEnd('\n');
            };
        }
        public MPResultInfo Execute(string content)
        {
            var info = new TwoWordLineInfo(content, null);
            if (!info.IsSuccess)
            {
                return new MPResultInfo() { Status = ExecuteStatus.忽略 };
            }
            string responseTxt = "";
            if (info.LeftWord.WType != WordType.空值 && info.RightWord.WType != WordType.空值)
            {
                //if (TwoWordFunc == null)
                //{
                //    return new MPResultInfo() { ExecuteStatus = SW.MiddlePlugin.ExecuteStatus.忽略 };
                //}
                responseTxt = string.Format("{0} {1}", info.LeftWord.Content, info.RightWord.Content);
                return new MPResultInfo() { Status = SW.MiddlePlugin.ExecuteStatus.继续, Contents = responseTxt };

            }
            else if (info.LeftWord.WType == WordType.空值 && info.RightWord.WType == WordType.空值)
            {
                if (NoWordFunc == null)
                {
                    return new MPResultInfo() { Status = SW.MiddlePlugin.ExecuteStatus.忽略 };
                }
                responseTxt = NoWordFunc(info.IsDis);
            }
            else
            {
                if (OneWordFunc == null)
                {
                    return new MPResultInfo() { Status = SW.MiddlePlugin.ExecuteStatus.忽略 };
                }
                var oneWord = info.LeftWord.WType == WordType.空值 ? info.RightWord.Content : info.LeftWord.Content;
                responseTxt = OneWordFunc(oneWord, info.IsDis);
            }
            if (string.IsNullOrWhiteSpace(responseTxt))
            {
                return new MPResultInfo() { Status = ExecuteStatus.忽略 };
            }
            return new MPResultInfo() { Status = SW.MiddlePlugin.ExecuteStatus.截断, Contents = responseTxt };
        }
    }
}
