﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmLIB.Emulator
{
    public enum ExitReason : ulong
    {
        Normal,
        UndefinedInstruction,
        Svc,
    }
}
