using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerServer
{
    public class TablesMonitor
    {
        public static List<Table> Tables = new List<Table>();

        public static Table Find(TableConfig tc)
        {
            Table tt = null;
           foreach(var t in Tables)
            {
                if (t.tableConfig.SimilarTo(tc))
                {
                    tt = t;
                }
            }
            return tt;
        }

        internal static Table CreateTable(TableConfig tc)
        {
            var tcd = new Table();
            tcd.Create(tc);
            tcd.tableConfig = tc;
            Tables.Add(tcd);
            return tcd;
        }

        internal static Table GetTable(string v)
        {
            return (from t in Tables where t.tableID == v select t).First();
        }
    }
}
