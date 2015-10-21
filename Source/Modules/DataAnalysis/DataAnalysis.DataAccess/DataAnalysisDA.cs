using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCN.Modules.DataAnalysis.BusinessEntity;

namespace CCN.Modules.DataAnalysis.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAnalysisDA : DataAnalysisDataAccessBase
    {
        /// <summary>
        /// 
        /// </summary>
        public DataAnalysisDA()
        {

        }

        /// <summary>
        /// 本地市场按月的持有量TOP10
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetLocalByMonthTop10()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select count(*) as cnt,month(post_time) as monthflg,model_id from car_info_bak 
                            where cityid=125 AND month(post_time)=10 group by monthflg,model_id order by cnt desc
                            limit 0, 10; ");

            var list = Helper.Query<dynamic>(sql.ToString());
            return list;
        }
    }
}
