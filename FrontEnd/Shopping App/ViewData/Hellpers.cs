using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopping_App.ViewData
{
    internal class Hellpers
    {
        public static void ClearForm(Form form)
        {
            form.Controls.OfType<Control>().ToList().ForEach(c =>
            {
                if (c.GetType() != typeof(MenuStrip))
                {
                    form.Controls.Remove(c);
                }
            });
        }
    }
}
