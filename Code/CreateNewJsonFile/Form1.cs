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
		List<string> languageList = new List<string>();
		List<string> fileList = new List<string>();
		String Descriptor = "{ListEntry}";
		String VariableName = "{VariableName}";
		String VariableType = "{VariableType}";
		public Form1()
		{
			InitializeComponent();
			readInSampleCodeFiles();
		}

		private void readInSampleCodeFiles()
		{
			String path = Directory.GetCurrentDirectory();
			if (File.Exists(path + "\\SampleCode.txt"))
			{
				String[] text = File.ReadAllLines(path + "\\SampleCode.txt");
				foreach (string line in text)
					fileList.Add(line.Substring(0, line.Length-4));
				foreach (string entry in fileList){
					String codeEntry = File.ReadAllText(path + "\\" + entry + ".txt");
						languageList.Add(entry + "|" + codeEntry);
				}				
			}
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
						list.Add(entry.ElementAt(0).Trim('"') + ":" + entry.ElementAt(1).Trim(' ').Trim('"'));
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
				list.RemoveAt(index);
				list.Insert(index, textBox2.Text + ":" + textBox3.Text);
			}
			catch(Exception ex) { MessageBox.Show(ex.Message); }
			return listB;
		}

		private void textBox2_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
				listBox1 = updateListBox(listBox1, listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);				
				
		}

		private void textBox3_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
				listBox1 = updateListBox(listBox1, listBox1.SelectedIndex, textBox2.Text + ":" + textBox3.Text);
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
					text = File.ReadAllText(openFileDialog.FileName).Trim('"');
				}
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
			return text;
		}

		private String replaceTextInFile(String text, String descriptor, String varName, String varType)
		{
			String test = null;
			try
			{
				test = text.Replace(Descriptor, descriptor);
				test = test.Replace(VariableName, varName);
				test = test.Replace(VariableType, varType);
			}
			catch(Exception ex) { MessageBox.Show(ex.Message); }
			return test.Trim('"');
		}

		private void button2_Click(object sender, EventArgs e)
		{
			String text = richTextBox1.Text;
			String descriptor = null;
			String varName = null;
			String varType = null;
			String[] entries = null;
			richTextBox2.Text = null;
			foreach (string item in list)
			{
				entries = item.Split(':');
				descriptor = item;
				varName = entries.ElementAt(0);
				varType = entries.ElementAt(1);
				richTextBox2.Text = richTextBox2.Text  + replaceTextInFile(text, descriptor, varName, varType) + "\r\n" + "\r\n";
			}			
		}

		private void button4_Click(object sender, EventArgs e)
		{
			String path = Directory.GetCurrentDirectory();
			String text = richTextBox2.Text;
			if (!exportFile(path, text))
				MessageBox.Show("Epic fail");
		}

		private Boolean exportFile(String path, String text)
		{
			Boolean success = false;
			try
			{
				SaveFileDialog saveFileDialog1 = new SaveFileDialog();
				saveFileDialog1.Filter = "CNJF create new json file|*.txt";
				saveFileDialog1.Title = "Save code file";
				saveFileDialog1.InitialDirectory = path;
				saveFileDialog1.ShowDialog();
				if (saveFileDialog1.FileName != "")
					File.AppendAllText(saveFileDialog1.FileName, text + Environment.NewLine);
				success = true;
			}
			catch (Exception ex) { MessageBox.Show(ex.Message); }
			return success;
		}

		private void comboBox1_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				if (richTextBox1.TextLength > 0)
				{
					languageList.Add(comboBox1.Text + "|" + richTextBox1.Text);
				}
			}
			
		}

		private void comboBox1_DropDown(object sender, EventArgs e)
		{
			if (languageList != null)
			{
				if (languageList.Count > 0)
				{
					comboBox1.Items.Clear();
					foreach (string item in languageList)
					{
						string[] entries = item.Split('|');
						comboBox1.Items.Add(entries.ElementAt(0));
					}						
				}
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.Text.Length > 0)
			{
				foreach (string item in languageList)
				{
					string[] entries = item.Split('|');
					string testComboBox = comboBox1.Text;
					string testLanguageList = entries.ElementAt(0);
					if (testComboBox.Equals(testLanguageList))
					{
						richTextBox1.Text = entries.ElementAt(1);
						break;
					}
				}
			}
		}
	}
}
