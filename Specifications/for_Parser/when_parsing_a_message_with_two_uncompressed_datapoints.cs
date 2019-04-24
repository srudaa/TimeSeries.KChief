/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;

namespace Dolittle.Edge.KChief.for_Parser
{
    [Subject(typeof(Parser))]
    public class when_parsing_a_message_with_two_uncompressed_datapoints : given.a_parser
    {
        Because of = () =>
        {
            var data = new byte[]
            {
                0x08, 0x01, // Library version
                0x18, 0x00, // Not compressed
                0x22, 0x67, // Message is 103 big

                0x0a, 0x32, // Payload of size 50
                0x1a, 0x30, 0x0a, 0x14, 0x43, 0x36, 0x30, 0x30, 0x2e, 0x54,
                0x61, 0x67, 0x2e, 0x43, 0x59, 0x4c, 0x30, 0x32, 0x5f, 0x42,
                0x57, 0x43, 0x2d, 0x32, 0x12, 0x18, 0x0a, 0x0c, 0x08, 0xf3,
                0xd3, 0x9b, 0xe5, 0x05, 0x10, 0xec, 0x86, 0xfa, 0xbc, 0x01,
                0x12, 0x08, 0xd9, 0xce, 0xf7, 0x53, 0xe3, 0xa5, 0xab, 0xbf,
                
                0x0a, 0x31, // Payload of size 49
                0x1a, 0x2f, 0x0a, 0x13, 0x43, 0x36, 0x30, 0x30, 0x2e, 0x54,
                0x61, 0x67, 0x2e, 0x46, 0x55, 0x45, 0x4c, 0x20, 0x49, 0x4e,
                0x44, 0x45, 0x58, 0x12, 0x18, 0x0a, 0x0c, 0x08, 0xf3, 0xd3,
                0x9b, 0xe5, 0x05, 0x10, 0xec, 0x86, 0xfa, 0xbc, 0x01, 0x12,
                0x08, 0x9a, 0x99, 0x99, 0x99, 0x99, 0x59, 0x52, 0x40,
            };

            parser.Parse(data, datapoints.Add);
        };

        It should_add_two_datapoints = () => datapoints.Count.ShouldEqual(2);
        It should_have_decoded_the_first_tagname_correctly = () => datapoints[0].Tag.Value.ShouldEqual("CYL02_BWC-2");
        It should_have_decoded_the_first_timestamp_correctly = () => datapoints[0].Timestamp.Value.ShouldEqual(1554442739396);
        It should_have_decoded_the_first_value_correctly = () => datapoints[0].Value.ShouldEqual(-0.054);
        It should_have_decoded_the_second_tagname_correctly = () => datapoints[1].Tag.Value.ShouldEqual("FUEL INDEX");
        It should_have_decoded_the_second_timestamp_correctly = () => datapoints[1].Timestamp.Value.ShouldEqual(1554442739396);
        It should_have_decoded_the_second_value_correctly = () => datapoints[1].Value.ShouldEqual(73.4);
        It should_have_logged_no_errors = () => errors.Count.ShouldEqual(0);
    }
}