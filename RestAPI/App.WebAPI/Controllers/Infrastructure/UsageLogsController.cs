using System.Collections.Generic;
using App.Library.CodeStructures.Behavioral;
using System.Linq;
using App.WebAPI.Infrastructure.UsageLogs;

namespace App.WebAPI.Controllers.Infrastructure
{
    public class UsageLogsController : BaseController
    {
        /// <summary>
        /// TODO: Convert to ODATA query at some point for easy inspecting
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Log> Get()
        {
            return usage_log_repository
                                .requests
                                //Exclude requests for the usage log
                                .Where(ul => !ul.log_summary.Contains("usagelog"))
                                .OrderByDescending(ul => ul.log_time);
        }

        public UsageLogsController(UsageLogRepository the_usage_log_repository)
        {
            usage_log_repository = Guard.IsNotNull(the_usage_log_repository, "the_usage_log_repository");
        }

        private readonly UsageLogRepository usage_log_repository;
    }
}
