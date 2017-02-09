/*
*   AmirHossein
*   22 Bahman 1395
*   Auto vote websites in iwmf.ir
*/
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iwmfVote
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new CookieContainer();
            Uri target = new Uri("http://www.iwmf.ir/website/vote");

            // after login, find your cookie in browser developer tool and replace here
            c.Add(new Cookie("remember_{remember_code}", "{cookie_value}") { Domain = target.Host });

            for (int i = 0; i < 250; i++)
            {
                var doc = GetPage("http://www.iwmf.ir/website/vote", c).DocumentNode;
                var div = doc.Descendants("div").Where(o => o.Attributes.Contains("class") && o.Attributes["class"].Value.Contains("voteBtnsBox"));
                var links = div.FirstOrDefault().ChildNodes;
                var tf = (i % 2 == 0) ? 1 : 3;
                var href = links[tf].Attributes["href"].Value;
                var x = GetPage(href, c).DocumentNode;
            }

            // wait for you free ticket :)
        }

        public static HtmlDocument GetPage(string url, CookieContainer _cookies)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.CookieContainer = _cookies;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();

            using (var reader = new StreamReader(stream))
            {
                string html = reader.ReadToEnd();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
        }
    }
}
