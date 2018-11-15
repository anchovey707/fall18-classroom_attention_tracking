using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobiiForm
{
    class Window
    {
         String tabInfo;
        private iFocus.Rect rectangle;
        private string v;

        public Window(String tabInfo, iFocus.Rect rect)
        {
            this.tabInfo = tabInfo;
            this.rectangle = rect;
        }

      

        public string GetTabInfo()
        {
            return tabInfo;
        }

        public void SetTabInfo(string value)
        {
            tabInfo = value;
        }

        public iFocus.Rect GetRectangleInfo()
        {
            return rectangle;
        }

        public void SetRectangleInfo(iFocus.Rect rect)
        {
            rectangle = rect;
        }

    }
}
