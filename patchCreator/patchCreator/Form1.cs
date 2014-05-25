using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace patchCreator
{
    public partial class Form1 : Form
    {
        string substringDirectory;
        string substringFile;

        string baseUri;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            DialogResult res = folderBrowserDialog1.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                treeView1.Nodes.Add(folderBrowserDialog1.SelectedPath);
                PopulateTreeView(folderBrowserDialog1.SelectedPath, treeView1.Nodes[0]);
                locRootDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        public void PopulateTreeView(string directoryValue, TreeNode parentNode)
        {
            string[] directoryArray =
             Directory.GetDirectories(directoryValue);

            try
            {
                if (directoryArray.Length != 0)
                {
                    foreach (string directory in directoryArray)
                    {
                        substringDirectory = directory.Substring(
                        directory.LastIndexOf('\\') + 1,
                        directory.Length - directory.LastIndexOf('\\') - 1);

                        TreeNode myNode = new TreeNode(substringDirectory);

                        string[] fileArray = Directory.GetFiles(directory);

                        foreach (string file in fileArray)
                        {
                            substringFile = file.Substring(file.LastIndexOf('\\') + 1, file.Length - file.LastIndexOf('\\') - 1);
                            TreeNode fileNode = new TreeNode(substringFile);
                            myNode.Nodes.Add(fileNode);
                        }

                        parentNode.Nodes.Add(myNode);

                        PopulateTreeView(directory, myNode);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                parentNode.Nodes.Add("Access denied");
            } // end catch
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.Nodes.Remove(treeView1.SelectedNode);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0)
            {
                MessageBox.Show(this, "Keine Daten", "tree", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            } else
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show(this, "Serverpfad angeben!", "path", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                List<DownloadItem> items = fillDownloadItemList(treeView1.Nodes[0]);
                string statement = "";

                foreach (DownloadItem it in items)
                {
                    String state = String.Format("INSERT INTO patchers (hashCode, urlFiles, version, isActive, isForMac, created_at, updated_at, name, urlLocalDir)" +
                        " VALUES (\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\"); ", it.md5, it.uri, tb_Version.Text, 0, checkBox1.Checked ? 1 : 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), it.name, it.locrelPath);
                    statement += state + "\n";
                }
                StreamWriter outfile = new StreamWriter(Application.StartupPath + "DB-Statement.txt");
                outfile.Write(statement);
                outfile.Close();
            }

        }

        List<DownloadItem> fillDownloadItemList(TreeNode node)
        {
            List<DownloadItem> lst = new List<DownloadItem>();

            foreach (TreeNode n in node.Nodes)
            {
                if (n.GetNodeCount(true) == 0) //
                {
                    if (!Directory.Exists(n.FullPath))
                    {
                        String s = n.FullPath.Replace(locRootDir.Text, "");
                        s = s.Replace("\\", "/");
                        lst.Add(new DownloadItem(textBox1.Text + s, n.Text, s, n.FullPath));
                    }
                }
                else
                {
                    lst.AddRange(fillDownloadItemList(n));
                }
            }
            return lst;
        }

        private void tb_Version_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                )
            {
                e.Handled = true;
            }

            // only allow one decimal point
        }
    }

    class DownloadItem
    {
        public string uri;
        public string locrelPath;
        public string name;
        public string md5;

        public DownloadItem(string uri, string name, string locrelPath, string localpath)
        {
            this.uri = uri;
            this.name = name;
            this.locrelPath = locrelPath;
            md5 = getMD5ofFile(localpath);
        }

        public string getMD5ofFile(string s) //gets MD5 from File on Disc!!
        {
            FileStream f = File.OpenRead(s);

            byte[] data = new byte[f.Length];
            f.Read(data, 0, (int)f.Length);

            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(data);

            f.Close();

            return BitConverter.ToString(hash);
        }
    }
}
