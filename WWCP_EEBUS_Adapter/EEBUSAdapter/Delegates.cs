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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;
using cloud.charging.open.protocols.WWCP.OverlayNetworking.WebSockets;

#endregion

namespace cloud.charging.open.protocols.EEBUS
{

    /// <summary>
    /// The delegate for the HTTP WebSocket request log.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the incoming request.</param>
    /// <param name="Server">The sending WebSocket server.</param>
    /// <param name="JSONRequest">The incoming JSON request.</param>
    public delegate Task WebSocketJSONRequestLogHandler          (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  IWebSocketConnection        WebSocketConnection,
                                                                  NetworkingNode_Id           NetworkingNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  JObject                     JSONRequest);

    /// <summary>
    /// The delegate for the HTTP WebSocket request log.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the incoming request.</param>
    /// <param name="Server">The sending WebSocket server.</param>
    /// <param name="BinaryRequest">The incoming binary request.</param>
    public delegate Task WebSocketBinaryRequestLogHandler        (DateTime                    Timestamp,
                                                                  IEventSender                Server,
                                                                  IWebSocketConnection        WebSocketConnection,
                                                                  NetworkingNode_Id           NetworkingNodeId,
                                                                  NetworkPath                 NetworkPath,
                                                                  EventTracking_Id            EventTrackingId,
                                                                  DateTime                    RequestTimestamp,
                                                                  Byte[]                      BinaryRequest);


    // Responses

    public delegate Task WebSocketJSONRequestJSONResponseLogHandler    (DateTime                    Timestamp,
                                                                        IEventSender                Server,
                                                                        IWebSocketConnection        WebSocketConnection,
                                                                        NetworkingNode_Id           NetworkingNodeId,
                                                                        NetworkPath                 NetworkPath,
                                                                        EventTracking_Id            EventTrackingId,
                                                                        DateTime                    RequestTimestamp,
                                                                        JObject                     JSONRequest,
                                                                        JObject?                    JSONResponse,
                                                                        JArray?                     ErrorResponse,
                                                                        TimeSpan                    Runtime);

    public delegate Task WebSocketJSONRequestBinaryResponseLogHandler  (DateTime                    Timestamp,
                                                                        IEventSender                Server,
                                                                        IWebSocketConnection        WebSocketConnection,
                                                                        NetworkingNode_Id           NetworkingNodeId,
                                                                        NetworkPath                 NetworkPath,
                                                                        EventTracking_Id            EventTrackingId,
                                                                        DateTime                    RequestTimestamp,
                                                                        JObject                     JSONRequest,
                                                                        Byte[]?                     BinaryResponse,
                                                                        JArray?                     ErrorResponse,
                                                                        TimeSpan                    Runtime);

    public delegate Task WebSocketBinaryRequestJSONResponseLogHandler  (DateTime                    Timestamp,
                                                                        IEventSender                Server,
                                                                        IWebSocketConnection        WebSocketConnection,
                                                                        NetworkingNode_Id           NetworkingNodeId,
                                                                        NetworkPath                 NetworkPath,
                                                                        EventTracking_Id            EventTrackingId,
                                                                        DateTime                    RequestTimestamp,
                                                                        Byte[]                      BinaryRequest,
                                                                        JObject?                    JSONResponse,
                                                                        JArray?                     ErrorResponse,
                                                                        TimeSpan                    Runtime);

    public delegate Task WebSocketBinaryRequestBinaryResponseLogHandler(DateTime                    Timestamp,
                                                                        IEventSender                Server,
                                                                        IWebSocketConnection        WebSocketConnection,
                                                                        NetworkingNode_Id           NetworkingNodeId,
                                                                        NetworkPath                 NetworkPath,
                                                                        EventTracking_Id            EventTrackingId,
                                                                        DateTime                    RequestTimestamp,
                                                                        Byte[]                      BinaryRequest,
                                                                        Byte[]?                     BinaryResponse,
                                                                        JArray?                     ErrorResponse,
                                                                        TimeSpan                    Runtime);


