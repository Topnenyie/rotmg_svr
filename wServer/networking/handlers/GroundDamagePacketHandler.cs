﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using wServer.networking.svrPackets;
using db;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    class GroundDamagePacketHandler : PacketHandlerBase<GroundDamagePacket>
    {
        public override PacketID ID { get { return PacketID.GroundDamage; } }

        protected override void HandlePacket(Client client, GroundDamagePacket packet)
        {
            //TODO: implement something
        }
    }
}
