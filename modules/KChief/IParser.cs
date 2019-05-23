/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using Dolittle.TimeSeries.Modules;

namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Defines the parser that is capable of parsing the data coming from KChief
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parse a binary message consisting of multiple datapoints
        /// </summary>
        /// <param name="data">Binary message</param>
        /// <param name="callback">Callback for each parsed datapoint</param>
        void Parse(byte[] data, Action<TagDataPoint<double>> callback);
    }
}