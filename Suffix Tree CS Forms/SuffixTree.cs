using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suffix_Tree_CS_Forms
{
    class SuffixTree
    {
        public SuffixNode root = new SuffixNode();
        private static char unique_char = '$';
        public string input;
        public int listNumber;
        public int nodeNumber;
        //public List<SuffixNode> returnLists = new List<SuffixNode>();
        public List<SuffixNode> returnList = new List<SuffixNode>();
        public List<SuffixNode> leafs = new List<SuffixNode>();
        public Stack<SuffixNode> theStack;
        public int treeDeep;
        public List<SuffixNode> selectedLeafs;

        public SuffixTree(string input)
        {
            this.input = input + unique_char;
            listNumber = 0;
            nodeNumber = 1;
            theStack = new Stack<SuffixNode>();
            treeDeep = 0;
            selectedLeafs = new List<SuffixNode>();
        }

        public void Build()
        {
            StringBuilder tempString = new StringBuilder();

            for (int i = 1; i <= input.Length; i++)
            {
                tempString.Append(input.Substring(0, i));
                AddSuffix(root, tempString, i, 0);
                for (int j = tempString.Length - 1; j > 0; j--)
                {
                    tempString.Remove(0, 1);
                    AddSuffix(root, tempString, i, i - j);
                }
                tempString.Clear();
            }
        }

        public void AddSuffix(SuffixNode node, StringBuilder suffix, int position, int removeIndex)
        {
            int AddNodePosition = 0;
            int suffix0_index = input.IndexOf(suffix[0]);
            SuffixNode currentNode = node.FindChild(input[removeIndex]);

            if (currentNode != null)
            {
                string currentSuffix = input.Substring(currentNode.position, currentNode.length);
                while (AddNodePosition != currentNode.length && AddNodePosition != suffix.Length && suffix[AddNodePosition] == currentSuffix[AddNodePosition]) // Пока длина суффикса не равна 0 и 
                                                                                                                                                               // совпадают первые символы суффиксов
                {
                    ++AddNodePosition;
                }
                if (AddNodePosition == suffix.Length)
                {
                    return;
                }
                else if (AddNodePosition == currentSuffix.Length && currentNode.child.Count == 0) // Если пришли в лист
                {
                    currentNode.length++;
                    currentNode.position = removeIndex;
                    return;
                }
                else if (AddNodePosition == currentSuffix.Length && currentNode.child.Count != 0)
                {
                    string str = suffix.ToString().Substring(AddNodePosition);
                    StringBuilder str1 = new StringBuilder();
                    str1.Append(str);
                    AddSuffix(currentNode, str1, position, removeIndex + AddNodePosition);
                }
                else // если остановились на мнимой вершине
                {
                    node.RemoveChild(currentNode.element);
                    node.child.Add(new SuffixNode(currentNode.position, AddNodePosition, input[currentNode.position]));
                    nodeNumber++;
                    nodeNumber++;
                    node.FindChild(input[currentNode.position]).child.Add(currentNode);
                    node.FindChild(input[currentNode.position]).child.Add(new SuffixNode(position - (suffix.Length - AddNodePosition), suffix.Length - AddNodePosition, input[position - (suffix.Length - AddNodePosition)], listNumber++));

                    SuffixNode temp_leaf = node.FindChild(input[currentNode.position]);
                    leafs.Add(temp_leaf.FindChild(input[position - (suffix.Length - AddNodePosition)]));

                    currentNode.position = currentNode.position + AddNodePosition;
                    currentNode.length = currentSuffix.Length - AddNodePosition;
                    currentNode.element = input[currentNode.position];
                    return;

                }
            }
            else // Если вершина не нашлась
            {
                node.child.Add(new SuffixNode(removeIndex, suffix.Length, input[removeIndex], listNumber++));
                leafs.Add(node.FindChild(input[removeIndex]));
                nodeNumber++;
            }

        }

        public List<SuffixNode> Search(string substring)
        {
            return SearchSuffix(root, substring);
        }

        public List<SuffixNode> SearchSuffix(SuffixNode node, string suffix)
        {
            //List<int> returnList = new List<int>();
            int AddNodePosition = 0;
            SuffixNode currentNode = node.FindChild(suffix[0]);
            if (currentNode != null)
            {
                string currentSuffix = input.Substring(currentNode.position, currentNode.length);
                while (AddNodePosition != currentNode.length && AddNodePosition != suffix.Length && suffix[AddNodePosition] == currentSuffix[AddNodePosition])                                                                                                                           // совпадают первые символы суффиксов
                {
                    ++AddNodePosition;
                }

                if (AddNodePosition == suffix.Length)
                {
                    if (currentNode.listNum != -1)
                    {
                        returnList.Add(currentNode);
                    }
                    else
                    {
                        ReturnLists(currentNode);
                    }
                }
                else
                {
                    SearchSuffix(currentNode, suffix.Substring(AddNodePosition));
                }
            }
            else
            {
                return returnList;
            }
            return returnList;
        }

        public void DeepDFS()
        {
            int maxDeep = 0;
            theStack.Push(root);
            root.wasVisited = true;
            SuffixNode currentNode = null;
            while (theStack.Count != 0)
            {
                currentNode = getUnvisitedChild(theStack.Peek());
                if (currentNode == null)
                {
                    theStack.Pop();
                    maxDeep--;
                }
                else
                {
                    currentNode.wasVisited = true;
                    theStack.Push(currentNode);
                    maxDeep++;
                    if (maxDeep > treeDeep)
                    {
                        treeDeep = maxDeep;
                    }
                }
            }

            
        }

        public void removeMarks()
        {
            int maxDeep = 0;
            theStack.Push(root);
            root.wasVisited = false;
            SuffixNode currentNode = null;
            while (theStack.Count != 0)
            {
                currentNode = getVisitedChild(theStack.Peek());
                if (currentNode == null)
                {
                    theStack.Pop();
                    maxDeep--;
                }
                else
                {
                    currentNode.wasVisited = false;
                    theStack.Push(currentNode);
                    
                }
            }
        }

        public SuffixNode getUnvisitedChild(SuffixNode node)
        {
            foreach (SuffixNode child in node.child)
            {
                if (child.wasVisited == false)
                {
                    return child;
                }
            }
            return null;
        }

        public SuffixNode getVisitedChild(SuffixNode node)
        {
            foreach (SuffixNode child in node.child)
            {
                if (child.wasVisited == true)
                {
                    return child;
                }
            }
            return null;
        }

        public List<SuffixNode> ReturnLists(SuffixNode node)
        {
            List<int> returnLists = new List<int>();

            for (int i = 0; i < node.child.Count; i++)
            {
                if (node.child[i].listNum != -1)
                {
                    returnList.Add(node.child[i]);
                }
                else
                {
                    ReturnLists(node.child[i]);
                }
            }
            return returnList;
        }



    }
}
