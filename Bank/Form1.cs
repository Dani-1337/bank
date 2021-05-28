using Bank_Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bank_Data.Context;
using Bank_Business;

namespace Bank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        AddressBusiness adrb = new AddressBusiness();
        private void button1_Click(object sender, EventArgs e)
        {
            var random = new Random();
            Address adr = new Address();
            adr.Block = random.Next(0, 1000);
            adr.Street = "Street #" + random.Next(0, 1000).ToString();
            adr.Neighborhood = "Geto #" + random.Next(0, 1000).ToString();
            adr.Country = "palatka #" + random.Next(0, 1000).ToString();
            adr.Floor = random.Next(0, 10);

            adrb.Add(adr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = string.Join("\n", adrb.GetAll().Select(ex => $"{ex.Block} {ex.Street} {ex.Neighborhood} {ex.Country} {ex.Floor}"));
        }
    }
}
