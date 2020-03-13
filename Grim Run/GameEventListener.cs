using GrimRun;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grim_Run
{
    class GameEventListener
    {
        private Thread pipeListener;
        private bool listen = false;

        public GameEventListener(GameEventParser parser)
        {
            pipeListener = new Thread(() => PipeServer(parser));
            pipeListener.Start();
            listen = true;
        }

        public void Stop()
        {
            listen = false;
            pipeListener.Join();
        }

        private void PipeServer(GameEventParser parser)
        {
            int bytesRead = 0;
            var pipe = new NamedPipeServerStream("GrimRunPipe", PipeDirection.In);
            pipe.WaitForConnection();

            while (listen)
            {
                var bytes = new byte[Marshal.SizeOf(typeof(GrimRunMessage))];
                bytesRead = pipe.Read(bytes, 0, Marshal.SizeOf(typeof(GrimRunMessage)));

                if (bytesRead > 0)
                {
                    GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    try
                    {
                        var msg = Marshal.PtrToStructure<GrimRunMessage>(handle.AddrOfPinnedObject());
                        Console.WriteLine($"Message type {msg.MessageType}");

                        parser.Parse(msg);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                else
                {
                    Console.Error.WriteLine("Failed to read from pipe, shutting my ears");
                    listen = false;
                }

            }
        }
    }
}
