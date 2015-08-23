using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Service.Framework
{
    public interface ICollectionRequest
    {
        int StartIndex { get; set; }
        int MaxResults { get; set; }
    }
}
