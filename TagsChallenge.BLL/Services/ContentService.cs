using HtmlAgilityPack;
using StopWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagsChallenge.BLL.Interfaces;

namespace TagsChallenge.BLL.Services
{
    public class ContentService : IContentService
    {
        public async Task<List<string>> GetSuggestedTags(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            var pageContent = await response.Content.ReadAsStringAsync();

            var topTags = GetTags(
                RemoveHtmlTags(pageContent)
            );
            
            return topTags;
        }

        private string RemoveHtmlTags(string content)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            
            htmlDoc.DocumentNode.SelectNodes("//style|//script|//header|//head|//nav|//footer")
                .ToList()
                .ForEach(n => n.Remove());
            string text = "";
            foreach (var node in htmlDoc.DocumentNode.ChildNodes)
            {
                text += " " + CleanPageContent(node.InnerText);
            }

            return text;
        }

        private string CleanPageContent(string content)
        {
            var temp = Regex.Replace(content, @"\r\n?|\n", " ");
            temp = Regex.Replace(temp, @"((http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)", " ");
            temp = Regex.Replace(temp, "(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])", " ");
            temp = Regex.Replace(temp, "/\b(as|and|at|he|the|was|quot|posted|replied|replies|comment)\b/g", " ");
            temp = Regex.Replace(temp, "[.!?,;:\\-]", " ");
            temp = Regex.Replace(temp, "[^a-zA-Z_ ,]+", "", RegexOptions.Compiled);
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            temp = regex.Replace(temp, " ");
            return temp;
        }

        private List<string> GetTags(string content)
        {
            content = content.RemoveStopWords("en");
            return Regex.Split(content.ToLower(), @"\W+")
                .Where(s => s.Length > 4)
                .GroupBy(s => s)
                .OrderByDescending(g => g.Count())
                .Take(10)
                .Select(s => s.Key)
                .ToList();
        }
    }
}
