/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Edge.Modules;

namespace Dolittle.Edge.KChief
{
    /// <summary>
    /// Defines the connector that is responsible for connecting and keeping it with KChief
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Starts the connector to begin streaming data
        /// </summary>
        void Start();

        /// <summary>
        /// Subscribe to <see cref="Channel"/> values coming
        /// </summary>
        /// <param name="subscriber">The subscriber method</param>
        void Subscribe(Action<TagDataPoint<double>> subscriber);
    }
}