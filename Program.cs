using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using LiquidTechnologies.XmlObjects;
using LiquidTechnologies.XmlObjects.NewsMLPower227;
using LiquidTechnologies.XmlObjects.NewsMLPower227.NsA;
using LiquidTechnologies.XmlObjects.NewsMLPower227.Tns;
using LiquidTechnologies.XmlObjects.NewsMLPower227.TnsA;
using LiquidTechnologies.XmlObjects.NewsMLPower227.Xs;

namespace test_liqutech_ng2_databinding_01
{
    class Program
    {
        static void Main(string[] args)
        {
            Test01();

            Console.WriteLine("Type a key to close this application.");
            var inkey = Console.ReadLine();
        }

        private static void Test01()
        {
            NewsItemElm newsItem = new NewsItemElm();
            newsItem.Standard = "NewsML-G2";
            newsItem.Standardversion = "2.27";
            newsItem.Guid = "urn:newsml:acmenews.com:20161121:US-FINANCE-FED";
            newsItem.Version = 11;

            ItemMetaElm itemMeta = new ItemMetaElm();
            newsItem.ItemMeta = itemMeta;
            VersionCreatedElm versionCreated = new VersionCreatedElm();
            versionCreated.Value = ;






            LxSerializer<NewsItemElm> serializer1 = new LxSerializer<NewsItemElm>();
            using (StringWriter sw = new StringWriter())
            {
                serializer1.Serialize(sw, newsItem);
                Console.WriteLine(sw.ToString());
            }
            

        }
    }
}
