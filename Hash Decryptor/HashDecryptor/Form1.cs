using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HashDecryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static char[] fCharList =
        {
        '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p',
                    'q','r','s','t','u','v','w','x','y','z','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
                    'Q','R','S','T','U','V','X','Y','Z','~','!','@','#','$','%','^','&','*','(',')','[',']','{','}','.',',','/','?','\'','|','"',';',':','<','>','\\','=','-','+','`','_'
        };
        /*private static char[] fCharList =
        {
        '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j' ,'k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
        };*/
        private StringBuilder psb = new StringBuilder();
        private ulong len;
        private int _max;
        private int _min;
        public int max { get { return _max; } set { _max = value; } }
        public int min { get { return _min; } set { _min = value; } }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(CurHash.Text))
            {
               Decrypt();
            }
        }

        public void Decrypt()
        {
            var threat = new Thread(() =>
            {
                string password = fCharList[0].ToString();
                for (int i = 0; i < 5000000; i++)
                {
                    this.ASCIIOutput.BeginInvoke((MethodInvoker)delegate () { this.ASCIIOutput.Text = password.ToString(); this.ASCIIOutput.Invalidate(); this.ASCIIOutput.Update(); ; });
                    this.HashOutput.BeginInvoke((MethodInvoker)delegate () { this.HashOutput.Text = CalculateMD5Hash(password); this.HashOutput.Invalidate(); this.HashOutput.Update(); ; });
                    this.AttemptCount.BeginInvoke((MethodInvoker)delegate () { this.AttemptCount.Text = i.ToString(); this.AttemptCount.Invalidate(); this.AttemptCount.Update(); ; });
                    if (HashOutput.Text == CurHash.Text.ToUpper())
                    {
                        ASCIIOutput.ForeColor = Color.Green;
                        break;
                    }
                    password = IncreasePassword(password);
                }
            });
            threat.Start();
        }

        public static string IncreasePassword(String pass)
        {
            bool changed = false;
            StringBuilder sb = new StringBuilder(pass);
            for (int i = 0; i < pass.Length && !changed; i++)
            {
                int index = Array.IndexOf(fCharList, sb[i]);
                if (index < fCharList.Length - 1)
                {
                    for (int j = i - 1; j >= 0 && sb[j] == fCharList[fCharList.Length - 1]; j--)
                    {
                        sb[j] = fCharList[0];
                    }
                    sb[i] = fCharList[index + 1];
                    changed = true;
                }
            }
            if (!changed)
            {
                return "".PadLeft(pass.Length + 1, fCharList[0]);
            }
            return sb.ToString();
        }

        public String CalculateMD5Hash(String Input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
