/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
namespace Dolittle.TimeSeries.KChief
{
    /// <summary>
    /// Defines the connector that connects to KChief system
    /// </summary>
    public interface ICoordinator
    {
        /// <summary>
        /// Initializes the connector
        /// </summary>
        void Initialize();
    }
}