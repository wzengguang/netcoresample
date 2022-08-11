using Sample.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Application
{
    public class SeviceNewId
    {
        private IdWorker idWorker = new IdWorker(1);


        protected long NewId()
        {
            return idWorker.nextId();
        }
    }
}
