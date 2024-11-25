using PocztaDesktop.EndPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocztaDesktop.ViewModel
{
    public class MainViewModel
    {
        public string message { get; set; }
        public MainViewModel()
        {
            message = GetEndPoint.getEndPoint();
        }
    }
}
