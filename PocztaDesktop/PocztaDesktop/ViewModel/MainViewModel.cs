using PocztaDesktop.EndPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocztaDesktop.Model;

namespace PocztaDesktop.ViewModel
{
    public class MainViewModel
    {
        public string message { get; set; }
        public MainViewModel()
        {
            List<Item> Items =  GetEndPoint.getEndPoint();
            message = Items[0].Name +"-"+ Items[0].Description;
        }
    }
}
