﻿using System;
using System.Threading.Tasks;
using WzWeb.Shared;

namespace WzWeb.Client.Services
{
    public interface IBrowserService
    {
        public BrowserConfig BrowserConfig { get; }
        public bool HasInit { get; }

        public Task Init();
    }
}
