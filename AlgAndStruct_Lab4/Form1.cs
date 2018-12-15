using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgAndStruct_Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static long capacity = 10;
        SmallHashTable<string> smallHashTableConcat = new SmallHashTable<string>(Convert.ToInt32(capacity), HashType.ConcatHash);
        SmallHashTable<string> smallHashTableAdaptive = new SmallHashTable<string>(Convert.ToInt32(capacity), HashType.AdaptiveHash);

        private void button1_Click(object sender, EventArgs e)
        {
            string[] rich = richStringTextBox.Text.Split(' ');
            foreach (var a in rich)
                smallHashTableConcat[a] = a;

            int[] collisions = smallHashTableConcat.collisions;

            foreach (var a in collisions)
                chart1.Series[0].Points.Add(Convert.ToDouble(a));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] rich = richStringTextBox.Text.Split(' ');

            foreach (var a in rich)
                smallHashTableAdaptive[a] = a;

            int[] collisions = smallHashTableAdaptive.collisions;

            foreach (var a in collisions)
                chart1.Series[0].Points.Add(Convert.ToDouble(a));
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
        }

        private void capacityButton_Click(object sender, EventArgs e)
        {
            capacity = Convert.ToUInt32(capacityTextBox.Text);
            smallHashTableConcat=new SmallHashTable<string>(Convert.ToInt32(capacity), HashType.ConcatHash);
            smallHashTableAdaptive = new SmallHashTable<string>(Convert.ToInt32(capacity), HashType.ConcatHash);
        }
    }
}