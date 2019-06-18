/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.TimeSeries.Modules;
using Dolittle.Logging;
using MQTTnet;
using Dolittle.TimeSeries.Modules.Connectors;
using MQTTnet.Server;

namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Represents an implementation for <see cref="IAmAStreamingConnector"/>
    /// </summary>
    public class Connector : IAmAStreamingConnector
    {
        /// <inheritdoc/>
        public event DataReceived DataReceived = (tag, ValueTask, timestamp) => {};
        readonly ILogger _logger;
        readonly IParser _parser;
        
        /// <summary>
        /// Initializes a new instance of <see cref="Connector"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        /// <param name="parser"><see cref="IParser"/> for dealing with the actual parsing</param>
        public Connector(ILogger logger, IParser parser)
        {
            _logger = logger;
            _parser = parser;
            _logger.Information($"Will expose MQTT");
        }

        /// <inheritdoc/>
        public Source Name => "KChief";

        /// <inheritdoc/>
        public void Connect()
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(1883);
            var options = optionsBuilder.Build();

            var server = new MqttFactory().CreateMqttServer();
            server.ClientConnected += (s,e) => _logger.Information($"Client {e.ClientId} connected");
            server.ClientDisconnected += (s,e) => _logger.Information($"Client {e.ClientId} disconnected");
            server.ApplicationMessageReceived += MessageReceived;
            server.StartAsync(options);
        }

        void MessageReceived(object sender, MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            _logger.Information($"Received MQTT Message on topic '{eventArgs.ApplicationMessage.Topic}'");
            _parser.Parse(eventArgs.ApplicationMessage.Payload, (tagDataPoint) => {
                DataReceived(tagDataPoint.Tag, tagDataPoint.Value, tagDataPoint.Timestamp);
            });
        }
    }
}