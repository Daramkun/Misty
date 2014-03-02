using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Daramkun.Blockar.Json;

namespace Daramkun.Misty.Utils.StringTableEditor
{
	public partial class MainWindow : Form
	{
		JsonContainer data;
		JsonContainer opened = null;
		string editKey;

		string savePath = null;
		bool saved = true;

		private void SetTitle () { Text = ( savePath != null ? Path.GetFileName ( savePath ) : "Untitled.json" ) + " - Misty Framework String Table Editor"; }

		private bool CheckSave ()
		{
			if ( saved ) return true;
			else
			{
				DialogResult mb = MessageBox.Show ( "Are you want to save this?", "String Table Editor", MessageBoxButtons.YesNoCancel );
				switch ( mb )
				{
					case DialogResult.Yes:
						return SaveFile ();
					case DialogResult.No:
						return true;
					default:
						return false;
				}
			}
		}

		private void NewFile ()
		{
			if ( !CheckSave () ) return;

			saved = true;
			savePath = null;

			data = new JsonContainer ( ContainType.Object );
			data.Add ( new JsonContainer ( ContainType.Object ), "unknown" );
			listViewLanguages.Items.Add ( "unknown" );

			SetTitle ();
		}

		private void OpenFile ()
		{
			if ( !CheckSave () ) return;

			if ( openFileDialog1.ShowDialog () == DialogResult.Cancel ) return;

			using ( Stream stream = new FileStream ( openFileDialog1.FileName, FileMode.Open, FileAccess.Read ) )
			{
				data = JsonParser.Parse ( stream );
				listViewLanguages.Items.Clear ();
				listViewEditor.Items.Clear ();
				listViewEditor.Enabled = false;
				opened = null;
				foreach ( KeyValuePair<object, object> i in data.GetDictionaryEnumerable () )
					listViewLanguages.Items.Add ( i.Key as string );
			}

			saved = true;
			savePath = openFileDialog1.FileName;

			SetTitle ();
		}

		private bool SaveFile ()
		{
			if ( savePath == null ) return SaveAsFile ();

			using ( Stream stream = new FileStream ( savePath, FileMode.Create, FileAccess.Write ) )
			{
				byte [] data;
				if ( Path.GetExtension ( saveFileDialog1.FileName ) == "bson" )
					data = this.data.ToBinary ();
				else data = Encoding.UTF8.GetBytes ( this.data.ToString () );
				stream.Write ( data, 0, data.Length );
			}
			saved = true;
			return true;
		}

		private bool SaveAsFile ()
		{
			if ( saveFileDialog1.ShowDialog () == DialogResult.Cancel ) return false;
			savePath = saveFileDialog1.FileName;
			SaveFile ();
			SetTitle ();
			return true;
		}

		private void AddLanguageFunc ()
		{
			AddLanguage window = new AddLanguage ();
			if ( window.ShowDialog () != DialogResult.OK ) return;
			if ( !data.Contains ( window.comboBox1.SelectedItem as string ) )
			{
				data.Add ( new JsonContainer ( ContainType.Object ), window.comboBox1.SelectedItem as string );
				listViewLanguages.Items.Add ( window.comboBox1.SelectedItem as string );
			}
			saved = false;
		}

		private void RemoveLanguage ()
		{
			if ( listViewLanguages.SelectedIndices.Count == 0 )
			{
				MessageBox.Show ( "Please select a language in Language Listview.", "String Table Editor" );
				return;
			}

			if ( listViewLanguages.SelectedItems [ 0 ].Text == "unknown" )
			{
				MessageBox.Show ( "Cannot remove the 'unknown' Language.", "String Table Editor" );
				return;
			}

			if ( MessageBox.Show ( "Are you really remove this language?", "String Table Editor", MessageBoxButtons.YesNo ) == DialogResult.No )
				return;

			string key = listViewLanguages.SelectedItems [ 0 ].Text;
			listViewLanguages.Items.Remove ( listViewLanguages.SelectedItems [ 0 ] );
			if ( data [ key ] == opened )
			{
				listViewEditor.Items.Clear ();
				listViewEditor.Enabled = false;
			}
			data.Remove ( key );
			saved = false;
		}

		private void AddItem ()
		{
			if ( opened == null ) return;

			int i = GetLastestNoNamedNumber ();
			opened.Add ( "", "nonamed" + i );
			listViewEditor.Items.Add ( "nonamed" + i );
			saved = false;
		}

