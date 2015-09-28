using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Connect
{
    public partial class Form1 : Form
    {
        private Core core = new Core();
        public Form1()
        {
            InitializeComponent();
            core.NewGame();
            bool b = core[0, 0].pc;
        }
    }
}
