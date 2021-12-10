using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace AudioPlayer
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer Anton = new WindowsMediaPlayer();
        string[] Musicas = new string[1000];
        List<string> Musica = new List<string>();
        int count;

        bool pauseplay;
        string[] path, strfilename;

        public Form1()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All files|*.*|MP3 files|*.mp3|WAV files|*.wav";
            open.FilterIndex = 1;

            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                strfilename = open.SafeFileNames;
                path = open.FileNames;
                Anton.URL = open.FileName;
                for (int i = 0; i < open.FileNames.LongLength; i++)
                {
                    if(listBox1.Items.Contains(open.FileNames.GetValue(i)) == false)
                    {
                        listBox1.Items.Add(open.SafeFileNames.GetValue(i));
                        Musicas[count] = open.FileNames.GetValue(i).ToString();
                        count += 1; 
                    }
                }

                if (listBox1.SelectedIndex < 0) { listBox1.SetSelected(0, true); }
                else if (listBox1.Items.Count >= 1) listBox1.SelectedIndex = listBox1.Items.Count - strfilename.Length;
                else listBox1.SetSelected(0, true);

                trackBar2.Enabled = true;
                timer1.Enabled = true;
                timer1.Interval = 1000;
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Anton.URL = Musicas[listBox1.SelectedIndex];
                timer1.Start();
                Anton.settings.volume = trackBar1.Value;
            }
            catch
            { 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Anton.controls.stop();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Anton.settings.volume = trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Anton.controls.currentPosition = trackBar2.Value;
            if (listBox1.SelectedIndex > 0) { trackBar1.Enabled = true; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar2.Maximum = Convert.ToInt32(Anton.currentMedia.duration);
            trackBar2.Value = Convert.ToInt32(Anton.controls.currentPosition);

            if (Anton != null)
            {
                int s = (int)Anton.currentMedia.duration;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                label2.Text = String.Format("{0:D}:{1:D2}:{2:D2}",h,m,s);

                s = (int)Anton.controls.currentPosition;
                h = s / 3600;
                m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                label1.Text = String.Format("{0:D}:{1:D2}:{2:D2}", h, m, s);
            }
            else
            {
                label2.Text = "0:00:00";
                label1.Text = "0:00:00";
            }    
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
                try
                {
                    if (listBox1.SelectedIndex != listBox1.Items.Count - 1)
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                        Anton.URL = Musicas[listBox1.SelectedIndex];
                        listBox1.Text = Anton.currentPlaylist.Item[1].name;
                    }
                    else
                    {
                        listBox1.SetSelected(listBox1.SelectedIndex - listBox1.Items.Count + 1, true);
                        Anton.URL = Musicas[listBox1.SelectedIndex];
                        listBox1.Text = Anton.currentPlaylist.Item[1].name;
                    }
                }
                catch
                {

                }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                if(listBox1.SelectedIndex != 0)
                {
                    listBox1.SetSelected(listBox1.SelectedIndex - 1, true);
                    Anton.URL = Musicas[listBox1.SelectedIndex];
                    listBox1.Text = Anton.currentPlaylist.Item[0].name;
                }
                else
                {
                    listBox1.SetSelected(listBox1.SelectedIndex + listBox1.Items.Count - 1, true);
                    Anton.URL = Musicas[listBox1.SelectedIndex];
                    listBox1.Text = Anton.currentPlaylist.Item[0].name;
                    button2_Click(sender, e);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "MP3 files|*.mp3|WMV files|*.wmv";
            open.FilterIndex = 1;

            if (open.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < open.FileNames.LongLength; i++)
                {
                    if (listBox1.Items.Contains(open.FileNames.GetValue(i)) == false)
                    {
                        listBox1.Items.Add(open.SafeFileNames.GetValue(i));
                        Musicas[count] = open.FileNames.GetValue(i).ToString();
                        count += 1;
                    }
                }    
                if (listBox1.SelectedIndex < 0) { listBox1.SetSelected(0, true); }
            }
            if (listBox1.SelectedIndex > 0) { trackBar2.Enabled = true; }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button4_Click(sender, e);
            count = 0;
            trackBar2.Value = 0;
            trackBar2.Enabled = false;
            label2.Text = "0:00:00";
            label1.Text = "0:00:00";
            timer1.Stop();
            if (listBox1.SelectedIndex > 0) { trackBar2.Enabled = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                Anton.controls.play();
                pauseplay = false;
                trackBar2.Enabled = true;
                timer1.Enabled = true;
                timer1.Interval = 1000;
                Anton.URL = Musicas[listBox1.SelectedIndex];
                timer1.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pauseplay = !pauseplay;
            if (pauseplay)
            {
                Anton.controls.pause();
            }
            if (!pauseplay)
            {
                Anton.controls.play();
            }
        }



    }
}
