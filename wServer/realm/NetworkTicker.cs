﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using wServer.networking;

namespace wServer.realm
{
    using Work = Tuple<Client, PacketID, byte[]>;
    public class NetworkTicker //Sync network processing
    {
        public RealmManager Manager { get; private set; }
        public NetworkTicker(RealmManager manager)
        {
            this.Manager = manager;
        }

        public void AddPendingPacket(Client client, PacketID id, byte[] packet)
        {
            pendings.Enqueue(new Work(client, id, packet));
        }
        static ConcurrentQueue<Work> pendings = new ConcurrentQueue<Work>();
        static SpinWait loopLock = new SpinWait();


        public void TickLoop()
        {
            Work work;
            while (true)
            {
                loopLock.Reset();
                while (pendings.TryDequeue(out work))
                {
                    if (work.Item1.Stage == ProtocalStage.Disconnected)
                    {
                        Client client;
                        Manager.Clients.TryRemove(work.Item1.Account.AccountId, out client);
                        continue;
                    }
                    try
                    {
                        Packet packet = Packet.Packets[work.Item2].CreateInstance();
                        packet.Read(work.Item1, work.Item3, 0, work.Item3.Length);
                        work.Item1.ProcessPacket(packet);
                    }
                    catch { }
                }
                while (pendings.Count == 0)
                    loopLock.SpinOnce();
            }
        }
    }
}