		private void RemoveItem ()
		{
			if ( opened == null ) return;

			if ( listViewEditor.SelectedItems.Count > 0 )
			{
				string key = listViewEditor.SelectedItems [ 0 ].Text;
				opened.Remove ( key );
				listViewEditor.Items.Remove ( listViewEditor.SelectedItems [ 0 ] );
			}
			saved = false;
		}

		public MainWindow ()
		{
			InitializeComponent ();

			NewFile ();
		}

		private void addLanguageToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			AddLanguageFunc ();
		}

		private void removeLanguageToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			RemoveLanguage ();
		}

		private int GetLastestNoNamedNumber ()
		{
			if ( opened == null ) return -1;
			int i = 0;
			foreach(KeyValuePair<object,object> o in opened.GetDictionaryEnumerable())
				if ( ( o.Key as string ).Substring ( 0, 7 ) == "nonamed" )
				{
					int temp;
					if ( !int.TryParse ( ( o.Key as string ).Substring ( 7, ( o.Key as string ).Length - 7 ), out temp ) )
						continue;
					if ( i < temp )
						i = temp;
				}
			return ++i;
		}

		private void addItemToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			AddItem ();
		}

		private void removeItemToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			RemoveItem ();
		}

		private void listViewLanguages_DoubleClick ( object sender, EventArgs e )
		{
			if ( listViewLanguages.SelectedItems.Count > 0 )
			{
				listViewEditor.Items.Clear ();
				string key = listViewLanguages.SelectedItems [ 0 ].Text;
				opened = data [ key ] as JsonContainer;
				listViewEditor.Enabled = true;
				foreach ( KeyValuePair<object, object> s in opened.GetDictionaryEnumerable () )
				{
					listViewEditor.Items.Add ( s.Key as string ).SubItems.Add ( s.Value as string );
				}
			}
		}

		private void listViewEditor_BeforeLabelEdit ( object sender, LabelEditEventArgs e )
		{
			editKey = listViewEditor.Items [ e.Item ].Text;
		}

		private void listViewEditor_AfterLabelEdit ( object sender, LabelEditEventArgs e )
		{
			if ( opened.Contains ( e.Label ) ) { e.CancelEdit = true; return; }
			string data = opened [ editKey ] as string;
			opened.Remove ( editKey );
			opened.Add ( data, e.Label );
		}

		private void listViewEditor_DoubleClick ( object sender, EventArgs e )
		{
			if ( listViewEditor.SelectedItems.Count > 0 )
			{
				EditItem editItem = new EditItem ();
				editItem.textBox1.Text = opened [ listViewEditor.SelectedItems [ 0 ].Text ] as string;
				if ( editItem.ShowDialog () == DialogResult.Cancel ) return;
				opened [ listViewEditor.SelectedItems [ 0 ].Text ] = editItem.textBox1.Text;
				if ( listViewEditor.SelectedItems [ 0 ].SubItems.Count != 2 )
					listViewEditor.SelectedItems [ 0 ].SubItems.Add ( editItem.textBox1.Text );
				else
					listViewEditor.SelectedItems [ 0 ].SubItems [ 1 ].Text = editItem.textBox1.Text;
			}
		}

		private void exitToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			Close ();
		}

		private void newToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			NewFile ();
		}

		private void openToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			OpenFile ();
		}

		private void saveToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			SaveFile ();
		}

		private void saveasToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			SaveAsFile ();
		}

		private void toolStripButton1_Click ( object sender, EventArgs e )
		{
			NewFile ();
		}

		private void toolStripButton2_Click ( object sender, EventArgs e )
		{
			OpenFile ();
		}

		private void toolStripButton3_Click ( object sender, EventArgs e )
		{
			SaveFile ();
		}

		private void toolStripButton9_Click ( object sender, EventArgs e )
		{
			AddLanguageFunc ();
		}

		private void toolStripButton10_Click ( object sender, EventArgs e )
		{
			RemoveLanguage ();
		}

		private void toolStripButton11_Click ( object sender, EventArgs e )
		{
			AddItem ();
		}

		private void toolStripButton12_Click ( object sender, EventArgs e )
		{
			RemoveItem ();
		}

		private void aboutToolStripMenuItem_Click ( object sender, EventArgs e )
		{
			new AboutWindow ().ShowDialog ();
		}

		private void toolStripButton13_Click ( object sender, EventArgs e )
		{
			new AboutWindow ().ShowDialog ();
		}

		private void MainWindow_FormClosing ( object sender, FormClosingEventArgs e )
		{
			e.Cancel = !CheckSave ();
		}
	}
}
