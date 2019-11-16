# test-liqutech-ng2-databinding-01

This is a Visual Studio (2017) Console App for testing the Liquid Technologies Liquid XML Objects [package for the NewsML-G2 version 2.27 schema](https://www.liquid-technologies.com/Reference/xml-objects/library/NewsML-Power/2-27).

This package is based on the XML schema of the [IPTC standard NewsML-G2](https://iptc.org/standards/newsml-g2/), in particular on [version 2.27](https://www.iptc.org/std/NewsML-G2/2.27/specification/NewsML-G2_2.27-spec-All-Power.xsd)

The basic layout of the app is to wrap different tests. (Currently only Test01 is implemented and made the default one.)

## The Tests

### test01

test01 uses a simple NewsML-G2 NewsItem example provided with NewsML-G2 version 2.27: [LISTING_2](https://www.iptc.org/std/NewsML-G2/2.27/examples/LISTING_2_NewsML-G2_Text_Document.xml). 
It was slightly modified to be at the "power" conformance level. See the corresponding reference file for this test LISTING_2-modPower XML document (in the NewsML-G2-referenceExamples folder of this respository).

test01 generates an XML document with the same content as the reference file. (As many elements don't have a fixed sequence in the XML schema they appear in a different order as in the reference file.)
The only exception is minimal content in the child element nitf of the element inlineXML.

See the results in the file LISTING_2-generated.xml (in the NewsML-G2-testresults folder of this repository).



