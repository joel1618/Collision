using Breeze.ContextProvider.EF6;
using Collision.Sql.Ef;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Collision.App_Start
{
    public static class MetadataScriptWriter
    {
        public static void Write()
        {
            // get the metadata the same way we get it for the controller
            var metadata = new EFContextProvider<CollisionDbContext>().Metadata();

            // construct the filename and runtime file location
            var fileName = HostingEnvironment.MapPath("~/App/services/breeze/metadata.js");

            // the same pre- and post-fix strings we used earlier
            const string prefix = "window.breeze = window.breeze || {}; windows.breeze.metadata = JSON.stringify(";

            const string postfix = ");";

            // write to file
            using (var writer = new StreamWriter(fileName))
            {
                writer.WriteLine(prefix + "\n" + metadata + "\n" + postfix);
            }
        }
    }
}