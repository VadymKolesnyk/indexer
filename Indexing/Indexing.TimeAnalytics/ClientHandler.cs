using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indexing.TimeAnalytics
{
    class ClientHendler
    {
        readonly Index _index;

        public ClientHendler(Index index)
        {
            _index = index;
        }

        public void Handle()
        {
            //TO DO
        }

        public Task HandleAsync() => Task.Run(Handle);

    }
}
