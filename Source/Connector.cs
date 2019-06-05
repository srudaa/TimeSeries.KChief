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
using Dolittle.TimeSeries.Modules;
using Dolittle.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Dolittle.Configuration;

namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Represents an implementation for <see cref="IConnector"/>
    /// </summary>
    public class Connector : IConnector
    {
        readonly ConnectorConfiguration _configuration;
        readonly ILogger _logger;
        readonly IParser _parser;
        readonly ConcurrentBag<Action<TagDataPoint<double>>> _subscribers;
        
        /// <summary>
        /// Initializes a new instance of <see cref="Connector"/>
        /// </summary>
        /// <param name="configuration"><see cref="ConnectorConfiguration">Configuration</see></param>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        /// <param name="parser"><see cref="IParser"/> for dealing with the actual parsing</param>
        public Connector(ConnectorConfiguration configuration, ILogger logger, IParser parser)
        {
            _configuration = configuration;
            _logger = logger;
            _parser = parser;
            _subscribers = new ConcurrentBag<Action<TagDataPoint<double>>>();
            _logger.Information($"Will connect to '{configuration.Ip}:{configuration.Port}'");
        }


        /// <inheritdoc/>
        public void Start()
        {
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
            _parser.Parse(eventArgs.ApplicationMessage.Payload, (tagDataPoint) => {
                _subscribers.ForEach(_ => _(tagDataPoint));
            });
        }

        /// <inheritdoc/>
        public void Subscribe(Action<TagDataPoint<double>> subscriber)
        {
            _subscribers.Add(subscriber);
        }
    }
}