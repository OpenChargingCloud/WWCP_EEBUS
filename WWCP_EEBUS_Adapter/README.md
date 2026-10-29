# WWCP_EEBUS_Adapter

This folder holds the **WWCP integration layer** of the original EEBUS prototype: it
bridges the standalone SHIP/SPINE/UseCases stack into WWCP's overlay networking.

Since WP-W1 it is part of `WWCP_EEBUS.sln` and **is built** — on every CI leg of this
repository and of `EEBUSConformanceTests`. It is **not tested**: no test touches a line of
it. "It compiles" is the entire claim this project currently supports.

## How it got here

From WP-W0 onwards the core was deliberately built on
[Styx](https://github.com/Vanaheimr/Styx) and [Hermod](https://github.com/Vanaheimr/Hermod)
alone — "variant A" — so that it could be developed and conformance-tested without
dragging in `WWCP_Core` / `WWCP_OverlayNetworking`. This project was left out of the
solution to keep that true.

That decision had a price, and it is worth naming, because it is the kind that is invisible
until it is paid. Being out of every solution meant no compiler ever read this code. Over
the months that followed it was edited exactly once, by a repository-wide rename, and
nothing noticed that the rename only covered one side of a cross-repository property name.
When the project was finally built for the first time, the compiler produced **thirty
errors in four families**:

| Family | What it was |
|---|---|
| `IEEBUSWebSocketAdapter{IN,OUT,FORWARD}` | three interfaces the classes claim to implement, which existed nowhere |
| `On*Delegate` (7) | delegate types for the adapter's events, likewise nowhere |
| `EEBUS.Version` | WP-W0 gave each project its own `Version`; this is `SHIP.Version` |
| Hermod API drift | `ProcessText/BinaryMessage` grew from 5 parameters to 7 and stopped returning a response; two `base(...)` calls had slid out of alignment; `HTTPResponse.Builder()` lost its parameterless form |

Four of the seven delegate signatures were read straight off their own invocation sites.
The three on `EEBUSWebSocketAdapterFORWARD` had none — nothing raises them — so their shape
was taken from the four that do, and `EEBUSAdapter/Delegates.cs` says so at the point of
declaration rather than leaving a reader to guess.

Both `base(...)` argument lists are now **named rather than positional**. That is the
actual fix for that family: a positional list slides silently when the constructor it calls
changes shape, and what the compiler then reports is "argument 11 wants a `UInt32?`", which
names neither the cause nor the remedy.

## Known defects, deliberately left warning

The first build also surfaced two things that are not compile errors and are not fixed
here, because fixing either is a design change with no test to hold it down:

1. **`AEEBUSNode.EEBUSWebSocketClients` / `.EEBUSWebSocketServers` can only ever return
   nothing.** They filter lists of `OverlayWebSocketClient`/`OverlayWebSocketServer` for
   `SHIPWebSocketClient`/`SHIPWebSocketServer`, which descend from Hermod's
   `WebSocketClient`/`WebSocketServer` instead — unrelated hierarchies, so the filter
   matches nothing whatever is registered. The compiler says exactly this (CS0184, CA2021).
   This was first written down here as "the real fix is for the SHIP client and server to
   descend from `AOverlayWebSocketClient`/`AOverlayWebSocketServer`". On examination that
   is wrong, and the correction is the more useful of the two findings, so it replaces the
   claim openly rather than quietly. Two things stand in the way.

   The two overlay base classes are built oppositely. `AOverlayWebSocketClient` derives
   from Hermod's `WebSocketClient`, so on the client side the change really would be an
   insertion into an existing chain. `AOverlayWebSocketServer` derives from nothing but
   `IEventSender` and *holds* a `WebSocketServer` in a private field, re-exposing its
   surface. Deriving from it would mean `SHIPWebSocketServer` is no longer a Hermod server
   at all: every `override`, the `base.OnValidate*` wiring and `SendBinaryMessage` would
   have to move onto the wrapped instance. That is a rewrite of the TLS and handshake path
   WP02 and WP05 were built on, not a change of base class.

   And it would not buy what it appears to buy. `Reachability` declares its two properties
   as `IWebSocketClient?` and `IWebSocketServer?` — the ones in `WWCP.OverlayNetworking`,
   its own namespace, not Hermod's. **No class in WWCP_Core or in this repository
   implements either of them**, `AOverlayWebSocketClient` included; and
   `WWCP.OverlayNetworking.IWebSocketServer` extends Hermod's `IWebSocketServer`, which no
   Hermod class implements either. Both properties can therefore only ever be null, and
   the eight `if (reachability.EEBusWebSocket… is not null)` branches in `EEBUSAdapter.cs`
   are unreachable whatever the SHIP classes descend from.

   So the cause is not in this project. Satisfying those interfaces means implementing
   `SendJSONRequest`, `SendJSONResponse`, `SendJSONRequestError` and
   `SendJSONResponseError` on a transport that is binary — which is the design of the
   EEBUS-to-WWCP bridge, and it commits us to a JSON message shape SHIP does not have.
   That decision is left open as WP-W2 in WORKPLAN.md of EEBUSConformanceTests.

2. **Four crypto methods on `AEEBUSNode` hide non-virtual base members** (CS0108) and return
   a hard-coded key. A call through the base type gets the base answer; a call through
   `AEEBUSNode` gets the constant. `new` would silence the warning by asserting the hiding
   is intended, and that is precisely what is unknown — a fixed key in a node class reads
   far more like test scaffolding that outlived its test than like a decision.

Neither warning is suppressed. Until somebody takes a decision on them, the compiler
repeating itself on every build is the most reliable record of them there is.

## Contents

| Path | Purpose |
|------|---------|
| `EEBUSAdapter/` | IN/OUT/FORWARD message adapter modelled after the OCPP WebSocket adapter |
| `EEBUSAdapter/Delegates.cs` | the event delegate types, with a note on which signatures are derived and which are inferred |
| `AEEBUSNode.cs`, `IEEBUSNetworkingNode.cs` | EEBUS node built on `AOverlayNetworkingNode` |
| `WebSocket/SHIPWebSocketClient.cs`, `WebSocket/SHIPWebSocketServer.cs` | WebSocket client/server carrying `NetworkingMode`, node registries etc. |

## What happens next

Not the hierarchy change, for the reasons above — that question is WP-W2 and wants a
decision before any code touches it. What is worth doing without one: the 24 events
this project declares and never raises are the debris of an adaptation abandoned half
way, their raise sites still sitting commented out, which is why `CS0067` appears 48
times in a build. And nothing here is exercised. The first test against this adapter is
worth more than any further tidying of it, and unlike the bridge it needs nobody’s
permission.
