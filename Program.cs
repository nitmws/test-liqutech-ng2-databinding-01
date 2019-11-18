using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using LiquidTechnologies.XmlObjects;
using LiquidTechnologies.XmlObjects.NewsMLPower227.NsA;
using LiquidTechnologies.XmlObjects.NewsMLPower227.TnsA;

namespace test_liqutech_ng2_databinding_01
{
    class Program
    {
        private static int defaultTestno = 1;

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Test00();
            }
            else
            {
                int testno = -1;
                try
                {
                    testno = Convert.ToInt32(args[0]);
                }
                catch (Exception ex)
                {
                    testno = defaultTestno;
                }
                switch (testno)
                {
                    case 1:
                        Test01();
                        break;
                    case 2:
                        Test02();
                        break;
                    case 3:
                        Test03();
                        break;
                    default:
                        Test01();
                        break;
                }
            }

            Console.WriteLine("Type a key to close this application.");
            var inkey = Console.ReadLine();
        }

        /// <summary>
        /// Dummy test, tells only how to select a specific test
        /// </summary>
        private static void Test00()
        {
            Console.WriteLine("Tests have to be selected by a number higher than 0 as command line parameter.");
        }

        /// <summary>
        /// Generates a NewsItem complying to the reference example LISTING_2-modPower_NewsML-G2_Text_Document.xml.
        /// The sequense of generating nodes/elements follows the sequence in the example XML document.
        /// </summary>
        private static void Test01()
        {
            // Generic variables
            Exception ex1;
            LxDateTime lxdt;
            ConceptNameTypeCt name;
            TruncatedDateTimeTypeUnion dtvaltdtt;

            NewsItemElm newsItem = new NewsItemElm();
            newsItem.Standard = "NewsML-G2";
            newsItem.Standardversion = "2.27";
            newsItem.Guid = "urn:newsml:acmenews.com:20161121:US-FINANCE-FED";
            newsItem.Version = 11;
            newsItem.Conformance = "power";

            // Adding 2 catalogRef-s:
            CatalogRefElm catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://www.iptc.org/std/catalog/catalog.IPTC-G2-Standards_30.xml";
            newsItem.CatalogRef.Add(catalogRef);
            catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://catalog.acmenews.com/news/ANM_G2_CODES_2.xml";
            newsItem.CatalogRef.Add(catalogRef);

            // Adding rightsInfo - bubble up
            CopyrightHolderElm copyrightHolder = new CopyrightHolderElm();
            copyrightHolder.Uri = "http://www.acmenews.com/about.html#copyright";
            name = new ConceptNameTypeCt();
            name.Value = "Acme News and Media LLC";
            copyrightHolder.Name.Add(name);
            CopyrightNoticeElm copyrightNotice = new CopyrightNoticeElm();
            copyrightNotice.Add("Copyright 2016-17 Acme News and Media LLC");
            RightsInfoTypeCt rightsInfo = new RightsInfoTypeCt();
            rightsInfo.CopyrightHolder = copyrightHolder;
            rightsInfo.CopyrightNotice.Add(copyrightNotice);
            newsItem.RightsInfo.Add(rightsInfo);

            // Adding itemMeta properties
            newsItem.ItemMeta.ItemClass.Qcode = "ninat:text";
            lxdt = LxDateTime.CreateDateTime(2017, 11, 21, 16, 25, 32, 0, 0, 0, -5, 0);
            newsItem.ItemMeta.VersionCreated.Value = lxdt;
            newsItem.ItemMeta.Provider.Uri = "http://www.acmenews.com/about/";

            QualPropTypeCt pubStatus = new QualPropTypeCt();
            pubStatus.Qcode = "stat:usable";
            newsItem.ItemMeta.PubStatus = pubStatus;

            // Adding contentMeta properties
            ContentMetadataAfDTypeCt contentMeta = new ContentMetadataAfDTypeCt(); // = the wrapper

            if (LxDateTime.TryParseXSDDateTime("2016-11-21T15:21:06-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentCreated = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentCreated.Value = dtvaltdtt;
                contentMeta.ContentCreated = contentCreated;
            }

            if (LxDateTime.TryParseXSDDateTime("2017-11-21T16:22:45-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentModified = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentModified.Value = dtvaltdtt;
                contentMeta.ContentModified = contentModified;
            }

            FlexLocationPropTypeCt located = new FlexLocationPropTypeCt();
            located.Qcode = "geoloc:NYC";
            name = new ConceptNameTypeCt();
            name.Value = "New York, NY";
            located.Name.Add(name);
            contentMeta.Located.Add(located);

            FlexAuthorPropTypeCt creator = new FlexAuthorPropTypeCt();
            creator.Uri = "http://www.acmenews.com/staff/mjameson";
            name = new ConceptNameTypeCt();
            name.Value = "Meredith Jameson";
            creator.Name.Add(name);
            contentMeta.Creator1.Add(creator); // .Creator = @creator, .Creator1 = /creator

            Flex1PartyPropTypeCt infoSource = new Flex1PartyPropTypeCt();
            infoSource.Qcode = "is:AP";
            name = new ConceptNameTypeCt();
            name.Value = "Associcate Press";
            infoSource.Name.Add(name);
            contentMeta.InfoSource.Add(infoSource);

            LanguageElm language = new LanguageElm();
            language.Tag = "en-US";
            contentMeta.Language.Add(language);

            // a sequence of subject elements
            SubjectElm subject = new SubjectElm();
            subject.Qcode = "medtop:04000000";
            name = new ConceptNameTypeCt();
            name.Value = "economy, business and finance";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            subject = new SubjectElm();
            subject.Qcode = "medtop:20000350";
            name = new ConceptNameTypeCt();
            name.Value = "central bank";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            subject = new SubjectElm();
            subject.Qcode = "medtop:20000379";
            name = new ConceptNameTypeCt();
            name.Value = "money and monetary policy";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            SluglineElm slugline = new SluglineElm();
            slugline.Value = "US-Finance-Fed";
            contentMeta.Slugline.Add(slugline);

            HeadlineElm headline = new HeadlineElm();
            headline.Value = "Fed to halt QE to avert \"bubble\"";
            contentMeta.Headline.Add(headline);

            // finally: add the filled contentMeta instance to the newsItem
            newsItem.ContentMeta = contentMeta;

            // Adding contentSet
            ContentSetElm contentSet = new ContentSetElm();

            // (create the XML tree outside the NewsML-G2 schema - in the IPTC NITF namespace
            XNamespace nitfNs = "http://iptc.org/std/NITF/2006-10-18/";
            XElement nitf = new XElement(nitfNs + "nitf",
                new XElement(nitfNs + "body",
                    new XElement(nitfNs + "body.head",
                        new XElement(nitfNs + "hedline",
                            new XElement(nitfNs + "hl1", new XText("Fed to halt QE to avert \"bubble\""))
                        ),
                        new XElement(nitfNs + "byline", new XText("By Meredith Jameson, Staff Reporter"))
                    ),
                    new XElement(nitfNs + "body.content",
                      new XElement(nitfNs + "p", new XText("(New York, NY - October 21) Et, sent luptat luptat, commy Nim zzriureet vendreetue modo etc")),
                      new XElement(nitfNs + "p", new XText("Ugiating ea feugait utat, venim velent nim quis nulluptat num Volorem inci enim dolobor eetuer sendre ercin utpatio dolorpercing"))
                    )
                )
            );

            ContentSetElm.InlineXMLElm inlineXml = new ContentSetElm.InlineXMLElm();
            inlineXml.AnyElement = nitf;
            contentSet.InlineXML.Add(inlineXml);
            newsItem.ContentSet = contentSet;

            // final action of the test procedure: serialized the XML document and create an output
            LxSerializer<NewsItemElm> serializer1 = new LxSerializer<NewsItemElm>();
            using (StringWriter sw = new StringWriter())
            {
                serializer1.Serialize(sw, newsItem);
                Console.WriteLine(sw.ToString());
            }
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(@"..\..\NewsML-G2-testresults", "test01-LISTING_2-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test01-LISTING_2-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test01 closes");

        } // Test01

        /// <summary>
        /// Generates a NewsItem complying to the reference example LISTING_2-modPower_NewsML-G2_Text_Document.xml.
        /// The sequense of generating nodes/elements DOES NOT follow the sequence in the example XML document.
        /// </summary>
        private static void Test02()
        {
            // Generic variables
            Exception ex1;
            LxDateTime lxdt;
            ConceptNameTypeCt name;
            TruncatedDateTimeTypeUnion dtvaltdtt;

            NewsItemElm newsItem = new NewsItemElm();
            newsItem.Standard = "NewsML-G2";
            newsItem.Standardversion = "2.27";
            newsItem.Guid = "urn:newsml:acmenews.com:20161121:US-FINANCE-FED";
            newsItem.Version = 11;
            newsItem.Conformance = "power";

            // Adding 2 catalogRef-s:
            CatalogRefElm catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://www.iptc.org/std/catalog/catalog.IPTC-G2-Standards_30.xml";
            newsItem.CatalogRef.Add(catalogRef);
            catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://catalog.acmenews.com/news/ANM_G2_CODES_2.xml";
            newsItem.CatalogRef.Add(catalogRef);

            // Adding rightsInfo - bubble up
            CopyrightHolderElm copyrightHolder = new CopyrightHolderElm();
            copyrightHolder.Uri = "http://www.acmenews.com/about.html#copyright";
            name = new ConceptNameTypeCt();
            name.Value = "Acme News and Media LLC";
            copyrightHolder.Name.Add(name);
            CopyrightNoticeElm copyrightNotice = new CopyrightNoticeElm();
            copyrightNotice.Add("Copyright 2016-17 Acme News and Media LLC");
            RightsInfoTypeCt rightsInfo = new RightsInfoTypeCt();
            rightsInfo.CopyrightHolder = copyrightHolder;
            rightsInfo.CopyrightNotice.Add(copyrightNotice);
            newsItem.RightsInfo.Add(rightsInfo);

            // Adding itemMeta properties
            newsItem.ItemMeta.ItemClass.Qcode = "ninat:text";
            // pubStatus moved up
            QualPropTypeCt pubStatus = new QualPropTypeCt();
            pubStatus.Qcode = "stat:usable";
            newsItem.ItemMeta.PubStatus = pubStatus;
            // provider moved up
            newsItem.ItemMeta.Provider.Uri = "http://www.acmenews.com/about/";
            lxdt = LxDateTime.CreateDateTime(2017, 11, 21, 16, 25, 32, 0, 0, 0, -5, 0);
            newsItem.ItemMeta.VersionCreated.Value = lxdt;

            // Adding contentSet --- moved prior to setting contentMeta
            ContentSetElm contentSet = new ContentSetElm();

            // (create the XML tree outside the NewsML-G2 schema - in the IPTC NITF namespace
            XNamespace nitfNs = "http://iptc.org/std/NITF/2006-10-18/";
            XElement nitf = new XElement(nitfNs + "nitf",
                new XElement(nitfNs + "body",
                    new XElement(nitfNs + "body.head",
                        new XElement(nitfNs + "hedline",
                            new XElement(nitfNs + "hl1", new XText("Fed to halt QE to avert \"bubble\""))
                        ),
                        new XElement(nitfNs + "byline", new XText("By Meredith Jameson, Staff Reporter"))
                    ),
                    new XElement(nitfNs + "body.content",
                      new XElement(nitfNs + "p", new XText("(New York, NY - October 21) Et, sent luptat luptat, commy Nim zzriureet vendreetue modo etc")),
                      new XElement(nitfNs + "p", new XText("Ugiating ea feugait utat, venim velent nim quis nulluptat num Volorem inci enim dolobor eetuer sendre ercin utpatio dolorpercing"))
                    )
                )
            );

            ContentSetElm.InlineXMLElm inlineXml = new ContentSetElm.InlineXMLElm();
            inlineXml.AnyElement = nitf;
            contentSet.InlineXML.Add(inlineXml);
            newsItem.ContentSet = contentSet;

            // Adding contentMeta properties
            ContentMetadataAfDTypeCt contentMeta = new ContentMetadataAfDTypeCt(); // = the wrapper

            if (LxDateTime.TryParseXSDDateTime("2016-11-21T15:21:06-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentCreated = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentCreated.Value = dtvaltdtt;
                contentMeta.ContentCreated = contentCreated;
            }

            if (LxDateTime.TryParseXSDDateTime("2017-11-21T16:22:45-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentModified = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentModified.Value = dtvaltdtt;
                contentMeta.ContentModified = contentModified;
            }

            FlexLocationPropTypeCt located = new FlexLocationPropTypeCt();
            located.Qcode = "geoloc:NYC";
            name = new ConceptNameTypeCt();
            name.Value = "New York, NY";
            located.Name.Add(name);
            contentMeta.Located.Add(located);

            FlexAuthorPropTypeCt creator = new FlexAuthorPropTypeCt();
            creator.Uri = "http://www.acmenews.com/staff/mjameson";
            name = new ConceptNameTypeCt();
            name.Value = "Meredith Jameson";
            creator.Name.Add(name);
            contentMeta.Creator1.Add(creator); // .Creator = @creator, .Creator1 = /creator

            Flex1PartyPropTypeCt infoSource = new Flex1PartyPropTypeCt();
            infoSource.Qcode = "is:AP";
            name = new ConceptNameTypeCt();
            name.Value = "Associcate Press";
            infoSource.Name.Add(name);
            contentMeta.InfoSource.Add(infoSource);

            LanguageElm language = new LanguageElm();
            language.Tag = "en-US";
            contentMeta.Language.Add(language);

            // a sequence of subject elements
            SubjectElm subject = new SubjectElm();
            subject.Qcode = "medtop:04000000";
            name = new ConceptNameTypeCt();
            name.Value = "economy, business and finance";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            subject = new SubjectElm();
            subject.Qcode = "medtop:20000350";
            name = new ConceptNameTypeCt();
            name.Value = "central bank";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            subject = new SubjectElm();
            subject.Qcode = "medtop:20000379";
            name = new ConceptNameTypeCt();
            name.Value = "money and monetary policy";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            SluglineElm slugline = new SluglineElm();
            slugline.Value = "US-Finance-Fed";
            contentMeta.Slugline.Add(slugline);

            HeadlineElm headline = new HeadlineElm();
            headline.Value = "Fed to halt QE to avert \"bubble\"";
            contentMeta.Headline.Add(headline);

            // finally: add the filled contentMeta instance to the newsItem
            newsItem.ContentMeta = contentMeta;


            // final action of the test procedure: serialized the XML document and create an output
            LxSerializer<NewsItemElm> serializer1 = new LxSerializer<NewsItemElm>();
            using (StringWriter sw = new StringWriter())
            {
                serializer1.Serialize(sw, newsItem);
                Console.WriteLine(sw.ToString());
            }
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(@"..\..\NewsML-G2-testresults", "test02-LISTING_2-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test02-LISTING_2-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test02 closes");

        } // Test02

        /// <summary>
        /// Generates a NewsItem based on the reference example LISTING_2-modPower_NewsML-G2_Text_Document.xml.
        /// It tests adding metadata properties to a wrapper after having added the wrapper to the node tree.
        /// </summary>
        private static void Test03()
        {
            // Generic variables
            Exception ex1;
            LxDateTime lxdt;
            ConceptNameTypeCt name;
            TruncatedDateTimeTypeUnion dtvaltdtt;

            NewsItemElm newsItem = new NewsItemElm();
            newsItem.Standard = "NewsML-G2";
            newsItem.Standardversion = "2.27";
            newsItem.Guid = "urn:newsml:acmenews.com:20161121:US-FINANCE-FED";
            newsItem.Version = 11;
            newsItem.Conformance = "power";

            // Adding 2 catalogRef-s:
            CatalogRefElm catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://www.iptc.org/std/catalog/catalog.IPTC-G2-Standards_30.xml";
            newsItem.CatalogRef.Add(catalogRef);
            catalogRef = new CatalogRefElm();
            catalogRef.Href = "http://catalog.acmenews.com/news/ANM_G2_CODES_2.xml";
            newsItem.CatalogRef.Add(catalogRef);

            // Adding rightsInfo - bubble up
            CopyrightHolderElm copyrightHolder = new CopyrightHolderElm();
            copyrightHolder.Uri = "http://www.acmenews.com/about.html#copyright";
            name = new ConceptNameTypeCt();
            name.Value = "Acme News and Media LLC";
            copyrightHolder.Name.Add(name);
            CopyrightNoticeElm copyrightNotice = new CopyrightNoticeElm();
            copyrightNotice.Add("Copyright 2016-17 Acme News and Media LLC");
            RightsInfoTypeCt rightsInfo = new RightsInfoTypeCt();
            rightsInfo.CopyrightHolder = copyrightHolder;
            rightsInfo.CopyrightNotice.Add(copyrightNotice);
            newsItem.RightsInfo.Add(rightsInfo);

            // Adding itemMeta properties
            newsItem.ItemMeta.ItemClass.Qcode = "ninat:text";
            lxdt = LxDateTime.CreateDateTime(2017, 11, 21, 16, 25, 32, 0, 0, 0, -5, 0);
            newsItem.ItemMeta.VersionCreated.Value = lxdt;
            newsItem.ItemMeta.Provider.Uri = "http://www.acmenews.com/about/";

            QualPropTypeCt pubStatus = new QualPropTypeCt();
            pubStatus.Qcode = "stat:usable";
            newsItem.ItemMeta.PubStatus = pubStatus;

            // Adding contentMeta properties
            ContentMetadataAfDTypeCt contentMeta = new ContentMetadataAfDTypeCt(); // = the wrapper

            if (LxDateTime.TryParseXSDDateTime("2016-11-21T15:21:06-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentCreated = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentCreated.Value = dtvaltdtt;
                contentMeta.ContentCreated = contentCreated;
            }

            if (LxDateTime.TryParseXSDDateTime("2017-11-21T16:22:45-05:00", out lxdt, out ex1))
            {
                TruncatedDateTimePropTypeCt contentModified = new TruncatedDateTimePropTypeCt();
                dtvaltdtt = new TruncatedDateTimeTypeUnion(lxdt);
                contentModified.Value = dtvaltdtt;
                contentMeta.ContentModified = contentModified;
            }

            FlexLocationPropTypeCt located = new FlexLocationPropTypeCt();
            located.Qcode = "geoloc:NYC";
            name = new ConceptNameTypeCt();
            name.Value = "New York, NY";
            located.Name.Add(name);
            contentMeta.Located.Add(located);

            FlexAuthorPropTypeCt creator = new FlexAuthorPropTypeCt();
            creator.Uri = "http://www.acmenews.com/staff/mjameson";
            name = new ConceptNameTypeCt();
            name.Value = "Meredith Jameson";
            creator.Name.Add(name);
            contentMeta.Creator1.Add(creator); // .Creator = @creator, .Creator1 = /creator

            Flex1PartyPropTypeCt infoSource = new Flex1PartyPropTypeCt();
            infoSource.Qcode = "is:AP";
            name = new ConceptNameTypeCt();
            name.Value = "Associcate Press";
            infoSource.Name.Add(name);
            contentMeta.InfoSource.Add(infoSource);

            LanguageElm language = new LanguageElm();
            language.Tag = "en-US";
            contentMeta.Language.Add(language);

            // only 1 subject element
            SubjectElm subject = new SubjectElm();
            subject.Qcode = "medtop:04000000";
            name = new ConceptNameTypeCt();
            name.Value = "economy, business and finance";
            subject.Name.Add(name);
            contentMeta.Subject.Add(subject);

            SluglineElm slugline = new SluglineElm();
            slugline.Value = "US-Finance-Fed";
            contentMeta.Slugline.Add(slugline);

            HeadlineElm headline = new HeadlineElm();
            headline.Value = "Fed to halt QE to avert \"bubble\"";
            contentMeta.Headline.Add(headline);

            // add the filled contentMeta instance to the newsItem
            newsItem.ContentMeta = contentMeta;

            // Adding an empty contentSet wrapper
            ContentSetElm contentSet = new ContentSetElm();

            // 2nd round of metadata activities:
            // remove a subect from contentMeta
            newsItem.ContentMeta.Subject.RemoveAt(0);
            // Now add contentMeta properties
            subject = new SubjectElm();
            subject.Qcode = "medtop:20000350";
            name = new ConceptNameTypeCt();
            name.Value = "central bank";
            subject.Name.Add(name);
            newsItem.ContentMeta.Subject.Add(subject);

            subject = new SubjectElm();
            subject.Qcode = "medtop:20000379";
            name = new ConceptNameTypeCt();
            name.Value = "money and monetary policy";
            subject.Name.Add(name);
            newsItem.ContentMeta.Subject.Add(subject);

            // Now add itemMeta properties
            BlockTypeCt edNote = new BlockTypeCt();
            edNote.Value = "Hello, dear NewsML-G2 testers. We hope you enjoy the results.";

            newsItem.ItemMeta.EdNote.Add(edNote);

            // final action of the test procedure: serialized the XML document and create an output
            LxSerializer<NewsItemElm> serializer1 = new LxSerializer<NewsItemElm>();
            using (StringWriter sw = new StringWriter())
            {
                serializer1.Serialize(sw, newsItem);
                Console.WriteLine(sw.ToString());
            }
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(@"..\..\NewsML-G2-testresults", "test03-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test03-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test03 closes");

        } // Test03


    }
}
