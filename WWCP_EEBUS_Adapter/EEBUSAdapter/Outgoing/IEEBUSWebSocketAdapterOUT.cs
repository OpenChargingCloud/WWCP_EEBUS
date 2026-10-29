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

using cloud.charging.open.protocols.WWCP.OverlayNetworking;
using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.EEBUS
{

    /// <summary>
    /// The common interface of all EEBUS adapters for sending outgoing messages.
    /// </summary>
    public interface IEEBUSWebSocketAdapterOUT
    {

        #region Events

        /// <summary>
        /// An event sent whenever a binary request was sent.
        /// </summary>
        event OnBinaryMessageRequestSentDelegate?   OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever a binary response was sent.
        /// </summary>
        event OnBinaryMessageResponseSentDelegate?  OnBinaryMessageResponseSent;

        #endregion


        /// <summary>
        /// Send (and forget) the given binary request message.
        /// </summary>
        /// <param name="BinaryRequestMessage">A binary request message.</param>
        Task<SendWebSocketMessageResult>  SendBinaryRequest                (BinaryRequestMessage   BinaryRequestMessage);

        /// <summary>
        /// Send the given binary request message and wait for its response.
        /// </summary>
        /// <param name="BinaryRequestMessage">A binary request message.</param>
        Task<SendRequestState>            SendBinaryRequestAndWait         (BinaryRequestMessage   BinaryRequestMessage);

        /// <summary>
        /// Notify about a sent binary response message.
        /// </summary>
        /// <param name="BinaryResponseMessage">A binary response message.</param>
        Task                              NotifyBinaryMessageResponseSent  (BinaryResponseMessage  BinaryResponseMessage);

    }

}
