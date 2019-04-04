/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Collections;
using Dolittle.Edge.Modules;
using Dolittle.Logging;

namespace Dolittle.Edge.KChief
{
    /// <summary>
    /// Represents an implementation for <see cref="IConnector"/>
    /// </summary>
    public class Connector : IConnector
    {
        readonly ILogger _logger;
        readonly IParser _parser;
        readonly ConcurrentBag<Action<Channel>> _subscribers;

        /// <summary>
        /// Initializes a new instance of <see cref="Connector"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        /// <param name="parser"><see cref="IParser"/> for dealing with the actual parsing</param>
        public Connector(ILogger logger, IParser parser)
        {
            _logger = logger;
            _parser = parser;
            _subscribers = new ConcurrentBag<Action<Channel>>();
        }

        /// <inheritdoc/>
        public void Start()
        {
        }

        /// <inheritdoc/>
        public void Subscribe(Action<Channel> subscriber)
        {
            _subscribers.Add(subscriber);
        }
    }
}