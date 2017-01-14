using System.Windows.Forms;

namespace AutoUpdater_Demo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            /* Onload Event */
            Text = "v" + AutoUpdater.CheckForUpdate(0.1m, "https://raw.githubusercontent.com/BISOON/AutoUpdater/master/prop.json");
        }
    }
}
