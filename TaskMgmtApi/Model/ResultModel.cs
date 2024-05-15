using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Presentation.Model
{
    public class ResultModel
    {
        private string _error;
        public bool Success { get; set; } = false;
        public string Error
        {
            get => string.IsNullOrEmpty(_error) ? string.Empty : _error;
            set => _error = value;
        }
        public object Data { get; set; }
    }

    public class DataResponseModel<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
