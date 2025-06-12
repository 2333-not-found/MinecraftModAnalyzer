using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MinecraftModAnalyzer
{
    public partial class MainForm : Form
    {
        private readonly BindingList<ModInfo> modList = new BindingList<ModInfo>();
        private readonly Dictionary<string, int> authorStats = new Dictionary<string, int>();
        private readonly Dictionary<string, int> dependencyStats = new Dictionary<string, int>();
        private readonly HashSet<string> minecraftVersions = new HashSet<string>();

        public MainForm()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupEventHandlers();
        }

        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "JarFile", HeaderText = "JAR文件", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "ModId", HeaderText = "Mod ID", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Name", HeaderText = "名称", Width = 150 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Version", HeaderText = "版本", Width = 100 },
                new DataGridViewTextBoxColumn { DataPropertyName = "MinecraftVersion", HeaderText = "MC版本", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Authors", HeaderText = "作者", Width = 200 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Dependencies", HeaderText = "依赖项", Width = 200 }
            });
            dataGridView.DataSource = modList;
        }

        private void SetupEventHandlers()
        {
            DragEnter += MainForm_DragEnter;
            DragDrop += MainForm_DragDrop;
            btnClear.Click += (s, e) => ClearAll();
            btnExport.Click += BtnExport_Click;
            btnAnalyze.Click += (s, e) => AnalyzeSelectedJars();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null)
                AddJarsToList(files);
        }

        private void AddJarsToList(string[] files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file) &&
                    file.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) &&
                    !lstJarFiles.Items.Contains(file))
                {
                    lstJarFiles.Items.Add(file);
                }
            }
        }

        private void AnalyzeSelectedJars()
        {
            if (lstJarFiles.Items.Count == 0)
            {
                MessageBox.Show("请先添加JAR文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            ClearResults();
            progressBar.Maximum = lstJarFiles.Items.Count;
            progressBar.Value = 0;

            foreach (var item in lstJarFiles.Items)
            {
                ProcessJarFile(item.ToString());
                progressBar.Value++;
                Application.DoEvents();
            }
            UpdateStatistics();
        }

        private void ProcessJarFile(string jarPath)
        {
            if (!File.Exists(jarPath))
            {
                modList.Add(new ModInfo
                {
                    SourcePath = jarPath,
                    JarFile = Path.GetFileName(jarPath),
                    Name = "错误: 文件不存在"
                });
                return;
            }

            try
            {
                using (var jar = ZipFile.OpenRead(jarPath))
                {
                    var modInfoEntry = jar.Entries.FirstOrDefault(e =>
                        e.Name.Equals("mcmod.info", StringComparison.OrdinalIgnoreCase) &&
                        string.IsNullOrEmpty(Path.GetDirectoryName(e.FullName)));

                    if (modInfoEntry == null)
                    {
                        modInfoEntry = jar.Entries.FirstOrDefault(e =>
                            e.FullName.EndsWith("/mcmod.info", StringComparison.OrdinalIgnoreCase));
                    }

                    if (modInfoEntry == null)
                    {
                        modList.Add(new ModInfo
                        {
                            JarFile = Path.GetFileName(jarPath),
                            Name = "未找到mcmod.info"
                        });
                        return;
                    }

                    using (var stream = modInfoEntry.Open())
                    using (var reader = new StreamReader(stream))
                    {
                        ParseModInfo(reader.ReadToEnd(), jarPath);
                    }
                }
            }
            catch (InvalidDataException)
            {
                modList.Add(new ModInfo
                {
                    SourcePath = jarPath,
                    JarFile = Path.GetFileName(jarPath),
                    Name = "错误: 无效的JAR文件"
                });
            }
            catch (Exception ex)
            {
                modList.Add(new ModInfo
                {
                    SourcePath = jarPath,
                    JarFile = Path.GetFileName(jarPath),
                    Name = "错误: " + ex.GetType().Name + " - " + ex.Message
                });
            }
        }

        private void ParseModInfo(string json, string jarPath)
        {
            try
            {
                var token = JToken.Parse(json);
                JArray modArray = null;

                if (token is JObject rootObj && rootObj["modList"] != null)
                {
                    modArray = rootObj["modList"] as JArray;
                }
                else if (token is JArray array)
                {
                    modArray = array;
                }

                if (modArray == null)
                {
                    modList.Add(new ModInfo
                    {
                        JarFile = Path.GetFileName(jarPath),
                        Name = "错误: 无法识别的mcmod.info格式"
                    });
                    return;
                }

                var jarName = Path.GetFileName(jarPath);
                foreach (JObject mod in modArray.Children<JObject>())
                {
                    try
                    {
                        var modInfo = new ModInfo
                        {
                            SourcePath = jarPath,
                            JarFile = jarName,
                            ModId = mod["modid"] != null ? mod["modid"].Value<string>() : "未知",
                            Name = mod["name"] != null ? mod["name"].Value<string>() : "未命名",
                            Version = mod["version"] != null ? mod["version"].Value<string>() : "未知",
                            MinecraftVersion = mod["mcversion"] != null ? mod["mcversion"].Value<string>() : "未知"
                        };

                        ParseAuthors(mod, modInfo);
                        ParseDependencies(mod, modInfo);

                        if (!string.IsNullOrEmpty(modInfo.MinecraftVersion))
                            minecraftVersions.Add(modInfo.MinecraftVersion);

                        modList.Add(modInfo);
                    }
                    catch (Exception ex)
                    {
                        modList.Add(new ModInfo
                        {
                            SourcePath = jarPath,
                            JarFile = jarName,
                            Name = "MOD解析错误: " + ex.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                modList.Add(new ModInfo
                {
                    SourcePath = jarPath,
                    JarFile = Path.GetFileName(jarPath),
                    Name = "解析错误: " + ex.Message
                });
            }
        }

        private void ParseAuthors(JObject mod, ModInfo modInfo)
        {
            try
            {
                var authorsToken = mod["authorList"] ?? mod["authors"];
                if (authorsToken == null) return;

                List<string> authors = null;

                if (authorsToken is JArray array)
                {
                    authors = array.Select(a => a.Value<string>()).ToList();
                }
                else if (authorsToken.Type == JTokenType.String)
                {
                    authors = authorsToken.Value<string>()
                        .Split(',')
                        .Select(a => a.Trim())
                        .Where(a => !string.IsNullOrEmpty(a))
                        .ToList();
                }

                if (authors == null || authors.Count == 0) return;

                modInfo.Authors = string.Join(", ", authors);
                UpdateStats(authors, authorStats);
            }
            catch (Exception ex)
            {
                modInfo.Authors = "解析错误: " + ex.Message;
            }
        }

        private void ParseDependencies(JObject mod, ModInfo modInfo)
        {
            try
            {
                var depsToken = mod["dependencies"] ?? mod["dependants"];
                if (depsToken == null) return;

                List<string> deps = null;

                if (depsToken is JArray array)
                {
                    deps = array.Select(d => d.Value<string>()).ToList();
                }
                else if (depsToken.Type == JTokenType.String)
                {
                    deps = depsToken.Value<string>()
                        .Split(',')
                        .Select(d => d.Trim())
                        .Where(d => !string.IsNullOrEmpty(d))
                        .ToList();
                }

                if (deps == null || deps.Count == 0) return;

                modInfo.Dependencies = string.Join(", ", deps);
                UpdateStats(deps, dependencyStats);
            }
            catch (Exception ex)
            {
                modInfo.Dependencies = "解析错误: " + ex.Message;
            }
        }

        private void UpdateStats(List<string> items, Dictionary<string, int> stats)
        {
            foreach (var item in items)
            {
                if (stats.ContainsKey(item))
                {
                    stats[item]++;
                }
                else
                {
                    stats[item] = 1;
                }
            }
        }

        private void UpdateStatistics()
        {
            lblModCount.Text = "找到MOD数量: " +
                modList.Count(m => !m.Name.StartsWith("错误") && !m.Name.StartsWith("未找到"));

            lblJarCount.Text = "处理JAR数量: " + lstJarFiles.Items.Count;
            lblMCVersions.Text = "MC版本: " + string.Join(", ", minecraftVersions);

            lstAuthors.Items.Clear();
            foreach (var author in authorStats.OrderByDescending(a => a.Value))
            {
                lstAuthors.Items.Add(author.Key + ": " + author.Value + "个MOD");
            }

            lstDependencies.Items.Clear();
            foreach (var dep in dependencyStats.OrderByDescending(d => d.Value))
            {
                lstDependencies.Items.Add(dep.Key + ": " + dep.Value + "次引用");
            }
        }

        private void ClearResults()
        {
            modList.Clear();
            authorStats.Clear();
            dependencyStats.Clear();
            minecraftVersions.Clear();
            lstAuthors.Items.Clear();
            lstDependencies.Items.Clear();
            lblModCount.Text = "找到MOD数量: 0";
            lblJarCount.Text = "处理JAR数量: 0";
            lblMCVersions.Text = "MC版本: ";
        }

        private void ClearAll()
        {
            lstJarFiles.Items.Clear();
            ClearResults();
            progressBar.Value = 0;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (modList.Count == 0)
            {
                MessageBox.Show("没有数据可导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV文件|*.csv";
                saveDialog.Title = "导出结果";
                saveDialog.FileName = "mod_analysis_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(saveDialog.FileName);
                }
            }
        }

        private void ExportToCsv(string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("JAR文件,Mod ID,名称,版本,MC版本,作者,依赖项,源路径");

                    foreach (var mod in modList)
                    {
                        writer.WriteLine(
                            "\"" + EscapeCsv(mod.JarFile) + "\"," +
                            "\"" + EscapeCsv(mod.ModId) + "\"," +
                            "\"" + EscapeCsv(mod.Name) + "\"," +
                            "\"" + EscapeCsv(mod.Version) + "\"," +
                            "\"" + EscapeCsv(mod.MinecraftVersion) + "\"," +
                            "\"" + EscapeCsv(mod.Authors) + "\"," +
                            "\"" + EscapeCsv(mod.Dependencies) + "\"," +
                            "\"" + EscapeCsv(mod.SourcePath) + "\"");
                    }
                }

                MessageBox.Show("成功导出 " + modList.Count + " 条记录到:\n" + filePath,
                    "导出完成", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private static string EscapeCsv(string input)
        {
            return string.IsNullOrEmpty(input) ? "" : input.Replace("\"", "\"\"");
        }
    }

    public class ModInfo
    {
        public string SourcePath { get; set; } = "";
        public string JarFile { get; set; } = "";
        public string ModId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
        public string MinecraftVersion { get; set; } = "";
        public string Authors { get; set; } = "";
        public string Dependencies { get; set; } = "";
    }
}