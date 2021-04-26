using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalouge.Core.Configuration
{
    public class ConnectionStrings
    {
        public string EFCoreDBFirstDemoDatabase { get; set; }
    }

    public class Authentication
    {
        public string Secret { get; set; }
        public string ExpiryInSeconds { get; set; }
    }

    public class GlobalConfig
    {
        public GlobalConfig()
        {
            ConnectionStrings = new ConnectionStrings();
            Authentication = new Authentication();
        }
        public ConnectionStrings ConnectionStrings { get; set; }
        public Authentication Authentication { get; set; }
    }
}
