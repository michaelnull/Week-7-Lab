using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pickme.Models
{
    public class PickUploadVM
    {
        public int Id { get; set; }
        public string YourName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}