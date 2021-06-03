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
			InitializeComponent();
			button2_Click(null, null);
		}
		AddressBusiness adrb = new AddressBusiness();
		UserBusiness usrb = new UserBusiness();
		AccountBusiness a = new AccountBusiness();
		BalanceBusiness b = new BalanceBusiness();
		CreditBusiness c = new CreditBusiness();
		HistoryBusiness d = new HistoryBusiness();
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

			usrb.Add(usr);
			adrb.Add(adr);
			b.Add(bal);
			a.Add(acc);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var a = adrb.GetAll();
			var b = a.Select(ex => $"{ex.Id} - {ex.Block} {ex.Street} {ex.Neighborhood} {ex.Country} {ex.Floor}");
			label1.Text = string.Join("\n", b);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				usrb.DeleteBulk(x => true);
			}
			catch (Exception) { }
			try
			{
				adrb.DeleteBulk(x => true);
			}
			catch (Exception) { }
		}
	}
}