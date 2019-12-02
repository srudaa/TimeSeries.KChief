/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.TimeSeries.Modules;
using Dolittle.Logging;
using MQTTnet;
using Dolittle.TimeSeries.Modules.Connectors;
using MQTTnet.Server;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Client;
using System;
using MQTTnet.Protocol;

namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Represents an implementation for <see cref="IAmAStreamingConnector"/>
    /// </summary>
    public class Connector : IAmAStreamingConnector
    {
        /// <inheritdoc/>
        public event DataReceived DataReceived = (tag, ValueTask, timestamp) => { };
        readonly ILogger _logger;
        readonly IParser _parser;
        readonly ConnectorConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of <see cref="Connector"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        /// <param name="parser"><see cref="IParser"/> for dealing with the actual parsing</param>
        /// <param name="configuration"><see cref="ConnectorConfiguration"/> holding all configuration</param>
        public Connector(ILogger logger, IParser parser, ConnectorConfiguration configuration)
        {
            _logger = logger;
            _parser = parser;
            _configuration = configuration;
            _logger.Information($"Will expose MQTT");

        }

        /// <inheritdoc/>
        public Source Name => "KChief";

        /// <inheritdoc/>
        public void Connect()
        {
             switch (_configuration.MQTTClient)
            {
                case true: ConnectMQTTClient(); break;
                case false: ConnectMQTTBroker(); break;
                default: _logger.Error("MQTTClient not defined"); break;
            }
            
        }

        void ConnectMQTTBroker()
        {
            _logger.Information("Starting MQTT Broker");

            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointPort(1883);
            var options = optionsBuilder.Build();

            var server = new MqttFactory().CreateMqttServer();
            server.ClientConnected += (s, e) => _logger.Information($"Client {e.ClientId} connected");
            server.ClientDisconnected += (s, e) => _logger.Information($"Client {e.ClientId} disconnected");
            server.ApplicationMessageReceived += MessageReceived;
            server.StartAsync(options);
        }

        void ConnectMQTTClient()
        {
            _logger.Information("Starting MQTT Client");
          
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("DolittleEdgeModule")
                    .WithTcpServer(_configuration.Ip, _configuration.Port)
                    .Build()
                )
                .Build();
            
            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            mqttClient.ApplicationMessageReceived += MessageReceived;
            mqttClient.SubscribeAsync("CloudBoundContainer", MqttQualityOfServiceLevel.AtLeastOnce).Wait();
            mqttClient.StartAsync(options);
        }
        void MessageReceived(object sender, MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            _logger.Information($"Received MQTT Message on topic '{eventArgs.ApplicationMessage.Topic}'");
            _parser.Parse(eventArgs.ApplicationMessage.Payload, (tagDataPoint) =>
            {
                DataReceived(tagDataPoint.Tag, tagDataPoint.Value, tagDataPoint.Timestamp);
            });
        }
    }
}