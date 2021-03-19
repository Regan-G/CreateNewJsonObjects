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

		private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
		{
			
		}

		private void listBox1_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				if (listBox1.Items.Count > 0)
				{
					String[] items = listBox1.SelectedItem.ToString().Split(':');
					if (items.Length > 1)
					{
						textBox2.Text = items.ElementAt(0);
						textBox3.Text = items.ElementAt(1);
					}
				}
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
		}

		private ListBox updateListBox(ListBox listB, int index, string text)
		{
			try
			{
				listBox1.Items.RemoveAt(index);
				listBox1.Items.Insert(index, text);
			}
			catch(Exception ex) { MessageBox.Show(ex.Message); }
			return listB;
		}

		private void textBox2_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				listBox1 = updateListBox(listBox1, listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);
				//listBox1.Items.RemoveAt(listBox1.SelectedIndex);
				//listBox1.Items.Insert(listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);
			}
		}

		private void textBox3_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				listBox1 = updateListBox(listBox1, listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);
				//listBox1.Items.RemoveAt(listBox1.SelectedIndex);
				//listBox1.Items.Insert(listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			String path = Directory.GetCurrentDirectory();
			String text = readInCodeFile(path);
			richTextBox1.Text = text;
		}

		private String readInCodeFile(String path)
		{
			String text = null;
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "CNJF create new json file|*.txt";
				openFileDialog.Title = "Open code sample file";
				openFileDialog.InitialDirectory = path;
				openFileDialog.ShowDialog();
				if (openFileDialog.FileName != "")
				{
					text = File.ReadAllText(openFileDialog.FileName);
				}
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
			return text;
		}
	}
}
