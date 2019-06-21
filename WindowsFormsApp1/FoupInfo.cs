using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferControl.Config;

namespace Adam
{
    class FoupInfo
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FoupInfo));
        private string recipe_file;
        private string login_user;
        private string foup_id;
        private string file_name;
        public waferInfo[] record;

        public FoupInfo(string recipe_file, string login_user, string foup_id)
        {
            this.recipe_file = recipe_file;
            this.login_user = login_user;
            this.foup_id = foup_id;
            string date = System.DateTime.Now.ToString("yyyyMMdd");
            string time = System.DateTime.Now.ToString("HHmmss");
            this.file_name = SystemConfig.Get().EquipmentID + "_" + foup_id + "_" + date + "_" + time + ".csv";
            record = new waferInfo[25];
        }
        public void Save()
        {
            try
            {
                //string fullPath = @"d:\log\foup\" + file_name;
                string path = SystemConfig.Get().FoupTxfLogPath.Replace("\\","/");
                path = path.EndsWith("/") ? path : path + "/" ;
                string date = System.DateTime.Now.ToString("yyyyMMdd");
                string fullPath = path + date  + "/" + file_name;
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                string data = "";
                //寫出列名稱
                data = "from_port,from_id,from_slot,to_port_id,to_id,to_slot,t7,m12,start_datedime,end_datetime,load_datetime,unload_datetime,recipe_file,login_user";
                sw.WriteLine(data);
                //寫出各行數據
                for (int i = 0; i < record.Length; i++)
                {
                    if (record[i] == null)
                    {
                        continue;
                    }
                    data = "";
                    string[] column = record[i].getData();
                    for (int j = 0; j < column.Length; j++)
                    {
                        string str = column[j] == null ? "" : column[j].ToString();
                        str = string.Format("\"{0}\"", str).Replace("\r", "\\r").Replace("\n", "\\n");
                        data += str;
                        data += ",";
                    }
                    data += recipe_file + ",";
                    data += login_user;
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();
                //Process.Start(fullPath);打開檔案
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
        }
    }
    public class waferInfo{

        string from_port;
        string from_id;
        string from_slot;
        string to_port_id;
        string to_id;
        string to_slot;
        string t7;
        string m12;
        string start_datetime;
        string end_datetime;
        string load_datetime;
        string unload_datetime;

        public string[] getData()
        {
            return new string[] { from_port, from_id, from_slot, to_port_id, to_id, to_slot, t7, m12, start_datetime, end_datetime, load_datetime, unload_datetime };
        }
        public waferInfo(string from_port, string from_id, string from_slot, string to_port_id, string to_id, string to_slot)
        {
            this.from_port = from_port;
            this.from_id = from_id;
            this.from_slot = from_slot;
            this.to_port_id = to_port_id;
            this.to_id = to_id;
            this.to_slot = to_slot;
        }

        public void SetStartTime(DateTime timeStamp)
        {
            this.start_datetime = timeStamp.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }
        public void SetEndTime(DateTime timeStamp)
        {
            this.end_datetime = timeStamp.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }
        public void setM12(string m12)
        {
            this.m12 = m12;
        }
        public void setT7(string t7)
        {
            this.t7 = t7;
        }
        public void SetLoadTime(DateTime timeStamp)
        {
            this.load_datetime = timeStamp.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }
        public void SetUnloadTime(DateTime timeStamp)
        {
            this.unload_datetime = timeStamp.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }
    }
}
