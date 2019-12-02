/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Configuration;

namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Represents the configuration for <see cref="Connector"/>
    /// </summary>
    [Name("Connector")]
    public class ConnectorConfiguration : IConfigurationObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConnectorConfiguration"/>
        /// </summary>
        /// <param name="ip">The IP address for the connector</param>
        /// <param name="port">The Port to connect to</param>
        /// <param name="mqttclient">MQTT Client true/false</param>

         public ConnectorConfiguration(string ip, int port, bool mqttclient)
        {
            Ip = ip;
            Port = port;
            MQTTClient = mqttclient;
        }

        /// <summary>
        /// Gets the Ip address that will be used for connecting
        /// </summary>
        public string Ip { get; }

        /// <summary>
        /// Gets the port that will be used for connecting
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets the if the connector should use MQTT Client.
        /// </summary>
        public bool MQTTClient { get; }

    }
}