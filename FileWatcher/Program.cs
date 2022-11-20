using System;
using System.IO;
using Quartz;
using Quartz.Impl;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;

namespace FileWatcher
{
    public class Program : IJob
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("這只是一個測試~~");
            //建立資料
            colleague eddy = new colleague(){Name = "Eddy", Action = "在辦公室看到一隻馬兒在奔跑" };
            colleague jack = new colleague() { Name = "Tom", Action = "給他一根香蕉" };
            /*重新new測試Dictionary正常*/
            FileWatcherService fs = new FileWatcherService();
            fs.FileWatch(eddy);
            FileWatcherService fs2 = new FileWatcherService();
            fs2.FileWatch(jack);
            //Quartz處理
            Quartz.Logging.LogProvider.IsDisabled = true;
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();//Scheduler Start
            await scheduler.ScheduleJob(Job1(), Trigger1());
            await scheduler.ScheduleJob(Job2(), Trigger2());
            Console.Read(); 
        }
        #region Job
        public static IJobDetail Job1()
        {
            IJobDetail? job = null;
            job = JobBuilder.Create<Program>()
                                    .WithIdentity("1", "1")
                                    .Build();
            job.JobDataMap["msg"] = "馬兒跑走ㄌ";
            job.JobDataMap["Name"] = "Eddy";
            return job;
        }
        public static IJobDetail Job2()
        {
            IJobDetail? job = null;
            job = JobBuilder.Create<Program>()
                                    .WithIdentity("2", "2")
                                    .Build();
            job.JobDataMap["msg"] = "沒有香蕉了";
            job.JobDataMap["Name"] = "Tom";
            return job;
        }
        #endregion
        #region Trigger
        public static ITrigger Trigger1()
        { 
            
            ISimpleTrigger trigger = TriggerBuilder.Create()
                            .WithIdentity("1", "1")
                            .StartNow()
                            .StartAt(DateTime.Now.AddSeconds(30))
                            .Build()
                             as ISimpleTrigger;
            return trigger;
        }
        public static ITrigger Trigger2()
        {
            ISimpleTrigger trigger = TriggerBuilder.Create()
                            .WithIdentity("2", "2")
                            .StartNow()
                            .StartAt(DateTime.Now.AddSeconds(20))
                            .Build()
                             as ISimpleTrigger;
            return trigger;
        }
        #endregion
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string msg = dataMap.GetString("msg");
            string name = dataMap.GetString("Name");    
            Console.WriteLine($"{DateTime.Now.ToString("HH:ss")+" "+msg}");
            FileWatcherService fs = new FileWatcherService();
            fs.CancelWatcher(name);
        }
    }
    public class colleague
    { 
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }
    }
}
