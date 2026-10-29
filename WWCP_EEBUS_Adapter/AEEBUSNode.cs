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

using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.WWCP.OverlayNetworking;

#endregion

namespace cloud.charging.open.protocols.EEBUS.SHIP
{

    public abstract class AEEBUSNode : AOverlayNetworkingNode,
                                       IEEBUSNetworkingNode
    {

        #region Data

        #endregion

        #region Properties

        public IEEBUSAdapter  EEBUS    { get; }


        // These two always return nothing, and the compiler says so: CS0184 "the given
        // expression is never of the provided type", plus CA2021 on the Cast. The lists
        // they read hold OverlayWebSocketClient/OverlayWebSocketServer, while
        // SHIPWebSocketClient/SHIPWebSocketServer descend from Hermod's WebSocketClient
        // and WebSocketServer - two unrelated hierarchies, so the filter matches nothing
        // whatever is registered.
        //
        // The fix is not here: it is that the SHIP client and server should descend from
        // AOverlayWebSocketClient/AOverlayWebSocketServer, which is what a node in this
        // overlay expects to be handed. That is a design change with no test to hold it
        // down, so it is left for whoever takes the adapter further. The warnings are
        // deliberately not suppressed - until that happens, the compiler saying this on
        // every build is the most reliable record of it there is.
        public IEnumerable<SHIPWebSocketClient> EEBUSWebSocketClients
            => base.webSocketClients.Where(cl => cl is SHIPWebSocketClient).Cast<SHIPWebSocketClient>();

        public IEnumerable<SHIPWebSocketServer> EEBUSWebSocketServers
            => base.webSocketServers.Where(cl => cl is SHIPWebSocketServer).Cast<SHIPWebSocketServer>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new networking node for testing.
        /// </summary>
        /// <param name="Id">The unique identification of this networking node.</param>
        public AEEBUSNode(NetworkingNode_Id  Id,
                          I18NString?        Description                 = null,

                          SignaturePolicy?   SignaturePolicy             = null,
                          SignaturePolicy?   ForwardingSignaturePolicy   = null,

                          Boolean            DisableSendHeartbeats       = false,
                          TimeSpan?          SendHeartbeatsEvery         = null,
                          TimeSpan?          DefaultRequestTimeout       = null,

                          Boolean            DisableMaintenanceTasks     = false,
                          TimeSpan?          MaintenanceEvery            = null,
                          DNSClient?         DNSClient                   = null)

            : base(Id,
                   Description,
                   SignaturePolicy,
                   ForwardingSignaturePolicy,
                   DisableSendHeartbeats,
                   SendHeartbeatsEvery,
                   DefaultRequestTimeout,
                   DisableMaintenanceTasks,
                   MaintenanceEvery,
                   DNSClient)

        {

            this.EEBUS  = new EEBUSAdapter(
                              this,
                              DisableSendHeartbeats,
                              SendHeartbeatsEvery,
                              DefaultRequestTimeout,
                              SignaturePolicy,
                              ForwardingSignaturePolicy
                          );

        }

        #endregion



        // CS0108 on all four below: AOverlayNetworkingNode declares them too, and not as
        // virtual, so these hide rather than override - a call through the base type gets
        // the base answer and a call through this type gets the fixed key written here.
        // "new" would silence the warning by asserting that the hiding is intended, and
        // that is precisely what is not known: a hard-coded key in a node class reads far
        // more like test scaffolding that outlived its test than like a decision. Left
        // warning on purpose.
        public Byte[] GetEncryptionKey(NetworkingNode_Id  DestinationNodeId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".FromHEX();
        }

        public Byte[] GetDecryptionKey(NetworkingNode_Id  SourceNodeId,
                                       UInt16?            KeyId   = null)
        {
            return "5a733d6660df00c447ff184ae971e1d5bba5de5784768795ee6535867130aa12".FromHEX();
        }


        public UInt64 GetEncryptionNonce(NetworkingNode_Id  DestinationNodeId,
                                         UInt16?            KeyId   = null)
        {
            return 1;
        }

        public UInt64 GetEncryptionCounter(NetworkingNode_Id  DestinationNodeId,
                                           UInt16?            KeyId   = null)
        {
            return 1;
        }



        #region (override) HandleErrors(Module, Caller, ErrorResponse)

        public override Task HandleErrors(String  Module,
                                          String  Caller,
                                          String  ErrorResponse)
        {

            return Task.CompletedTask;

        }

        #endregion

        #region (override) HandleErrors(Module, Caller, ExceptionOccured)

        public override Task HandleErrors(String     Module,
                                          String     Caller,
                                          Exception  ExceptionOccurred)
        {

            return Task.CompletedTask;

        }

        #endregion


    }

}
