using System;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace Suffix_Tree_CS_Forms
{
    public partial class Form1 : Form
    {
        private DrawTree drawTree;
        public int dX;
        public int dY = 200;

        private Form loginForm;
        private SuffixTree suffixTree;

        public Form1()
        {
            InitializeComponent();
            //drawTree = new DrawTree(sheet.Width, sheet.Height);
            //sheet.Image = drawTree.GetBitmap();
            inputTextBox.Text = "mississippi";
            //drawTree.DrawNode(dX, 10);
            //drawTree.gr.DrawLine(drawTree.blackPen, 0, 0, sheet.Width, sheet.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void sheet_Click(object sender, EventArgs e)
        {
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            var input = inputTextBox.Text;

            suffixTree = new SuffixTree(input);
            suffixTree.Build();

            DrawTree(suffixTree);

            //DrawTree(suffixTree);
        }

        private void DrawTree(SuffixTree suffixTree)
        {
            suffixTree.removeMarks();
            dX = sheet.Width / suffixTree.nodeNumber;

            drawTree = new DrawTree(sheet.Width, sheet.Height, suffixTree.input);
            var x = drawTree.R + 10;
            var y = drawTree.R;
            //drawTree.clearSheet();
            sheet.Image = drawTree.GetBitmap();
            drawTree.clearSheet();

            suffixTree.theStack.Push(suffixTree.root);
            suffixTree.root.wasVisited = true;

            suffixTree.root.center.X = sheet.Width / 2;
            suffixTree.root.center.Y = y;

            drawTree.DrawNode(suffixTree.root.center);

            SuffixNode currentNode = null;
            SuffixNode parentNode = null;

            while (suffixTree.theStack.Count != 0)
            {
                parentNode = suffixTree.theStack.Peek();
                currentNode = suffixTree.getUnvisitedChild(suffixTree.theStack.Peek());
                if (currentNode == null)
                {
                    suffixTree.theStack.Pop();
                    y -= dY;
                }
                else
                {
                    y += dY;
                    currentNode.wasVisited = true;
                    suffixTree.theStack.Push(currentNode);
                    sheet.Image = drawTree.GetBitmap();

                    currentNode.center.X = x;
                    currentNode.center.Y = y;

                    drawTree.DrawEdge(parentNode, currentNode);
                    x += dX;
                }
            }

            for (var i = 0; i < suffixTree.selectedLeafs.Count; i++)
                drawTree.DrawSelectedLeaf(suffixTree.selectedLeafs[i]);
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //suffixTree.Build();
            DrawTree(suffixTree);
        }

        private void search_Button_Click(object sender, EventArgs e)
        {
            suffixTree.selectedLeafs.Clear();
            //sheet.Image = drawTree.GetBitmap();
            //drawTree.clearSheet();

            suffixTree.returnList.Clear();
            //suffixTree.returnLists.Clear();

            var searchList = suffixTree.Search(searchTextBox.Text);

            for (var i = 0; i < searchList.Count; i++)
            for (var j = 0; j < suffixTree.leafs.Count; j++)
                if (searchList[i] == suffixTree.leafs[j])
                    //drawTree.DrawSelectedLeaf(suffixTree.leafs[j]);
                    suffixTree.selectedLeafs.Add(suffixTree.leafs[j]);

            DrawTree(suffixTree);
        }


        private bool checkLogging() //функция проверки залогиненности
        {
            var currentHWID = GetHWID(); //получает текущий хвид пользователя
            var filePath =
                Path.GetFullPath(
                    "HWIDList.txt"); //список с существующими пользователи, которые при входе в профиль нажали галочку "запомнить меня"
            if (File.Exists(filePath)) //если файл со списком существует
                using (var reader = new StreamReader(filePath)
                ) //то проверяет, есть ли в этом списке текущий хвид пользователя
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        if (line.Contains(currentHWID)) //если содержит текущий хвид, то залогинен
                        {
                            reader.Dispose();
                            return true;
                        }
                    }

                    reader.Dispose();
                    return false;
                }

            return false; //иначе не залогинен
        }

        private string GetHWID() //функция получения хвида
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            var mbsList = mbs.Get();
            var id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }

            mbs.Dispose();
            return id;
        }

        private void Form1_Shown_1(object sender, EventArgs e)
        {
            if (checkLogging() == false) //проверки залогиненности пользователя
            {
                loginForm = new Form2(this); //если нет, то открывается вторая форма с окном регистрации/входа
                loginForm.ShowDialog();
            }
        }
    }
}