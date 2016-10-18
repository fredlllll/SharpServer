using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class FilePage : Page
    {
        //public FileInfo File { get; internal set; }

        public override void Emit()
        {
            FileInfo fi = RequestedSiteFile.LocalFile;
            Response.ContentType = MimeTypes.MimeTypeMap.GetMimeType(fi.Extension);
            Response.ContentLength64 = fi.Length;
            Response.ContentEncoding = null;
            //t Response.AddHeader("Content-disposition", "attachment; filename="+fi.Name);
            using(var file = fi.OpenRead())
            {
                file.CopyTo(Response.OutputStream);
            }
        }

        public override void Reset()
        {
            
        }
    }
}
