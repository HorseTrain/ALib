﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public interface IOpCodeImm
    {
        public long Imm     { get; set; }
    }
}
