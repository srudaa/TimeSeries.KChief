/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Dolittle.Edge.Modules;
using Dolittle.Logging;
using Dolittle.Edge.KChief.Protobuf;
using System.IO.Compression;

namespace Dolittle.Edge.KChief
{
    /// <summary>
    /// Represents an implementation of <see cref="IParser"/>
    /// </summary>
    public class Parser : IParser
    {
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="ILogger"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> to use for logging</param>
        public Parser(ILogger logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void Parse(byte[] data, Action<TagDataPoint<double>> callback)
        {
            try
            {
                var message = Message.Parser.ParseFrom(data);
                
                Stream stream = new MemoryStream(message.Data.ToByteArray());
                if (message.Compressed) stream = new GZipStream(stream, CompressionMode.Decompress);

                using (stream)
                {
                    var payloads = Payloads.Parser.ParseFrom(stream);

                    foreach (var payload in payloads.Payloads_)
                    {
                        foreach (var tagDataPoint in payload.Tagdatapoints)
                        {
                            foreach (var dataPoint in tagDataPoint.Datapoints)
                            {
                                callback(new TagDataPoint<double>{
                                    ControlSystem = "KChief",
                                    Tag = tagDataPoint.Tag.ToLower().StartsWith("c600.tag.") ? tagDataPoint.Tag.Substring(9) : tagDataPoint.Tag,
                                    Timestamp = dataPoint.Timestamp.Seconds*1000 + dataPoint.Timestamp.Nanos/1000000,
                                    Value = dataPoint.Value[0],
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while parsing payload");
            }
        }
    }
}