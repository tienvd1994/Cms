﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyWeb.Presentation.Startup))]
namespace MyWeb.Presentation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
