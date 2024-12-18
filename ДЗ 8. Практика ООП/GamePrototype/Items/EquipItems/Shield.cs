﻿using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public sealed class Shield : EquipItem
    {
        public Shield(uint defence, uint durability, string name) : base(durability, name) => Defence = defence;

        public uint Defence { get; }

        public override EquipSlot Slot => EquipSlot.Shield;
    }
}
