using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using EditorNDS.FileHandlers;

namespace EditorNDS
{
	public partial class NewProject : UserControl
	{
		public NewProject()
		{
			InitializeComponent();

			this.DoubleBuffered = true;
			
			backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
			backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
			backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.WorkerSupportsCancellation = true;
			
			progressBar1.Maximum = progressBar1.Width;
			progressBar2.Maximum = progressBar2.Width;
		}

		string name = "";
		string author = "";
		string folder = "";
		string file = "";
		string path = "";

		string title = "";
		string code = "";

		public void backgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
		{
			Tuple<int, string, int, int> report = e.UserState as Tuple<int, string, int, int>;

			progressBar1.Text = report.Item2;
			if ( e.ProgressPercentage > 0 )
			{
				int value = Convert.ToInt32(( (double)progressBar1.Maximum / (double)e.ProgressPercentage ) * report.Item1);
				if ( progressBar1.Value != value )
				{
					progressBar1.Value = value;
				}
			}

			if ( report.Item3 > 0 )
			{
				int value = Convert.ToInt32(( (double)progressBar2.Maximum / (double)report.Item3 ) * report.Item4);
				if ( progressBar2.Value != value )
				{
					progressBar2.Text = report.Item4 + " / " + report.Item3;
					progressBar2.Value = value;
				}
			}
		}

		public void backgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
		{
			if ( e.Cancelled )
			{
				{
					DialogResult result = CustomMessageBox.Show(
							"The unpacking operation has been cancelled."
							+ " Would you like to delete the currently extracted files?",
							"Unpacking Cancelled",
							400,
							new List<string>() { "Keep", "Delete" },
							new List<DialogResult>() { DialogResult.Cancel, DialogResult.Yes });

					if ( result == DialogResult.Yes )
					{
						try
						{
							Directory.Delete(path, true);
							buttonCreate.Enabled = true;
							progressBar1.Value = 0;
							progressBar1.Text = "Unpacking Cancelled; Files Deleted";
							progressBar2.Value = 0;

						}
						catch ( Exception exception )
						{
							DialogResult dialog = CustomMessageBox.Show(
								"There was an error deleting the extracted files."
								+ " The folder may have been deleted, moved, locked, or renamed by another process."
								+ " If the folder persists, the files will be left there."
								+ " If you wish to delete them, you must do so manually.",
								exception.GetType().ToString(),
								400,
								new List<string>(),
								new List<DialogResult>());

							path = "";
							labelROMInput.Text = "";
							progressBar1.Text = "Unpacking Cancelled; Files Not Deleted";
							progressBar2.Text = ( ( progressBar1.Value * 100 ) / ( progressBar1.Maximum * 100 ) ) + "%";
							progressBar2.Value = 0;
						}
					}
					else
					{
						path = "";
						labelROMInput.Text = "";
						progressBar1.Text = "Unpacking Cancelled; Files Not Deleted";
						progressBar2.Text = ( ( progressBar1.Value * 100 ) / ( progressBar1.Maximum * 100 ) ) + "%";
						progressBar2.Value = 0;
					}
				}
			}
			else
			{
				path = "";
				labelROMInput.Text = "";
				progressBar1.Text = "Unpacking Completed";
				progressBar2.Text = "100%";
			}

			labelFolderInput.Enabled = true;
			labelROMInput.Enabled = true;
			buttonFolder.Enabled = true;
			buttonROM.Enabled = true;

			progressBar1.Refresh();
			progressBar2.Refresh();
		}

		private void textBoxName_TextChanged( object sender, EventArgs e )
		{

		}

		private void textBoxName_GotFocus( object sender, EventArgs e )
		{
			if ( textBoxName.Text == "Type in your project name." )
			{
				textBoxName.Text = "";
				textBoxName.ForeColor = Color.Black;
			}
		}

		private void textBoxName_LostFocus( object sender, EventArgs e )
		{
			if ( textBoxName.Text == "" )
			{
				textBoxName.Text = "Type in your project name.";
				textBoxName.ForeColor = Color.FromArgb(127, 127, 127);
			}
			else
			{
				name = textBoxName.Text;
			}

			Check();
		}

		private void textBoxAuthor_TextChanged( object sender, EventArgs e )
		{

		}

