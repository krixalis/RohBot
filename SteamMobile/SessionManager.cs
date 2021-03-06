﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SuperWebSocket;

namespace SteamMobile
{
    public class SessionManager
    {
        private class Server : WebSocketServer<Connection>
        {
            
        }
       
        private Server _server;
        private ConcurrentDictionary<string, Session> _sessions;
        private Stopwatch _timer;

        public SessionManager()
        {
            _sessions = new ConcurrentDictionary<string, Session>();
            _timer = Stopwatch.StartNew();
            _server = new Server();
        }

        public void Start()
        {
            _server.Setup("0.0.0.0", 12000);
            _server.Start();

            _server.NewMessageReceived += OnReceive;
        }

        public void Broadcast(Packet packet, Func<Session, bool> filter = null)
        {
            var packetStr = Packet.WriteToMessage(packet);

            foreach (var session in _sessions.Values)
            {
                if (filter == null || filter(session))
                {
                    session.Send(packetStr);
                }
            }
        }

        public void Update()
        {
            var emptySessions = _sessions.Where(kv => !kv.Value.IsActive);
            foreach (var empty in emptySessions)
            {
                Session removedSession;

                if (_sessions.TryRemove(empty.Key, out removedSession))
                {
                    var rooms = Program.RoomManager.List.Where(r => removedSession.IsInRoom(r.RoomInfo.ShortName));
                    foreach (var r in rooms)
                    {
                        r.SessionDisconnect(removedSession);
                    }
                }
            }

            foreach (var session in _sessions)
            {
                session.Value.Update((float)_timer.Elapsed.TotalSeconds);
            }

            _timer.Restart();
        }

        public ICollection<Session> List
        {
            get
            {
                return _sessions.Values;
            }
        }

        public Session GetOrCreate(Account account)
        {
            return _sessions.GetOrAdd(account.Name, k => new Session(account));
        }

        public Session Get(string name)
        {
            Session result;
            _sessions.TryGetValue(name, out result);
            return result;
        }

        private void OnReceive(Connection connection, string message)
        {
            try
            {
                Packet.ReadFromMessage(message).Handle(connection);
            }
            catch (Exception e)
            {
                Program.Logger.Error(string.Format("Bad packet from {0}: {1}", connection.Address, message), e);
            }
        }
    }
}
