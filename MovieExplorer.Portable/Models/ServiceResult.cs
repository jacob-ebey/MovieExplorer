// <copyright file="ServiceResult.cs">
//     Copyright (c) 2017 Jacob Ebey
// </copyright>

using System;

namespace MovieExplorer.Models
{
    public class ServiceResult<T>
    {
        public Exception InnerException { get; set; }

        public bool Succeeded { get; set; }

        public T Data { get; set; }
    }
}