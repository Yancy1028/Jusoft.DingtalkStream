using Jusoft.DingtalkStream;

using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingtalkStreamDemo
{
    public class Worker : BackgroundService
    {
        private readonly DingtalkStreamClient client;

        public Worker(DingtalkStreamClient client)
        {
            this.client = client;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);// 延迟一小会儿再开始;
            try
            {
                this.client.RegisterEventSubscription();

                _ = client.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
