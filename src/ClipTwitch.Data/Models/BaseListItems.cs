using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipTwitch.Data.Models
{
    public class BaseListItems<T>
    {
        public List<T> data { get; set; }
    }
}