    // Generic binary messages
    //
    // The four signatures below are read off their own invocation sites in
    // EEBUSWebSocketAdapterIN/OUT, which call them as (Timestamp.Now, this, message)
    // - Timestamp.Now is a DateTimeOffset, "this" is the adapter raising the event.

    /// <summary>
    /// A delegate sent whenever a binary request message was received.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that received the message.</param>
    /// <param name="BinaryRequestMessage">The received binary request message.</param>
    public delegate Task OnBinaryMessageRequestReceivedDelegate   (DateTimeOffset                 Timestamp,
                                                                   IEEBUSWebSocketAdapterIN       Sender,
                                                                   BinaryRequestMessage           BinaryRequestMessage);

    /// <summary>
    /// A delegate sent whenever a binary response message was received.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that received the message.</param>
    /// <param name="BinaryResponseMessage">The received binary response message.</param>
    public delegate Task OnBinaryMessageResponseReceivedDelegate  (DateTimeOffset                 Timestamp,
                                                                   IEEBUSWebSocketAdapterIN       Sender,
                                                                   BinaryResponseMessage          BinaryResponseMessage);

    /// <summary>
    /// A delegate sent whenever a binary request message was sent.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that sent the message.</param>
    /// <param name="BinaryRequestMessage">The sent binary request message.</param>
    public delegate Task OnBinaryMessageRequestSentDelegate       (DateTimeOffset                 Timestamp,
                                                                   IEEBUSWebSocketAdapterOUT      Sender,
                                                                   BinaryRequestMessage           BinaryRequestMessage);

    /// <summary>
    /// A delegate sent whenever a binary response message was sent.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that sent the message.</param>
    /// <param name="BinaryResponseMessage">The sent binary response message.</param>
    public delegate Task OnBinaryMessageResponseSentDelegate      (DateTimeOffset                 Timestamp,
                                                                   IEEBUSWebSocketAdapterOUT      Sender,
                                                                   BinaryResponseMessage          BinaryResponseMessage);


    // Forwarded JSON messages
    //
    // These three have no invocation site: EEBUSWebSocketAdapterFORWARD declares the
    // events and nothing raises them, because the only live forwarding methods it has
    // are the binary ones. Their shape is therefore taken from the four above - the
    // same (Timestamp, Sender, message) triple every other event in this adapter
    // family uses - and not from an upstream original. If forwarding ever grows a JSON
    // path, check these against what it actually wants to report before relying on them.

    /// <summary>
    /// A delegate sent whenever a JSON request message was forwarded.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that forwarded the message.</param>
    /// <param name="JSONRequestMessage">The forwarded JSON request message.</param>
    public delegate Task OnJSONRequestMessageSentLoggingDelegate      (DateTimeOffset                 Timestamp,
                                                                       IEEBUSWebSocketAdapterFORWARD  Sender,
                                                                       JSONRequestMessage             JSONRequestMessage);

    /// <summary>
    /// A delegate sent whenever a JSON response message was forwarded.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that forwarded the message.</param>
    /// <param name="JSONResponseMessage">The forwarded JSON response message.</param>
    public delegate Task OnJSONResponseMessageSentLoggingDelegate     (DateTimeOffset                 Timestamp,
                                                                       IEEBUSWebSocketAdapterFORWARD  Sender,
                                                                       JSONResponseMessage            JSONResponseMessage);

    /// <summary>
    /// A delegate sent whenever a JSON request error message was forwarded.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The adapter that forwarded the message.</param>
    /// <param name="JSONRequestErrorMessage">The forwarded JSON request error message.</param>
    public delegate Task OnJSONRequestErrorMessageSentLoggingDelegate (DateTimeOffset                 Timestamp,
                                                                       IEEBUSWebSocketAdapterFORWARD  Sender,
                                                                       JSONRequestErrorMessage        JSONRequestErrorMessage);

}