		private void textBoxAuthor_GotFocus( object sender, EventArgs e )
		{
			if ( textBoxAuthor.Text == "Type in your name or nickname." )
			{
				textBoxAuthor.Text = "";
				textBoxAuthor.ForeColor = Color.Black;
			}
		}

		private void textBoxAuthor_LostFocus( object sender, EventArgs e )
		{
			if ( textBoxAuthor.Text == "" )
			{
				textBoxAuthor.Text = "Type in your name or nickname.";
				textBoxAuthor.ForeColor = Color.FromArgb(127, 127, 127);
			}
			else
			{
				author = textBoxAuthor.Text;
			}

			Check();
		}

		private void buttonFolder_Click( object sender, EventArgs e )
		{
			using ( FolderBrowserDialog browser = new FolderBrowserDialog() )
			{
				browser.Description = "Select a location to save the project."
					+ " A project folder will be created in the selected"
					+ " directory in which your project files will be stored.";
				browser.RootFolder = Environment.SpecialFolder.Desktop;
				DialogResult result = browser.ShowDialog();

				if ( result != DialogResult.OK )
				{
					// Presumption of cancellation.
					return;
				}
				else
				{
					folder = browser.SelectedPath;
					labelFolderInput.Text = folder;
					labelFolderInput.ForeColor = Color.Black;
				}
			}

			Check();
		}

		private void buttonROM_Click( object sender, EventArgs e )
		{
			//----- Creating the open file dialog box. -----\\

			using ( OpenFileDialog openFileDialog = new OpenFileDialog() )
			{
				openFileDialog.Title = "Select a ROM to edit.";
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Filter = "NDS ROM (*.nds)|*.nds";
				DialogResult result = openFileDialog.ShowDialog();

				//---- Testing the open file dialog result. ----\\

				if ( result != DialogResult.OK )
				{
					// Presumption of cancellation.
					return;
				}

				byte[] rom;

				if ( openFileDialog.CheckFileExists )
				{
					// Tests the select file and reads them to an array.
					rom = File.ReadAllBytes(openFileDialog.FileName);
				}
				else
				{
					// Presumes an error in choice of file.
					return;
				}

				//---- Testing the selected ROM for errors. ----\\

				if ( rom.Length < 136 )
				{
					result = CustomMessageBox.Show(
						"This file isn't long enough to define a header."
							+ " Header size is 4 bytes long and stored at byte 132 (0x84). This file is only "
							+ rom.Length + " (0x" + rom.Length.ToString("X") + ") bytes long.",
						"File Length Error",
						400,
						new List<string>(),
						new List<DialogResult>());

					return;
				}

				using ( MemoryStream memoryStream = new MemoryStream(rom) )
				{
					rom = new byte[4];
					memoryStream.Position = 132;
					memoryStream.Read(rom, 0, 4);
					int headerSize = BitConverter.ToInt32(rom, 0);

					if ( headerSize != 16384 )
					{
						result = CustomMessageBox.Show(
							"The 4 bytes at 132 (0x84) indicate that the header size is "
							+ headerSize + " (0x" + headerSize.ToString("X") + ") bytes long."
							+ " All known headers are 16384 (0x4000) bytes long,"
							+ " The header size is either incorrect or the ROM is corrupted."
							+ " Would you like to proceed anyway?",
							"Header Size Error",
							400,
							new List<string>() { "Yes", "No" },
							new List<DialogResult>() { DialogResult.Yes, DialogResult.No });

						if ( result != DialogResult.Yes )
						{
							return;
						}
					}

					if ( memoryStream.Length < headerSize )
					{
						result = CustomMessageBox.Show(
							"The header size defined at 132 (0x84) indicates a header size of "
							+ headerSize + "(0x" + headerSize.ToString("X") + ") bytes."
							+ "This file is only " + memoryStream.Length + " (0x" + memoryStream.Length.ToString("X") + ") bytes long.",
							"Header Size Error",
							400,
							new List<string>() { },
							new List<DialogResult>() { });

						return;
					}

					//----- Creates a preliminary folder name. -----\\

					rom = new byte[16];
					memoryStream.Position = 0;
					memoryStream.Read(rom, 0, 16);

					string gameTitle = System.Text.Encoding.UTF8.GetString(rom, 0, 10);
					string gameCode = System.Text.Encoding.UTF8.GetString(rom, 12, 4);

					if ( !Program.IsValidROM(gameTitle, gameCode) )
					{
						CustomMessageBox.Show(
							"The selected ROM is invalid."
							+ " This program can only work with HeartGold"
							+ " and SoulSilver ROMs or (most) edits thereof."
							+ " The selected ROM has been identified as [" + gameCode + "] " + gameTitle + ".",
							"Incorrect ROM",
							400,
							new List<string>() { },
							new List<DialogResult>() { });

						return;
					}
					else
					{

						title = gameTitle;
						code = gameCode;
					}
				}

				file = openFileDialog.FileName;
				labelROMInput.Text = file;
				labelROMInput.ForeColor = Color.Black;
			}

			Check();
		}

