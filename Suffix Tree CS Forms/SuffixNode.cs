using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Suffix_Tree_CS_Forms
{
    class SuffixNode
    {
        public List<SuffixNode> child;
        public int position;
        public int length;
        public char element;
        private int nextChildIndex = 0;
        public int listNum = -1;
        public bool wasVisited;
        public PointF center;

        public SuffixNode()
        {
            child = new List<SuffixNode>();
            position = -1;
            length = -1;
            //listNum = -1;
            wasVisited = false;
            center = new PointF(0, 0);
        }

        public SuffixNode(int position, int length, char element)
        {
            child = new List<SuffixNode>();
            this.position = position;
            this.length = length;
            this.element = element;
        }

        public SuffixNode(int position, int length, char element, int listNum)
        {
            child = new List<SuffixNode>();
            this.position = position;
            this.length = length;
            this.element = element;
            this.listNum = listNum;
        }

        public SuffixNode NextChild()
        {
            if ((nextChildIndex + 1) == child.Count)
            {
                return child[nextChildIndex++];
            }
            else
            {
                nextChildIndex = 0;
                return null;
            }


        }

        public SuffixNode FindChild(char element)
        {
            for (int i = 0; i < child.Count; i++)
            {
                if (element == child[i].element)
                {
                    return child[i];
                }
            }
            return null;
        }

        public void RemoveChild(char element)
        {
            for (int i = 0; i < child.Count; i++)
            {
                if (element == child[i].element)
                {
                    child.Remove(child[i]);
                }
            }
        }

        //public int ReturnLists()
        //{
        //    List<int> returnLists = new List<int>();

        //    for (int i = 0; i < child.Count; i++)
        //    {
        //        if (child[i].listNum != -1)
        //        {
        //            returnLists.Add(child[i].listNum);
        //        }
        //        else
        //        {
        //            ReturnLists()
        //        }
        //    }
        //}
    }
}
