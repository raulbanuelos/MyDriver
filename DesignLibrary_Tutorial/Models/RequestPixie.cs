using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DesignLibrary_Tutorial.Models
{
    public class RequestPixie
    {
        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}