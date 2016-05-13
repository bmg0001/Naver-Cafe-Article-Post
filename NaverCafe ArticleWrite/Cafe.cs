using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinHttp;

namespace NaverCafe_ArticleWrite
{
    public class Cafe
    {
        public void Post (string NaverID , string NaverPW,int clubid , int menuid , string title , string content)
        {
            content = content + "<br>본 게시글은 ES Network NaverCafe ArticleWrite DLL 로 작성되었습니다. ";
            WinHttpRequest ht = new WinHttpRequest();
            ht.Open("POST", "https://nid.naver.com/nidlogin.login");
            ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            ht.SetRequestHeader("Referer", "https://nid.naver.com/nidlogin.login");
            ht.Send("enctp=2&url=http://www.naver.com&enc_url=http://www.naver.com&postDataKey=&saveID=0&nvme=0&smart_level=1&id=" + NaverID + "&pw=" + NaverPW);
            ht.WaitForResponse();
            string[] cookie = ht.GetAllResponseHeaders().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int cookieCount = 0;
            foreach (String header in cookie)
            {
                if (header.StartsWith("Set-Cookie: "))
                {
                    cookieCount++;
                }
            }
            String[] cookies = new String[cookieCount];
            cookieCount = 0;
            foreach (String header in cookie)
            {
                if (header.StartsWith("Set-Cookie: "))
                {
                    String cookie1 = header.Replace("Set-Cookie: ", "");
                    cookies[cookieCount] = cookie1;
                }
            }
            string Fcookie = "";

            foreach(string cds in cookies)
            {
                Fcookie += cds;
            }

            ht.Open("POST", "http://m.cafe.naver.com/ArticleWrite.nhn?m=write&clubid="+clubid.ToString()+ "&menuid="+menuid.ToString());
            ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            ht.SetRequestHeader("Referer", "https://nid.naver.com/nidlogin.login");
            ht.SetRequestHeader("Cookie",Fcookie);
            ht.Send();
            string personacon = Regex.Split(Regex.Split(ht.ResponseText, "input type=\"hidden\" name=\"personacon\" value=\"")[1], "\">")[0];
            string branchCode = Regex.Split(Regex.Split(ht.ResponseText, "input type=\"hidden\" name=\"branchCode\" value=\"")[1], "\"/>")[0];

            ht.Open("POST", "http://m.cafe.naver.com/ArticlePost.nhn");
            ht.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            ht.SetRequestHeader("Referer", "http://m.cafe.naver.com/ArticleWrite.nhn?m=write&clubid=" + clubid.ToString() + "&menuid=" + menuid.ToString());
            ht.SetRequestHeader("Cookie", Fcookie);
            ht.Send("tagnames=&menuid="+menuid+"&headid=&subject="+title+"&content="+ System.Web.HttpUtility.UrlEncode(content) + "&clubid="+clubid.ToString()+"&articleid=&m=write&openyn=Y&replyyn=Y&searchopen=1&scrapyn=Y&rclick=0&autosourcing=1&ccl=0&metoo=true&attachfiles=&attachfileyn=&attachimageyn=&attachmovielist=&attachmovie=&attachLink=&attachmaplist=&article.attachCalendarList=&attachPollids=&attachinfolist=&personacon="+personacon+"&tempsaveid=1&branchCode="+branchCode+"&historyBack=");
        }
    }
}
