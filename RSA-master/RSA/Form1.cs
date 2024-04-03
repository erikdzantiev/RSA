using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA
{
    public partial class Form1 : Form
    {
        public int P;
        public int Q;
        public int E;
        public int D;
        public int N;
        public string Sentences;
        public Dictionary<char, int> Alfavit = new Dictionary<char, int>();


        public Form1()
        {
            InitializeComponent();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < alpha.Length; i++)
            {
                Alfavit.Add(alpha[i], (i + 1) % alpha.Length);
            }
        }

        private void buttonEncrypted_Click(object sender, EventArgs e)
        {
            P = int.Parse(textBoxP.Text);
            Q = int.Parse(textBoxQ.Text);
            Sentences = textBoxSentence.Text.ToUpper();
            StartRSA();
        }

        public void StartRSA()
        {
            N = P * Q;
            int m = (P - 1) * (Q - 1);
            E = GetE(m);//////////////
            D = GetInverseNumberforModule(E, m);///////////////
            List<int> T = new List<int>();
            for (int i = 0; i < Sentences.Length; i++)
            {
                    T.Add(Alfavit[Sentences[i]]);
            }
            List<long> C = new List<long>();
            for (int i = 0; i < T.Count; i++)
            {
                C.Add(PowForMod(T[i], E, N));
            }
            ShowEnCrypted(C);
        }

        public int GetE(int m)///////////
        {
            int res = 2;
            for (int i = 2; i < m; i++)/////////
            {
                if (IsPrime(i) && m % i != 0)//////////
                {
                    res = i;
                    break;
                }
            }
            return res;
        }

        public bool IsPrime(int a)
        {
            for (int i = 2; i <= a/2; i++)
            {
                if (a % i == 0) return false;
            }
            return true;
        }

        public int GetInverseNumberforModule(int opr, int mod)
        {
            for (int i = 0; i < mod; i++)
            {
                if ((opr * i) % mod == 1)
                {
                    return i;
                }
            }
            return 1;
        }

        public long PowForMod(int a, int pow, int mod)
        {
            long res = 1;
            for (int i = 0; i < pow; i++)
            {
                res *= a;
                res %= mod;
            }
            return res;
        }
        public void ShowEnCrypted(List<long> lst)
        {
            string s = "";
            foreach (var item in lst)
            {
                s += item.ToString() + ", ";
            }
            textBoxEncrypted.Text = s.Substring(0, s.Length-2);
        }
        public void ShowUnCrypted(List<long> lst)
        {
            string s = "";
            foreach (var item in lst)
            {
                s += Alfavit.FirstOrDefault(x => x.Value == item).Key;

            }
            textBoxUncrypted.Text = s;
        }

        private void buttonUncrypted_Click(object sender, EventArgs e)
        {
            List<int> C = new List<int>();
            string[] s = textBoxEncrypted.Text.Split(',');
            for (int i = 0; i < s.Length; i++)
            {
                C.Add(int.Parse(s[i].Trim()));
            }

            List<long> T = new List<long>();
            for (int i = 0; i < C.Count; i++)
            {
                T.Add(PowForMod(C[i], D, N));
            }
            ShowUnCrypted(T);
        }
    }
}
