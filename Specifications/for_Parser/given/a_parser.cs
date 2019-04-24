/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using Dolittle.Logging;
using Dolittle.Edge.Modules;
using Machine.Specifications;
using Moq;
using System.Runtime.CompilerServices;

namespace Dolittle.Edge.KChief.for_Parser.given
{
    public abstract class a_parser : given.a_logger
    {
        protected static Parser parser;
        protected static List<TagDataPoint<double>> datapoints;

        Establish context = () =>
        {
            datapoints = new List<TagDataPoint<double>>();
            parser = new Parser(logger);
        };
    }
}