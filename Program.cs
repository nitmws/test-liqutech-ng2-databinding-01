﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using LiquidTechnologies.XmlObjects;
using LiquidTechnologies.XmlObjects.NewsMLPower227;
using LiquidTechnologies.XmlObjects.NewsMLPower227.NsA;
using LiquidTechnologies.XmlObjects.NewsMLPower227.TnsA;
using LiquidTechnologies.XmlObjects.NewsMLPower227.Tns;
using LiquidTechnologies.XmlObjects.NewsMLPower227.Xs;

namespace test_liqutech_ng2_databinding_01
{
    class Program
    {
        private static int defaultTestno = 1;
        private static string testOutputDir =  @"..\..\NewsML-G2-testresults"; 

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
                    case 4:
                        Test04();
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
        /// The sequence of generating nodes/elements follows the sequence in the example XML document.
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
            name.Value = "Associcated Press";
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
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(testOutputDir, "test01-LISTING_2-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test01-LISTING_2-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test01 closes");

        } // Test01

        /// <summary>
        /// Generates a NewsItem complying to the reference example LISTING_2-modPower_NewsML-G2_Text_Document.xml.
        /// The sequence of generating nodes/elements DOES NOT follow the sequence in the example XML document.
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
            name.Value = "Associcated Press";
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
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(testOutputDir, "test02-LISTING_2-generated.xml")))
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
            name.Value = "Associcated Press";
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

            // TEST: 2nd round of metadata activities:
            // remove a subject from contentMeta
            newsItem.ContentMeta.Subject.RemoveAt(0);
            // TEST: Now add contentMeta properties
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

            // TEST: Now add itemMeta properties
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
            using (StreamWriter outputSw = new StreamWriter(Path.Combine(testOutputDir, "test03-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test03-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test03 closes");

        } // Test03

        /// <summary>
        /// Generates a NewsItem based on the reference example LISTING_2-modPower_NewsML-G2_Text_Document.xml.
        /// It tests a) adding element from an "other" namespace to itemMeta and contentMeta
        /// and b) adding the <itemClass> element to the contentMeta where it must not exist by the NewsML-G2 XML schema.
        /// </summary>
        private static void Test04()
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

            // TEST: create an element in an "other" namespace and add it to the extension point of itemMeta
            XNamespace specdNs = "http://example.org/ns/specialdata01";
            XElement specdAlarm1 = new XElement(specdNs + "alarm", new XText("This news item should be published instantly"));
            newsItem.ItemMeta.AnyElement.Add(specdAlarm1);

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


            HeadlineElm headline = new HeadlineElm();
            headline.Value = "Testing, testing, testing";
            contentMeta.Headline.Add(headline);

            // TEST: create an element in an "other" namespace and add it to the extension point of contentMeta
            XNamespace addldNs = "http://example.org/ns/additionaldata01";
            XElement addldHeadline = new XElement(addldNs + "ourHeadline", new XText("Look at this special news item"));
            contentMeta.AnyElement.Add(addldHeadline);

            // TEST: create an element defined as child of itemMeta and try to add it to the extension point of contentMeta
            // Attempt 1:
            ItemClassElm itemClassWrong1 = new ItemClassElm();
            itemClassWrong1.Qcode = "blabla:anything";
            // contentMeta.AnyElement.Add(itemClassWrong1); // this line of code cannot be compiled, wrong data type

            // Attempt 2: build the <itemClass> as generic XElement
            XNamespace narNs = "http://iptc.org/std/nar/2006-10-01/";
            XElement itemClassWrong2 = new XElement(narNs + "itemClass", new XAttribute("qcode","blabla:anything"));
            // the line of code below generates an <itemClass> element as child of <contentMeta>: this is invalid by the NewsML-G2 schema
            contentMeta.AnyElement.Add(itemClassWrong2); 

            // finally: add the filled contentMeta instance to the newsItem
            newsItem.ContentMeta = contentMeta;

            // Adding an empty contentSet
            ContentSetElm contentSet = new ContentSetElm();
            newsItem.ContentSet = contentSet;

            // final action of the test procedure: serialized the XML document and create an output
            LxSerializer<NewsItemElm> serializer1 = new LxSerializer<NewsItemElm>();
            using (StringWriter sw = new StringWriter())
            {
                serializer1.Serialize(sw, newsItem);
                Console.WriteLine(sw.ToString());
                // ValidateNG2newsItem(sw.ToString()); // calling it throws an exception
            }

            using (StreamWriter outputSw = new StreamWriter(Path.Combine(testOutputDir, "test04-generated.xml")))
            {
                serializer1.Serialize(outputSw, newsItem);
                Console.WriteLine("!! Test results file test04-generated.xml was generated");
            }
            Console.WriteLine("!!!!! Test04 closes");

        } // Test04

        private static void ValidateNG2newsItem(string serializedNG2)
        {
            // executing the line below throws an exeption: the XsdValidator needs more data (maybe of the XML Schema)
            LiquidTechnologies.XmlObjects.NewsMLPower227.XsdValidator validator = new LiquidTechnologies.XmlObjects.NewsMLPower227.XsdValidator();
            using (XmlReader validatingReader = validator.CreateValidatingReader(serializedNG2, ValidatingReaderErrorHandler))
            {
                LxSerializer<NewsItemElm> serializer = new LxSerializer<NewsItemElm>();
                LxReaderSettings lxReaderSettings = new LxReaderSettings()
                {
                    ErrorHandler = LxErrorHandler
                };
                NewsItemElm newsItemElm = serializer.Deserialize(validatingReader, lxReaderSettings);
                
            }

        }
   
        private static void ValidatingReaderErrorHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($".Net XSD Validator : {e.Severity} : {e.Message}");
        }

        private static void LxErrorHandler(string msg, LxErrorSeverity severity, LxErrorCode errorCode, TextLocation location, object targetObject)
        {
            Console.WriteLine($"Liquid XML Objects Validator : {severity} : {msg}");
        }
    }
}
