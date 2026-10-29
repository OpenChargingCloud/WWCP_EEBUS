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
    /// The common interface of all EEBUS adapters for forwarding messages.
    /// </summary>
    public interface IEEBUSWebSocketAdapterFORWARD
    {

        #region Properties

        /// <summary>
        /// The default forwarding result, when no forwarding processor decided otherwise.
        /// </summary>
        ForwardingResult            DefaultResult      { get; set; }

        /// <summary>
        /// The anycast networking node identifications messages may be forwarded to.
        /// </summary>
        HashSet<NetworkingNode_Id>  AnycastIdsAllowed  { get; }

        /// <summary>
        /// The anycast networking node identifications messages must not be forwarded to.
        /// </summary>
        HashSet<NetworkingNode_Id>  AnycastIdsDenied   { get; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a JSON request message was sent.
        /// </summary>
        event OnJSONRequestMessageSentLoggingDelegate?       OnJSONRequestMessageSent;

        /// <summary>
        /// An event sent whenever a JSON response message was sent.
        /// </summary>
        event OnJSONResponseMessageSentLoggingDelegate?      OnJSONResponseMessageSent;

        /// <summary>
        /// An event sent whenever a JSON request error message was sent.
        /// </summary>
        event OnJSONRequestErrorMessageSentLoggingDelegate?  OnJSONRequestErrorMessageSent;

        #endregion


        /// <summary>
        /// Return the networking node identification a request with the given
        /// request identification was forwarded on behalf of.
        /// </summary>
        /// <param name="RequestId">A request identification.</param>
        NetworkingNode_Id?  GetForwardedNodeId          (Request_Id             RequestId);

        /// <summary>
        /// Forward the given binary request message.
        /// </summary>
        /// <param name="BinaryRequestMessage">A binary request message.</param>
        Task                ProcessBinaryRequestMessage (BinaryRequestMessage   BinaryRequestMessage);

        /// <summary>
        /// Forward the given binary response message.
        /// </summary>
        /// <param name="BinaryResponseMessage">A binary response message.</param>
        Task                ProcessBinaryResponseMessage(BinaryResponseMessage  BinaryResponseMessage);

    }

}
