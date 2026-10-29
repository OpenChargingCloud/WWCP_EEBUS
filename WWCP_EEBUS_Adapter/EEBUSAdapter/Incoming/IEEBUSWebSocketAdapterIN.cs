/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP EEBUS <https://github.com/OpenChargingCloud/WWCP_EEBUS>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;

#endregion

namespace cloud.charging.open.protocols.EEBUS
{

    /// <summary>
    /// The common interface of all EEBUS adapters for processing incoming messages.
    /// </summary>
    public interface IEEBUSWebSocketAdapterIN
    {

        #region Properties

        /// <summary>
        /// The networking node identifications this node also answers to.
        /// </summary>
        HashSet<NetworkingNode_Id>  AnycastIds    { get; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a binary request was received.
        /// </summary>
        event OnBinaryMessageRequestReceivedDelegate?   OnBinaryMessageRequestReceived;

        /// <summary>
        /// An event sent whenever a binary response was received.
        /// </summary>
        event OnBinaryMessageResponseReceivedDelegate?  OnBinaryMessageResponseReceived;

        #endregion


        /// <summary>
        /// Process the given incoming binary message.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="WebSocketConnection">The WebSocket connection the message arrived on.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        Task<WebSocketBinaryMessageResponse> ProcessBinaryMessage(DateTimeOffset        RequestTimestamp,
                                                                  IWebSocketConnection  WebSocketConnection,
                                                                  Byte[]                BinaryMessage,
                                                                  EventTracking_Id      EventTrackingId,
                                                                  CancellationToken     CancellationToken);

    }

}
