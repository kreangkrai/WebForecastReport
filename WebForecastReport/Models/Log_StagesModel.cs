using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Models
{
    public class Log_StagesModel
    {
		public int Id { get; set; }
		public string quotation { get; set; }
		public string project_name { get; set; }
		public string date_edit { get; set; }
		public string stages_from { get; set; }
		public string stages_to { get; set; }
		public string reason { get; set; }
		public string name { get; set; }
	}
}
