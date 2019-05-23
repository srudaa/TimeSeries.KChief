/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;

namespace Dolittle.TimeSeries.KChief.for_Parser
{
    [Subject(typeof(Parser))]
    public class when_parsing_a_message_with_one_compressed_datapoint : given.a_parser
    {
        Because of = () =>
        {
            var data = new byte[]
            {
                0x08, 0x01, // Library version
                0x18, 0x01, // Compressed
                0x22, 0x53, // Message is 83 big

                0x1f, 0x8b, 0x08, 0x08, 0xc9, 0x65, 0xc0, 0x5c, 0x00, 0x03,
                0x66, 0x69, 0x72, 0x73, 0x74, 0x2e, 0x62, 0x69, 0x6e, 0x00,
                0xe3, 0x32, 0x92, 0x32, 0xe0, 0x12, 0x71, 0x36, 0x33, 0x30,
                0xd0, 0x0b, 0x49, 0x4c, 0xd7, 0x73, 0x8e, 0xf4, 0x31, 0x30,
                0x8a, 0x77, 0x0a, 0x77, 0xd6, 0x35, 0x12, 0x92, 0xe0, 0xe2,
                0xe1, 0xf8, 0x7c, 0x79, 0xf6, 0x53, 0x56, 0x81, 0x37, 0x6d,
                0xbf, 0xf6, 0x30, 0x0a, 0x71, 0xdc, 0x3c, 0xf7, 0x3d, 0xf8,
                0xf1, 0xd2, 0xd5, 0xfb, 0x01, 0x5d, 0x63, 0xb5, 0xc9, 0x34,
                0x00, 0x00, 0x00,
            };

            parser.Parse(data, datapoints.Add);
        };

        It should_add_one_datapoint = () => datapoints.Count.ShouldEqual(1);
        It should_have_decoded_the_tagname_correctly = () => datapoints[0].Tag.Value.ShouldEqual("CYL02_BWC-2");
        It should_have_decoded_the_timestamp_correctly = () => datapoints[0].Timestamp.Value.ShouldEqual(1554442739396);
        It should_have_decoded_the_value_correctly = () => datapoints[0].Value.ShouldEqual(-0.054);
        It should_have_logged_no_errors = () => errors.Count.ShouldEqual(0);
    }
}