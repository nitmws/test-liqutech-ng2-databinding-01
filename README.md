# test-liqutech-ng2-databinding-01

This is a Visual Studio (2017) Console App for testing the [Liquid Technologies Liquid XML Objects](https://www.liquid-technologies.com/xml-objects) package for the [NewsML-G2 version 2.27 schema](https://www.liquid-technologies.com/Reference/xml-objects/library/NewsML-Power/2-27).

This package is based on the XML schema of the [IPTC standard NewsML-G2](https://iptc.org/standards/newsml-g2/), in particular on [version 2.27](https://www.iptc.org/std/NewsML-G2/2.27/specification/NewsML-G2_2.27-spec-All-Power.xsd)

The basic layout of the app is to wrap different test proceudures.

## The Tests

The major goal of the tests is to check how items or news messages as defined by IPTC's NewsML-G2 can be created with this 

The tests are structured as different test procedures, named Test.. with a 2 digit number starting at 01. Each one can be launched by a number greater than 0 as the only command line parameter of the app.

The source code includes many comments on generating the NewsML-G2 document.

### Test01

Test01 uses a simple NewsML-G2 NewsItem example provided with NewsML-G2 version 2.27: [LISTING_2](https://www.iptc.org/std/NewsML-G2/2.27/examples/LISTING_2_NewsML-G2_Text_Document.xml). 
It was slightly modified to be at the "power" conformance level. See the corresponding reference file for this test LISTING_2-modPower XML document (in the NewsML-G2-referenceExamples folder of this respository).

Test01 generates an XML document with the same content as the reference file. (As many elements don't have a fixed sequence in the XML schema they appear in a different order as in the reference file.)
The only exception is minimal content in the child element nitf of the element inlineXML.

See the results in the file test01-LISTING_2-generated.xml (in the NewsML-G2-testresults folder of this repository).

### Test02

Test02 is based on the same example NewsItem as Test01. 

The code of Test02 generates the same document but in a different sequence of elements. Of interest is if explicitly set sequences as defined by the XML Schema are delived in the serialized XML document. The test result is positive: the itemClass, provider, versionCreated and pubStatus element are delived in the specified sequence, the contentSet wrapper is the last element at the second level of the document.

See the results in the file test02-LISTING_2-generated.xml (in the NewsML-G2-testresults folder of this repository). Comparing this XML document with the one created by Test01 shows no difference between them.

### Test03

Test03 is based on the same example NewsItem as Test01. 

Test03 tests if and how metadata inside a wrapper like itemMeta and contentMeta can be added after an element created as wrapper and getting some child elements added was added as child to the root element. Also removing an existing metadata property was tested. The test result is positive: properties can be added to the itemMeta and contentMeta wrapper "later", the removed element is not present in the serialized XML document.

See the results in the file test03-generated.xml (in the NewsML-G2-testresults folder of this repository). 

