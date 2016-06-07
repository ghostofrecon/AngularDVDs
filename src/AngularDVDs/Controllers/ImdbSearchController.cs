using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AngularDVDs.Controllers
{
    using System.IO;
    using System.Net;
    using System.Text.Encodings.Web;

    using Newtonsoft.Json.Linq;

    [Produces("application/json")]
    [Route("api/ImdbSearch")]
    public class ImdbSearchController : Controller
    {
        [HttpGet("{title}")]
        public object title(string title)
        {
            var urlenc = UrlEncoder.Create();
            string encodedTitleSeg = urlenc.Encode(title);
            WebRequest wr = WebRequest.Create(new Uri("http://www.imdb.com/xml/find?json=1&nr=1&tt=on&q="+encodedTitleSeg));
            var response = wr.GetResponse();
            Stream ds = response.GetResponseStream();
            var sr = new StreamReader(ds);
            return sr.ReadToEnd();
            

        }
    }
}