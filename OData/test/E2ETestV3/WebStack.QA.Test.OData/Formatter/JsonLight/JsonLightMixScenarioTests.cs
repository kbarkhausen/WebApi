﻿using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Linq;
using System.Text;
using System.Web.Http;
using Nuwa;
using WebStack.QA.Test.OData.Common;
using Xunit.Extensions;

namespace WebStack.QA.Test.OData.Formatter.JsonLight
{
    public class JsonLightMixScenarioTests : MixScenarioTestsOData
    {
        public string AcceptHeader { get; set; }

        public override DataServiceContext WriterClient(Uri serviceRoot, DataServiceProtocolVersion protocolVersion)
        {
            var ctx = base.WriterClient(serviceRoot, protocolVersion);
            new JsonLightConfigurator(ctx, AcceptHeader).Configure();
            return ctx;
        }

        public override DataServiceContext ReaderClient(Uri serviceRoot, DataServiceProtocolVersion protocolVersion)
        {
            var ctx = base.ReaderClient(serviceRoot, protocolVersion);
            new JsonLightConfigurator(ctx, AcceptHeader).Configure();
            return ctx;
        }

        [NuwaConfiguration]
        public static void UpdateConfiguration(HttpConfiguration configuration)
        {
            configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            configuration.EnableODataSupport(GetEdmModel(configuration), "odata");
        }

        [NuwaWebConfig]
        public static void UpdateWebConfig(WebStack.QA.Common.WebHost.WebConfigHelper config)
        {
            config.AddODataLibAssemblyRedirection();
        }

        [Theory]
        [InlineData("application/json;odata=minimalmetadata;streaming=true")]
        [InlineData("application/json;odata=minimalmetadata;streaming=false")]
        [InlineData("application/json;odata=minimalmetadata")]
        [InlineData("application/json;odata=fullmetadata;streaming=true")]
        [InlineData("application/json;odata=fullmetadata;streaming=false")]
        [InlineData("application/json;odata=fullmetadata")]
        [InlineData("application/json;streaming=true")]
        [InlineData("application/json;streaming=false")]
        [InlineData("application/json")]
        public void ODataCRUDShouldWorkJsonLight(string acceptHeader)
        {
            AcceptHeader = acceptHeader;
            ODataCRUDShouldWork();
        }
    }
}
