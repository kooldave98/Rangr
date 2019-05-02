using System;
using System.Collections.Generic;

namespace App.WebAPI.Infrastructure.UsageLogs
{

    public class UsageLogRepository
    {
        private readonly List<Log> _requests = new List<Log>();

        private readonly List<Log> _responses = new List<Log>();


        public IList<Log> requests
        {
            get { return _requests; }
        }

        public IList<Log> responses
        {
            get { return _responses; }
        }
    }

    public class Log
    {
        public string log_summary { get; set; }
        public DateTime log_time { get; set; }
    }
}