		private void Check()
		{
			if (name != "" && author != "" && folder != "" && file != "" )
			{
				buttonCreate.Enabled = true;
			}
			else
			{
				buttonCreate.Enabled = false;
			}
		}

		private void DisableButtons()
		{
			textBoxName.Enabled = false;
			textBoxAuthor.Enabled = false;
			labelFolderInput.Enabled = false;
			buttonFolder.Enabled = false;
			labelROMInput.Enabled = false;
			buttonROM.Enabled = false;
			buttonCreate.Enabled = false;
		}

		private void EnableButtons()
		{
			textBoxName.Enabled = true;
			textBoxAuthor.Enabled = true;
			labelFolderInput.Enabled = true;
			buttonFolder.Enabled = true;
			labelROMInput.Enabled = true;
			buttonROM.Enabled = true;
			buttonCreate.Enabled = true;
		}

		private void buttonCreate_Click( object sender, EventArgs e )
		{
			DisableButtons();

			// For the off-chance of no file selection or file of no size.
			if ( file == null || file.Length == 0 )
			{
				DialogResult result = CustomMessageBox.Show(
					"Something can't come from nothing. This, unfortunately, is nothing. That's actually incorrect."
					+ " \"This is nothing\" doesn't make sense. You can't have nothing isn't."
					+ " If nothing wasn't, there'd be all kinds of stuff, like giant ants with top hats dancing around."
					+ " There's no room for all that.",
					"No File Chosen",
					400,
					new List<string>(),
					new List<DialogResult>());

				EnableButtons();
				return;
			}
			
			// Ditto for folder selection.
			if ( folder == null || folder == "" )
			{
				DialogResult result = CustomMessageBox.Show(
					"You haven't chosen a directory to store the unpacked file tree."
					+ "Where do you think you're going to put a tree that big?",
					"No Directory Chosen",
					400,
					new List<string>(),
					new List<DialogResult>());

				EnableButtons();
				return;
			}

			if ( name == null || name == "" )
			{
				DialogResult result = CustomMessageBox.Show(
					"You haven't chosen a project name."
					+ " What's the project folder going to be named if you don't name the project?"
					+ " How are you going to find a project folder with no name?"
					+ " Better question: how is a computer program going to find it?",
					"No Project Name",
					400,
					new List<string>(),
					new List<DialogResult>());

				EnableButtons();
				return;
			}

			path = folder + "\\" + name;

			while ( true )
			{
				// If the folder exists ...
				if ( Directory.Exists(path) )
				{
					// ... run through existing folders for an appropriate suffix.
					int i = 2;
					while ( true )
					{
						if ( Directory.Exists(path + " (" + i + ")") )
						{
							i++;
						}
						else
						{
							break;
						}
					}
					DialogResult result = CustomMessageBox.Show(
						"The selected folder contains a folder with the same name as the new directory."
						+ " Would you like to create a second directory here, appending \" (" + i + ")\" to the directory name,"
						+ " or would you prefer to overwrite the existing folder with a new directory?",
						"Directory Already Exists",
						400,
						new List<string>() { "New Directory", "Overwrite", "Cancel" },
						new List<DialogResult>() { DialogResult.Ignore, DialogResult.Retry, DialogResult.Cancel });

					if ( result == DialogResult.Ignore )
					{
						// Append suffix to folder name.
						path += " (" + i + ")";
						try
						{
							Directory.CreateDirectory(path);
						}
						catch ( Exception exception )
						{
							DialogResult dialog = CustomMessageBox.Show(
								exception.Message,
								exception.GetType().ToString(),
								400,
								new List<string>(),
								new List<DialogResult>());

							EnableButtons();
							return;
						}
						break;
					}
					else if ( result == DialogResult.Retry )
					{
						DialogResult delete = CustomMessageBox.Show(
							"Are you sure you would like to delete ths folder and its contents?"
							+ "\n\n" + path + "\n\n"
							+ "These folder and its files cannot (easily) be recovered.",
							"Delete Folder?",
							450,
							new List<string>() { "Yes", "No" },
							new List<DialogResult>() { DialogResult.Yes, DialogResult.No});

						if ( delete == DialogResult.Yes )
						{
							try
							{
								Directory.Delete(path, true);
							}
							catch ( Exception exception )
							{
								DialogResult dialog = CustomMessageBox.Show(
									exception.Message,
									exception.GetType().ToString(),
									400,
									new List<string>(),
									new List<DialogResult>());
							}
							try
							{
								Directory.CreateDirectory(path);
							}
							catch ( Exception exception )
							{
								DialogResult dialog = CustomMessageBox.Show(
									exception.Message,
									exception.GetType().ToString(),
									400,
									new List<string>(),
									new List<DialogResult>());

								EnableButtons();
								return;
							}
							break;
						}
						else
						{
							continue;
						}
					}
					else
					{
						return;
					}
				}
				else
				{
					try
					{
						Directory.CreateDirectory(path);
					}
					catch ( Exception exception )
					{
						DialogResult dialog = CustomMessageBox.Show(
							exception.Message,
							exception.GetType().ToString(),
							400,
							new List<string>(),
							new List<DialogResult>());

						EnableButtons();
						return;
					}
					break;
				}
			}

			backgroundWorker.RunWorkerAsync();
		}


