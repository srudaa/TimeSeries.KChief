/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;

namespace Dolittle.Edge.KChief.for_Parser
{
    [Subject(typeof(Parser))]
    public class when_parsing_garbage : given.a_parser
    {
        Because of = () =>
        {
            var data = new byte[]
            {
                0x07, // Not a valid protobuf key
            };

            parser.Parse(data, datapoints.Add);
        };

        It should_add_no_datapoints = () => datapoints.Count.ShouldEqual(0);
        It should_have_logged_no_errors = () => errors.Count.ShouldBeGreaterThanOrEqualTo(1);
    }
}