using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobiiForm 
{

    public class JsonObject
    {
        public String localTimeStamp;
        public String[] eyeGazeLocation = new String[2];
        public String app;
        public String selfReport;
        

        public JsonObject( String timeStamp, String x, String y, String tab, String result)
        {
            
            localTimeStamp = timeStamp;
            eyeGazeLocation[0] = x;
            eyeGazeLocation[1] = y;
            app = tab;
            selfReport = result;
        }



        //NaN Object...
        public JsonObject(String timeStamp)
        {
            localTimeStamp = timeStamp;
        }
    }


}