		private void backgroundWorker_DoWork( object sender, DoWorkEventArgs e )
		{
			BackgroundWorker worker = (BackgroundWorker)sender;
			worker.ReportProgress(0, new Tuple<int, string, int, int>(0, "Reading System Files", 0, 0));

			string fsPath = path + "\\" + "File System";

			try
			{
				Directory.CreateDirectory(fsPath);
			}
			catch ( Exception exception )
			{
				DialogResult dialog = CustomMessageBox.Show(
					exception.Message,
					exception.GetType().ToString(),
					400,
					new List<string>(),
					new List<DialogResult>());

				return;
			}			

			if ( worker.CancellationPending )
			{
				e.Cancel = true;
				return;
			}

			File.Copy(file, path + "\\" + "[" + code + "] " + title + ".nds");

			using ( MemoryStream romStream = new MemoryStream(File.ReadAllBytes(file)) )
			{
				using ( BinaryReader romReader = new BinaryReader(romStream) )
				{
					romReader.BaseStream.Position = 32;
					int arm9Offset = Convert.ToInt32(romReader.ReadUInt32());
					romReader.BaseStream.Position = 44;
					int arm9Length = Convert.ToInt32(romReader.ReadUInt32());
					int arm7Offset = Convert.ToInt32(romReader.ReadUInt32());
					romReader.BaseStream.Position = 60;
					int arm7Length = Convert.ToInt32(romReader.ReadUInt32());
					int fntOffset = Convert.ToInt32(romReader.ReadUInt32());
					int fntLength = Convert.ToInt32(romReader.ReadUInt32());
					int fatOffset = Convert.ToInt32(romReader.ReadUInt32());
					int fatLength = Convert.ToInt32(romReader.ReadUInt32());
					int oat9Offset = Convert.ToInt32(romReader.ReadUInt32());
					int oat9Length = Convert.ToInt32(romReader.ReadUInt32());
					int oat7Offset = Convert.ToInt32(romReader.ReadUInt32());
					int oat7Length = Convert.ToInt32(romReader.ReadUInt32());
					romReader.BaseStream.Position = 104;
					int bnrOffset = Convert.ToInt32(romReader.ReadUInt32());
					romReader.BaseStream.Position = bnrOffset;
					int bnrLength = Convert.ToInt32(romReader.ReadUInt16());
					switch ( bnrLength )
					{
						case 1:
							bnrLength = 2112;
							break;
						case 2:
							bnrLength = 2112;
							break;
						case 3:
							bnrLength = 3072;
							break;
						case 259:
							bnrLength = 9216;
							break;
					}

					byte[] fntArray = new byte[fntLength];
					byte[] fatArray = new byte[fatLength];

					romReader.BaseStream.Position = fntOffset;
					romReader.Read(fntArray, 0, fntLength);
					romReader.BaseStream.Position = fatOffset;
					romReader.Read(fatArray, 0, fatLength);

					if ( fatArray.Count() % 8 > 0 )
					{
						// Exception handling for rewrite later.
						throw new Exception(
							"Table length must be a multiple of 8. This table has a length of " + fatArray.Count() + "."
							+ "\n " + fatArray.Count() + " ÷ 8 = " + ( fatArray.Count() / 8 )
							+ "\n Remainder: " + ( fatArray.Count() % 8 )
							);
					}

					List<NDSFile> fileList = new List<NDSFile>();
					fileList.Add(new NDSFile("Header", "", "", 0, 16384));
					fileList.Add(new NDSFile("File Name Table", "", "", fntOffset, fntLength));
					fileList.Add(new NDSFile("File Allocation Table", "", "", fatOffset, fatLength));
					fileList.Add(new NDSFile("Banner", "", ".bnr", bnrOffset, bnrLength));
					fileList.Add(new NDSFile("ARM9 Binary", "", ".bin", arm9Offset, arm9Length));
					fileList.Add(new NDSFile("ARM9 Overlay Table", "", "", oat9Offset, oat9Length));
					if ( arm7Length > 0 )
					{
						fileList.Add(new NDSFile("ARM7 Binary", "", ".bin", arm7Offset, arm9Length));
					}
					if ( oat7Length > 0 )
					{
						fileList.Add(new NDSFile("ARM7 Overlay Table", "", "", oat7Offset, oat9Length));
					}

					int directoryCount = BitConverter.ToUInt16(fntArray, 6);
					int firstFile = BitConverter.ToInt16(fntArray, 4);
					int fileCount = fatArray.Count() / 8;
					int workload = ( fileCount * 4 ) + ( directoryCount * 2 );
					int progress = 0;

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading File Name And Allocation Tables", fileCount, 0));
					NDSFile[] files = new NDSFile[fileCount];

					for ( int i = 0; i < fileCount; i++ )
					{
						if ( worker.CancellationPending )
						{
							e.Cancel = true;
							return;
						}

						files[i] = new NDSFile();
						NDSFile currentFile = files[i];
						files[i].Offset = Convert.ToInt32(BitConverter.ToUInt32(fatArray, i * 8));
						files[i].Length = Convert.ToInt32(BitConverter.ToUInt32(fatArray, i * 8 + 4)) - files[i].Offset;

						progress++;
						worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading File Allocation Table", fileCount, i + 1));
					}

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading Overlay Allocation Table", firstFile, 0));

					string[] directories;
					List<string> dirList = new List<string>();
					directories = new string[directoryCount];
					if ( firstFile > 0 )
					{
						directories[0] = "\\Root";
						dirList.Add("\\Overlays");
						for ( int i = 0; i < firstFile; i++ )
						{
							if ( worker.CancellationPending )
							{
								e.Cancel = true;
								return;
							}

							files[i].Name = "Overlay " + i.ToString("D" + firstFile.ToString().Length) + ".bin";
							files[i].Path = "\\Overlays";

							progress++;
							worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading Overlay Allocation Table", firstFile, i + 1));
						}
					}
					else
					{
						directories[0] = "";
					}

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading File Name Table", fileCount + directoryCount, 0));

					int unnamedCount = 0;
					int dirProgress = 0;
					int fileProgress = 0;

					for ( int i = 0; i < directoryCount; i++ )
					{
						int entryPos = Convert.ToInt32(BitConverter.ToUInt32(fntArray, i * 8));
						int fileIndex = Convert.ToInt32(BitConverter.ToUInt16(fntArray, ( i * 8 ) + 4));

						while ( true )
						{
							if ( worker.CancellationPending )
							{
								e.Cancel = true;
								return;
							}

							byte entryByte = fntArray[entryPos++];

							if ( entryByte == 0 )
							{
								break;
							}

							else if ( entryByte == 128 )
							{
								int index = BitConverter.ToUInt16(fntArray, entryPos) - 61440;
								directories[index] = directories[i] + "\\Unnamed " + unnamedCount++;
								dirProgress++;
								entryPos += 2;
							}

							else if ( entryByte > 128 )
							{
								int index = BitConverter.ToUInt16(fntArray, ( entryPos ) + ( entryByte - 128 )) - 61440;
								directories[index] = directories[i] + "\\" + System.Text.Encoding.UTF8.GetString(fntArray, entryPos, entryByte - 128);
								dirProgress++;
								entryPos += ( entryByte - 128 ) + 2;
							}

							else
							{
								files[fileIndex].Name = System.Text.Encoding.UTF8.GetString(fntArray, entryPos, entryByte);
								files[fileIndex].Path = directories[i];
								fileIndex++;
								fileProgress++;
								entryPos += entryByte;
							}

							progress++;
							worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading File Name Table", fileCount + directoryCount, dirProgress + fileProgress));
						}
					}

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Getting File Extensions", fileCount, 0));

					fileList.AddRange(files.ToList());
					dirList.AddRange(directories.ToList());
					files = null;
					directories = null;
					List<NDSFile> narcList = new List<NDSFile>();
					for ( int i = 0; i < fileCount; i++ )
					{
						if ( worker.CancellationPending )
						{
							e.Cancel = true;
							return;
						}

						NDSFile file = fileList[i];
						file.GetExtension(romStream);

						if ( file.Extension == ".narc" )
						{
							narcList.Add(file);
							workload++;
						}

						progress++;
						worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Getting File Extensions", fileCount, i + 1));
					}

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading NARC Files", directoryCount, 0));

					if ( narcList.Count > 0 )
					{
						for ( int i = 0; i < narcList.Count; i++ )
						{
							if ( worker.CancellationPending )
							{
								e.Cancel = true;
								return;
							}

							NDSFile file = narcList[i];
							NARC narc = new NARC(romStream, file);
							if ( narc.isValid )
							{
								fileList.Remove(file);
								dirList.AddRange(narc.dirList);
								fileList.AddRange(narc.fileList);
							}

							progress++;
							worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Reading NARC Files", narcList.Count, i + 1));
						}
					}

					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Writing Folder Structure", directoryCount, 0));

					double compWorkload = directoryCount / dirList.Count;
					for ( int i = 0; i < dirList.Count; i++ )
					{
						if ( worker.CancellationPending )
						{
							e.Cancel = true;
							return;
						}

						Directory.CreateDirectory(fsPath + dirList[i]);

						int p = Convert.ToInt32(( i + 1 ) * compWorkload);
						worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress + p, "Writing Folder Structure", directoryCount, (int)( ( i + 1 ) * compWorkload )));
					}

					progress += directoryCount;
					worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress, "Extracting Files", fileList.Count, 0));

					compWorkload = (double)fileCount / (double)fileList.Count;
					for ( int i = 0; i < fileList.Count; i++ )
					{
						if ( worker.CancellationPending )
						{
							e.Cancel = true;
							return;
						}

						NDSFile file = fileList[i];
						romReader.BaseStream.Position = file.Offset;
						byte[] image = new byte[file.Length];
						romReader.Read(image, 0, file.Length);

                        Directory.CreateDirectory(path + "\\Text Files");
                        if (file.Path == "\\Root\\a\\0\\2\\7.narc")
						{
							PokeTextIV TextFile = new PokeTextIV(romStream, file);

                            System.IO.File.WriteAllLines(path + "\\Text Files\\" + file.Name + ".txt", TextFile.StringList);
                        }
						
						/*
						using ( BinaryWriter writer = new BinaryWriter(File.Open(fsPath + file.Path + "\\" + file.Name + file.Extension, FileMode.Create)) )
						{
							writer.Write(image, 0, file.Length);
						}
						*/

						int p = Convert.ToInt32(( i + 1 ) * compWorkload);
						worker.ReportProgress(workload, new Tuple<int, string, int, int>(progress + p, "Extracting Files", fileList.Count, ( i + 1 )));
					}
				}
			}
		}
	}
}
