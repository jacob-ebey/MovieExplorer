// <copyright file="ILogger.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using System;
using System.Runtime.CompilerServices;

namespace MovieExplorer.Services
{
    public interface ILogger
    {
        void LogException(Exception e, [CallerMemberName] string callerMemberName = null);
        void Init();
    }
}
