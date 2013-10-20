﻿using System;
using SmartElk.Antler.Common;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public interface IAntlerConfigurator : ISyntax, IDisposable
    {
        AntlerConfiguration Configuration { get; }
    }
}
