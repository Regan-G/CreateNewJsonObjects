﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateNewJsonFile
{
	public partial class Form1 : Form
	{
		List<string> list = new List<string>();
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			String path  = Directory.GetCurrentDirectory();
			String[] items = importFile(path);
			if (items != null)
			{
				char[] delimiter = textBox1.Text.ToCharArray();
				String[] returnData = splitFile(items, delimiter);
				if (returnData != null)
				{
					listBox1.Items.Clear();
					foreach (string line in returnData)
						listBox1.Items.Add(line);
				}
				
			}
			
		}

		private string[] splitFile(String[] items, char[] delimiter)
		{
			String[] returnData = null;
			try
			{
				String[] entry = null;
				//String[] variables = null;

				foreach (string line in items)
				{
					entry = line.Split(delimiter);					
					if (entry.Length > 1)
						list.Add(entry.ElementAt(0) + ":" + entry.ElementAt(1));					
				}
				returnData = list.ToArray();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			return returnData;
		}

		private String[] importFile(String path)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "CNJF create new json file|*.txt";
			openFileDialog.Title = "Open list of variables and types file";
			openFileDialog.InitialDirectory = path;
			openFileDialog.ShowDialog();
			String[] text = null;
			if (openFileDialog.FileName != "")
			{
				text = File.ReadAllLines(openFileDialog.FileName);				
			}
			return text;
		}
	}
}
