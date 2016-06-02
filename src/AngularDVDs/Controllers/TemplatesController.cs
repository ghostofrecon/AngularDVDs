using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AngularDVDs.Controllers
{
    public class TemplatesController : Controller
    {
        public IActionResult DvdModal()
        {
            return this.PartialView();
        }

        public IActionResult AddDvdInline()
        {
            return this.PartialView();
        }
    }
}