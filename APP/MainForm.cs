﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SpeakRec
{
    public partial class MainForm : Form
    {
        public Recorder recorder;
        public string audioFile = "*.wav;*.aac;*.wma;*.wmv;*.mp3";
        public string[] videoFile =
        {
            "dat", "wmv", "3g2", "3gp", "3gp2", "3gpp", "amv", "asf", "avi", "bin", "cue", "divx", "dv", "flv", "gxf", "iso", "m1v", "m2v", "m2t", "m2ts", "m4v", "mkv", "mov", "mp2", "mp2v", "mp4", "mp4v", "mpa", "mpe", "mpeg", "mpeg", "mpeg", "mpeg", "mpg", "mpv2", "mts", "nsv", "nuv", "ogg", "ogm", "ogv", "ogx", "ps", "rec", "rm", "rmvb", "tod", "ts", "tts", "vob", "vro", "webm"
        };
        public string filePath, fileName;
        public bool isVideo = false;
        private int indexItemSubSelect;
        private System.Diagnostics.Process process;
        public MainForm()
        {
            InitializeComponent();
            listPerson.Columns.Add("", 0);
            listPerson.Columns.Add("Tên", 300);
            listPerson.View = View.Details;
            listPerson.GridLines = true;
            listPerson.FullRowSelect = true;
            ListSub.Columns.Add("Tên", 125);
            ListSub.Columns.Add("Thời gian", 225);
            ListSub.Columns.Add("Văn bản", 1000);
            ListSub.View = View.Details;
            ListSub.GridLines = true;
            ListSub.FullRowSelect = true;
            initServer();
            Utils.disableButton(btnShowSub, Properties.Resources.play, showSubToolStripMenuItem);
            Utils.disableButton(btnExportText, Properties.Resources.export, exportSubToolStripMenuItem);
            Utils.disableButton(btnOpenFile, Properties.Resources.open, openFileToolStripMenuItem);
            Utils.disableButton(btnRecord, Properties.Resources.record, recordToolStripMenuItem);
        }

        public void loadListJoin()
        {
            API.GetListJoin(resJoin =>
            {
                List<Person> listJoin = resJoin.data;
                listPerson.Items.Clear();
                ListViewItem itm;
                for (int i = 0; i < listJoin.Count; i++)
                {
                    itm = new ListViewItem(new string[] {
                "",listJoin[i].name });
                    listPerson.Items.Add(itm);
                }
                if (listPerson.Items.Count > 0)
                {
                    Utils.enableButton(btnRecord, Properties.Resources.record, recordToolStripMenuItem);
                    Utils.enableButton(btnOpenFile, Properties.Resources.open, openFileToolStripMenuItem);
                }
                else
                {
                    Utils.disableButton(btnRecord, Properties.Resources.record, recordToolStripMenuItem);
                    Utils.disableButton(btnOpenFile, Properties.Resources.open, openFileToolStripMenuItem);
                }
            });

        }

        private void putTextNameFile()
        {
            this.Text = this.fileName + " - " + this.filePath;
        }

        private void putTextLengthSound(string lengthSound)
        {

            this.Text = this.fileName + " - " + this.filePath + " - " + lengthSound;
        }


        public void disableButton()
        {
            Utils.disableButton(btnShowSub, Properties.Resources.play, showSubToolStripMenuItem);
            Utils.disableButton(btnExportText, Properties.Resources.export, exportSubToolStripMenuItem);
            Utils.disableButton(btnOpenFile, Properties.Resources.open, openFileToolStripMenuItem);
        }

        public void enableButton()
        {
            Utils.enableButton(btnShowSub, Properties.Resources.play, showSubToolStripMenuItem);
            Utils.enableButton(btnExportText, Properties.Resources.export, exportSubToolStripMenuItem);
            Utils.enableButton(btnOpenFile, Properties.Resources.open, openFileToolStripMenuItem);
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Mở file âm thanh";
            dlg.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.filePath = dlg.FileName;
                this.fileName = filePath.Split('\\')[filePath.Split('\\').Length - 1];
                putTextNameFile();
                string ext = fileName.Split('.')[1];
                isVideo = false;
                foreach (string str in videoFile)
                    if (ext.Equals(str))
                    {
                        isVideo = true;
                        break;
                    }

                API.GenSub(filePath, json =>
                {
                    addSubToListSub(json.path);
                    putTextLengthSound(new AudioFileReader(filePath).TotalTime.ToString().Split('.')[0]);
                    enableButton();
                });

            }
        }


        private void initServer()
        {
            string cmdText = @"/C cd ./core/ && .\venv\Scripts\uvicorn.exe start_server:app";
            process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmdText;
            process.StartInfo = startInfo;
            process.Start();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                API.ClearJoin(() => {
                    process.CloseMainWindow();
                });
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void btnShowSub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            string pathCmd = "";
            pathCmd = '"' + filePath;
            pathCmd += '"';
            string strCmdText = @"/K .\VLC\vlc.exe " + pathCmd;
            if (!isVideo)
                strCmdText += " --audio-visual=visual --effect-list=spectrum";
            startInfo.Arguments = strCmdText;
            process.StartInfo = startInfo;
            process.Start();
        }

        private void btnShowListPerson_Click(object sender, EventArgs e)
        {
            new ListPersonForm(this).ShowDialog();
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (btnRecord.Tag.ToString().Equals("r"))
            {
                disableButton();
                btnRecord.Tag = "s";
                btnRecord.BackgroundImage = Properties.Resources.stop;
                this.filePath = ".\\cache\\meeting\\";
                this.fileName = DateTime.Now.Ticks + ".wav";
                recorder = new Recorder(0, filePath, fileName, (text) =>
                {
                    putTextLengthSound(text);
                });
                recorder.StartRecording();
                putTextNameFile();
                this.filePath = filePath + fileName;
                string ext = fileName.Split('.')[1];
                isVideo = false;
                foreach (string str in videoFile)
                    if (ext.Equals(str))
                    {
                        isVideo = true;
                        break;
                    }
            }
            else
            {
                StopRecord();
            }
        }

        private void btnExportText_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Lưu file văn bản";
            dlg.Filter = "Văn bản|*.txt;";
            dlg.FileName = this.fileName.Split('.')[0];
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string createText = "";
                foreach (ListViewItem item in ListSub.Items)
                    createText += item.SubItems[0].Text + ": " + item.SubItems[2].Text + "\n\n";
                File.WriteAllText(dlg.FileName, createText);
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = @"/K " + dlg.FileName;
                process.StartInfo = startInfo;
                process.Start();
            }
        }

        private void addSubToListSub(string pathSub = @"C:\Users\hoang\Downloads\Buoc Qua Mua Co Don - Vu.srt")
        {
            tbName.Enabled = false;
            tbName.Text = String.Empty;
            tbSub.Enabled = false;
            tbSub.Text = String.Empty;
            string text = File.ReadAllText(pathSub);
            ListSub.Items.Clear();
            var arr = text.Split(new string[] { "\n\n" }, StringSplitOptions.None);
            foreach (var item in arr)
            {
                try
                {
                    var subArr = item.Split('\n');
                    var nameItem = "Không xác định";
                    var textItem = subArr[2];
                    if (subArr[2].Split(':').Length > 1)
                    {
                        nameItem = subArr[2].Split(':')[0].Trim();
                        textItem = subArr[2].Split(':')[1].Trim();
                    }
                    ListViewItem itm = new ListViewItem(new string[] {
                    nameItem, subArr[1], textItem
                });
                    ListSub.Items.Add(itm);
                }
                catch (Exception)
                {

                }
            }
        }

        private void ListSub_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                indexItemSubSelect = e.ItemIndex;
                tbName.Enabled = true;
                tbSub.Enabled = true;
                tbName.Text = e.Item.SubItems[0].Text;
                tbSub.Text = e.Item.SubItems[2].Text;
            }
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            ListSub.Items[indexItemSubSelect].SubItems[0].Text = tbName.Text;
        }

        private void tbSub_TextChanged(object sender, EventArgs e)
        {
            ListSub.Items[indexItemSubSelect].SubItems[2].Text = tbSub.Text;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ManagerForm(this).ShowDialog();
        }

        private void StopRecord()
        {
            btnRecord.Tag = "r";
            recorder.RecordEnd();
            API.GenSub("." + this.filePath, (json) =>
            {
                addSubToListSub(Path.GetFullPath(filePath.Replace("wav", "srt")));
                enableButton();
                btnRecord.BackgroundImage = Properties.Resources.record;
            });
        }
    }
}
