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
using Bank_Business.Business;

namespace Bank
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			xx = new CreditBusiness();

			InitializeComponent();

			button1.Name = "Add random to DB";
			button2.Name = "Read from DB";
			button3.Name = "Delete ALL from DB";

			button2_Click(null, null);
		}
		CreditBusiness xx;
		private void button1_Click(object sender, EventArgs e)
		{
			var random = new Random();
			Address adr = new Address();
			adr.Block = random.Next(0, 1000);
			adr.Street = "Street #" + random.Next(0, 1000).ToString();
			adr.Neighborhood = "Geto #" + random.Next(0, 1000).ToString();
			adr.Country = "palatka #" + random.Next(0, 1000).ToString();
			adr.Floor = random.Next(0, 10);

			User usr = new User();
			usr.Address = adr;
			usr.Age = 23;
			usr.Gender = Gender.Male;
			usr.LastName = "asd";
			usr.Name = "hgf";

			Balance bal = new Balance();
			bal.Credits = null;
			bal.Histories = null;
			bal.MainBalance = 213;

			Account acc = new Account();
			acc.Balance = bal;
			acc.Email = "sg";
			acc.Password = "hgt";
			acc.User = usr;


			List<History> hs = new List<History>();
			History hs1 = new History();
			History hs2 = new History();
			hs1.Date = DateTime.Now;
			hs1.Value = 213;
			hs2.Date = DateTime.Now;
			hs2.Value = 21323;
			hs.Add(hs1);
			hs.Add(hs2);
			Credit c1 = new Credit();
			c1.CreateDate = DateTime.Now;
			c1.CreditLeft = 213;
			c1.CreditTaken = 2135;
			c1.Histories = hs;

			xx.Add(c1);
		}

		private void button2_Click(object sender, EventArgs e)
		{
		}

		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				xx.DeleteBulk(x => true);
			}
			catch (Exception) { }
		}
	}
}