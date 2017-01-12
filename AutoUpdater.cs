using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization /* Add System.Web.Extesnion Reference */;
using System.Windows.Forms;

class AutoUpdater
{
    private class Properties
    {
        public string Version { get;   set; }
        public string DownloadUrl { get;  set; }
        public string ChangeLog{ get;  set; }
        public bool IsClosed { get;  set; }
        public string ClosedMessage { get;  set; }
    }
    private static string _mediaFireDirectLink(string mediaFirelink)
    {
        return Regex.Match(new WebClient().DownloadString(mediaFirelink), @"http://download.*.mediafire.com.*(.[\w])").Value;
    }
    private static void _Close()
    {
        System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
    private static DialogResult Msg(object msg, MessageBoxIcon icon, bool ques = false)
    {
        return MessageBox.Show(msg.ToString(), "Message", ques ? MessageBoxButtons.YesNo : MessageBoxButtons.OK, icon);
    }
    /// <summary>
    /// Check For Updates With Message Box
    /// </summary>
    /// <param name="currentVersion">Current Application Version</param>
    /// <param name="urlLink">The url that contains your application details as JSON</param>
    /// <returns>Return Current Version</returns>
    public static decimal CheckForUpdate(decimal currentVersion, string urlLink)
    {
        try
        {
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                Properties prop = new JavaScriptSerializer().Deserialize<Properties>(wb.DownloadString(urlLink));
                if (prop.IsClosed)
                {
                    Msg(prop.ClosedMessage, MessageBoxIcon.Error);
                    _Close();
                }
                if ((decimal.Parse(prop.Version) > currentVersion))
                {
                    if (Msg("New Update Available!\nCurrent Version: " + currentVersion + "\nNew Version: " + prop.Version, MessageBoxIcon.Information, true) == DialogResult.Yes)
                    {
                        using (SaveFileDialog sf = new SaveFileDialog() { Filter = "Rar File |*.rar", FileName = "M7MD v" + prop.Version})
                        {
                            if (sf.ShowDialog() == DialogResult.OK)
                            {
                                wb.DownloadFile(_mediaFireDirectLink(prop.DownloadUrl), sf.FileName);
                                Msg("File Has Been Downloaded", MessageBoxIcon.Asterisk);
                                _Close();
                            }
                        }
                    }
                    else
                    {
                        Msg("You have To use the new Version: " + prop.Version, MessageBoxIcon.Error);
                        _Close();

                    }
                }

            }
        }
        catch (Exception ex)
        {
            Msg(ex.Message, MessageBoxIcon.Error);
            _Close();
        }

        return currentVersion;
    }
}
