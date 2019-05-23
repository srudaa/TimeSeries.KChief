/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;

namespace Dolittle.TimeSeries.KChief.for_Parser
{
    [Subject(typeof(Parser))]
    public class when_parsing_a_message_without_any_data : given.a_parser
    {
        Because of = () =>
        {
            var data = new byte[]
            {
                0x08, 0x01, // Library version
                0x18, 0x01, // Compressed
                0x22, 0x00, // Zero-length data
            };

            parser.Parse(data, datapoints.Add);
        };

        It should_add_no_datapoints = () => datapoints.Count.ShouldEqual(0);
        It should_have_logged_no_errors = () => errors.Count.ShouldEqual(0);
    
    }
}