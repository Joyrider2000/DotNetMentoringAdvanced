using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Application.Services.Message
{
    public interface IMessageService
    {
        public Task Publish(object entity);
    }
}
