using System;
using System.Threading;
using DotNetty.Buffers;
using dotNetty_kcp;
using fec;

namespace example_Kcp
{
    public class KcpRttExampleServer:KcpListener
    {

        public static void start()
        {
            KcpRttExampleServer kcpRttExampleServer = new KcpRttExampleServer();

            ChannelConfig channelConfig = new ChannelConfig();
            channelConfig.initNodelay(true,40,2,true);
            channelConfig.Sndwnd=512;
            channelConfig.Rcvwnd=512;
            channelConfig.Mtu=512;
            channelConfig.FecDataShardCount=3;
            channelConfig.FecParityShardCount=1;
            channelConfig.AckNoDelay=true;
            channelConfig.TimeoutMillis=10000;
            channelConfig.AutoSetConv=true;
            channelConfig.UseConvChannel = true;
            KcpServer kcpServer = new KcpServer();
            kcpServer.init(Environment.ProcessorCount, kcpRttExampleServer,channelConfig,20003);
        }

        public void handleReceive(IByteBuffer byteBuf, Ukcp ukcp,int protocolType)
        {
            short curCount = byteBuf.GetShort(byteBuf.ReaderIndex);
            Console.WriteLine(Thread.CurrentThread.Name+" 收到消息 "+curCount);
            ukcp.writeKcpMessage(byteBuf);
            if (curCount == -1) {
                ukcp.notifyCloseEvent();
            }
        }

        public void handleException(Exception ex, Ukcp ukcp)
        {
            throw new NotImplementedException();
        }

        public void handleClose(Ukcp ukcp)
        {
            Console.WriteLine(Snmp.snmp.ToString());
            Snmp.snmp = new Snmp();
        }
    }
}