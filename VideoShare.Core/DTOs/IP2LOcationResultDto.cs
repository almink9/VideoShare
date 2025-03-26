using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoShare.Core.DTOs
{
  public class IP2LOcationResultDto
  {
    public string IP { get; set; }
    public string Country_Code { get; set; }
    public string Country_Name { get; set; }
    public string Region_Name { get; set; }
    public string City_Name { get; set; }
    public string Zip_Code { get; set; }
    public string Is_Proxy { get; set; }
  }
}
